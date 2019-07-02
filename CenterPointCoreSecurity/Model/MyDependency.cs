using CenterPointCoreSecurity.Data;
using System.Collections.Generic;

namespace Security.Model
{
    public class MyDependency
    {
        public string username { get; set; }

        public  List<RoleInfo> GetUserInRoles(string username)
        {
            return UserInfoDC.GetUserRoleList(username);
        }
        
    }

}
