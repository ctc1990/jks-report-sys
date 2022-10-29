using JKS_Report.Model;
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
    public class CSVTool
    {
        static string CSVPath = WebConfigurationManager.AppSettings.Get("CsvPath");
        public static DataTable CreatePLCDataTable<T>(IEnumerable<T> list)
        {

            var assembly = Assembly.GetExecutingAssembly();

            string[] columnlist = null;

            using (Stream stream = assembly.GetManifestResourceStream("JKS_Report.Text.CsvColumn.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                columnlist = result.Split(',');
            }

            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            var j = 0;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(info.Name + " " + columnlist[j]);
                j++;
            }

            dataTable.Rows.Add(columnlist);

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
        public static DataTable CreateMainDataTable(clsMainVariable list)
        {
            Type type = typeof(clsMainVariable);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(clsMainVariable).FullName;
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


            return dataTable;
        }
        public static DataTable CreateMemoryDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            var j = 0;
            foreach (PropertyInfo info in properties)
            {

                dataTable.Columns.Add(info.Name);

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
        public static void dataTableToCSV(List<DataTable> dtDataTable, string sourceName)
        {
            string fileName = DirectoryFolder(sourceName);


            using (StreamWriter sw = new StreamWriter(CSVPath + "/" + fileName, true))
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
        public static string DirectoryFolder(string sourceName)
        {
            string result = null;
            string logFileDateStr = DateTime.Now.ToString("yyyyMMdd");
            string latestFileName = sourceName + "_" + logFileDateStr;
            string todayLogFilePath = CSVPath + sourceName + "_" + logFileDateStr + ".csv";
            int fileNameIdentityNum = 1;

            try
            {
                if (!Directory.Exists(CSVPath))
                {
                    Directory.CreateDirectory(CSVPath);
                }

                if (!File.Exists(todayLogFilePath))
                {
                    try
                    {
                        FileStream file = new FileStream(todayLogFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                        file.Close();
                    }
                    catch (Exception)
                    {

                    }
                }

                DirectoryInfo d = new DirectoryInfo(CSVPath);//Assuming Test is your Folder
                FileInfo[] logFiles = d.GetFiles(latestFileName + "*.csv");
                if (logFiles.Length > 0)
                {
                    Array.Sort(logFiles, delegate (FileInfo f1, FileInfo f2)
                    {
                        return f1.CreationTime.CompareTo(f2.CreationTime) * -1;
                    });

                    FileInfo latestFile = logFiles[0];
                    fileNameIdentityNum = GetFileNameIdentityNum(latestFile, out latestFileName);
                    if (fileNameIdentityNum == 0 && logFiles.Length > 1)
                    {
                        FileInfo nextFile = logFiles[1];
                        fileNameIdentityNum = GetFileNameIdentityNum(nextFile, out latestFileName);
                    }

                    result = latestFile.FullName;
                }
                else
                {
                    result = todayLogFilePath;
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        protected static int GetFileNameIdentityNum(FileInfo fileInfo, out string fileName)
        {
            int fileNameIdentityNum = 0;

            fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string[] fileNameArr = fileName.Split('-');
            if (fileNameArr.Length >= 2)
            {
                fileName = fileNameArr[0];

                int.TryParse(fileNameArr[fileNameArr.Length - 1], out fileNameIdentityNum);
            }

            return fileNameIdentityNum;
        }
    }
}
