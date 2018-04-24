using AlgoStoreData.DependancyInjection;
using AlgoStoreData.DTOs;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.AlgoStore;
using XUnitTestData.Entities.AlgoStore;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.AlgoStore;

namespace AlgoStoreData.Fixtures
{
    [TestFixture]
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        private ConfigBuilder _configBuilder;
        private ConfigBuilder _configBuilderDB;
        private TimeSpan timespan = TimeSpan.FromSeconds(120);
        public ApiConsumer Consumer;
        private OAuthConsumer User;
        private IContainer _container;
        private IContainer _containerDB;
        public GenericRepository<MetaDataEntity, IMetaData> MetaDataRepository;
        public GenericRepository<RuntimeDataEntity, IStatistics> RuntimeDataRepository;
        public GenericRepository<ClientInstanceEntity, IClientInstance> ClientInstanceRepository;
        public GenericRepository<AlgoRatingsTableEntity, IAlgoRatingsTable> AlgoRatingsRepository;
        public GenericRepository<AlgoStoreApiLogEntity, IAlgoStoreApiLog> AlgoApiLogRepository;
        public GenericRepository<CSharpAlgoTemplateLogEntity, ICSharpAlgoTemplateLog> CSharpAlgoTemplateLogRepository;
        public GenericRepository<CSharpAlgoTemplateUserLogEntity, ICSharpAlgoTemplateUserLog> CSharpAlgoTemplateUserLogRepository;
        public GenericRepository<PublicsAlgosTableEntity, IPublicAlgosTable> PublicAlgosRepository;
        public GenericRepository<StatisticsEntity, IStatisticss> StatisticsRepository;
        public GenericRepository<AlgoInstanceStatisticsEntity, IAlgoInstanceStatistics> AlgoInstanceStaticsticsRepository;
        public GenericRepository<UserPermissionsEntity, IUserPermissions> UserPermissionsRepository;
        public List<BuilInitialDataObjectDTO> PreStoredMetadata;
        public AlgoBlobRepository BlobRepository;
        public static string CSharpAlgoStringFile = Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "AlgoStore" + Path.DirectorySeparatorChar, "TestData" + Path.DirectorySeparatorChar, "DummyAlgo.txt");
        public string CSharpAlgoString = File.ReadAllText(CSharpAlgoStringFile);
        protected InstanceDataDTO instanceForAlgo;
        protected InstanceDataDTO postInstanceData;


        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("AlgoStore");

            _configBuilderDB = new ConfigBuilder("AlgoApi", "Db");

            User = new OAuthConsumer(_configBuilder);
            Consumer = new ApiConsumer(_configBuilder, User);

            PrepareDependencyContainer();
            PrepareTestData().Wait();
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AlgoStoreTestModule(_configBuilder));
            _container = builder.Build();


            var builderDB = new ContainerBuilder();
            builderDB.RegisterModule(new AlgoStoreTestModule(_configBuilderDB));
            _containerDB = builderDB.Build();

            MetaDataRepository = RepositoryUtils.ResolveGenericRepository<MetaDataEntity, IMetaData>(this._containerDB);
            RuntimeDataRepository = RepositoryUtils.ResolveGenericRepository<RuntimeDataEntity, IStatistics>(this._containerDB);
            ClientInstanceRepository = RepositoryUtils.ResolveGenericRepository<ClientInstanceEntity, IClientInstance>(this._containerDB);
            AlgoRatingsRepository = RepositoryUtils.ResolveGenericRepository<AlgoRatingsTableEntity, IAlgoRatingsTable>(this._containerDB);
            AlgoApiLogRepository = RepositoryUtils.ResolveGenericRepository<AlgoStoreApiLogEntity, IAlgoStoreApiLog>(this._containerDB);
            CSharpAlgoTemplateLogRepository = RepositoryUtils.ResolveGenericRepository<CSharpAlgoTemplateLogEntity, ICSharpAlgoTemplateLog>(this._containerDB);
            CSharpAlgoTemplateUserLogRepository = RepositoryUtils.ResolveGenericRepository<CSharpAlgoTemplateUserLogEntity, ICSharpAlgoTemplateUserLog>(this._containerDB);
            PublicAlgosRepository = RepositoryUtils.ResolveGenericRepository<PublicsAlgosTableEntity, IPublicAlgosTable>(this._containerDB);
            StatisticsRepository = RepositoryUtils.ResolveGenericRepository<StatisticsEntity, IStatisticss>(this._containerDB);
            BlobRepository = new AlgoBlobRepository(_configBuilderDB.Config["TableStorageConnectionString"], timespan);
            AlgoInstanceStaticsticsRepository = RepositoryUtils.ResolveGenericRepository<AlgoInstanceStatisticsEntity, IAlgoInstanceStatistics>(this._containerDB);
            UserPermissionsRepository = RepositoryUtils.ResolveGenericRepository<UserPermissionsEntity, IUserPermissions>(this._containerDB);
        }

        private async Task PrepareTestData()
        {
            PreStoredMetadata = await UploadSomeBaseMetaData(1);
            DataManager.storeMetadata(PreStoredMetadata);
        }

        private async Task ClearTestData()
        {
            await ClearAllCascadeDelete(DataManager.getAllMetaData());
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Wait.ForPredefinedTime(100000);
            ClearTestData().Wait(1000000);
            GC.SuppressFinalize(this);
        }
    }
}
