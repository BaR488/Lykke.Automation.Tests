using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IUserPermissions : IDictionaryItem
    {
        string DisplayName { get; set; }
    }
}
