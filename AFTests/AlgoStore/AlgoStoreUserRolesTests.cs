using AlgoStoreData.DTOs;
using AlgoStoreData.Fixtures;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
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

        private String getAllRolesPath = ApiPaths.ALGO_STORE_USER_ROLES_GET_ALL_ROLES;
        private String getRoleByIdPath = ApiPaths.ALGO_STORE_USER_ROLES_GET_ROLE_BY_ID;
        private String getRoleByClientIdPath = ApiPaths.ALGO_STORE_USER_ROLES_GET_ROLE_BY_CLIENT_ID;
        private String saveRolePath = ApiPaths.ALGO_STORE_USER_ROLES_SAVE_ROLE;
        private String assignRolePath = ApiPaths.ALGO_STORE_USER_ROLES_ASSIGN_ROLE;
        private String revokeRolePath = ApiPaths.ALGO_STORE_USER_ROLES_REVOKE_ROLE;
        private String verifyRolePath = ApiPaths.ALGO_STORE_USER_ROLES_VERIFY_ROLE;
        private String deleteRolePath = ApiPaths.ALGO_STORE_USER_ROLES_DELETE_ROLE;

        #endregion

        [Test]
        [Category("AlgoStore")]
        public async Task GetAllRoles()
        {
            var response = await Consumer.ExecuteRequest(getAllRolesPath, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));

            var allRoles = JsonUtils.DeserializeJson<List<UserRoleDTO>>(response.ResponseJson);
            Assert.That(allRoles, Is.Not.Null);
        }
    }
}
