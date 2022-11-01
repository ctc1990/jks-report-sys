using JKS_Report.Function.DB;
using JKS_Report.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JKS_Report.Function.Tools
{
    public class CSVFunction
    {
        public static DataTable CreatePLCDataCsvTable<T>(IEnumerable<T> list, string SymbolOnNewRow = "", string PlcRename = "")
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
            dataTable.TableName = typeof(T).FullName;
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


            dataTable.Rows.Add(SymbolList);


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
        public static DataTable CreateMainDataCsvTable(plcMainVariable list, string PlcRename = "")
        {
            var assembly = Assembly.GetExecutingAssembly();

            string[] MainSymbolList = null;

            if (!string.IsNullOrEmpty(PlcRename))
            {
                using (Stream stream = assembly.GetManifestResourceStream(PlcRename))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    MainSymbolList = result.Split(',');
                }
            }

            Type type = typeof(plcMainVariable);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(plcMainVariable).FullName;
            if (MainSymbolList != null)
            {
                foreach (var item in MainSymbolList)
                {
                    dataTable.Columns.Add(item);
                }
            }
            //foreach (PropertyInfo info in properties)
            //{
            //    dataTable.Columns.Add(info.Name);
            //}          
            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(list);
            }

            dataTable.Rows.Add(values);


            return dataTable;
        }
        public static void ToCSV(List<DataTable> dtDataTable, string Info,bool dateReport)
        {
            string filePath = LibDBHelper.CreateCsvFile("Amsonic", Info, dateReport);

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                foreach (DataTable item in dtDataTable)
                {
                    sw.Write(sw.NewLine);
                    //headers  
                    for (int i = 0; i < item.Columns.Count; i++)
                    {
                        sw.Write(item.Columns[i]);
                        if (i < item.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                    //rows
                    foreach (DataRow dr in item.Rows)
                    {
                        for (int i = 0; i < item.Columns.Count; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                string value = dr[i].ToString();
                                if (value.Contains(','))
                                {
                                    value = String.Format("\"{0}\"", value);
                                    sw.Write(value);
                                }
                                else
                                {
                                    sw.Write(dr[i].ToString());
                                }
                            }
                            if (i < item.Columns.Count - 1)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                }
            }
        }
        public static DataTable CreateSinglePLCDataTable<T>(IEnumerable<T> list, string recordName, string SymbolOnNewRow = "", string PlcRename = "")
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
        public static DataTable CreateSingleMainDataTable(clsCsvMainVariableSingle list, string recordName, string MainSymbol = "")
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

            Type type = typeof(clsCsvMainVariableSingle);
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
    }
}
