using CenterPointCoreSecurity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Model
{
    public class RoleInfo
    {
        public string RoleId_PK { get; set; }
        public string RoleName { get; set; }
        public static List<RoleInfo> GetAllRolls()
        {
            return UserInfoDC.GetAllRolls();
        }
    }
}
