using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Model
{
    public class UserInRole
    {
        /// <summary>
        /// Get or Set RoleID
        /// </summary>
        public string RoleID { get; set; }

        /// <summary>
        /// Get and Set UserID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Get or Set UserInRoleId
        /// </summary>
        public string UserRoleID_PK { get; set; }
        
    }
}
