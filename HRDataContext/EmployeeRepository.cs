using CommonInterface;
using CommonModel;
using ConnectionGateway;
using HRInterface;
using HRModel;
using LSCrud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace HRDataContext
{
    public class EmployeeRepository: IEmployeeRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public EmployeeRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string Employee_TBL ="HR_Employee";
        public List<Employee> GetEmployeeAll()
        {
            List<Employee> objEmployeeList = new List<Employee>();
            Employee objEmployee;
            string vComTxt = @"SELECT EmployeeCode
                                ,EmployeeId
                                ,Name
                                ,DepartmentCode
                                ,dbo.fxn_FileName(DepartmentCode) AS Department
                                ,DesignationCode
                                ,dbo.fxn_FileName(DesignationCode) AS Designation
                                ,JobStatus
                                ,EligibleForOT
                                FROM  HR_Employee WHERE CompanyCode =1 AND ActionType != 'Delete' ORDER BY EmployeeId ASC";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objEmployee = new Employee();
                objEmployee.EmployeeCode_PK = dr["EmployeeCode"].ToString();
                objEmployee.EmployeeId = dr["EmployeeId"].ToString();
                objEmployee.Name = dr["Name"].ToString();
                objEmployee.DepartmentCode_FK = Convert.ToInt16(dr["DepartmentCode"].ToString());
                objEmployee.Department_VW = dr["Department"].ToString();
                objEmployee.DesignationCode_FK = Convert.ToInt16(dr["DesignationCode"].ToString());
                objEmployee.Designation_VW = dr["Designation"].ToString();

                objEmployeeList.Add(objEmployee);
            }
            return objEmployeeList;
        }
        public string SaveEmployee(Employee objEmployee)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objEmployee.TableName_TBL = Employee_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objEmployee.EmployeeCode_PK))
                {
                    objEmployee.EmployeeCode_PK = Guid.NewGuid().ToString();
                    objEmployee.EmployeeId = _iIDGenCriteriaInfo.GenerateID(trans, objEmployee, EnumIdCategory.EmployeeId);
                }
                vQueryList.Add(GetQuery(objEmployee));

                try
                {
                    using (SqlCommand command = _supplierDbContext.GetCommand())
                    {
                        foreach (string obj_temp in vQueryList)
                        {
                            command.CommandText = obj_temp;
                            vResult = command.ExecuteNonQuery();
                        }

                    }
                    if (vResult > 0)
                    {
                        trans.Commit();
                        vOut = "Employee Saved Successfully";
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
        public string GetQuery(object objObject)
        {
            BaseModel obj_Temp = (BaseModel)objObject;
            string vQuery = "";
            if (obj_Temp.IsNew)
            {

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
        public Employee GetEmployeeByID(string pEmployeeId, int pCompanyCode)
        {
            Employee objEmployee = new Employee();

            string vComTxt = @" SELECT 
                             EmployeeCode
                            ,EmployeeId,Name,Sex,FatherName,MotherName
                            ,ReportToCode
                            ,dbo.fxn_EmployeeNameByCode(ReportToCode) AS ReportTo
                            ,PresentAddress
                            ,ContactNumber
                            ,DepartmentCode
                            ,GC.FileName AS Department
                            ,DesignationCode
                            ,dbo.fxn_FileName( DesignationCode ) AS Designation
                            ,JobStatus
                            ,JobStatusChangedDate
                        FROM HR_Employee HRE JOIN General_CodeFile GC ON GC.FileCode=HRE.DepartmentCode AND HRE.CompanyCode=GC.CompanyCode WHERE EmployeeId = '" + pEmployeeId.Replace("'", "''") + "' AND HRE.ActionType != 'Delete' AND  HRE.CompanyCode =" + pCompanyCode + "";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            if (dr.Read())
            {
                objEmployee.EmployeeCode_PK = dr["EmployeeCode"].ToString();
                objEmployee.EmployeeId = dr["EmployeeId"].ToString();
                objEmployee.Name = dr["Name"].ToString();
                if (!String.IsNullOrEmpty(dr["Sex"].ToString()))
                    objEmployee.Sex = Convert.ToInt32(dr["Sex"].ToString());
                objEmployee.FatherName = dr["FatherName"].ToString();
                objEmployee.MotherName = dr["MotherName"].ToString();
                if (!String.IsNullOrEmpty(dr["ReportToCode"].ToString()))
                    objEmployee.ReportToCode = dr["ReportToCode"].ToString();
                objEmployee.ReportToName_VW = dr["ReportTo"].ToString();
                objEmployee.PresentAddress = dr["PresentAddress"].ToString();
                if (dr["ContactNumber"] != DBNull.Value)
                {
                    objEmployee.ContactNumber = dr["ContactNumber"].ToString();
                }
                if (!String.IsNullOrEmpty(dr["DepartmentCode"].ToString()))
                    objEmployee.DepartmentCode_FK = Convert.ToInt32(dr["DepartmentCode"].ToString());
                objEmployee.Department_VW = dr["Department"].ToString();
                if (!String.IsNullOrEmpty(dr["DesignationCode"].ToString()))
                    objEmployee.DesignationCode_FK = Convert.ToInt32(dr["DesignationCode"].ToString());
                objEmployee.Designation_VW = dr["Designation"].ToString();
                if (!String.IsNullOrEmpty(dr["JobStatus"].ToString()))
                    objEmployee.JobStatus = Convert.ToInt32(dr["JobStatus"].ToString());
                if (dr["JobStatusChangedDate"] != DBNull.Value)
                {
                    objEmployee.JobStatusChangedDate = dr.GetDateTime(dr.GetOrdinal("JobStatusChangedDate")).ToString("dd-MM-yyyy");
                }
            }
            return objEmployee;
        }

    }
    }
