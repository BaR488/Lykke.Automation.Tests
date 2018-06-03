﻿using BlockchainsIntegration.BlockchainWallets;
using BlockchainsIntegration.Api;
using BlockchainsIntegration.Sign;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;
using Newtonsoft.Json;
using System.IO;
using AFTests.BlockchainsIntegration;
using XUnitTestCommon.TestsCore;
using System.Linq;
using BlockchainsIntegration.Models;
using Lykke.Client.AutorestClient.Models;
using System.Net;
using XUnitTestCommon.ServiceSettings;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace AFTests.BlockchainsIntegrationTests
{
    [NonParallelizable]
    class BlockchainsIntegrationBaseTest : BaseTest
    {
        private static object _lock = new object();
        private static Queue<WalletCreationResponse> result = null;

        private static Lazy<BlockchainSpecificModel> _currentSettings
        {
            get
            {
                return new Lazy<BlockchainSpecificModel>
                                    (BlockchainSpecificSettingsFactory.BlockchainSpecificSettings(SpecificBlockchain()));
            }
        }

       protected static string SpecificBlockchain()
       {
            return Environment.GetEnvironmentVariable("BlockchainIntegration") ?? "Dash"; //"monero"; //"RaiBlocks";//"bitshares";// "stellar-v2";//"Zcash"; //"Ripple";// "Dash"; "Litecoin";
        }

        #region test values

        protected static string BlockchainApi { get { return _currentSettings.Value.BlockchainApi; } }
        protected BlockchainApi blockchainApi = new BlockchainApi(BlockchainApi);
        protected BlockchainSign blockchainSign = new BlockchainSign(_currentSettings.Value.BlockchainSign);
        protected BlockchainWallets blockchainWallets = new BlockchainWallets();
        protected static string HOT_WALLET = _currentSettings.Value.HotWallet;
        protected static string HOT_WALLET_KEY = _currentSettings.Value.HotWalletKey;
        protected static string HOT_WALLET_CONTEXT = _currentSettings.Value.HotWalletAddressContext;
        protected static string BlockChainName = _currentSettings.Value.BlockchainIntegration;
        protected static string ASSET_ID = _currentSettings.Value.AssetId;
        protected static string EXTERNAL_WALLET = _currentSettings.Value.ExternalWalletAddress;
        protected static string EXTERNAL_WALLET_KEY = _currentSettings.Value.ExternalWalletKey;
        protected static string EXTERNAL_WALLET_ADDRESS_CONTEXT = _currentSettings.Value.ExternalWallerAddressContext;
        protected static string AMOUNT = "20000001";
        protected static string AMOUT_WITH_FEE = "29000001";
        protected static long BLOCKCHAIN_MINING_TIME = _currentSettings.Value.BlockchainMiningTime ?? 10;

        #endregion

        [SetUp]
        public void SetUp()
        {
            TestContext.Progress.WriteLine($"Started test {TestContext.CurrentContext.Test.Name}");
        }

        [OneTimeTearDown]
        public void SetProperty()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Date", DateTime.Now.ToString());
            try
            {
                var isAlive = blockchainApi.IsAlive.GetIsAlive().GetResponseObject();
                AllurePropertiesBuilder.Instance.AddPropertyPair("Env", isAlive.Env);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Name", isAlive.Name);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
                AllurePropertiesBuilder.Instance.AddPropertyPair("ContractVersion", isAlive.ContractVersion);
            }
            catch (Exception) { /*do nothing*/}

            new Allure2Report().CreateEnvFile();
        }

        #region Helpers methods


        protected static Queue<WalletCreationResponse> Wallets()
        {
            lock (_lock)
            {
                if (result == null)
                {
                    result = new Queue<WalletCreationResponse>();
                    BlockchainSign blockchainSign = new BlockchainSign(_currentSettings.Value.BlockchainSign);
                    for (var i = 0; i < 20; i++)
                    {
                        var wallet = blockchainSign.PostWallet();
                        if (wallet.StatusCode != HttpStatusCode.OK)
                            throw new Exception($"Cant create wallet. Got: {wallet.StatusCode}.  {wallet.Content}");
                        result.Enqueue(wallet.GetResponseObject());
                    }

                    if (!SetBalanceWIthManyOutputs(result.ToList()))
                    {
                        result.ToList().ForEach(w => AddCyptoToBalanceFromExternal(w.PublicAddress, w.PrivateKey, false));
                        result.ToList().ForEach(w => WaitForBalance(w.PublicAddress));
                    }
                }
                result.ToList().ForEach(w =>
                {
                    TestContext.Out.WriteLine($"wallet {w.PublicAddress} balance: { new BlockchainApi(_currentSettings.Value.BlockchainApi).Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(wallet => wallet.Address == w.PublicAddress)?.Balance}");
                });
            }

            return result;
        }

        private static bool SetBalanceWIthManyOutputs(List<WalletCreationResponse> wallets)
        {
            var run = new BlockchainApi(_currentSettings.Value.BlockchainApi).Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported;
            if (!run.Value)
                return false;

            List<TransactionOutputContract> transactions = new List<TransactionOutputContract>();

            wallets.ForEach(w => transactions.Add(new TransactionOutputContract() { Amount = AMOUT_WITH_FEE, ToAddress = w.PublicAddress }));
            var request = new BuildTransactionWithManyOutputsRequest()
            {
                AssetId = ASSET_ID,
                OperationId = Guid.NewGuid(),
                FromAddress = EXTERNAL_WALLET,
                Outputs = transactions
            };

            wallets.ForEach(w =>
                new BlockchainApi(_currentSettings.Value.BlockchainApi).Balances.PostBalances(w.PublicAddress));

            var response = new BlockchainApi(_currentSettings.Value.BlockchainApi).Operations.PostTransactionsManyOutputs(request);
            response.Validate.StatusCode(HttpStatusCode.OK);

            var signResponse = new BlockchainSign(_currentSettings.Value.BlockchainSign).PostSign(new SignRequest() { TransactionContext = response.GetResponseObject().TransactionContext, PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY } });

            var broadcastRequset = new BroadcastTransactionRequest() { OperationId = request.OperationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
            var broadcatedResponse = new BlockchainApi(_currentSettings.Value.BlockchainApi).Operations.PostTransactionsBroadcast(broadcastRequset);

            WaitForBalance(wallets[0].PublicAddress);

            return true;
        }

        protected static void AddCyptoToBalanceFromExternal(string walletAddress, string walletKey = null, bool wait = true)
        {
            var api = new BlockchainApi(BlockchainApi);
            var sign = new BlockchainSign(_currentSettings.Value.BlockchainSign);

            var transferSupported = api.Capabilities.GetCapabilities().GetResponseObject().IsTestingTransfersSupported;
            var recieveSupport = api.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired;
            if (transferSupported != null && transferSupported.Value)
            {
                api.Balances.PostBalances(walletAddress);
                TestingTransferRequest request = new TestingTransferRequest() { amount = AMOUT_WITH_FEE, assetId = ASSET_ID, fromAddress = EXTERNAL_WALLET, fromPrivateKey = EXTERNAL_WALLET_KEY, toAddress = walletAddress };
                var response = api.Testing.PostTestingTransfer(request);
            }
            else if (BlockChainName == "RaiBlocks" || ( recieveSupport != null && recieveSupport.Value)) //raiblocks - temp. will be removed after capablities enabled
            {
                AddCryptoToWalletWithRecieveTransaction(walletAddress, walletKey, wait);
            }
            else
            {
                api.Balances.PostBalances(walletAddress);
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUT_WITH_FEE,
                    AssetId = ASSET_ID,
                    FromAddress = EXTERNAL_WALLET,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = walletAddress,
                    FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT
                };

                var responseTransaction = api.Operations.PostTransactions(model).GetResponseObject();
                Assert.That(responseTransaction.TransactionContext, Is.Not.Null, "Transaction context is null");
                string operationId = model.OperationId.ToString();

                var signResponse = sign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                Assert.That(signResponse.SignedTransaction, Is.Not.Null, "Signed transaction is null");

                var response = api.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var getResponse = api.Operations.GetOperationId(operationId);

                if(wait)
                    WaitForOperationGotCompleteStatus(operationId);
            }
            if (wait)
                WaitForBalance(walletAddress);
        }

        protected static void AddCryptoToWalletWithRecieveTransaction(string walletAddress, string walletKey, bool wait = true)
        {
            var api = new BlockchainApi(BlockchainApi);
            var sign = new BlockchainSign(_currentSettings.Value.BlockchainSign);

            //build send transaction
            api.Balances.PostBalances(walletAddress);
            var model = new BuildSingleTransactionRequest()
            {
                Amount = AMOUT_WITH_FEE,
                AssetId = ASSET_ID,
                FromAddress = EXTERNAL_WALLET,
                IncludeFee = false,
                OperationId = Guid.NewGuid(),
                ToAddress = walletAddress,
                FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT
            };

            var responseTransaction = api.Operations.PostTransactions(model).GetResponseObject();
            string operationId = model.OperationId.ToString();

            var signResponse = sign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

            var response = api.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

            // wait for wallet present in history

            var history = api.Operations.GetTransactionHistorToAddress(walletAddress, "500").GetResponseObject();

            int i = 0;
            while(i++<150 && api.Operations.GetTransactionHistorToAddress(walletAddress, "500").GetResponseObject().Length == 0)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            }

            history = api.Operations.GetTransactionHistorToAddress(walletAddress, "500").GetResponseObject();
            //BuildSingleReceiveTransactionRequest recieve transaction

            var reciveModel = new BuildSingleReceiveTransactionRequest() { operationId = Guid.NewGuid(), sendTransactionHash = history.ToList().First().hash };
            var recieve = api.Operations.PostTranstactionSingleRecieve(reciveModel).GetResponseObject();
            var signReciveResponse = sign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { walletKey }, TransactionContext = recieve.transactionContext}).GetResponseObject();

            var responseRecieve = api.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = reciveModel.operationId, SignedTransaction = signReciveResponse.SignedTransaction });

            if (wait)
                WaitForBalance(walletAddress);
        }

        public static void WaitForOperationGotCompleteStatus(string operationId)
        {
            var api = new BlockchainApi(BlockchainApi);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
            {
                var currentState = api.Operations.GetOperationId(operationId).GetResponseObject().State;
                if (currentState != BroadcastedTransactionState.Completed)
                {
                    if(currentState == BroadcastedTransactionState.Failed)
                        break;
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                else
                    break;
            }
            sw.Stop();
        }

        public static void WaitForBalanceBlockIncreased(string walletAddress, long startBlock)
        {
            var api = new BlockchainApi(BlockchainApi);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
            {
                var currentBlock = api.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == walletAddress)?.Block;
                if (startBlock  < currentBlock)
                    return;
                    
                else
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            sw.Stop();
        }

        public static void WaitForBalance(string wallet)
        {
            var api = new BlockchainApi(BlockchainApi);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
            {
                if (!api.Balances.GetBalances("500", null).GetResponseObject().Items.Any(w => w.Address == wallet))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                else
                    break;
            }
            sw.Stop();
        }

        #endregion
    }
}
