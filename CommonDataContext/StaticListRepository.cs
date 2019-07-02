using CommonInterface;
using CommonModel;
using ConnectionGateway;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CommonDataContext
{
    public class StaticListRepository: IStaticListRepository
    {
        ISupplierDbContext _supplierDbContext;
        public  StaticListRepository (ISupplierDbContext supplierDbContext)
        {
            _supplierDbContext = supplierDbContext;
        }

        public List<StaticDropDownListItem> GetStaticDropDownListByCode(int? DropdownCode)
        {
            List<StaticDropDownListItem> ddListItem = new List<StaticDropDownListItem>();
            StaticDropDownListItem item = new StaticDropDownListItem();
            string vComTxt = string.Empty;
            try
            {
                if (DropdownCode != null)
                {
                    vComTxt = @"SELECT  *FROM LS_StaticDropDownListItem Where DropdownCode = '" + DropdownCode + "'";
                }
                else
                {
                    vComTxt = @"SELECT  *FROM LS_StaticDropDownListItem";
                }
                SqlConnection connection = _supplierDbContext.GetConn();
                connection.Open();
                SqlDataReader dr;
                SqlCommand command = new SqlCommand(vComTxt, connection);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    item = new StaticDropDownListItem();
                    item.DropDownCode_PK = Convert.ToInt32(dr["DropdownCode"]);
                    item.ItemCode = Convert.ToInt32(dr["ItemCode"]);
                    item.ItemValue = Convert.ToInt32(dr["ItemValue"]);
                    item.ItemText = dr["ItemText"].ToString();

                    ddListItem.Add(item);
                }

                return ddListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
