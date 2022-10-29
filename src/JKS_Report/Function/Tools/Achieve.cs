using Dapper;
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
using System.Web.Configuration;

namespace JKS_Report.Function.Tools
{
    static class Achieve
    {
        static string ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public static DataTable CreatePLCDataTable<T>(IEnumerable<T> list, string recordName, string SymbolOnNewRow = "", string PlcRename = "")
        {

            var assembly = Assembly.GetExecutingAssembly();

            string[] SymbolList = null;
            string[] PlcRenameList = null;

            if (!string.IsNullOrEmpty(SymbolOnNewRow))
            {
                using (Stream stream = assembly.GetManifestResourceStream(SymbolOnNewRow))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    SymbolList = result.Split(',');
                }
            }

            if (!string.IsNullOrEmpty(PlcRename))
            {
                using (Stream stream = assembly.GetManifestResourceStream(PlcRename))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    PlcRenameList = result.Split(',');
                }
            }

            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = recordName;
            var j = 0;
            foreach (PropertyInfo info in properties)
            {
                if (PlcRenameList != null)
                {
                    dataTable.Columns.Add(PlcRenameList[j]);
                    //dataTable.Columns.Add(info.Name + " " + columnlist[j]);
                    //dataTable.Columns.Add(new DataColumn(info.Name + " " + columnlist[j], Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                    j++;
                }
                else
                {
                    dataTable.Columns.Add(new DataColumn(info.Name));
                }
            }

            if (SymbolList != null)
            {
                dataTable.Rows.Add(SymbolList);
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        public static DataTable CreateMainDataTable(clsPdfMainVariable list, string recordName, string MainSymbol = "")
        {
            var assembly = Assembly.GetExecutingAssembly();

            string[] MainSymbolList = null;

            if (!string.IsNullOrEmpty(MainSymbol))
            {
                using (Stream stream = assembly.GetManifestResourceStream(MainSymbol))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    MainSymbolList = result.Split(',');
                }
            }

            Type type = typeof(clsPdfMainVariable);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = recordName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(info.Name);
            }
            if (MainSymbolList != null)
            {
                dataTable.Rows.Add(MainSymbolList);
            }

            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(list);
            }

            dataTable.Rows.Add(values);


            return dataTable;
        }
        public static DataTable CreatePalletADataTable(clsPdfBarcodePalletA list)
        {
            Type type = typeof(clsPdfBarcodePalletA);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = "PalletA";
            var j = 0;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(info.Name);
            }

            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(list);
            }

            dataTable.Rows.Add(values);

            DataTable dtnew = new DataTable();

            string[] columnname = { "PalletA", " " };
            //Convert all the rows to columns
            for (int i = 0; i <= dataTable.Rows.Count; i++)
            {
                dtnew.Columns.Add(columnname[i]);
                //dtnew.Columns.Add(Convert.ToString(i));
            }
            // Convert All the Columns to Rows
            for (int m = 0; m < dataTable.Columns.Count; m++)
            {
                var dr = dtnew.NewRow();
                dr[0] = dataTable.Columns[m].ToString();
                for (int k = 1; k <= dataTable.Rows.Count; k++)
                    dr[k] = dataTable.Rows[k - 1][m];
                dtnew.Rows.Add(dr);
            }
            return dtnew;
        }
        public static DataTable CreatePalletBDataTable(clsPdfBarcodePalletB list)
        {
            Type type = typeof(clsPdfBarcodePalletB);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = "PalletB";
            var j = 0;
            foreach (PropertyInfo info in properties)
            {

                dataTable.Columns.Add(info.Name);

            }

            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(list);
            }

            dataTable.Rows.Add(values);

            DataTable dtnew = new DataTable();
            string[] columnname = { "PalletB", "  " };
            //Convert all the rows to columns
            for (int i = 0; i <= dataTable.Rows.Count; i++)
            {
                dtnew.Columns.Add(columnname[i]);
                //dtnew.Columns.Add(Convert.ToString(i));
            }
            // Convert All the Columns to Rows
            for (int m = 0; m < dataTable.Columns.Count; m++)
            {
                var dr = dtnew.NewRow();
                dr[0] = dataTable.Columns[m].ToString();
                for (int k = 1; k <= dataTable.Rows.Count; k++)
                    dr[k] = dataTable.Rows[k - 1][m];
                dtnew.Rows.Add(dr);
            }
            return dtnew;
        }
        public static DataTable CreatePalletCDataTable(clsPdfBarcodePalletC list)
        {
            Type type = typeof(clsPdfBarcodePalletC);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = "PalletC";
            var j = 0;
            foreach (PropertyInfo info in properties)
            {

                dataTable.Columns.Add(info.Name);

            }

            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(list);
            }

            dataTable.Rows.Add(values);

            DataTable dtnew = new DataTable();
            string[] columnname = { "PalletC", "   " };
            //Convert all the rows to columns
            for (int i = 0; i <= dataTable.Rows.Count; i++)
            {
                dtnew.Columns.Add(columnname[i]);
                //dtnew.Columns.Add(Convert.ToString(i));
            }
            // Convert All the Columns to Rows
            for (int m = 0; m < dataTable.Columns.Count; m++)
            {
                var dr = dtnew.NewRow();
                dr[0] = dataTable.Columns[m].ToString();
                for (int k = 1; k <= dataTable.Rows.Count; k++)
                    dr[k] = dataTable.Rows[k - 1][m];
                dtnew.Rows.Add(dr);
            }
            return dtnew;
        }
        public static DataTable CreatePalletDDataTable(clsPdfBarcodePalletD list)
        {
            Type type = typeof(clsPdfBarcodePalletD);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = "PalletD";
            var j = 0;
            foreach (PropertyInfo info in properties)
            {

                dataTable.Columns.Add(info.Name);

            }

            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(list);
            }

            dataTable.Rows.Add(values);

            DataTable dtnew = new DataTable();
            string[] columnname = { "PalletD", "    " };
            //Convert all the rows to columns
            for (int i = 0; i <= dataTable.Rows.Count; i++)
            {
                dtnew.Columns.Add(columnname[i]);
                //dtnew.Columns.Add(Convert.ToString(i));
            }
            // Convert All the Columns to Rows
            for (int m = 0; m < dataTable.Columns.Count; m++)
            {
                var dr = dtnew.NewRow();
                dr[0] = dataTable.Columns[m].ToString();
                for (int k = 1; k <= dataTable.Rows.Count; k++)
                    dr[k] = dataTable.Rows[k - 1][m];
                dtnew.Rows.Add(dr);
            }
            return dtnew;
        }
        public static DataTable DataTableMerge(List<DataTable> dataTables, string recordName)
        {
            List<int> oList = new List<int>();
            DataTable mergedDataTable = new DataTable();
            mergedDataTable.TableName = recordName + "- barcode";
            foreach (DataTable dt in dataTables)
            {
                oList.Add(dt.Rows.Count);
                foreach (DataColumn dc in dt.Columns)
                {
                    mergedDataTable.Columns.Add(dc.ColumnName);
                }
                //mergedDataTable.Columns.Add(dt.TableName + "-" + "Space");
            }
            int temp = 0;
            for (int m = 0; m < oList.Count; m++)
            {
                for (int n = 0; n < oList.Count - 1; n++)
                {
                    if (oList[n] > oList[n + 1])
                    {
                        temp = oList[n + 1];
                        oList[n + 1] = oList[n];
                        oList[n] = temp;
                    }
                }
            }
            int maxRow = oList[oList.Count - 1];
            for (int o = 0; o < maxRow; o++)
            {
                DataRow newRow = mergedDataTable.NewRow();
                int mergedDataTableColumn = 0;
                foreach (DataTable dt in dataTables)
                {
                    if (dt.Rows.Count > o)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            newRow[mergedDataTableColumn] = dt.Rows[o][k];
                            mergedDataTableColumn++;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            newRow[mergedDataTableColumn] = DBNull.Value;
                            mergedDataTableColumn++;
                        }
                    }
                    //newRow[mergedDataTableColumn] = DBNull.Value;
                    //mergedDataTableColumn++;
                }
                mergedDataTable.Rows.Add(newRow);
            }
            return mergedDataTable;
        }

        public static void PDFGenerate(DateTime dateFrom, DateTime dateTo)
        {
            List<clsPdfFullDataVariable> clsPdfFullDataVariableList = new List<clsPdfFullDataVariable>();
            DataTable dtMain;
            DataTable dtPLC;
            DataTable dtPalletA;
            DataTable dtPalletB;
            DataTable dtPalletC;
            DataTable dtPalletD;
            DataTable dtAfterPalletMerge;
            DataTable dtAfterPalletMergebc;
            List<DataTable> dtBeforePalletList = new List<DataTable>();
            List<DataTable> dtBeforePalletListbc = new List<DataTable>();
            List<DataTable> DtList = new List<DataTable>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string Query = "SELECT * FROM mainvariable WHERE CreatedOn >= @dateFrom AND CreatedOn <= @dateTo";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@dateFrom", dateFrom, DbType.DateTime, ParameterDirection.Input);
                    parameters.Add("@dateTo", dateTo, DbType.DateTime, ParameterDirection.Input);
                    List<clsMainVariable> _clsMainVariable = connection.Query<clsMainVariable>(Query, parameters).ToList();

                    if (_clsMainVariable.Count > 0)
                    {
                        foreach (var item in _clsMainVariable)
                        {
                            clsPdfFullDataVariable clsPdfFullDataVariableItem = new clsPdfFullDataVariable();

                            clsPdfMainVariable clsPdfMainVariable = new clsPdfMainVariable();
                            clsPdfMainVariable.Username = item.Username;
                            clsPdfMainVariable.BasketNumber = item.BasketNumber;
                            clsPdfMainVariable.LoadingId = item.LoadingId.ToString();
                            clsPdfMainVariable.UnloadingId = item.UnloadingId.ToString();
                            clsPdfMainVariable.LoadingNo = item.LoadingNo.ToString();
                            
                            Query = "Select * from plcvariable where ReferenceId = @ReferenceId";
                            parameters = new DynamicParameters();
                            parameters.Add("@ReferenceId", item.Id, DbType.Int32, ParameterDirection.Input);
                            List<clsStationVariable> _clsPlcVariableList = connection.Query<clsStationVariable>(Query, parameters).ToList();

                            Query = @"SELECT * FROM partmemory WHERE ReferenceId = @ReferenceId";
                            parameters = new DynamicParameters();
                            parameters.Add("@ReferenceId", item.Id, DbType.Int32, ParameterDirection.Input);
                            clsPartMemory _clsPartMemory = connection.Query<clsPartMemory>(Query, parameters).FirstOrDefault();

                            clsPdfFullDataVariableItem.clsPdfMainVariable = clsPdfMainVariable;
                            List<clsPdfPlcVariable> clsPdfPlcVariableList = new List<clsPdfPlcVariable>();

                            foreach (var item1 in _clsPlcVariableList)
                            {
                                clsPdfPlcVariable _clsPdfPlcVariable = new clsPdfPlcVariable();
                                
                                _clsPdfPlcVariable.EffectiveTime = item1.EffectiveTime.ToString();
                                _clsPdfPlcVariable.TemperaturePV = item1.TemperaturePV.ToString();
                                _clsPdfPlcVariable.ConductivityPV = item1.ConductivityPV.ToString();
                                _clsPdfPlcVariable.Quality = item1.Quality;
                                clsPdfPlcVariableList.Add(_clsPdfPlcVariable);
                            }

                            clsPdfFullDataVariableItem.clsPdfPlcVariables = clsPdfPlcVariableList;

                            if (_clsPartMemory != null)
                            {
                                if (!string.IsNullOrEmpty(_clsPartMemory.PalletA))
                                {
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA = new clsPdfBarcodePalletA();
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO1 = _clsPartMemory.PalletA_WO1;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO2 = _clsPartMemory.PalletA_WO2;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO3 = _clsPartMemory.PalletA_WO3;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO4 = _clsPartMemory.PalletA_WO4;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO5 = _clsPartMemory.PalletA_WO5;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO6 = _clsPartMemory.PalletA_WO6;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO7 = _clsPartMemory.PalletA_WO7;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletA.PalletA_WO8 = _clsPartMemory.PalletA_WO8;
                                }
                                if (!string.IsNullOrEmpty(_clsPartMemory.PalletB))
                                {
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB = new clsPdfBarcodePalletB();
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO1 = _clsPartMemory.PalletB_WO1;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO2 = _clsPartMemory.PalletB_WO2;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO3 = _clsPartMemory.PalletB_WO3;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO4 = _clsPartMemory.PalletB_WO4;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO5 = _clsPartMemory.PalletB_WO5;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO6 = _clsPartMemory.PalletB_WO6;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO7 = _clsPartMemory.PalletB_WO7;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletB.PalletB_WO8 = _clsPartMemory.PalletB_WO8;
                                }
                                if (!string.IsNullOrEmpty(_clsPartMemory.PalletC))
                                {
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC = new clsPdfBarcodePalletC();
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO1 = _clsPartMemory.PalletC_WO1;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO2 = _clsPartMemory.PalletC_WO2;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO3 = _clsPartMemory.PalletC_WO3;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO4 = _clsPartMemory.PalletC_WO4;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO5 = _clsPartMemory.PalletC_WO5;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO6 = _clsPartMemory.PalletC_WO6;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO7 = _clsPartMemory.PalletC_WO7;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletC.PalletC_WO8 = _clsPartMemory.PalletC_WO8;
                                }
                                if (!string.IsNullOrEmpty(_clsPartMemory.PalletD))
                                {
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD = new clsPdfBarcodePalletD();
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO1 = _clsPartMemory.PalletD_WO1;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO2 = _clsPartMemory.PalletD_WO2;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO3 = _clsPartMemory.PalletD_WO3;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO4 = _clsPartMemory.PalletD_WO4;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO5 = _clsPartMemory.PalletD_WO5;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO6 = _clsPartMemory.PalletD_WO6;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO7 = _clsPartMemory.PalletD_WO7;
                                    clsPdfFullDataVariableItem.clsPdfBarcodesPalletD.PalletD_WO8 = _clsPartMemory.PalletD_WO8;
                                }
                            }

                            clsPdfFullDataVariableList.Add(clsPdfFullDataVariableItem);
                        }
                    }
                }

                if (clsPdfFullDataVariableList.Count > 0)
                {
                    int recordNo = 0;
                    foreach (var item in clsPdfFullDataVariableList)
                    {
                        dtBeforePalletList.Clear();
                        dtBeforePalletListbc.Clear();

                        if (item.clsPdfMainVariable != null)
                        {
                            dtMain = CreateMainDataTable(item.clsPdfMainVariable, "Record" + recordNo.ToString());
                            DtList.Add(dtMain);
                        }
                        if (item.clsPdfPlcVariables.Count > 0)
                        {
                            dtPLC = CreatePLCDataTable(item.clsPdfPlcVariables, "Record" + recordNo.ToString(), "JKS_Report.Text.CsvColumnSymbol_Main.txt");
                            DtList.Add(dtPLC);
                        }
                        if (item.clsPdfBarcodesPalletA != null)
                        {
                            dtPalletA = CreatePalletADataTable(item.clsPdfBarcodesPalletA);
                            dtBeforePalletList.Add(dtPalletA);
                        }
                        if (item.clsPdfBarcodesPalletB != null)
                        {
                            dtPalletB = CreatePalletBDataTable(item.clsPdfBarcodesPalletB);
                            dtBeforePalletList.Add(dtPalletB);
                        }
                        if (item.clsPdfBarcodesPalletC != null)
                        {
                            dtPalletC = CreatePalletCDataTable(item.clsPdfBarcodesPalletC);
                            dtBeforePalletListbc.Add(dtPalletC);
                        }
                        if (item.clsPdfBarcodesPalletD != null)
                        {
                            dtPalletD = CreatePalletDDataTable(item.clsPdfBarcodesPalletD);
                            dtBeforePalletListbc.Add(dtPalletD);
                        }

                        if (dtBeforePalletList.Count > 0)
                        {
                            dtAfterPalletMerge = DataTableMerge(dtBeforePalletList, "Record" + recordNo.ToString());

                            if (dtAfterPalletMerge != null)
                            {
                                DtList.Add(dtAfterPalletMerge);
                            }
                        }
                        if (dtBeforePalletListbc.Count > 0)
                        {
                            dtAfterPalletMergebc = DataTableMerge(dtBeforePalletListbc, "Record" + recordNo.ToString());
                            if (dtAfterPalletMergebc != null)
                            {
                                DtList.Add(dtAfterPalletMergebc);
                            }
                        }
                    }
                }

                if (DtList.Count > 0)
                {
                    string date1 = dateFrom.ToString("yyyyMMdd");
                    string date2 = dateTo.ToString("yyyyMMdd");
                    //PDFFunction.ExportToPdf(DtList, date1 + "_" + date2, clsPdfFullDataVariableList);
                }
            }
            catch
            {
                throw;
            }

        }
    }
}
