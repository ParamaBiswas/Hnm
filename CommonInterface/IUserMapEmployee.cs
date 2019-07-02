using LS.General.ModelBiz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CommonInterface
{
    public interface IUserMapEmployee
    {
        List<UserMapEmployee> GetUserList(string pUserName, Int32 pStringMatchOptionValue);

        string InsertUserMapEmployee(UserMapEmployee objUserMapEmployee);

        UserMapEmployee GetUserMapEmployeeByUserCode(string pUserCode, bool isActive);
    }
}
