﻿using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace XUnitTestCommon
{
    public class ApiPaths
    {
        // API V2
        public static readonly string WALLETS_BASE_PATH = "/api/wallets";
        public static readonly string WALLETS_BALANCES_PATH = "/api/wallets/balances";
        public static readonly string WALLETS_TRADING_BALANCES_PATH = "/api/wallets/trading/balances";
        private static readonly string walletBalanceByAssetID = "/api/wallets/{0}/balances/{1}";

        public static readonly string OPERATIONS_BASE_PATH = "/api/operations";
        public static readonly string OPERATIONS_DETAILS_PATH = "/api/operationsDetails";
        public static readonly string OPERATIONS_DETAILS_CREATE_PATH = "/api/operationsDetails/create";
        public static readonly string OPERATIONS_DETAILS_REGISTER_PATH = "/api/operationsDetails/register";
        public static readonly string OPERATIONS_TRANSFER_PATH = "/api/operations/transfer";
        public static readonly string OPERATIONS_CANCEL_PATH = "/api/operations/cancel";

        public static readonly string HFT_BASE_PATH = "/api/hft";

        public static readonly string CLIENT_BASE_PATH = "/api/client";
        public static readonly string CLIENT_REGISTER_PATH = "/api/client/register";
        public static readonly string CLIENT_INFO_PATH = "/api/client/userInfo";

        public static readonly string ASSETS_BASE_PATH = "/api/assets";
        public static readonly string ASSETS_BASEASSET_PATH = "/api/assets/baseAsset";

        public static readonly string ISALIVE_BASE_PATH = "/api/isAlive";        

        public static readonly string TRANSACTION_HISTORY_BASE_PATH = "/api/transactionHistory";

        // blue API
        public static readonly string PLEDGES_BASE_PATH = "/api/pledges";

        public static readonly string TWITTER_BASE_PATH = "/api/twitter/getTweetsJSON";

        public static readonly string REFERRAL_LINKS_PATH = "/api/referralLinks";
        public static readonly string REFERRAL_LINKS_INVITATION_PATH = "/api/referralLinks/invitation";

        // API V2 prefix
        public static readonly string ASSETS_V2_BASE_PATH = "/api/v2/assets";
        public static readonly string ASSETS_DEFAULT_PATH = "/api/v2/assets/default";
        public static readonly string ASSET_ATTRIBUTES_PATH = "/api/v2/asset-attributes";
        public static readonly string ASSET_CATEGORIES_PATH = "/api/v2/asset-categories";
        public static readonly string ASSET_EXTENDED_INFO_PATH = "/api/v2/asset-extended-infos";
        public static readonly string ASSET_GROUPS_PATH = "/api/v2/asset-groups";
        public static readonly string ASSET_PAIRS_PATH = "/api/v2/asset-pairs";
        public static readonly string ASSET_SETTINGS_PATH = "/api/v2/asset-settings";
        public static readonly string ISSUERS_BASE_PATH = "/api/v2/issuers";

        public static readonly string CLIENTS_BASE_PATH = "/api/v2/clients";

        public static readonly string MARGIN_ASSET_BASE_PATH = "/api/v2/margin-assets";
        public static readonly string MARGIN_ASSET_PAIRS_PATH = "/api/v2/margin-asset-pairs";
        public static readonly string MARGIN_ISSUERS_PATH = "/api/v2/margin-issuers";

        public static readonly string WATCH_LIST_BASE_PATH = "/api/v2/watch-lists";
        public static readonly string WATCH_LIST_PREDEFINED_PATH = "/api/v2/watch-lists/predefined";
        public static readonly string WATCH_LIST_CUSTOM_PATH = "/api/v2/watch-lists/custom";
        public static readonly string WATCH_LIST_ALL_PATH = "/api/v2/watch-lists/all";

        public static readonly string ERC20TOKENS_BASE_PATH = "/api/v2/erc20-tokens";
      
        //client account service
        public static readonly string CLIENT_ACCOUNT_SERVICE_PREFIX = "client-account";
        public static readonly string CLIENT_ACCOUNT_SERVICE_BASEURL = "lykke-service.svc.cluster.local";
        public static readonly string CLIENT_ACCOUNT_PATH = "/api/ClientAccount";


        //balances service
        public static readonly string BALANCES_IS_ALIVE = "/api/IsAlive";
        public static readonly string BALANCES_WALLET_CREDENTIAL = "/api/WalletCredential";
        public static readonly string BALANCES_WALLET_CREDENTIALS_HISTORY = "/api/WalletCredentialsHistory";
        public static readonly string BALANCES_WALLET_BALANCES = "/api/WalletsClientBalances";


        //Algo Store
        public static readonly string ALGO_STORE_IS_ALIVE = "/api/IsAlive";
        public static readonly string ALGO_STORE_METADATA = "/api/v1/clientData/metadata";
        public static readonly string ALGO_STORE_GET_ALGO_METADATA = "/api/v1/clientData/algoMetadata";
        public static readonly string ALGO_STORE_CASCADE_DELETE = "/api/v1/clientData/metadata/cascadeDelete";
        public static readonly string ALGO_STORE_UPLOAD_BINARY = "/api/v1/clientData/imageData/upload/binary";
        public static readonly string ALGO_STORE_UPLOAD_STRING = "/api/v1/clientData/imageData/upload/string";
        public static readonly string ALGO_STORE_DEPLOY_BINARY = "/api/v1/management/deploy/binary";
        public static readonly string ALGO_STORE_ALGO_START = "/api/v1/management/test/start";
        public static readonly string ALGO_STORE_ALGO_STOP = "/api/v1/management/test/stop";
        public static readonly string ALGO_STORE_ALGO_LOG = "/api/v1/management/test/log";
        public static readonly string ALGO_STORE_ALGO_TAIL_LOG = "/api/v1/management/test/tailLog";
        public static readonly string ALGO_STORE_ALGO_GET_ALL_INSTANCE_DATA = "/api/v1/clientData/instanceData/allByAlgoIdAndClientId";
        public static readonly string ALGO_STORE_ALGO_INSTANCE_DATA = "/api/v1/clientData/instanceData";
        public static readonly string ALGO_STORE_CLIENT_DATA_GET_ALL_ALGOS = "/api/v1/clientData/getAllAlgos";
        public static readonly string ALGO_STORE_ADD_TO_PUBLIC = "/api/v1/clientData/addToPublic";
        public static readonly string ALGO_STORE_STATISTICS = "/api/v1/statistics";

        #region Algo Store User Permissions

        public static readonly string ALGO_STORE_USER_PERMISSIONS_GET_ALL_PERMSSIONS = "/api/v1/permissions/getAll";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_GET_PERMISSIONS_BY_ID = "/api/v1/permissions/getById";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_GET_PERMISSIONS_BY_ROLE_ID = "/api/v1/permissions/getByRoleId";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_SAVE_PERMISSION = "/api/v1/permissions/savePermission";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_ASSIGN_PERMISSION = "/api/v1/permissions/assignPermission";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_ASSIGN_PERMISSIONS = "/api/v1/permissions/assignPermissions";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_REVOKE_PERMISSION = "/api/v1/permissions/revokePermission";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_REVOKE_PERMISSIONS = "/api/v1/permissions/revokePermissions";
        public static readonly string ALGO_STORE_USER_PERMISSIONS_DELETE_PERMISSION = "/api/v1/permissions/deletePermission";

        #endregion

        #region Algo Store User Roles

        public static readonly string ALGO_STORE_USER_ROLES_GET_ALL_ROLES = "/api/v1/roles/getAll";
        public static readonly string ALGO_STORE_USER_ROLES_GET_ROLE_BY_ID = "/api/v1/roles/getById";
        public static readonly string ALGO_STORE_USER_ROLES_GET_ROLE_BY_CLIENT_ID = "/api/v1/roles/getByClientId";
        public static readonly string ALGO_STORE_USER_ROLES_SAVE_ROLE = "/api/v1/roles/saveRole";
        public static readonly string ALGO_STORE_USER_ROLES_ASSIGN_ROLE = "/api/v1/roles/assignRole";
        public static readonly string ALGO_STORE_USER_ROLES_REVOKE_ROLE = "/api/v1/roles/revokeRole";
        public static readonly string ALGO_STORE_USER_ROLES_VERIFY_ROLE = "/api/v1/roles/verifyRole";
        public static readonly string ALGO_STORE_USER_ROLES_DELETE_ROLE = "/api/v1/roles/deleteRole";

        #endregion

        public static String WALLET_BALANCE_BY_ASSET_ID(string walletId, string assetId)
        {
            return String.Format(walletBalanceByAssetID, walletId, assetId);
        }
    }
}
