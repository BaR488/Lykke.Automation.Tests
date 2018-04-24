using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class UserRoleDTO
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String CanBeModified { get; set; }
        public String CanBeDeleted { get; set; }
        public List<UserPermissionDTO> Permissions { get; set; }
    }
}
