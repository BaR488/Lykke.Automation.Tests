using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class UserPermissionsEntity : TableEntity, IUserPermissions
    {
        public string Id => PartitionKey;
        public string Name => RowKey;
        public string DisplayName { get;  set; }
    }
}
