using Dapper;
using JKS_Report.Function.DB;
using JKS_Report.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JKS_Report.Function.API
{
    public class MontlyReport
    {
        public static string ReportGeneration(DateTime dateFrom, DateTime dateTo)
        {
            string result = null;
            try
            {
                //List<clsFullDataVariable> fullDataVariables = new List<clsFullDataVariable>();
                //using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=reporting_jks;Uid=root;Pwd=abcd1234;SslMode=none;"))
                //{
                //    string Query = "SELECT * FROM mainvariable WHERE CreatedOn >= '2021-12-05' AND CreatedOn <= '2022-02-05' ";

                //    List<clsMainVariable> _clsMainVariableList = connection.Query<clsMainVariable>(Query).ToList();

                //    if (_clsMainVariableList.Count > 0)
                //    {
                //        foreach (var item in _clsMainVariableList)
                //        {
                //            clsFullDataVariable clsFullDataVariable = new clsFullDataVariable();
                //            clsFullDataVariable.clsMainVariable = item;

                //            Query = @"SELECT * FROM plcvariable WHERE ReferenceId = @ReferenceId";
                //            DynamicParameters parameters = new DynamicParameters();
                //            parameters.Add("@ReferenceId", item.Id, DbType.Int32, ParameterDirection.Input);
                //            List<clsStationVariable> _clsPlcVariableList = connection.Query<clsStationVariable>(Query, parameters).ToList();

                //            clsFullDataVariable.clsStationVariables = _clsPlcVariableList;

                //            Query = @"SELECT * FROM partmemory WHERE ReferenceId = @ReferenceId";
                //            parameters = new DynamicParameters();
                //            parameters.Add("@ReferenceId", item.Id, DbType.Int32, ParameterDirection.Input);
                //            List<clsPartMemory> _clsPartMemory = connection.Query<clsPartMemory>(Query, parameters).ToList();

                //            clsFullDataVariable.clsPartMemories = _clsPartMemory;

                //            fullDataVariables.Add(clsFullDataVariable);
                //        }
                //    }
                //}
            }
            catch
            {
                throw;
            }
            return result;
        }

        
    }
}
