using LSCrud;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ConnectionGateway
{
    public interface ISupplierDbContext
    {
        SqlConnection GetConn();
        SqlCommand GetCommand();
        //ICRUD CRUDBuilder();

    }
}
