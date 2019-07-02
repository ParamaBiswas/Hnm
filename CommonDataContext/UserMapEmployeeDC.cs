using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LS.General.ModelBiz;
using LSCrud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace CommonDataContext
{
    public class UserMapEmployeeDC : IUserMapEmployee
    {
        static string COMPANY_CODE = "1";

        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        public UserMapEmployeeDC(ISupplierDbContext supplierDbContext, ICRUD cRUD)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
        }
        
        public List<UserMapEmployee> GetUserList(string pUserName, Int32 pStringMatchOptionValue)
        {
            List<UserMapEmployee> objUserMapEmployeeList = new List<UserMapEmployee>();
            UserMapEmployee objUserMapEmployee;

            string vComTxt = @" SELECT A.UserId
                                        ,A.UserName
                                        ,C.UserMapCode
                                FROM [Dev_LS_SecurityDB_HNM].[dbo].[LS_User] A 
                                JOIN [Dev_LS_SecurityDB_HNM].[dbo].[LS_UserInfo] B 
                                ON  A.UserId = B.UserId 
                                LEFT JOIN [General_UserMapEmployee] C
                                ON A.UserId = C.UserId
                                WHERE B.CompanyCode = " + COMPANY_CODE + "  AND A.ActionType<>'DELETE'" +
                                "AND A.UserId NOT IN" +
                                    "(SELECT UserCode FROM [LSP_PMS_SupplierInfo])";

            if (!String.IsNullOrEmpty(pUserName))
            {
                // 0 = StartsWith, 1 = EndsWith, 2 = Likely
                if (pStringMatchOptionValue == 0)
                {
                    vComTxt += " AND UserName LIKE '" + pUserName + "%'";

                }
                else if (pStringMatchOptionValue == 1)
                {
                    vComTxt += " AND UserName LIKE '%" + pUserName + "'";

                }
                else
                {
                    vComTxt += " AND UserName LIKE '%" + pUserName + "%'";
                }
            }
            vComTxt += " ORDER BY UserName ";

            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();

            while (dr.Read())
            {
                objUserMapEmployee = new UserMapEmployee();
                objUserMapEmployee.UserName_VW = dr["UserName"].ToString();
                objUserMapEmployee.UserId_FK = dr["UserId"].ToString();
                objUserMapEmployee.UserMapCode_PK = dr["UserMapCode"].ToString();
                //objUserMapEmployee.TableName_TBL = dr["TableName_TBL"].ToString();
                objUserMapEmployeeList.Add(objUserMapEmployee);
            }

            return objUserMapEmployeeList;
        }

        static readonly string UserMapEmployee_TBL = "[General_UserMapEmployee]";

        public string InsertUserMapEmployee(UserMapEmployee objUserMapEmployee)
        {
            UserMapEmployee objNewUserMapEmployee = new UserMapEmployee();
            objUserMapEmployee.TableName_TBL = UserMapEmployee_TBL;

            int vResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objUserMapEmployee.TableName_TBL = UserMapEmployee_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();


            using (SqlTransaction trans = connection.BeginTransaction())
            {
                try
                {
                    #region To check is username already used by another employee?
                    string vComTxt = @"SELECT UserMapCode
                            ,EmployeeCode
                            ,UserId
                            ,IsActive
                            FROM " + objUserMapEmployee.TableName_TBL +
                              @" WHERE  ActionType <> 'DELETE' AND UserId = '" + objUserMapEmployee.UserId_FK + "' AND IsActive = 1"; ;
                    #endregion

                    
                    SqlDataReader dr;
                    SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
                    objDbCommand.Transaction = trans;
                    dr = objDbCommand.ExecuteReader();

                    if (dr.Read())
                    {
                        vOut = "This User Name is already active for another employee.";
                    }
                    else
                    {
                        if (objUserMapEmployee.IsNew)
                        {
                            objUserMapEmployee.UserMapCode_PK = Guid.NewGuid().ToString();
                            objUserMapEmployee.ActionType = "INSERT";
                            //objDbCommand.Parameters.AddWithValue("UserMapCode_PK", Guid.NewGuid().ToString());
                            vQueryList.Add(GetQuery(objUserMapEmployee));

                            using (SqlCommand command = _supplierDbContext.GetCommand())
                            {
                                foreach (string obj_temp in vQueryList)
                                {

                                    command.CommandText = obj_temp;
                                    command.Transaction = trans;
                                    vResult = command.ExecuteNonQuery();
                                }
                                if (vResult > 0)
                                {
                                    trans.Commit();
                                    vOut = "User & Employee Map Successfully";
                                }
                                else
                                    vOut = "Exception Occured";
                            }
                        }
                        else
                        {
                            vOut = "Exception Occured";
                        }
                    }
                }
                catch (DbException ex)
                {
                    trans.Rollback();
                    throw ex;

                }
                finally
                {
                    connection.Close();
                }
            }
            return vOut;
        }

        public UserMapEmployee GetUserMapEmployeeByUserCode(string pUserCode, bool IsActive)
        {
            UserMapEmployee objNewUserMapEmployee = new UserMapEmployee();
            //objUserMapEmployee.TableName_TBL = UserMapEmployee_TBL;
            string vComTxt = @"SELECT UserMapCode
                            ,EmployeeCode
                            ,UserId
                            ,IsActive
                            FROM " + UserMapEmployee_TBL +
                           @" WHERE   ActionType <> 'DELETE' AND UserId = '" + pUserCode + "' ";
            if (IsActive)
            {
                vComTxt = vComTxt + " AND IsActive=1";
            }

            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();

            if (dr.Read())
            {
                objNewUserMapEmployee.UserMapCode_PK = dr["UserMapCode"].ToString();
                objNewUserMapEmployee.EmployeeCode_FK = dr["EmployeeCode"].ToString();
                objNewUserMapEmployee.UserId_FK = dr["UserId"].ToString();
                if (dr["IsActive"] != DBNull.Value)
                    objNewUserMapEmployee.IsActive = Convert.ToInt16(dr["IsActive"].ToString());
                objNewUserMapEmployee.IsActive_VW = Convert.ToInt32(dr["IsActive"]) == 1 ? true : false;
            }
            //connection.Close();

            string vComTxt1 = @"SELECT EmployeeId
                                ,Name
                                ,DepartmentCode
                                FROM [HR_Employee] 
                                WHERE  EmployeeCode = '" + objNewUserMapEmployee.EmployeeCode_FK + "' AND ActionType <> 'DELETE'";
            //connection.Open();
            SqlDataReader dr2;
            SqlCommand objDbCommand2 = new SqlCommand(vComTxt1, connection);
            dr2 = objDbCommand2.ExecuteReader();

            if (dr2.Read())
            {
                objNewUserMapEmployee.UserID_VW = dr2["EmployeeId"].ToString();
                objNewUserMapEmployee.UserName_VW = dr2["Name"].ToString();
                objNewUserMapEmployee.DeptCode_VW = dr2["DepartmentCode"].ToString();

            }
            connection.Close();

            return objNewUserMapEmployee;
        }

        public string GetQuery(object objObject)
        {
            BaseModel obj_Temp = (BaseModel)objObject;
            string vQuery = "";
            if (obj_Temp.IsNew)
            {
                //objObject.UserMapCode_PK = Guid.NewGuid().ToString();
                vQuery = _cRUD.CREATEQuery(objObject);
            }
            else if (obj_Temp.IsDeleted == true)
            {
                Hashtable pMarkDELColNameValues = new Hashtable();
                pMarkDELColNameValues.Clear();
                obj_Temp.ActionType = "DELETE";
                pMarkDELColNameValues.Add("ActionType", obj_Temp.ActionType);
                pMarkDELColNameValues.Add("ActionDate", obj_Temp.ActionDate);
                pMarkDELColNameValues.Add("UserCode", obj_Temp.UserCode);
                pMarkDELColNameValues.Add("CompanyCode", obj_Temp.CompanyCode_FK);
                vQuery = _cRUD.DELETEQuery(objObject, true, pMarkDELColNameValues);
            }
            else
            {
                vQuery = _cRUD.UPDATEQuery(objObject);
            }
            return vQuery;
        }




    }
}
