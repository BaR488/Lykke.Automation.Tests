﻿using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using XUnitTestData.Repositories.Assets;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using Common.Log;
using System.Threading.Tasks;
using XUnitTestData.Services;
using XUnitTestCommon.Utils;

namespace FirstXUnitTest.DependencyInjection
{
    class AssetsTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public AssetsTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AssetsRepository(
                    new AzureTableStorage<AssetEntity>(
                        _configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null)))
                .As<IDictionaryRepository<IAsset>>();

            RepositoryUtils.RegisterDictionaryManager<IAsset>(builder);



            builder.Register(c => new AssetDescriptionRepository(
                    new AzureTableStorage<AssetDescriptionEntity>(
                        _configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null)))
                .As<IDictionaryRepository<IAssetDescription>>();

            RepositoryUtils.RegisterDictionaryManager<IAssetDescription>(builder);

            builder.Register(c => new AssetCategoryRepository(
                    new AzureTableStorage<AssetCategoryEntity>(
                        _configBuilder.Config["DictionariesConnectionString"], "AssetCategories", null)))
                .As<IDictionaryRepository<IAssetCategory>>();

            RepositoryUtils.RegisterDictionaryManager<IAssetCategory>(builder);

            builder.Register(c => new AssetAttributesRepository(
                    new AzureTableStorage<AssetAttributesEntity>(
                        _configBuilder.Config["DictionariesConnectionString"], "AssetAttributes", null)))
                .As<IDictionaryRepository<IAssetAttributes>>();

            RepositoryUtils.RegisterDictionaryManager<IAssetAttributes>(builder);

            base.Load(builder);
        }
    }
}
