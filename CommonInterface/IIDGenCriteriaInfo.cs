using CommonModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CommonInterface
{
    public interface IIDGenCriteriaInfo
    {
        string GenerateID(SqlTransaction objTransaction, object objModel, EnumIdCategory pCriteriaId);
    }
}
