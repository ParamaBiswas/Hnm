using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Security.Model
{
    public class PermissionsRequirement : IAuthorizationRequirement
    {
        //RoleInfo RoleInfo = new RoleInfo();
        //List<RoleInfo> Permission = RoleInfo.GetAllRolls();
        public string Permission { get; set; }
        public PermissionsRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
