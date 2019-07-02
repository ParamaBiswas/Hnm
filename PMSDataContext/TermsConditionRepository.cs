using ConnectionGateway;
using LSCrud;
using PMSInterface;
using PMSModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace PMSDataContext
{
    public class TermsConditionRepository: ITermsConditionRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;

        public TermsConditionRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
        }
        static readonly string TermsAndCondition_TBL = "PMS_TermsAndCondition";
        public string SaveTermsAndCondition(List<TermsAndCondition> objTermsAndCondition)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";

            List<string> vQueryList = new List<string>();
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                foreach (TermsAndCondition termsAndCondition in objTermsAndCondition)
                {
                    termsAndCondition.TableName_TBL = TermsAndCondition_TBL;
                    if (string.IsNullOrEmpty(termsAndCondition.ConditionCode_PK))
                    {
                        termsAndCondition.ConditionCode_PK = Guid.NewGuid().ToString();
                        vQueryList.Add(_cRUD.CREATEQuery(termsAndCondition));
                    }
                    else
                    {
                        vQueryList.Add(_cRUD.UPDATEQuery(termsAndCondition));
                    }
                    
                }
                
                try
                {
                    using (SqlCommand command = new SqlCommand("", connection, trans))
                    {
                        foreach (string obj_temp in vQueryList)
                        {
                            command.CommandText = obj_temp;
                            command.Transaction = trans;
                            vResult = command.ExecuteNonQuery();
                        }
                    }
                    if (vResult > 0)
                    {
                        trans.Commit();
                        vOut = "Terms and Condition Saved Successfully";
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
        public List<TermsAndCondition> GetTermsAndCondition()
        {
            List<TermsAndCondition> termsAndConditions = new List<TermsAndCondition>();
            TermsAndCondition objTermsAndConditions;
            string vComTxt = @"SELECT ConditionCode,ConditionValue,Condition,ValueType,(case when ValueType=1 then 'Fixed' else 'Percentage' end)valuetypenm
                            FROM PMS_TermsAndCondition";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objTermsAndConditions = new TermsAndCondition();
                objTermsAndConditions.ConditionCode_PK = dr["ConditionCode"].ToString();
                objTermsAndConditions.ConditionValue=Convert.ToDecimal(dr["ConditionValue"].ToString());
                objTermsAndConditions.Condition = dr["Condition"].ToString();
                objTermsAndConditions.ValueType = Convert.ToInt16(dr["ValueType"].ToString());
                objTermsAndConditions.ValueTypeName_VW = dr["valuetypenm"].ToString();

                termsAndConditions.Add(objTermsAndConditions);
            }
            return termsAndConditions;
            }
        }
}
