using Dapper;
using JKS_Report.Model;
using JKS_Report.Function.DB;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Web.Hosting;
using MySql.Data.MySqlClient;
using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web.Configuration;
using JKS_Report.Function.Tools;
using System.Globalization;

namespace JKS_Report.Function.API
{
    public class DateReport
    {
        static string ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public static void PDFGenerate(DateTime dateFrom, DateTime dateTo, string Lang, clsSystemSetting clsSystemSetting)
        {
            List<clsPdfFullDataVariable> clsPdfFullDataVariableList = new List<clsPdfFullDataVariable>();
            
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
                            clsPdfMainVariable.RecipeNo = item.RecipeNo.ToString();
                            clsPdfMainVariable.RecipeDescription = item.RecipeDescription.ToString();
                            clsPdfMainVariable.TimeStart = item.TimeStart.ToString();
                            clsPdfMainVariable.TimeEnd = item.TimeEnd.ToString();
                            clsPdfMainVariable.NumberOfBasket = item.NumberOfBasket.ToString();

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
                                _clsPdfPlcVariable.TimeIn = item1.TimeIn.ToShortTimeString();
                                _clsPdfPlcVariable.TimeOut = item1.TimeOut.ToShortTimeString();
                                _clsPdfPlcVariable.StationNo = item1.StationNo.ToString() + "-" + item1.Description.ToString();
                                _clsPdfPlcVariable.Quality = item1.Quality;
                                _clsPdfPlcVariable.SequenceRecipe = item1.SequenceRecipe.ToString();
                                _clsPdfPlcVariable.EffectiveTime = item1.EffectiveTime.ToString();
                                _clsPdfPlcVariable.TemperaturePV = item1.TemperaturePV.ToString();
                                _clsPdfPlcVariable.UltrasonicBottomAPower = item1.USonicBottomAPowerPV.ToString();
                                _clsPdfPlcVariable.UltrasonicSideAPower = item1.USonicBottomBPowerPV.ToString();
                                _clsPdfPlcVariable.ConductivityPV = item1.ConductivityPV.ToString();
                                _clsPdfPlcVariable.PumpFlow = item1.PumpFlowPV.ToString();
                                clsPdfPlcVariableList.Add(_clsPdfPlcVariable);
                            }

                            clsPdfFullDataVariableItem.clsPdfPlcVariables = clsPdfPlcVariableList;

                            if (_clsPartMemory != null)
                            {
                                clsPdfFullDataVariableItem.clsPdfBarcodesPalletA = new clsPdfBarcodePalletA();
                                clsPdfFullDataVariableItem.clsPdfBarcodesPalletB = new clsPdfBarcodePalletB();
                                clsPdfFullDataVariableItem.clsPdfBarcodesPalletC = new clsPdfBarcodePalletC();
                                clsPdfFullDataVariableItem.clsPdfBarcodesPalletD = new clsPdfBarcodePalletD();

                                if (!string.IsNullOrEmpty(_clsPartMemory.PalletA))
                                {
                                    
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

                clsLang clsLang = ReportLanguage(Lang);
                string language = "";
                if (Lang.ToLower() == "english")
                {
                    language = "EN";
                }
                else
                {
                    language = "GE";
                }

                string date1 = dateFrom.ToString("yyyyMMdd");
                string date2 = dateTo.ToString("yyyyMMdd");
                MultipleRecordReport.ExportToPdf(date1 + "_" + date2, clsPdfFullDataVariableList, clsLang, clsSystemSetting, language);

            }
            catch
            {
                throw;
            }

        }
        public static void CSVGenerate(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<plcCsvVariable> _plcCsvVariableList = new List<plcCsvVariable>();
                DataTable dtMain;
                DataTable dtPLC;
                List<DataTable> DtList = new List<DataTable>();

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
                            plcCsvVariable _plcCsvVariableItem = new plcCsvVariable();
                            Query = "Select * from plcvariable where ReferenceId = @ReferenceId";
                            parameters = new DynamicParameters();
                            parameters.Add("@ReferenceId", item.Id, DbType.Int32, ParameterDirection.Input);
                            List<clsStationVariable> _clsPlcVariableList = connection.Query<clsStationVariable>(Query, parameters).ToList();

                            Query = @"SELECT * FROM partmemory WHERE ReferenceId = @ReferenceId";
                            parameters = new DynamicParameters();
                            parameters.Add("@ReferenceId", item.Id, DbType.Int32, ParameterDirection.Input);
                            clsPartMemory _clsPartMemory = connection.Query<clsPartMemory>(Query, parameters).FirstOrDefault();

                            plcMainVariable clsCSVMainVariable = new plcMainVariable();
                            clsCSVMainVariable.Username = item.Username;
                            clsCSVMainVariable.LoadingId = item.LoadingId.ToString();
                            clsCSVMainVariable.UnloadingId = item.UnloadingId.ToString();
                            clsCSVMainVariable.RecipeNo = item.RecipeNo.ToString();
                            clsCSVMainVariable.RecipeDescription = item.RecipeDescription.ToString();
                            clsCSVMainVariable.LoadingNo = item.LoadingNo.ToString();
                            clsCSVMainVariable.LoadingTotalNo = item.LoadingTotalNo.ToString();
                            clsCSVMainVariable.ProgrammeBarcode = item.ProgrammeBarcode;
                            clsCSVMainVariable.ProgrammeNo = item.ProgrammeNumber;
                            clsCSVMainVariable.BasketBarcode = item.BasketBarcode.ToString();
                            clsCSVMainVariable.BasketNumber = item.BasketNumber.ToString();

                            clsCSVMainVariable.PalletA = _clsPartMemory.PalletA;
                            clsCSVMainVariable.PalletA_WO1 = _clsPartMemory.PalletA_WO1;
                            clsCSVMainVariable.PalletA_WO2 = _clsPartMemory.PalletA_WO2;
                            clsCSVMainVariable.PalletA_WO3 = _clsPartMemory.PalletA_WO3;
                            clsCSVMainVariable.PalletA_WO4 = _clsPartMemory.PalletA_WO4;
                            clsCSVMainVariable.PalletA_WO5 = _clsPartMemory.PalletA_WO5;
                            clsCSVMainVariable.PalletA_WO6 = _clsPartMemory.PalletA_WO6;
                            clsCSVMainVariable.PalletA_WO7 = _clsPartMemory.PalletA_WO7;
                            clsCSVMainVariable.PalletA_WO8 = _clsPartMemory.PalletA_WO8;
                            clsCSVMainVariable.PalletB = _clsPartMemory.PalletB;
                            clsCSVMainVariable.PalletB_WO1 = _clsPartMemory.PalletB_WO1;
                            clsCSVMainVariable.PalletB_WO2 = _clsPartMemory.PalletB_WO2;
                            clsCSVMainVariable.PalletB_WO3 = _clsPartMemory.PalletB_WO3;
                            clsCSVMainVariable.PalletB_WO4 = _clsPartMemory.PalletB_WO4;
                            clsCSVMainVariable.PalletB_WO5 = _clsPartMemory.PalletB_WO5;
                            clsCSVMainVariable.PalletB_WO6 = _clsPartMemory.PalletB_WO6;
                            clsCSVMainVariable.PalletB_WO7 = _clsPartMemory.PalletB_WO7;
                            clsCSVMainVariable.PalletB_WO8 = _clsPartMemory.PalletB_WO8;
                            clsCSVMainVariable.PalletC = _clsPartMemory.PalletC;
                            clsCSVMainVariable.PalletC_WO1 = _clsPartMemory.PalletC_WO1;
                            clsCSVMainVariable.PalletC_WO2 = _clsPartMemory.PalletC_WO2;
                            clsCSVMainVariable.PalletC_WO3 = _clsPartMemory.PalletC_WO3;
                            clsCSVMainVariable.PalletC_WO4 = _clsPartMemory.PalletC_WO4;
                            clsCSVMainVariable.PalletC_WO5 = _clsPartMemory.PalletC_WO5;
                            clsCSVMainVariable.PalletC_WO6 = _clsPartMemory.PalletC_WO6;
                            clsCSVMainVariable.PalletC_WO7 = _clsPartMemory.PalletC_WO7;
                            clsCSVMainVariable.PalletC_WO8 = _clsPartMemory.PalletC_WO8;
                            clsCSVMainVariable.PalletD = _clsPartMemory.PalletD;
                            clsCSVMainVariable.PalletD_WO1 = _clsPartMemory.PalletD_WO1;
                            clsCSVMainVariable.PalletD_WO2 = _clsPartMemory.PalletD_WO2;
                            clsCSVMainVariable.PalletD_WO3 = _clsPartMemory.PalletD_WO3;
                            clsCSVMainVariable.PalletD_WO4 = _clsPartMemory.PalletD_WO4;
                            clsCSVMainVariable.PalletD_WO5 = _clsPartMemory.PalletD_WO5;
                            clsCSVMainVariable.PalletD_WO6 = _clsPartMemory.PalletD_WO6;
                            clsCSVMainVariable.PalletD_WO7 = _clsPartMemory.PalletD_WO7;
                            clsCSVMainVariable.PalletD_WO8 = _clsPartMemory.PalletD_WO8;

                            _plcCsvVariableItem.csvMainVariable = clsCSVMainVariable;

                            _plcCsvVariableItem.csvStationVariable = new List<clsCsvStation>();
                            foreach (var item1 in _clsPlcVariableList)
                            {
                                clsCsvStation clsCsvStation = new clsCsvStation();
                                clsCsvStation.CreatedOn = item1.CreatedOn.ToShortTimeString();
                                clsCsvStation.StationNo = item1.StationNo;
                                clsCsvStation.SequenceRecipe = item1.SequenceRecipe;
                                clsCsvStation.SubRecipe = item1.SubRecipe;
                                clsCsvStation.MinimumTime = item1.MinimumTime;
                                clsCsvStation.MaximumTime = item1.MaximumTime;
                                clsCsvStation.EffectiveTime = item1.EffectiveTime;
                                clsCsvStation.TemperatureSV = item1.TemperatureSV;
                                clsCsvStation.TemperaturePV = item1.TemperaturePV;
                                clsCsvStation.USonicSideAPowerSV = item1.USonicSideAPowerSV;
                                clsCsvStation.USonicSideAPowerPV = item1.USonicSideAPowerPV;
                                clsCsvStation.USonicSideAFrequency = item1.USonicSideAFrequency;
                                clsCsvStation.USonicSideBPowerSV = item1.USonicSideBPowerSV;
                                clsCsvStation.USonicSideBPowerPV = item1.USonicSideBPowerPV;
                                clsCsvStation.USonicSideBFrequency = item1.USonicSideBFrequency;
                                clsCsvStation.USonicBottomAPowerSV = item1.USonicBottomAPowerSV;
                                clsCsvStation.USonicBottomAPowerPV = item1.USonicBottomAPowerPV;
                                clsCsvStation.USonicBottomAFrequency = item1.USonicBottomAFrequency;
                                clsCsvStation.USonicBottomBPowerSV = item1.USonicBottomBPowerSV;
                                clsCsvStation.USonicBottomBPowerPV = item1.USonicBottomBPowerPV;
                                clsCsvStation.USonicBottomBFrequency = item1.USonicBottomBFrequency;
                                clsCsvStation.ConductivityPV = item1.ConductivityPV;
                                clsCsvStation.VacuumSV = item1.VacuumSV;
                                clsCsvStation.VacuumPV = item1.VacuumPV;
                                clsCsvStation.ResistivityPV = item1.ResistivityPV;
                                clsCsvStation.PhPV = item1.PhPV;
                                clsCsvStation.Quality = item1.Quality;
                                clsCsvStation.ActualTime = item1.ActualTime;

                                _plcCsvVariableItem.csvStationVariable.Add(clsCsvStation);
                            }
                            _plcCsvVariableList.Add(_plcCsvVariableItem);
                        }
                    }

                    if (_plcCsvVariableList.Count > 0)
                    {
                        foreach (var item in _plcCsvVariableList)
                        {
                            dtMain = CSVFunction.CreateMainDataCsvTable(item.csvMainVariable, "JKS_Report.Text.CsvColumnMain_Full.txt");
                            dtPLC = CSVFunction.CreatePLCDataCsvTable(item.csvStationVariable, "JKS_Report.Text.CsvColumn_SymbolPLC_Full.txt", "JKS_Report.Text.CsvColumn_NamePLC_Full.txt");

                            DtList.Add(dtMain);
                            DtList.Add(dtPLC);
                        }

                        string date1 = dateFrom.ToString("yyyyMMdd");
                        string date2 = dateTo.ToString("yyyyMMdd");
                        CSVFunction.ToCSV(DtList, date1 + "_" + date2);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public static clsLang ReportLanguage(string lang)
        {
            clsLang result = null;
            try
            {
                if (lang.ToLower() == "english")
                {
                    result = new clsLang();
                    result.Machine = "Machine:";
                    result.Name = "Name:";
                    result.Software = "Software:";
                    result.Page = "Page:";
                    result.Time = "Date:";
                    result.LoadNumber = "Load Number:";
                    result.TimeStart = "Time Start:";
                    result.TimeEnd = "Time End";
                    result.RecipeNumber = "Recipe Number:";
                    result.RecipeDesc = "Reciep Description:";
                    result.LoadingID = "Loading ID:";
                    result.UnLoadingID = "Unloading ID:";
                    result.Operator = "Operator:";
                    result.BasketNumber = "Basket Number:";
                    result.station = new Station();
                    result.station.tProgramSequence = "Program Sequence";
                    result.station.NumberOfBasket = "Number of Basket:";
                    result.station.TimeIn = "Time In";
                    result.station.TimeOut = "Time Out";
                    result.station.StationName = "Station";
                    result.station.Quality = "Quality";
                    result.station.ProgramSequence = "Sequence Recipe";
                    result.station.EffectiveTime = "Effective Time \n[s]";
                    result.station.Temperature = "Temperature \n[˚C]";
                    result.station.USBottomA = "U/Sonic Bottom A Power \n[%]";
                    result.station.USSideA = "U/Sonic Side A Power \n[%]";
                    result.station.Conductivity = "Conductivity \n[µS/cm]";
                    result.station.Pressure = "Pressure \n[bar]";
                    result.barcode = new Model.Barcode();
                    result.barcode.Orders = "Orders";
                    result.barcode.No = "No.";
                    result.barcode.PalletA = "Pallet A";
                    result.barcode.PalletB = "Pallet B";
                    result.barcode.PalletC = "Pallet C";
                    result.barcode.PalletD = "Pallet D";
                }
                else
                {
                    result = new clsLang();
                    result.Machine = "Maschine:";
                    result.Name = "Name:";
                    result.Software = "Software:";
                    result.Page = "Seite:";
                    result.Time = "Datum:";
                    result.LoadNumber = "Chargen Nummer:";
                    result.TimeStart = "Start Zeit :";
                    result.TimeEnd = "End Zeit:";
                    result.RecipeNumber = "Rezept Nummer:";
                    result.RecipeDesc = "Rezept Name:";
                    result.LoadingID = "Ladestation:";
                    result.UnLoadingID = "Entladestation:";
                    result.Operator = "Benutzer:";
                    result.BasketNumber = "Korbunmmer:";
                    result.station = new Station();
                    result.station.tProgramSequence = "Programmablauf";
                    result.station.NumberOfBasket = "Körbe seit Badwechsel:";
                    result.station.TimeIn = "Startzeit";
                    result.station.TimeOut = "Endzeit";
                    result.station.StationName = "Station";
                    result.station.Quality = "Resultat";
                    result.station.ProgramSequence = "Sequenz Rezept";
                    result.station.EffectiveTime = "Effektive Zeit \n[s]";
                    result.station.Temperature = "Temperatur \n[˚C]";
                    result.station.USBottomA = "Leistung Ultraschall Unten \n[%]";
                    result.station.USSideA = "Leistung Ultraschall Seite \n[%]";
                    result.station.Conductivity = "Leitwert \n[µS/cm]";
                    result.station.Pressure = "Filterdruck \n[bar]";
                    result.barcode = new Model.Barcode();
                    result.barcode.Orders = "Aufträge";
                    result.barcode.No = "Nummer";
                    result.barcode.PalletA = "Palette A";
                    result.barcode.PalletB = "Palette B";
                    result.barcode.PalletC = "Palette C";
                    result.barcode.PalletD = "Palette D";
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
