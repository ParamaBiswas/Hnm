using LSCrud;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ConnectionGateway
{
    public class SupplierDbContext : ISupplierDbContext
    {
        private IConfiguration _config;
        private string _connectionString;
        private SqlConnection _connection;
        private SqlCommand _cmd;
        //private ICRUD _iCRUD;
        public SupplierDbContext(IConfiguration configuration)
        {
            _config = configuration;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public SqlConnection GetConn()
        {
            _connection = new SqlConnection(_connectionString);
            return _connection;
        }
        public SqlCommand GetCommand()
        {
            _cmd = _connection.CreateCommand();
            return _cmd;
        }
        //public  ICRUD CRUDBuilder()
        //{
        //    _iCRUD= new CRUDEngine();
        //    return _iCRUD;
        //}
    }
}
