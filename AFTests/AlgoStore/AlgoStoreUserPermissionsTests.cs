using AlgoStoreData.DTOs;
using AlgoStoreData.Fixtures;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStoreuserRoles")]
    public partial class AlgoStoreTests : AlgoStoreTestDataFixture
    {
        #region Path Variables

        private String getAllPermissonsPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_GET_ALL_PERMSSIONS;
        private String getPermissonsByIdPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_GET_PERMISSIONS_BY_ID;
        private String getPermissonsByRoleIdPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_GET_PERMISSIONS_BY_ROLE_ID;
        private String savePermissionPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_SAVE_PERMISSION;
        private String assignPermissionPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_ASSIGN_PERMISSION;
        private String assignPermissionsPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_ASSIGN_PERMISSIONS;
        private String revokePermissionPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_REVOKE_PERMISSION;
        private String revokePermissionsPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_REVOKE_PERMISSIONS;
        private String deletePermissionPath = ApiPaths.ALGO_STORE_USER_PERMISSIONS_DELETE_PERMISSION;

        #endregion

        [Test]
        [Category("AlgoStore")]
        public async Task GetAllPermissions()
        {
            var response = await Consumer.ExecuteRequest(getAllPermissonsPath, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));

            var allUsersPermissions = JsonUtils.DeserializeJson<List<UserPermissionDTO>>(response.ResponseJson);
            Assert.That(allUsersPermissions, Is.Not.Null);
        }
    }
}
