using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace CommonDataContext
{
    public class IDGenCriteriaInfoDC : IIDGenCriteriaInfo
    {
        //string _CriteriaInfoTable = "LS_IDGenCriteria";
        //string _CriteriaConditionTable = "LS_IDGenCriteriaCondition";
        //static readonly string LS_DynamicPageTableName = "LS_DFPages#";
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        public IDGenCriteriaInfoDC(ISupplierDbContext supplierDbContext, ICRUD cRUD)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
        }
        public string GenerateID(SqlTransaction objTransaction, object objModel, EnumIdCategory pCriteriaId)
        {
            return GenerateIDWithDate(objTransaction, objModel, pCriteriaId, DateTime.Now);
        }
        public string GenerateIDWithDate(SqlTransaction objTransaction, object objModel, EnumIdCategory pCriteriaId, DateTime pDateTime)
        {
            IDGenCriteriaInfo objIDGenCriteriaInfo = new IDGenCriteriaInfo();
            objIDGenCriteriaInfo.CriteriaID = ((int)pCriteriaId).ToString();
            PropertyInfo pi = objModel.GetType().GetProperty("CompanyCode_FK");
            if (pi == null)
            {
                pi = objModel.GetType().GetProperty("CompanyCode");
                objIDGenCriteriaInfo.CompanyCode_FK = Convert.ToInt32(pi.GetValue(objModel, null));
            }
            else
            {
                objIDGenCriteriaInfo.CompanyCode_FK = (int)pi.GetValue(objModel, null);
            }
            List<IDGenCriteriaInfo> objIDGenCriteriaInfoList = GetObjectByID(objTransaction, objIDGenCriteriaInfo);
            StringBuilder sbGenId = new StringBuilder();
            if (objIDGenCriteriaInfoList.Count > 0)
            {
                List<IDGenCriteriaCondition> objCriteriaConditionList = objIDGenCriteriaInfoList[0].IDGenCriteriaConditionList_VW;
                for (int i = 0; i < objCriteriaConditionList.Count; i++)
                {
                    IDGenCriteriaCondition objCriteriaCondition = (IDGenCriteriaCondition)objCriteriaConditionList[i];
                    switch (objCriteriaCondition.ConditionType)
                    {
                        case "0"://"Business Object";                      
                            Type tp = objModel.GetType();

                            PropertyInfo objProperty = tp.GetProperty(objCriteriaCondition.ConditionValue.Split('.')[1].ToString());
                            if (objProperty != null)
                            {
                                string vText = objProperty.GetValue(objModel, null).ToString();
                                if (vText.Length > objCriteriaCondition.ConditionValueLength)
                                {
                                    vText = vText.Substring(0, objCriteriaCondition.ConditionValueLength);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(objCriteriaCondition.PaddingText))
                                    {
                                        char PaddingText = objCriteriaCondition.PaddingText[0];

                                        vText = vText.PadRight(objCriteriaCondition.ConditionValueLength, PaddingText);
                                    }
                                    else
                                    {
                                        vText = vText.PadLeft(objCriteriaCondition.ConditionValueLength, '0');
                                    }
                                }
                                sbGenId.Append(vText);
                            }
                            break;
                        case "2"://"System Year";
                            if (objCriteriaCondition.ConditionValueLength > 0)
                            {
                                string vYear = pDateTime.Year.ToString();
                                if (vYear.Length >= objCriteriaCondition.ConditionValueLength)
                                {
                                    vYear = vYear.Substring(vYear.Length - objCriteriaCondition.ConditionValueLength, objCriteriaCondition.ConditionValueLength);
                                    sbGenId.Append(vYear);
                                }
                                else
                                {
                                    sbGenId.Append(vYear);
                                }
                            }
                            break;
                        case "3"://"System Month";
                            if (objCriteriaCondition.ConditionValue.ToUpper() == "MMM")
                            {
                                sbGenId.Append(pDateTime.ToString("MMM"));
                            }
                            else if (objCriteriaCondition.ConditionValue.ToUpper() == "MM")
                            {
                                sbGenId.Append(pDateTime.ToString("MM"));
                            }
                            else
                            {
                                sbGenId.Append(pDateTime.ToString("MM"));
                            }
                            break;
                        case "4"://"System Day";
                            //Bug Fixed [01-01-2012] By Bashar
                            string tempDay = pDateTime.Day.ToString();
                            string tempDayPad = tempDay.PadLeft(tempDay.Length + 2, '0');
                            sbGenId.Append(tempDayPad.Substring(tempDayPad.Length - 2, 2));
                            //sbGenId.Append(DateTime.Now.Day.ToString());    
                            break;
                        case "5"://"System Hour";
                            sbGenId.Append(DateTime.Now.Hour.ToString());
                            break;
                        case "6"://"System Min";
                            sbGenId.Append(DateTime.Now.Minute.ToString());
                            break;
                        case "7"://"System Sec";
                            sbGenId.Append(DateTime.Now.Second.ToString());
                            break;
                        case "8"://"Static Value";
                            sbGenId.Append(objCriteriaCondition.ConditionValue.ToString());
                            break;
                        case "9"://"Auto Increment";
                            sbGenId = GetAutoIncrimentValue(objTransaction, objModel, objCriteriaCondition, sbGenId, objCriteriaConditionList, pDateTime);
                            break;
                    }
                }
            }
            return sbGenId.ToString();
        }
        public List<IDGenCriteriaInfo> GetObjectByID(SqlTransaction objTransaction, IDGenCriteriaInfo objIDGenCriteriaInfo)
        {
            string pCompanyCode = String.Empty;
            pCompanyCode = objIDGenCriteriaInfo.CompanyCode_FK.ToString();
            List<IDGenCriteriaInfo> objIDGenCriteriaInfoList = new List<IDGenCriteriaInfo>();
            IDGenCriteriaInfo IDGenCriteriaInfo = new IDGenCriteriaInfo();
            StringBuilder vComText = new StringBuilder();
            vComText.Append("SELECT CriteriaCode,CAST(CriteriaID as NUMERIC) CriteriaID,CriteriaName,CompanyCode,UserCode,ActionDate,ActionType,CriteriaConditionText FROM LS_IDGenCriteria Where CompanyCode =" + pCompanyCode + "");
            if (!string.IsNullOrEmpty(objIDGenCriteriaInfo.CriteriaCode_PK))
            {
                vComText.Append(" AND CriteriaCode='" + objIDGenCriteriaInfo.CriteriaCode_PK + "'");
            }
            else if (!string.IsNullOrEmpty(objIDGenCriteriaInfo.CriteriaID))
            {
                vComText.Append(" AND CriteriaID='" + objIDGenCriteriaInfo.CriteriaID + "'");
            }
            vComText.Append(" Order By CriteriaId");
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComText.ToString(), connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                IDGenCriteriaInfo = new IDGenCriteriaInfo();
                IDGenCriteriaInfo.IsNew = false;
                IDGenCriteriaInfo.CompanyCode_FK = Convert.ToInt16(dr["CompanyCode"]);
                IDGenCriteriaInfo.UserCode = dr["UserCode"].ToString();
                IDGenCriteriaInfo.ActionDate = dr["ActionDate"].ToString();
                IDGenCriteriaInfo.ActionType = dr["ActionType"].ToString();
                IDGenCriteriaInfo.CriteriaCode_PK = dr["CriteriaCode"].ToString();
                IDGenCriteriaInfo.CriteriaID = dr["CriteriaID"].ToString();
                IDGenCriteriaInfo.CriteriaName = dr["CriteriaName"].ToString();
                IDGenCriteriaInfo.CriteriaConditionText = dr["CriteriaConditionText"].ToString();
                objIDGenCriteriaInfoList.Add(IDGenCriteriaInfo);
            }
            dr.Close();
            if (objIDGenCriteriaInfoList.Count == 1)
            {
                IDGenCriteriaInfo.IDGenCriteriaConditionList_VW = GetCriteriaConditionList(objTransaction, IDGenCriteriaInfo);
            }
            return objIDGenCriteriaInfoList;

        }
        public List<IDGenCriteriaCondition> GetCriteriaConditionList(SqlTransaction objTransaction, IDGenCriteriaInfo objIDGenCriteriaInfo)
        {
            string pCompanyCode = String.Empty;
            pCompanyCode = objIDGenCriteriaInfo.CompanyCode_FK.ToString();
            List<IDGenCriteriaCondition> CriteriaConditionList = new List<IDGenCriteriaCondition>();
            IDGenCriteriaCondition objCriteriaCondition;
            if (!string.IsNullOrEmpty(objIDGenCriteriaInfo.CriteriaCode_PK))
            {
                string vComText = @"SELECT * FROM LS_IDGenCriteriaCondition 
                                    WHERE CompanyCode = " + pCompanyCode + " AND CriteriaCode='" + objIDGenCriteriaInfo.CriteriaCode_PK + "' And ActionType !='Delete'  ORDER BY SortOrder";
                SqlConnection connection = _supplierDbContext.GetConn();
                connection.Open();
                SqlDataReader dr;
                SqlCommand objDbCommand = new SqlCommand(vComText.ToString(), connection);
                dr = objDbCommand.ExecuteReader();
                while (dr.Read())
                {
                    objCriteriaCondition = new IDGenCriteriaCondition();
                    objCriteriaCondition.IsNew = false;
                    objCriteriaCondition.UserCode = dr["UserCode"].ToString();
                    objCriteriaCondition.CompanyCode_FK = Convert.ToInt16(dr["CompanyCode"]);
                    objCriteriaCondition.ActionDate = dr["ActionDate"].ToString();
                    objCriteriaCondition.ActionType = dr["ActionType"].ToString();
                    objCriteriaCondition.ConditionCode_PK = dr["ConditionCode"].ToString();
                    objCriteriaCondition.SortOrder = Convert.ToInt16(dr["SortOrder"]);
                    objCriteriaCondition.ConditionText = dr["ConditionText"].ToString();
                    objCriteriaCondition.ConditionType = dr["ConditionType"].ToString();
                    objCriteriaCondition.ConditionValue = dr["ConditionValue"].ToString();
                    objCriteriaCondition.ConditionValueLength = Convert.ToInt16(dr["ConditionValueLength"]);
                    objCriteriaCondition.CriteriaCode_FK = dr["CriteriaCode"].ToString();
                    objCriteriaCondition.AutoIncrementCriteria = dr["AutoIncrementCriteria"].ToString();
                    objCriteriaCondition.PaddingText = dr["PaddingText"].ToString();
                    CriteriaConditionList.Add(objCriteriaCondition);
                }
                dr.Close();
            }
            return CriteriaConditionList;
        }
        private StringBuilder GetAutoIncrimentValue(SqlTransaction objTransaction, object objModel, IDGenCriteriaCondition objCriteriaCondition, StringBuilder sbGenId, List<IDGenCriteriaCondition> objCriteriaConditionList, DateTime pDateTime)
        {
            string vExistingValue = string.Empty;
            StringBuilder vNewValue = new StringBuilder();
            string vDbValue = string.Empty;
            int vLength = 0;
            PropertyInfo pi = objModel.GetType().GetProperty("CompanyCode_FK");
            if (pi == null)
            {
                pi = objModel.GetType().GetProperty("CompanyCode");
            }
            int pCompanyCode = Convert.ToInt32(pi.GetValue(objModel, null));
            string vDependValue = objCriteriaCondition.AutoIncrementCriteria;
            IDGenCriteriaCondition vDepandCon = objCriteriaConditionList.Find(crcon => crcon.ConditionText.ToUpper() == vDependValue.ToUpper());
            List<IDGenCriteriaCondition> oChildList = objCriteriaConditionList.FindAll(crcon => crcon.SortOrder <= vDepandCon.SortOrder);
            for (int i = 0; i < oChildList.Count; i++)
            {
                IDGenCriteriaCondition vCondition = oChildList[i];
                switch (vCondition.ConditionType)
                {
                    case "0"://"Business Object";                      
                        Type tp = objModel.GetType();

                        PropertyInfo objProperty = tp.GetProperty(vCondition.ConditionValue.Split('.')[1].ToString());
                        if (objProperty != null)
                        {
                            string vText = objProperty.GetValue(objModel, null).ToString();
                            if (vText.Length > vCondition.ConditionValueLength)
                            {
                                vText = vText.Substring(0, vCondition.ConditionValueLength);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(vCondition.PaddingText))
                                {
                                    char PaddingText = vCondition.PaddingText[0];

                                    vText = vText.PadRight(vCondition.ConditionValueLength, PaddingText);
                                }
                                else
                                {
                                    vText = vText.PadLeft(vCondition.ConditionValueLength, '0');
                                }
                            }
                            vNewValue.Append(vText);
                        }
                        vLength += vCondition.ConditionValueLength;
                        break;
                    case "2"://"System Year";
                        if (vCondition.ConditionValueLength > 0)
                        {
                            string vYear = pDateTime.Year.ToString();
                            if (vYear.Length >= vCondition.ConditionValueLength)
                            {
                                vYear = vYear.Substring(vYear.Length - vCondition.ConditionValueLength, vCondition.ConditionValueLength);
                                vNewValue.Append(vYear);
                            }
                            else
                            {
                                vNewValue.Append(vYear);
                            }
                        }
                        vLength += vCondition.ConditionValueLength;
                        break;
                    case "3"://"System Month";
                        if (vCondition.ConditionValue.ToUpper() == "MMM")
                        {
                            vNewValue.Append(pDateTime.ToString("MMM"));
                        }
                        else if (objCriteriaCondition.ConditionValue.ToUpper() == "MM")
                        {
                            vNewValue.Append(pDateTime.ToString("MM"));
                        }
                        else
                        {
                            vNewValue.Append(pDateTime.ToString("MM"));
                        }
                        vLength += vCondition.ConditionValueLength;
                        break;
                    case "4"://"System Day";
                        //Bug Fixed [01-01-2012] by Bashar
                        string tempDay = pDateTime.Day.ToString();
                        string tempDayPad = tempDay.PadLeft(tempDay.Length + 2, '0');
                        vNewValue.Append(tempDayPad.Substring(tempDayPad.Length - 2, 2));
                        vLength += vCondition.ConditionValueLength;
                        //vNewValue.Append(DateTime.Now.Day.ToString());
                        //vLength += vCondition.ConditionValueLength;
                        break;
                    case "5"://"System Hour";
                        vNewValue.Append(DateTime.Now.Hour.ToString());
                        vLength += vCondition.ConditionValueLength;
                        break;
                    case "6"://"System Min";
                        vNewValue.Append(DateTime.Now.Minute.ToString());
                        vLength += vCondition.ConditionValueLength;
                        break;
                    case "7"://"System Sec";
                        vNewValue.Append(DateTime.Now.Second.ToString());
                        vLength += vCondition.ConditionValueLength;
                        break;
                    case "8"://"Static Value";
                        vNewValue.Append(vCondition.ConditionValue.ToString());
                        vLength += vCondition.ConditionValueLength;
                        break;
                }
            }
            // left padding
            string Length = string.Empty;
            for (int j = 0; j < objCriteriaCondition.ConditionValueLength; j++)
            {
                Length += "0";
            }
            //Get ID from DC
            IDGenCriteriaCondition objCondition = objCriteriaConditionList.Find(crcon => crcon.ConditionType == "1");
            List<string> oList = new List<string>();
            //For Table name
            oList.Add(objCondition.ConditionValue.Split('.')[0].ToString());
            //For Column name
            oList.Add(objCondition.ConditionValue.Split('.')[1].ToString());
            //Unique Text
            oList.Add(vNewValue.ToString());
            vDbValue = GetColumnValueOfDBObjectForApp( oList, pCompanyCode);
            if (!string.IsNullOrEmpty(vDbValue))
            {
                vExistingValue = vDbValue.Substring(0, vLength);

                if (vExistingValue != vNewValue.ToString())
                {
                    string vPostText = sbGenId.ToString().Substring(vNewValue.Length, (sbGenId.ToString().Length - vNewValue.Length));
                    sbGenId = new StringBuilder();

                    sbGenId.Append(vNewValue);
                    sbGenId.Append(vPostText);
                    sbGenId.Append(1.ToString(Length));
                }
                else
                {
                    int vIntPart = Convert.ToInt32(vDbValue.Substring(vDbValue.Length - objCriteriaCondition.ConditionValueLength, objCriteriaCondition.ConditionValueLength)) + 1;
                    sbGenId.Append(vIntPart.ToString(Length));
                }

            }
            else
            {
                sbGenId.Append(1.ToString(Length));
            }
            return sbGenId;
        }
        public string GetColumnValueOfDBObjectForApp(List<string> tableColumn, int pCompanyCode)
        {
            string vResult = string.Empty;
            SqlConnection connection = _supplierDbContext.GetConn();
            try
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                vResult = GetColumnValueOfDBObjectForApp(trans, tableColumn, pCompanyCode);
                trans.Commit();
            }
            catch (Exception ex)
            {
                    throw ex;
            }
            //finally
            //{
            //    connection.Close();
            //}
            return vResult;
        }
        public string GetColumnValueOfDBObjectForApp(SqlTransaction objTransaction, List<string> tableColumn, int pCompanyCode)
        {
            string vResult = string.Empty;
            string vComText = string.Empty;
            string vValue = string.Empty;
            string vColumnName = tableColumn[1];
            string vTableName = tableColumn[0];
            if (tableColumn.Count > 2)
                vValue = tableColumn[2];
            string tempStr = string.Empty;
            if (tableColumn != null && tableColumn.Count > 0)
            {
                vComText = @" SELECT TOP 1 " + vColumnName + " FROM " + vTableName + " WITH (TABLOCKX) WHERE " + vColumnName + " LIKE'" + vValue + "%' and companycode=" + pCompanyCode + "  Order BY " + vColumnName + " DESC";
            }
            SqlDataReader dr;
            SqlCommand cmd = _supplierDbContext.GetCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = vComText;
            cmd.Transaction = objTransaction;
            cmd.CommandTimeout = 60000;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                vResult = dr[0].ToString();
            }
            dr.Close();

            return vResult;

        }
}
}
