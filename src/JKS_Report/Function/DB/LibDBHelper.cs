using Dapper;
using JKS_Report.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace JKS_Report.Function.DB
{
    public class LibDBHelper
    {
        public static DateTime DateTimeNow = new DateTime();

        public static string ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static MySqlConnection _DataBaseConnection;
        private static readonly object lockObject = new object();
        public static MySqlConnection DataBaseConnection
        {
            get
            {
                if (_DataBaseConnection == null)
                {
                    _DataBaseConnection = new MySqlConnection(ConnectionString);

                }

                return _DataBaseConnection;
            }
        }
        public static void DisposeConnection()
        {
            if (_DataBaseConnection != null)
            {
                if (_DataBaseConnection.State == System.Data.ConnectionState.Connecting || _DataBaseConnection.State == System.Data.ConnectionState.Open || _DataBaseConnection.State == System.Data.ConnectionState.Executing || _DataBaseConnection.State == System.Data.ConnectionState.Fetching)
                {
                    _DataBaseConnection.Close();
                    _DataBaseConnection.Dispose();
                }

                _DataBaseConnection = null;
            }
        }
        public static void Open()
        {
            DataBaseConnection.Open();
        }
        public static string CreatePdfFile(string sourceName, string lang, bool dateReport,string filepath)
        {
            bool writeLogHaveError = false;

            //string LogPath = WebConfigurationManager.AppSettings.Get("PDFFilePath");           

            string LogPath = filepath + "\\PDF";

            string logFileDateStr = DateTime.Now.ToString("yyyyMMdd'-'HHmm");

            int fileNameIdentityNum = 1;
            string latestFileName = "", todayLogFilePath = "";

            if (!dateReport)
            {
                latestFileName = logFileDateStr + "_" +sourceName ;
                todayLogFilePath = LogPath + "\\" + latestFileName  + ".pdf";
            }           


            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
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
                    writeLogHaveError = true;
                }
            }

            DirectoryInfo d = new DirectoryInfo(LogPath);//Assuming Test is your Folder
            FileInfo[] logFiles = d.GetFiles(latestFileName + "*.pdf");
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

                todayLogFilePath = latestFile.FullName;
            }


            while (writeLogHaveError)
            {
                fileNameIdentityNum++;

                int i = 0;
                while (i <= 3)
                {
                    try
                    {
                        lock (lockObject)
                        {
                            FileStream file = new FileStream(LogPath + latestFileName + "-" + fileNameIdentityNum.ToString() + ".pdf", FileMode.Append, FileAccess.Write, FileShare.Read);
                            file.Close();
                        }
                        writeLogHaveError = false;
                        break;
                    }
                    catch (Exception)
                    {
                        writeLogHaveError = true;
                        i++;
                    }
                }
            }

            return todayLogFilePath;
        }
        public static string CreateCsvFile(string sourceName, string lang, bool dateReport, string FilePath)
        {
            bool writeLogHaveError = false;
            //string LogPath = WebConfigurationManager.AppSettings.Get("CSVFilePath");          

            string LogPath = FilePath + "\\CSV";

            string logFileDateStr = DateTime.Now.ToString("yyyyMMdd'-'HHmm");

            int fileNameIdentityNum = 1;
            string latestFileName = "", todayLogFilePath = "";

            if (!dateReport)
            {
                latestFileName = logFileDateStr + "_" + sourceName;
                todayLogFilePath = LogPath + "\\" + latestFileName  + ".csv";
            }


            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
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
                    writeLogHaveError = true;
                }
            }

            DirectoryInfo d = new DirectoryInfo(LogPath);//Assuming Test is your Folder
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

                todayLogFilePath = latestFile.FullName;
            }

            while (writeLogHaveError)
            {
                fileNameIdentityNum++;

                int i = 0;
                while (i <= 3)
                {
                    try
                    {
                        lock (lockObject)
                        {
                            FileStream file = new FileStream(LogPath + latestFileName + "-" + fileNameIdentityNum.ToString() + ".csv", FileMode.Append, FileAccess.Write, FileShare.Read);
                            file.Close();
                        }
                        writeLogHaveError = false;
                        break;
                    }
                    catch (Exception)
                    {
                        writeLogHaveError = true;
                        i++;
                    }
                }
            }

            return todayLogFilePath;
        }
        protected static int GetFileNameIdentityNum(FileInfo fileInfo, out string fileName)
        {
            int fileNameIdentityNum = 0;

            fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string[] fileNameArr = fileName.Split('_');
            if (fileNameArr.Length >= 2)
            {
                fileName = fileNameArr[0];

                int.TryParse(fileNameArr[fileNameArr.Length - 1], out fileNameIdentityNum);
            }

            return fileNameIdentityNum;
        }
        public static int MainStationRecord(clsMainVariable _clsMainVariable)
        {
            int result = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    
                    string Query = @"INSERT IGNORE INTO mainvariable "
                                + "(`UserName`,`TimeIn`, `LoadingId`, `UnloadingId`, `BasketNumber`, `RecipeNo`, `RecipeDescription`, `LoadingNo`, `ProgrammeBarcode`, `ProgrammeNumber`, `BasketBarcode`, `LoadingTotalNo`, `CreatedOn`) VALUES "
                                + "(@Username,@TimeIn ,@LoadingId,@UnloadingId,@BasketNumber, @RecipeNo,@RecipeDescription ,@LoadingNo,@ProgrammeBarcode,@ProgrammeNumber,@BasketBarcode, @LoadingTotalNo, @CreatedOn); SELECT LAST_INSERT_ID();";

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Username", _clsMainVariable.Username, DbType.String, ParameterDirection.Input);
                    parameters.Add("@TimeIn", _clsMainVariable.TimeIn, DbType.String, ParameterDirection.Input);
                    parameters.Add("@LoadingId", _clsMainVariable.LoadingId, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@UnloadingId", _clsMainVariable.UnloadingId, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@BasketNumber", _clsMainVariable.BasketNumber, DbType.String, ParameterDirection.Input);
                    parameters.Add("@RecipeNo", _clsMainVariable.RecipeNo, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@RecipeDescription", _clsMainVariable.RecipeDescription, DbType.String, ParameterDirection.Input);
                    parameters.Add("@LoadingNo", _clsMainVariable.LoadingNo, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@ProgrammeBarcode", _clsMainVariable.ProgrammeBarcode, DbType.String, ParameterDirection.Input);
                    parameters.Add("@ProgrammeNumber", _clsMainVariable.ProgrammeNumber, DbType.String, ParameterDirection.Input);
                    parameters.Add("@BasketBarcode", _clsMainVariable.BasketBarcode, DbType.String, ParameterDirection.Input);
                    parameters.Add("@LoadingTotalNo", _clsMainVariable.LoadingTotalNo, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@CreatedOn", _clsMainVariable.CreatedOn, DbType.DateTime, ParameterDirection.Input);

                    result = connection.Query<int>(Query, parameters).FirstOrDefault();                  
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
        public static int SingleStationRecord(clsStationVariable _clsPlcVariable)
        {
            int result = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {                   
                    //string Query = @"INSERT IGNORE INTO plcvariable "
                    //            + "(ReferenceId, StationNo, SequenceRecipe, SubRecipe, MinimumTime, MaximumTime, EffectiveTime,"
                    //            + " TemperatureSV, TemperaturePV, USonicSideAPowerSV, USonicSideAPowerPV, USonicSideAFrequency, USonicSideBPowerSV, USonicSideBPowerPV, "
                    //            + " USonicSideBFrequency, USonicBottomAPowerSV, USonicBottomAPowerPV, USonicBottomAFrequency, USonicBottomBPowerSV, USonicBottomBPowerPV, "
                    //            + "USonicBottomBFrequency, VacuumSV, VacuumPV, ConductivityPV, PumpFlowPV, ResistivityPV, PhPV, Quality, ActualTime, CreatedOn) VALUES "
                    //            + "(@ReferenceId, @StationNo, @SequenceRecipe, @SubRecipe,@MinimumTime, @MaximumTime, @EffectiveTime,"
                    //            + "@TemperatureSV, @TemperaturePV, @USonicSideAPowerSV, "
                    //            + "@USonicSideAPowerPV, @USonicSideAFrequency, @USonicSideBPowerSV, @USonicSideBPowerPV, @USonicSideBFrequency, @USonicBottomAPowerSV, "
                    //            + "@USonicBottomAPowerPV, @USonicBottomAFrequency, @USonicBottomBPowerSV, @USonicBottomBPowerPV, @USonicBottomBFrequency, @VacuumSV, "
                    //            + "@VacuumPV, @ConductivityPV, @PumpFlowPV, @ResistivityPV, @PhPV, @Quality, @ActualTime, @CreatedOn); SELECT LAST_INSERT_ID();";

                    string Query = @"INSERT INTO `plcvariable` (`ReferenceId`,`TimeIn`,`RefLoadingNo`, `StationNo`,`Description`, `SequenceRecipe`, `SubRecipe`, `MinimumTime`, `MaximumTime`, `EffectiveTime`, `TemperatureSV`, `TemperaturePV`, `USonicSideAPowerSV`, `USonicSideAPowerPV`, `USonicSideAFrequency`, `USonicSideBPowerSV`, `USonicSideBPowerPV`, `USonicSideBFrequency`, `USonicBottomAPowerSV`, `USonicBottomAPowerPV`, `USonicBottomAFrequency`, `USonicBottomBPowerSV`, `USonicBottomBPowerPV`, `USonicBottomBFrequency`, `VacuumSV`, `VacuumPV`, `ConductivityPV`, `PumpFlowPV`, `ResistivityPV`, `PhPV`, `Quality`, `ActualTime`, `CreatedOn`) VALUES "
                                  + "(@ReferenceId,@TimeIn,@RefLoadingNo,@StationNo,@Description,@SequenceRecipe,@SubRecipe,@MinimumTime,@MaximumTime,@EffectiveTime,@TemperatureSV, @TemperaturePV, @USonicSideAPowerSV,@USonicSideAPowerPV, @USonicSideAFrequency, @USonicSideBPowerSV, @USonicSideBPowerPV, @USonicSideBFrequency, @USonicBottomAPowerSV,@USonicBottomAPowerPV, @USonicBottomAFrequency, @USonicBottomBPowerSV, @USonicBottomBPowerPV, @USonicBottomBFrequency, @VacuumSV,@VacuumPV, @ConductivityPV, @PumpFlowPV, @ResistivityPV, @PhPV, @Quality, @ActualTime, @CreatedOn); SELECT LAST_INSERT_ID();";

                    //DynamicParameters parameters = new DynamicParameters();
                    //parameters.Add("@ReferenceId", _clsPlcVariable.ReferenceID, DbType.Int64, ParameterDirection.Input);
                    //parameters.Add("@StationNo", _clsPlcVariable.StationNo, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@SequenceRecipe", _clsPlcVariable.SequenceRecipe, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@SubRecipe", _clsPlcVariable.SubRecipe, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@MinimumTime", _clsPlcVariable.MinimumTime, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@MaximumTime", _clsPlcVariable.MaximumTime, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@EffectiveTime", _clsPlcVariable.EffectiveTime, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@TemperatureSV", _clsPlcVariable.TemperatureSV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@TemperaturePV", _clsPlcVariable.TemperaturePV, DbType.Decimal, ParameterDirection.Input);
                    //parameters.Add("@USonicSideAPowerSV", _clsPlcVariable.USonicSideAPowerSV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicSideAPowerPV", _clsPlcVariable.USonicSideAPowerPV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicSideAFrequency", _clsPlcVariable.USonicSideAFrequency, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicSideBPowerSV", _clsPlcVariable.USonicSideBPowerSV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicSideBPowerPV", _clsPlcVariable.USonicSideBPowerPV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicSideBFrequency", _clsPlcVariable.USonicSideBFrequency, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicBottomAPowerSV", _clsPlcVariable.USonicBottomAPowerSV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicBottomAPowerPV", _clsPlcVariable.USonicBottomAPowerPV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicBottomAFrequency", _clsPlcVariable.USonicBottomAFrequency, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicBottomBPowerSV", _clsPlcVariable.USonicBottomBPowerSV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicBottomBPowerPV", _clsPlcVariable.USonicBottomBPowerPV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@USonicBottomBFrequency", _clsPlcVariable.USonicBottomBFrequency, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@VacuumSV", _clsPlcVariable.VacuumSV, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@VacuumPV", _clsPlcVariable.VacuumPV, DbType.Decimal, ParameterDirection.Input);
                    //parameters.Add("@ConductivityPV", _clsPlcVariable.ConductivityPV, DbType.Decimal, ParameterDirection.Input);
                    //parameters.Add("@PumpFlowPV", _clsPlcVariable.PumpFlowPV, DbType.Decimal, ParameterDirection.Input);
                    //parameters.Add("@ResistivityPV", _clsPlcVariable.ResistivityPV, DbType.Decimal, ParameterDirection.Input);
                    //parameters.Add("@PhPV", _clsPlcVariable.PhPV, DbType.Decimal, ParameterDirection.Input);
                    //parameters.Add("@Quality", _clsPlcVariable.Quality, DbType.Decimal, ParameterDirection.Input);
                    //parameters.Add("@ActualTime", _clsPlcVariable.ActualTime, DbType.Int32, ParameterDirection.Input);
                    //parameters.Add("@CreatedOn", _clsPlcVariable.CreatedOn, DbType.DateTime, ParameterDirection.Input);

                    //result = connection.Query<int>(Query, parameters).FirstOrDefault();

                    result = connection.Query<int>(Query, new
                    {
                        ReferenceId = _clsPlcVariable.ReferenceID,
                        StationNo = _clsPlcVariable.StationNo,
                        TimeIn = _clsPlcVariable.TimeIn,
                        RefLoadingNo = _clsPlcVariable.RefLoadingNo,
                        Description = _clsPlcVariable.Description,
                        SequenceRecipe = _clsPlcVariable.SequenceRecipe,
                        SubRecipe = _clsPlcVariable.SubRecipe,
                        MinimumTime = _clsPlcVariable.MinimumTime,
                        MaximumTime = _clsPlcVariable.MaximumTime,
                        EffectiveTime = _clsPlcVariable.EffectiveTime,
                        TemperatureSV = _clsPlcVariable.TemperatureSV,
                        TemperaturePV = _clsPlcVariable.TemperaturePV,
                        USonicSideAPowerSV = _clsPlcVariable.USonicSideAPowerSV,
                        USonicSideAPowerPV = _clsPlcVariable.USonicSideAPowerPV,
                        USonicSideAFrequency = _clsPlcVariable.USonicSideAFrequency,
                        USonicSideBPowerSV = _clsPlcVariable.USonicSideBPowerSV,
                        USonicSideBPowerPV = _clsPlcVariable.USonicSideBPowerPV,
                        USonicSideBFrequency = _clsPlcVariable.USonicSideBFrequency,
                        USonicBottomAPowerSV = _clsPlcVariable.USonicBottomAPowerSV,
                        USonicBottomAPowerPV = _clsPlcVariable.USonicBottomAPowerPV,
                        USonicBottomAFrequency = _clsPlcVariable.USonicBottomAFrequency,
                        USonicBottomBPowerSV = _clsPlcVariable.USonicBottomBPowerSV,
                        USonicBottomBPowerPV = _clsPlcVariable.USonicBottomBPowerPV,
                        USonicBottomBFrequency = _clsPlcVariable.USonicBottomBFrequency,
                        VacuumSV = _clsPlcVariable.VacuumSV,
                        VacuumPV = _clsPlcVariable.VacuumPV,
                        ConductivityPV = _clsPlcVariable.ConductivityPV,
                        PumpFlowPV = _clsPlcVariable.PumpFlowPV,
                        ResistivityPV = _clsPlcVariable.ResistivityPV,
                        PhPV = _clsPlcVariable.PhPV,
                        Quality = _clsPlcVariable.Quality,
                        ActualTime = _clsPlcVariable.ActualTime,
                        CreatedOn = _clsPlcVariable.CreatedOn
                    }).FirstOrDefault();                 
                }
            }
            catch
            {

                throw;
            }

            return result;
        }
        public static int BarcodeMemory(clsPartMemory _clsPartMemory)
        {
            int result = 0;
            try
            {
                if (_clsPartMemory != null)
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        string Query = "INSERT IGNORE INTO `partmemory` " +
                                       "(`ReferenceId`, `PalletAActivated`, `PalletBActivated`, `PalletCActivated`, `PalletDActivated`,`PalletA`, `PalletA_WO1`, `PalletA_WO2`, `PalletA_WO3`, `PalletA_WO4`, `PalletA_WO5`, `PalletA_WO6`, `PalletA_WO7`, `PalletA_WO8`, `PalletB_WO8`, `PalletC_WO8`, `PalletD_WO8`, `PalletB`, `PalletB_WO1`, `PalletB_WO2`, `PalletB_WO3`, `PalletB_WO4`, `PalletB_WO5`, `PalletB_WO6`, `PalletB_WO7`, `PalletC`, `PalletC_WO1`, `PalletC_WO2`, `PalletC_WO3`, `PalletC_WO4`, `PalletC_WO5`, `PalletC_WO6`, `PalletC_WO7`, `PalletD`, `PalletD_WO1`, `PalletD_WO2`, `PalletD_WO3`, `PalletD_WO4`, `PalletD_WO5`, `PalletD_WO6`, `PalletD_WO7`) " +
                                       " VALUES " +
                                       "(@ReferenceId, @PalletAActivated, @PalletBActivated, @PalletCActivated, @PalletDActivated, @PalletA, @PalletA_WO1, @PalletA_WO2, @PalletA_WO3, @PalletA_WO4, @PalletA_WO5, @PalletA_WO6, @PalletA_WO7, @PalletA_WO8, @PalletB_WO8, @PalletC_WO8, @PalletD_WO8, @PalletB, @PalletB_WO1, @PalletB_WO2, @PalletB_WO3, @PalletB_WO4, @PalletB_WO5, @PalletB_WO6, @PalletB_WO7, @PalletC, @PalletC_WO1, @PalletC_WO2, @PalletC_WO3, PalletC_WO4, @PalletC_WO5, @PalletC_WO6, @PalletC_WO7, @PalletD, @PalletD_WO1, @PalletD_WO2, PalletD_WO3, @PalletD_WO4, @PalletD_WO5, @PalletD_WO6, @PalletD_WO7)";

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@ReferenceId", _clsPartMemory.ReferenceID, DbType.Int64, ParameterDirection.Input);
                        parameters.Add("@PalletAActivated", _clsPartMemory.PalletAActivated, DbType.Boolean, ParameterDirection.Input);
                        parameters.Add("@PalletA", _clsPartMemory.PalletA, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO1", _clsPartMemory.PalletA_WO1, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO2", _clsPartMemory.PalletA_WO2, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO3", _clsPartMemory.PalletA_WO3, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO4", _clsPartMemory.PalletA_WO4, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO5", _clsPartMemory.PalletA_WO5, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO6", _clsPartMemory.PalletA_WO6, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO7", _clsPartMemory.PalletA_WO7, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletA_WO8", _clsPartMemory.PalletA_WO8, DbType.String, ParameterDirection.Input);

                        parameters.Add("@PalletBActivated", _clsPartMemory.PalletBActivated, DbType.Boolean, ParameterDirection.Input);
                        parameters.Add("@PalletB", _clsPartMemory.PalletB, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO1", _clsPartMemory.PalletB_WO1, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO2", _clsPartMemory.PalletB_WO2, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO3", _clsPartMemory.PalletB_WO3, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO4", _clsPartMemory.PalletB_WO4, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO5", _clsPartMemory.PalletB_WO5, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO6", _clsPartMemory.PalletB_WO6, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO7", _clsPartMemory.PalletB_WO7, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletB_WO8", _clsPartMemory.PalletB_WO8, DbType.String, ParameterDirection.Input);

                        parameters.Add("@PalletCActivated", _clsPartMemory.PalletCActivated, DbType.Boolean, ParameterDirection.Input);
                        parameters.Add("@PalletC", _clsPartMemory.PalletC, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO1", _clsPartMemory.PalletC_WO1, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO2", _clsPartMemory.PalletC_WO2, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO3", _clsPartMemory.PalletC_WO3, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO4", _clsPartMemory.PalletC_WO4, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO5", _clsPartMemory.PalletC_WO5, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO6", _clsPartMemory.PalletC_WO6, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO7", _clsPartMemory.PalletC_WO7, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletC_WO8", _clsPartMemory.PalletC_WO8, DbType.String, ParameterDirection.Input);

                        parameters.Add("@PalletDActivated", _clsPartMemory.PalletDActivated, DbType.Boolean, ParameterDirection.Input);
                        parameters.Add("@PalletD", _clsPartMemory.PalletD, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO1", _clsPartMemory.PalletD_WO1, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO2", _clsPartMemory.PalletD_WO2, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO3", _clsPartMemory.PalletD_WO3, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO4", _clsPartMemory.PalletD_WO4, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO5", _clsPartMemory.PalletD_WO5, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO6", _clsPartMemory.PalletD_WO6, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO7", _clsPartMemory.PalletD_WO7, DbType.String, ParameterDirection.Input);
                        parameters.Add("@PalletD_WO8", _clsPartMemory.PalletD_WO8, DbType.String, ParameterDirection.Input);

                        result = connection.Query<int>(Query, parameters).FirstOrDefault();
                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        public static int UpdateSystemSetting(clsSystemSetting _clsSystemSetting)
        {
            int result = 0;
            try
            {

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {

                    string Query = @"UPDATE systemsetting SET Machine = @Machine, Name = @Name, Software = @Software, ModifiedOn = @ModifiedOn WHERE ReferenceKey = 'PDFHeaderSetting';";

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Machine", _clsSystemSetting.Machine, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Name", _clsSystemSetting.Name, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Software", _clsSystemSetting.Software, DbType.String, ParameterDirection.Input);
                    parameters.Add("@ModifiedOn", _clsSystemSetting.ModifiedOn, DbType.DateTime, ParameterDirection.Input);

                    int regardId = connection.Query<int>(Query, parameters).FirstOrDefault();

                    if (regardId > 0)
                    {
                        result++;
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
        public static clsSystemSetting getSystemSettings()
        {
            clsSystemSetting result = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string query = @"SELECT * FROM systemsetting WHERE ReferenceKey = 'PDFHeaderSetting'";
                    result = connection.Query<clsSystemSetting>(query).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
        public static clsMainVariable getMainRecord(int recipeNo, int loadingNo)
        {
            clsMainVariable result = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string query = @"SELECT * FROM mainvariable where LoadingNo = @LoadingNo AND RecipeNo = @RecipeNo ORDER BY CreatedOn DESC LIMIT 1";
                    result = connection.Query<clsMainVariable>(query, new { LoadingNo = loadingNo, recipeNo = recipeNo }).SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
        public static List<clsSystemSetting> getFilePath()
        {
            List<clsSystemSetting> result = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string query = @"SELECT * FROM systemsetting WHERE ReferenceKey LIKE '%FilePath%'";
                    result = connection.Query<clsSystemSetting>(query).ToList();
                }
            }
            catch
            {
                throw;
            }

            return result;
        }       
        public static int UpdateFilePath(clsSystemSetting _clsSystemSetting, string recordName)
        {
            int result = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {

                    string Query = @"UPDATE systemsetting SET Machine = @Machine, Name = @Name, Software = @Software, ModifiedOn = @ModifiedOn WHERE ReferenceKey = @recordname;";

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Machine", _clsSystemSetting.Machine, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Name", _clsSystemSetting.Name, DbType.String, ParameterDirection.Input);
                    parameters.Add("@Software", _clsSystemSetting.Software, DbType.String, ParameterDirection.Input);
                    parameters.Add("@ModifiedOn", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                    parameters.Add("@recordname", recordName, DbType.String, ParameterDirection.Input);

                    int regardId = connection.Query<int>(Query, parameters).FirstOrDefault();

                    if (regardId > 0)
                    {
                        result++;
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;
        }      
        public static int UpdateMainTimeRecord(string RecipeNo, string LoadingNo, string TimeOut)
        {
            int result = 0;
            try
            {
                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "UpdateMainTime : " + RecipeNo + "," + LoadingNo + ","+ TimeOut + Environment.NewLine);
                if (!string.IsNullOrEmpty(RecipeNo))
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {

                        string query = @"UPDATE mainvariable SET TimeOut = @TimeOut WHERE RecipeNo = @RecipeNo AND LoadingNo = @LoadingNo ORDER BY CreatedOn DESC limit 1";

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@TimeOut", TimeOut, DbType.String, ParameterDirection.Input);
                        parameters.Add("@RecipeNo", RecipeNo, DbType.Int16, ParameterDirection.Input);
                        parameters.Add("@LoadingNo", LoadingNo, DbType.Int16, ParameterDirection.Input);

                        result = connection.Query<int>(query, parameters).FirstOrDefault();

                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        public static int UpdateStationTimeRecord(int StationNo, string LoadingNo, string TimeOut)
        {
            int result = 0;
            try
            {
                if (!string.IsNullOrEmpty(LoadingNo))
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        string query = @"UPDATE plcvariable SET TimeOut = @TimeOut WHERE StationNo = @StationNo AND RefLoadingNo = @LoadingNo ORDER BY CreatedOn DESC limit 1";

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@TimeOut", TimeOut, DbType.String, ParameterDirection.Input);
                        parameters.Add("@StationNo", StationNo, DbType.Int16, ParameterDirection.Input);
                        parameters.Add("@LoadingNo", LoadingNo, DbType.String, ParameterDirection.Input);

                        result = connection.Query<int>(query, parameters).FirstOrDefault();

                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        public static clsStationVariable GetStationRecord(int StationNo, string LoadingNo)
        {
            clsStationVariable result = null;
            try
            {
               
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@StationNo", StationNo, DbType.Int16, ParameterDirection.Input);
                    parameters.Add("@RefLoadingNo", LoadingNo, DbType.String, ParameterDirection.Input);

                    string query = @"SELECT * FROM plcvariable WHERE RefLoadingNo = @RefLoadingNo AND StationNo = @StationNo ORDER BY CreatedOn DESC limit 1";
                    result = connection.Query<clsStationVariable>(query, parameters).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}
