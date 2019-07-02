using CenterPointCoreSecurity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Security.Model
{
    public class PermissionHandler:AuthorizationHandler<PermissionsRequirement>
    {
        private ApplicationDBContext context;
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,PermissionsRequirement resource)
        {
            MyDependency myDependency = new MyDependency();
            var user=context.User.FindFirst(c=> c.Issuer == "Parama" && c.Type== ClaimTypes.Name).Value;

            if (user == null)
            {
                return null;
            }
            else
            {
                List<RoleInfo> test = myDependency.GetUserInRoles(user);

                if (test.Count > 0)
                {
                    foreach(RoleInfo roleInfos in test)
                    {
                        if (roleInfos.RoleName==resource.Permission)
                        {
                            context.Succeed(resource);
                        }
                    }
                    
                }
            }
            return Task.CompletedTask;
        }
        }
        }

