using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace JKS_Report.Function.DB
{
    public class ErrorHelper
    {
        static string ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public static void LogError(string ServiceName,string ServiceCode,string Exception = "",string Info = "",string AdditionalInfo = "")
        {
            int result = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string Query = @"INSERT INTO `loggingservice` (`ServiceName`, `ServiceCode`, `Exception`, `Info`, `AddtionalInfo`, `CreatedOn`) VALUES (@ServiceName, @ServiceCode, @Exception, @Info, @AddtionalInfo, @CreatedOn)";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ServiceName", ServiceName, DbType.String, ParameterDirection.Input);
                    parameters.Add("@ServiceCode", ServiceCode, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Exception", Exception, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Info", Info, DbType.String, ParameterDirection.Input);
                    parameters.Add("@AddtionalInfo", AdditionalInfo, DbType.String, ParameterDirection.Input);
                    parameters.Add("@CreatedOn", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                    result = connection.Query<int>(Query, parameters).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
