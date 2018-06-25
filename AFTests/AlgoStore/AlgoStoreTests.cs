﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AlgoStoreData.Fixtures;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using AlgoStoreData.DTOs;
using XUnitTestData.Entities.AlgoStore;
using AlgoStoreData.HelpersAlgoStore;
using System.Text.RegularExpressions;
using System.Linq;

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStore")]
    public partial class AlgoStoreTests : AlgoStoreTestDataFixture
    {
        #region Path Variables

        private String isAlivePath = ApiPaths.ALGO_STORE_IS_ALIVE;
        private String metaDataPath = ApiPaths.ALGO_STORE_METADATA;
        private String uploadStringPath = ApiPaths.ALGO_STORE_UPLOAD_STRING;
        private String algoInstanceDataPath = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;
        private String algoGetAllInstanceDataPath = ApiPaths.ALGO_STORE_ALGO_GET_ALL_INSTANCE_DATA;
        private String makeAlgoPublicPath = ApiPaths.ALGO_STORE_ADD_TO_PUBLIC;
        private String makeAlgoPrivatePath = ApiPaths.ALGO_STORE_REMOVE_FROM_PUBLIC;

        #endregion

        [Test]
        [Category("AlgoStore")]
        public async Task CheckIfServiceIsAlive()
        {
            var response = await Consumer.ExecuteRequest(isAlivePath, Helpers.EmptyDictionary, null, Method.GET);

            Assert.That(response.Status , Is.EqualTo(HttpStatusCode.OK));
            var baseDate = JsonUtils.DeserializeJson<IsAliveDTO>(response.ResponseJson).Name;
            Assert.That(baseDate, Is.EqualTo("Lykke.AlgoStore.Api"));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task UploadMetadata()
        {        
            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(8),
                Description = Helpers.RandomString(8)
            };
            
            var response = await Consumer.ExecuteRequest(metaDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status , Is.EqualTo(HttpStatusCode.OK));
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            Assert.That(metadata.Name, Is.EqualTo(responseMetaData.Name));
            Assert.That(metadata.Description, Is.EqualTo(responseMetaData.Description));
            Assert.NotNull(responseMetaData.DateCreated);
            Assert.NotNull(responseMetaData.DateModified);
            Assert.NotNull(responseMetaData.Id);
            Assert.Null(responseMetaData.AlgoVisibility);

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == responseMetaData.Id) as MetaDataEntity;

            Assert.NotNull(metaDataEntity);
            Assert.AreEqual(metaDataEntity.Id, responseMetaData.Id);
            Assert.AreEqual(metaDataEntity.Name, responseMetaData.Name);
            Assert.AreEqual(metaDataEntity.Description, responseMetaData.Description);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task EditMetadata()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(8),
                Description = Helpers.RandomString(8)
            };

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            url = ApiPaths.ALGO_STORE_METADATA;

            EditAlgoDTO editMetaData = new EditAlgoDTO()
            {
                Id = responseMetaData.Id,
                Name = Helpers.RandomString(9),
                Description = Helpers.RandomString(9)
            };

            var responseMetaDataAfterEditRequest = await Consumer.ExecuteRequest(metaDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responseMetaDataAfterEditRequest.Status , Is.EqualTo(HttpStatusCode.OK));
            AlgoDataDTO responseMetaDataAfterEdit = JsonUtils.DeserializeJson<AlgoDataDTO>(responseMetaDataAfterEditRequest.ResponseJson);

            Assert.AreEqual(responseMetaDataAfterEdit.Name, editMetaData.Name);
            Assert.AreEqual(responseMetaDataAfterEdit.Description, editMetaData.Description);
            Assert.NotNull(responseMetaDataAfterEdit.DateCreated);
            Assert.NotNull(responseMetaDataAfterEdit.DateModified);
            Assert.NotNull(responseMetaDataAfterEdit.Id);
            Assert.Null(responseMetaDataAfterEdit.AlgoVisibility);


            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == responseMetaDataAfterEdit.Id) as MetaDataEntity;

            Assert.NotNull(metaDataEntity);
            Assert.AreEqual(metaDataEntity.Id, responseMetaDataAfterEdit.Id);
            Assert.AreEqual(metaDataEntity.Name, responseMetaDataAfterEdit.Name);
            Assert.AreEqual(metaDataEntity.Description, responseMetaDataAfterEdit.Description);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetAllMetadataForClient()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;
            var responceAllClientMetadata = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(responceAllClientMetadata.Status , Is.EqualTo(HttpStatusCode.OK));
            Object responceClientGetAll = JsonUtils.DeserializeJson(responceAllClientMetadata.ResponseJson);
            List<AlgoDataDTO> listAllClinetObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlgoDataDTO>>(responceClientGetAll.ToString());
            List<string> keysPreBuildData = new List<string>();
            DataManager.getAllMetaData().ForEach(e => keysPreBuildData.Add(e.AlgoId));
            int mathcedKeysCounter = 0;
            foreach (AlgoDataDTO currentData in listAllClinetObjects)
            {
                bool Exists = keysPreBuildData.Contains(currentData.Id);
                if (Exists)
                {
                    mathcedKeysCounter++;
                }
            }
            Assert.That(keysPreBuildData.Count , Is.EqualTo(mathcedKeysCounter));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetTailLog()
        {
            List<BuilInitialDataObjectDTO> metadataForUploadedBinaryList = await CreateAlgoAndStartInstance(1);
            BuilInitialDataObjectDTO metadataForUploadedBinary = metadataForUploadedBinaryList[metadataForUploadedBinaryList.Count - 1];

            string AlgoID = metadataForUploadedBinary.AlgoId;

            string url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                { "InstanceId" , metadataForUploadedBinary.InstanceId },
                {"AlgoClientId" , "e658abfc-1779-427c-8316-041a2deb1db8"  },
                {"Tail" , "60" }
            };
            int retryCounter = 0;

            var algoIDTailLogResponse = await Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);

            while ((algoIDTailLogResponse.Status.Equals(HttpStatusCode.InternalServerError) || algoIDTailLogResponse.Status.Equals(HttpStatusCode.NotFound)) && retryCounter <= 30)
            {
                System.Threading.Thread.Sleep(100000);
                algoIDTailLogResponse = await Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
                retryCounter++;
            }

            Assert.That(algoIDTailLogResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task UploadString()
        {
            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await Consumer.ExecuteRequest(metaDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responseMetaData.Id,
                Data = "TEST FOR NOW NOT WORKING ALGO"
            };

            var responsetemp = await Consumer.ExecuteRequest(uploadStringPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            bool blobExists = await BlobRepository.CheckIfBlobExists(stringDTO.AlgoId, BinaryAlgoFileType.STRING);
            Assert.That(blobExists , Is.EqualTo(true));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetUploadedString()
        {
            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await Consumer.ExecuteRequest(metaDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responseMetaData.Id,
                Data =  CSharpAlgoString
            };

            var responsetemp = await Consumer.ExecuteRequest(uploadStringPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", responseMetaData.Id }
            };

            var responceGetUploadString = await Consumer.ExecuteRequest(uploadStringPath, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadString.Status , Is.EqualTo(HttpStatusCode.OK));

            UploadStringDTO uploadedStringContent = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadString.ResponseJson);

            Assert.That(stringDTO.Data, Is.EqualTo(uploadedStringContent.Data));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetAllClientInstanceData()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            //InstanceDataDTO instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(algoID);
            
            var postInstanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);

            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , algoID }
            };

            var responceAllClientInstance = await Consumer.ExecuteRequest(algoGetAllInstanceDataPath, queryParmas, null, Method.GET);
            Assert.That(responceAllClientInstance.Status , Is.EqualTo(HttpStatusCode.OK));
            Object responceClientGetAll = JsonUtils.DeserializeJson(responceAllClientInstance.ResponseJson);
            List<InstanceDataDTO> listAllClinetObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InstanceDataDTO>>(responceClientGetAll.ToString());
            int mathcedKeysCounter = 0;
            foreach (InstanceDataDTO currentData in listAllClinetObjects)
            {
                bool Exists = currentData.InstanceId.Equals(postInstanceData.InstanceId);
                if (Exists)
                {
                    mathcedKeysCounter++;
                }
            }
            Assert.That(mathcedKeysCounter , Is.EqualTo(1));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task PostInstanceDataForAlgo()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            //InstanceDataDTO instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(algoID);

            var postInstanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo((instanceForAlgo.AlgoId)));
            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));


            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetInstanceDataForAlgo()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            //InstanceDataDTO instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(algoID);

            var postInstanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , postInstanceData.AlgoId },
                { "instanceId", postInstanceData.InstanceId}
            };

            var getInstanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, queryParmas, null, Method.GET);
            Assert.That(getInstanceDataResponse.Status == HttpStatusCode.OK);
            InstanceDataDTO getInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(getInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task EditInstanceDataForAlgo()
        {

            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            //InstanceDataDTO instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(algoID);

            var postInstanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
            InstanceDataDTO instanceForAlgoEdit = postInstanceData;
            postInstanceData.InstanceName = "EditedTest";

            var postInstanceDataResponseEdit = await Consumer.ExecuteRequest(algoInstanceDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgoEdit), Method.POST);

            Assert.That(postInstanceDataResponseEdit.Status , Is.EqualTo(HttpStatusCode.OK));
            InstanceDataDTO postInstanceDataEdit = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponseEdit.ResponseJson);
            Assert.That(postInstanceDataEdit.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceDataEdit.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceDataEdit.InstanceId, Is.EqualTo(instanceDataEntityExists.Id));
            Assert.That(postInstanceDataEdit.InstanceName, Is.EqualTo("EditedTest"));

            ClientInstanceEntity instanceDataEntityExistsEdit = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceDataEdit.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExistsEdit);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetInstanceData()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            //InstanceDataDTO instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(algoID);

            var postInstanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);

            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , algoID },
                { "instanceId", postInstanceData.InstanceId}
            };

            var instanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, queryParmas, null, Method.GET);
            Assert.That(instanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO returnedClinetInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(instanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task ClientDataGetAllAlgos()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            string url = ApiPaths.ALGO_STORE_ADD_TO_PUBLIC;

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == algoID) as MetaDataEntity;
            Assert.NotNull(metaDataEntity);

            AddToPublicDTO addAlgo = new AddToPublicDTO()
            {
                AlgoId = algoID,
                ClientId = metaDataEntity.PartitionKey
            };

            var addAlgoToPublicEndpoint = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(addAlgo), Method.POST);
            Assert.That(addAlgoToPublicEndpoint.Status, Is.EqualTo(HttpStatusCode.OK));

            url = ApiPaths.ALGO_STORE_CLIENT_DATA_GET_ALL_ALGOS;

            var clientDataAllAlgos = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(clientDataAllAlgos.Status, Is.EqualTo(HttpStatusCode.OK));

            Object responceclientDataAllAlgos = JsonUtils.DeserializeJson(clientDataAllAlgos.ResponseJson);
            List<AlgoDTO> listAllAlgos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlgoDTO>>(responceclientDataAllAlgos.ToString());
            AlgoDTO expectedAlgoDTO = listAllAlgos.FindLast(t => t.Id.Equals(algoID));
            string AlgoIdFromGettAllAlgos = expectedAlgoDTO.Id;
            Assert.That(algoID, Is.EqualTo(AlgoIdFromGettAllAlgos));
            foreach (AlgoDTO algo in listAllAlgos)
            {
                Assert.Zero(expectedAlgoDTO.Rating);
                Assert.NotZero(expectedAlgoDTO.UsersCount);
                Assert.NotNull(expectedAlgoDTO.Id);
                Assert.NotNull(expectedAlgoDTO.Name);
                Assert.NotNull(expectedAlgoDTO.Description);
                Assert.NotNull(expectedAlgoDTO.Date);
                Assert.NotNull(expectedAlgoDTO.Author);
            }
        }
       
        [Category("AlgoStore")]
        [TestCase("")]
        [TestCase("getFromData")]
        public async Task GetAlgoMetaData(string clientIdTemp)
        {
            List<BuilInitialDataObjectDTO> metadataForUploadedBinaryList = await CreateAlgoAndStartInstance(1);
            BuilInitialDataObjectDTO metadataForUploadedBinary = metadataForUploadedBinaryList[metadataForUploadedBinaryList.Count - 1];

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == metadataForUploadedBinary.AlgoId) as MetaDataEntity;

            if (clientIdTemp.Equals("getFromData"))
            {
                clientIdTemp = metaDataEntity.PartitionKey;
            }

            string url = ApiPaths.ALGO_STORE_GET_ALGO_METADATA;

            Dictionary<string, string> quaryParamAlgoData = new Dictionary<string, string>()
            {
                {"AlgoId",  metadataForUploadedBinary.AlgoId },
                {"clientId", clientIdTemp}
            };

            var responceAlgoMetadata = await Consumer.ExecuteRequest(url, quaryParamAlgoData, null, Method.GET);
            Assert.That(responceAlgoMetadata.Status , Is.EqualTo(HttpStatusCode.OK));

            GetAlgoMetaDataDTO postInstanceData = JsonUtils.DeserializeJson<GetAlgoMetaDataDTO>(responceAlgoMetadata.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo(metadataForUploadedBinary.AlgoId));
            Assert.That(postInstanceData.Name, Is.EqualTo(metadataForUploadedBinary.Name));
            Assert.That(postInstanceData.Description, Is.EqualTo(metadataForUploadedBinary.Description));
            Assert.That(postInstanceData.Date, Is.Not.Null);
            Assert.That(postInstanceData.Author, Is.Not.Null);
            Assert.That(postInstanceData.Rating, Is.Zero);
            Assert.That(postInstanceData.UsersCount, Is.Not.Zero);
            Assert.That(postInstanceData.AlgoMetaDataInformation, Is.Null);
        }
        [Test]
        [Category("AlgoStore")]
        public async Task DeployStringAlgo()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
             {
                 Name = Helpers.RandomString(13),
                 Description = Helpers.RandomString(13)
             };
        
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
                {
                    AlgoId = responseMetaData.Id,
                    Data = CSharpAlgoString
                };

            var responsetemp = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.That(responsetemp.Status, Is.EqualTo(HttpStatusCode.NoContent));

            //InstanceDataDTO instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(stringDTO.AlgoId);

            url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            DeployBinaryDTO deploy = new DeployBinaryDTO()
                {
                    AlgoId = stringDTO.AlgoId,
                    InstanceId = postInstanceData.InstanceId,
                };

            var deployBynaryResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deploy), Method.POST);
            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            int retryCounter = 0;

            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
                {
                    AlgoId = postInstanceData.AlgoId,
                    InstanceId = postInstanceData.InstanceId
                };
            var responceCascadeDelete = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);

           // Currently we can not send cascade delete to kubernatees if he has not build the algo before that thorus not found and we leave data

           while (responceCascadeDelete.Status.Equals(System.Net.HttpStatusCode.NotFound) && retryCounter <= 30)
                {
                    System.Threading.Thread.Sleep(10000);
                    responceCascadeDelete = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
                    retryCounter++;
                }

           Assert.That(responceCascadeDelete.Status, Is.EqualTo(HttpStatusCode.NoContent));

            }

        [Test]
        [Category("AlgoStore")]
        public async Task StopAlgo()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responseMetaData.Id,
                Data = CSharpAlgoString
            };

            var responsetemp = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.That(responsetemp.Status, Is.EqualTo(HttpStatusCode.NoContent));

            //InstanceDataDTO instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(stringDTO.AlgoId);

            url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            DeployBinaryDTO deploy = new DeployBinaryDTO()
            {
                AlgoId = stringDTO.AlgoId,
                InstanceId = postInstanceData.InstanceId,
            };

            var deployBynaryResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deploy), Method.POST);
            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            url = ApiPaths.ALGO_STORE_ALGO_STOP;
            int retryCounter = 0;

            StopBinaryDTO stopAlgo = new StopBinaryDTO()
            {
                AlgoId = postInstanceData.AlgoId,
                InstanceId = postInstanceData.InstanceId
            };
            var responceCascadeDelete = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);

            StopBinaryResponseDTO stopAlgoResponce = JsonUtils.DeserializeJson<StopBinaryResponseDTO>(responceCascadeDelete.ResponseJson); ;

            while ((stopAlgoResponce.Status.Equals("Deploying") || stopAlgoResponce.Status.Equals("Started")) && retryCounter <= 30)
            {
                System.Threading.Thread.Sleep(10000);
                responceCascadeDelete = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);

                stopAlgoResponce = JsonUtils.DeserializeJson<StopBinaryResponseDTO>(responceCascadeDelete.ResponseJson);

                retryCounter++;
            }

            Assert.That(responceCascadeDelete.Status, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(stopAlgoResponce.Status, Is.EqualTo("Stopped"));

            ClientInstanceEntity algoInstanceEntitiy = await ClientInstanceRepository.TryGetAsync(t => t.PartitionKey == "algo_"+stringDTO.AlgoId) as ClientInstanceEntity;

            Assert.That(algoInstanceEntitiy.AlgoInstanceStatusValue, Is.EqualTo("Stopped"));

        }

        private async Task<UploadStringDTO> UploadStringAlgo()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);


            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responseMetaData.Id,
                Data = CSharpAlgoString
            };

            var responsetemp = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            return stringDTO;
        }

        [Test, Description("AL-363")]
        [Category("AlgoStore")]
        public async Task CheckWalletBalanceCalculatedBasedOnBestPrice()
        {
            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , postInstanceData.AlgoId },
                { "instanceId", postInstanceData.InstanceId}
            };

            var instanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, queryParmas, null, Method.GET);
            Assert.That(instanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            // Get expected values from Azure
            AlgoInstanceStatisticsEntity algoInstanceStatisticsEntity = await AlgoInstanceStaticsticsRepository.TryGetAsync(t => t.InstanceId == postInstanceData.InstanceId && t.Id == "Summary") as AlgoInstanceStatisticsEntity;

            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            Dictionary<string, string> statisticsQueryParams = new Dictionary<string, string>()
            {
                { "instanceId", postInstanceData.InstanceId}
            };

            // Get actual values from statistics endpoint 6 times within 1 minute
            StatisticsDTO statistics = null;
            for (int i = 0; i < 6; i++)
            {
                statistics = await AlgoStoreCommonSteps.GetStatisticsResponseAsync(Consumer, postInstanceData);
            }

            Assert.That(algoInstanceStatisticsEntity.InstanceId, Is.EqualTo(statistics.InstanceId));
            Assert.That(algoInstanceStatisticsEntity.TotalNumberOfTrades, Is.LessThanOrEqualTo(statistics.TotalNumberOfTrades));
            Assert.That(algoInstanceStatisticsEntity.TotalNumberOfStarts, Is.LessThanOrEqualTo(statistics.TotalNumberOfStarts));
            Assert.That(algoInstanceStatisticsEntity.InitialWalletBalance, Is.EqualTo(statistics.InitialWalletBalance));
            Assert.That(algoInstanceStatisticsEntity.LastWalletBalance, Is.Not.EqualTo(statistics.LastWalletBalance));
            Assert.That(algoInstanceStatisticsEntity.AssetOneBalance, Is.EqualTo(statistics.AssetOneBalance));
            Assert.That(algoInstanceStatisticsEntity.AssetTwoBalance, Is.EqualTo(statistics.AssetTwoBalance));
            Assert.That(algoInstanceStatisticsEntity.UserCurrencyBaseAssetId, Is.EqualTo(statistics.UserCurrencyBaseAssetId));
            //Assert.That(statistics.NetProfit, Is.EqualTo(statistics.LastWalletBalance - algoInstanceStatisticsEntity.InitialWalletBalance));
        }

        [Test, Description("AL-357")]
        [Category("AlgoStore")]
        public async Task CheckSummaryRowUpdatedWhenAlogIsStopped()
        {
            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);

            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , postInstanceData.AlgoId },
                { "instanceId", postInstanceData.InstanceId}
            };

            var instanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, queryParmas, null, Method.GET);
            Assert.That(instanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            // Get initial statistics values from Azure
            AlgoInstanceStatisticsEntity initialAlgoInstanceStatisticsEntity = await AlgoInstanceStaticsticsRepository.TryGetAsync(t => t.InstanceId == postInstanceData.InstanceId && t.Id == "Summary") as AlgoInstanceStatisticsEntity;

            // Assert LastTradedAssetBalance and LastAssetTwoBalance equal InitialTradedAssetBalance and InitialAssetTwoBalance
            // Before an algo is started, LastTradedAssetBalance and LastAssetTwoBalance should be equal to InitialTradedAssetBalance and InitialAssetTwoBalance
            Assert.That(initialAlgoInstanceStatisticsEntity.InitialTradedAssetBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.LastTradedAssetBalance));
            Assert.That(initialAlgoInstanceStatisticsEntity.InitialAssetTwoBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.LastAssetTwoBalance));

            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            // Get actual values from statistics endpoint 6 times within 1 minute
            StatisticsDTO statistics;
            for (int i = 0; i < 6; i++)
            {
                statistics = await AlgoStoreCommonSteps.GetStatisticsResponseAsync(Consumer, postInstanceData);
            }

            // Wait for 5 seconds before getting statistics values from Azure
            Wait.ForPredefinedTime(5000);

            // Get updated statistics values from Azure
            AlgoInstanceStatisticsEntity updatedAlgoInstanceStatisticsEntity = await AlgoInstanceStaticsticsRepository.TryGetAsync(t => t.InstanceId == postInstanceData.InstanceId && t.Id == "Summary") as AlgoInstanceStatisticsEntity;

            // Assert updated LastTradedAssetBalance and LastAssetTwoBalance does not equal InitialTradedAssetBalance and InitialAssetTwoBalance
            // In algo is not stopped, invoking statistics endpoint should update LastTradedAssetBalance and LastAssetTwoBalance
            Assert.That(updatedAlgoInstanceStatisticsEntity.LastTradedAssetBalance, Is.Not.EqualTo(updatedAlgoInstanceStatisticsEntity.InitialTradedAssetBalance));
            Assert.That(updatedAlgoInstanceStatisticsEntity.LastAssetTwoBalance, Is.Not.EqualTo(updatedAlgoInstanceStatisticsEntity.InitialAssetTwoBalance));
            // Assert InitialTradedAssetBalance and InitialAssetTwoBalance are not updated after invoking statistics endpoint
            Assert.That(updatedAlgoInstanceStatisticsEntity.InitialTradedAssetBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.InitialTradedAssetBalance));
            Assert.That(updatedAlgoInstanceStatisticsEntity.InitialAssetTwoBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.InitialAssetTwoBalance));

            // Stop the algo instance
            await AlgoStoreCommonSteps.StopAlgoInstance(Consumer, postInstanceData);

            // Get statistics values from Azure after the algo is stopped and before calling statistics endpoint
            AlgoInstanceStatisticsEntity afterStoppingAlgoInstanceStatisticsEntity = await AlgoInstanceStaticsticsRepository.TryGetAsync(t => t.InstanceId == postInstanceData.InstanceId && t.Id == "Summary") as AlgoInstanceStatisticsEntity;

            // Assert after stopping LastTradedAssetBalance and LastAssetTwoBalance does not equal updated LastTradedAssetBalance and LastAssetTwoBalance
            // After stopping an algo, statistics should be updated with the current values
            Assert.That(afterStoppingAlgoInstanceStatisticsEntity.LastTradedAssetBalance, Is.Not.EqualTo(updatedAlgoInstanceStatisticsEntity.LastTradedAssetBalance));
            Assert.That(afterStoppingAlgoInstanceStatisticsEntity.LastAssetTwoBalance, Is.Not.EqualTo(updatedAlgoInstanceStatisticsEntity.LastAssetTwoBalance));
            // Assert InitialTradedAssetBalance and InitialAssetTwoBalance are not updated after stoppong the algo
            Assert.That(afterStoppingAlgoInstanceStatisticsEntity.InitialTradedAssetBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.InitialTradedAssetBalance));
            Assert.That(afterStoppingAlgoInstanceStatisticsEntity.InitialAssetTwoBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.InitialAssetTwoBalance));

            // Wait for a minute and invoke statistics endpoint again
            statistics = await AlgoStoreCommonSteps.GetStatisticsResponseAsync(Consumer, postInstanceData, 60000);

            // Wait for 5 seconds before getting statistics values from Azure
            Wait.ForPredefinedTime(5000);

            // Get statistics values from Azure after calling statistics endpoint
            AlgoInstanceStatisticsEntity finalAlgoInstanceStatisticsEntity = await AlgoInstanceStaticsticsRepository.TryGetAsync(t => t.InstanceId == postInstanceData.InstanceId && t.Id == "Summary") as AlgoInstanceStatisticsEntity;

            // Assert final LastTradedAssetBalance and LastAssetTwoBalance equal LastTradedAssetBalance and LastAssetTwoBalance from after stopping the algo
            // After stopping an algo, invoking statistics endpoint should not update LastTradedAssetBalance and LastAssetTwoBalance
            Assert.That(finalAlgoInstanceStatisticsEntity.LastTradedAssetBalance, Is.EqualTo(afterStoppingAlgoInstanceStatisticsEntity.LastTradedAssetBalance));
            Assert.That(finalAlgoInstanceStatisticsEntity.LastAssetTwoBalance, Is.EqualTo(afterStoppingAlgoInstanceStatisticsEntity.LastAssetTwoBalance));
            // Assert InitialTradedAssetBalance and InitialAssetTwoBalance are not updated after stoppong the algo
            Assert.That(finalAlgoInstanceStatisticsEntity.InitialTradedAssetBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.InitialTradedAssetBalance));
            Assert.That(finalAlgoInstanceStatisticsEntity.InitialAssetTwoBalance, Is.EqualTo(initialAlgoInstanceStatisticsEntity.InitialAssetTwoBalance));
        }

        [Test, Description("AL-379")]
        [Category("AlgoStore")]
        public async Task CheckStatisticsReturnsTradedAssetAndAssetTwoNames()
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>()
            {
                { "algoId" , postInstanceData.AlgoId },
                { "instanceId", postInstanceData.InstanceId}
            };

            var instanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, queryParams, null, Method.GET);
            Assert.That(instanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            StatisticsDTO statistics = await AlgoStoreCommonSteps.GetStatisticsResponseAsync(Consumer, postInstanceData);

            // TODO: Update the test to use dynamic TradedAsset and AssetTwo
            // Assert statistics endpoint returns "TradedAssetName" and "AssetTwoName"
            Assert.That(statistics.TradedAssetName, Is.EqualTo("EUR"));
            Assert.That(statistics.AssetTwoName, Is.EqualTo("BTC"));

            // Stop the algo instance
            await AlgoStoreCommonSteps.StopAlgoInstance(Consumer, postInstanceData);
        }

        [Test, Description("AL-488")]
        [Category("AlgoStore")]
        public async Task CheckTokenGeneratedOnInstanceCreation()
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>()
            {
                { "algoId" , postInstanceData.AlgoId },
                { "instanceId", postInstanceData.InstanceId}
            };

            var instanceDataResponse = await Consumer.ExecuteRequest(algoInstanceDataPath, queryParams, null, Method.GET);
            Assert.That(instanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            // Get Algo Instance Data from DB
            ClientInstanceEntity instanceDataFromDB = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;

            // Assert AuthToken is not null
            Assert.That(instanceDataFromDB.AuthToken, Is.Not.Null, "AuthToken is null whereas it should not be null");

            // Assert that AuthToken is Giud
            Regex regex = new Regex(GlobalConstants.GuidRegexPattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(instanceDataFromDB.AuthToken);
            string errorMessage = $"The AuthToken is not a Guid. Value: '{instanceDataFromDB.AuthToken}'";
            Assert.That(match.Success, Is.True, errorMessage);

            // Stop the algo instance
            await AlgoStoreCommonSteps.StopAlgoInstance(Consumer, postInstanceData);
        }

        [Test, Description("AL-467")]
        [Category("AlgoStore")]
        public async Task CheckGetAllUserAlgos()
        {
            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            // Get all my algoes from Azure
            List<AlgoEntity> expectedResult = await AlgoRepository.GetAllAsync(t => t.ClientId == postInstanceData.AlgoClientId) as List<AlgoEntity>;

            // Get my algos from service
            var myAlgos = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_GET_MY_ALGOS, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(myAlgos.Status, Is.EqualTo(HttpStatusCode.OK));
            List<AlgoDataDTO> actualResult = JsonUtils.DeserializeJson<List<AlgoDataDTO>>(myAlgos.ResponseJson);

            // Assert expected result and actual result counts are the same
            Assert.That(expectedResult.Count, Is.EqualTo(actualResult.Count));

            // Sort actual and expected algos by algo id
            expectedResult.Sort((x, y) => x.AlgoId.CompareTo(y.AlgoId));
            actualResult.Sort((x, y) => x.Id.CompareTo(y.Id));

            // Assert actual algos are the same as expected algos
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.That(expectedResult[i].AlgoId, Is.EqualTo(actualResult[i].Id));
                Assert.That(expectedResult[i].ClientId, Is.EqualTo(actualResult[i].ClientId));
                Assert.That(expectedResult[i].Name, Is.EqualTo(actualResult[i].Name));
                Assert.That(expectedResult[i].Description, Is.EqualTo(actualResult[i].Description));
                Assert.That(expectedResult[i].AlgoVisibilityValue, Is.EqualTo(actualResult[i].AlgoVisibility.ToString()));
                Assert.That(expectedResult[i].DateCreated.ToString("s"), Is.EqualTo(actualResult[i].DateCreated.ToString("s")));
            }

            // Stop the algo instance
            await AlgoStoreCommonSteps.StopAlgoInstance(Consumer, postInstanceData);
        }

        [Test, Description("AL-520")]
        [Category("AlgoStore")]
        public async Task CheckGetAllAvailableWallets()
        {
            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            // Get all running Live instances of the user
            List<ClientInstanceEntity> allRunningInstances = await ClientInstanceRepository.GetAllAsync(t => t.ClientId == postInstanceData.AlgoClientId && t.AlgoInstanceTypeValue == "Live" && t.AlgoInstanceStatusValue != "Stopped") as List<ClientInstanceEntity>;
            
            // Get unique walletIds from all running instances
            var uniqueUsedWallets = (from i in allRunningInstances select i.WalletId).Distinct().ToList();

            // Get expected results
            var expectedResult = (await GetAllWalletsOfUser()).Where(t => !uniqueUsedWallets.Contains(t.Id)).ToList();

            // Get actual results from the service
            var myAlgos = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_GET_AVAILABLE_WALLETS, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(myAlgos.Status, Is.EqualTo(HttpStatusCode.OK));
            List<AvailableWalletDTO> actualResult = JsonUtils.DeserializeJson<List<AvailableWalletDTO>>(myAlgos.ResponseJson);

            // Assert expected result and actual result counts are the same
            Assert.That(expectedResult.Count, Is.EqualTo(actualResult.Count));

            // Sort actual and expected wallets by wallet id
            expectedResult.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualResult.Sort((x, y) => x.Id.CompareTo(y.Id));

            // Assert actual wallets are the same as expected wallets
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.That(expectedResult[i].Id, Is.EqualTo(actualResult[i].Id));
                Assert.That(expectedResult[i].Name, Is.EqualTo(actualResult[i].Name));
            }

            // Stop the algo instance
            await AlgoStoreCommonSteps.StopAlgoInstance(Consumer, postInstanceData);
        }

        [Test, Description("AL-307")]
        [Category("AlgoStore")]
        public async Task CheckAlgoDeletion()
        {
            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            // Delete the algo
            var deleteAlgoRequest = await DeleteAlgo(postInstanceData, false);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "Cannot delete algo because it has algo instances.");

            // Force delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, true);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "Cannot delete algo because it has running algo instances.");

            // Delete the instance
            await AlgoStoreCommonSteps.DeleteAlgoInstance(Consumer, postInstanceData);

            // Make the algo public
            await AlgoStoreCommonSteps.MakeAlgoPublic(Consumer, postInstanceData);

            // Delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, false);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "The algo must not be public.");

            // Force delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, true);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "The algo must not be public.");

            // Create an instance
            InstanceDataDTO instanceData = await SaveInstance(algoData, instanceType);
            postInstanceData = instanceData;
            await DeployInstance(instanceData);

            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            // Delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, false);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "The algo must not be public.");

            // Force delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, true);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "The algo must not be public.");

            // Stop the instance
            await AlgoStoreCommonSteps.StopAlgoInstance(Consumer, postInstanceData);

            // Delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, false);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "The algo must not be public.");

            // Force delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, true);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "The algo must not be public.");

            // Delete the instance
            await AlgoStoreCommonSteps.DeleteAlgoInstance(Consumer, postInstanceData);

            // Make the algo private
            await AlgoStoreCommonSteps.MakeAlgoPrivate(Consumer, postInstanceData);

            // Create an instance
            instanceData = await SaveInstance(algoData, instanceType);
            postInstanceData = instanceData;
            await DeployInstance(instanceData);

            // Wait up to 3 minutes for the algo to be started
            await AlgoStoreCommonSteps.WaitAlgoToStart(ClientInstanceRepository, postInstanceData);

            // Stop the instance
            await AlgoStoreCommonSteps.StopAlgoInstance(Consumer, postInstanceData);

            // Delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, false);
            await AssertAlgoNotDeleted(deleteAlgoRequest, postInstanceData, "Cannot delete algo because it has algo instances.");

            // Force delete the algo
            deleteAlgoRequest = await DeleteAlgo(postInstanceData, true);
            await AssertAlgoDeleted(deleteAlgoRequest, postInstanceData);
        }
    }
}
