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

namespace JKS_Report.Function.API
{
    public class SingleReport
    {
        static string ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        
        public static void PDFGenerate(int LoadingNo, string Lang, clsSystemSetting clsSystemSetting)
        {
            clsPdfFullDataVariable clsPdfFullDataVariable = new clsPdfFullDataVariable();           

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string Query = "SELECT * FROM mainvariable WHERE LoadingNo = @LoadingNo";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@LoadingNo", LoadingNo, DbType.Int32, ParameterDirection.Input);
                    clsMainVariable _clsMainVariable = connection.Query<clsMainVariable>(Query, parameters).FirstOrDefault();

                    Query = "Select * from plcvariable where ReferenceId = @ReferenceId";
                    parameters = new DynamicParameters();
                    parameters.Add("@ReferenceId", _clsMainVariable.Id, DbType.Int32, ParameterDirection.Input);
                    List<clsStationVariable> _clsPlcVariableList = connection.Query<clsStationVariable>(Query, parameters).ToList();

                    Query = @"SELECT * FROM partmemory WHERE ReferenceId = @ReferenceId";
                    parameters = new DynamicParameters();
                    parameters.Add("@ReferenceId", _clsMainVariable.Id, DbType.Int32, ParameterDirection.Input);
                    clsPartMemory _clsPartMemory = connection.Query<clsPartMemory>(Query, parameters).FirstOrDefault();


                    clsPdfMainVariable clsPdfMainVariable = new clsPdfMainVariable();
                    clsPdfMainVariable.Username = _clsMainVariable.Username;
                    clsPdfMainVariable.BasketNumber = _clsMainVariable.BasketNumber;
                    clsPdfMainVariable.LoadingId = _clsMainVariable.LoadingId.ToString();
                    clsPdfMainVariable.UnloadingId = _clsMainVariable.UnloadingId.ToString();
                    clsPdfMainVariable.LoadingNo = _clsMainVariable.LoadingNo.ToString();
                    clsPdfMainVariable.RecipeNo = _clsMainVariable.RecipeNo.ToString();
                    clsPdfMainVariable.RecipeDescription = _clsMainVariable.RecipeDescription.ToString();
                    clsPdfMainVariable.TimeStart = _clsMainVariable.TimeStart.ToString();
                    clsPdfMainVariable.TimeEnd = _clsMainVariable.TimeEnd.ToString();
                    clsPdfMainVariable.NumberOfBasket = _clsMainVariable.NumberOfBasket.ToString();


                    clsPdfFullDataVariable.clsPdfMainVariable = clsPdfMainVariable;

                    List<clsPdfPlcVariable> clsPdfPlcVariableList = new List<clsPdfPlcVariable>();

                    foreach (var item in _clsPlcVariableList)
                    {
                        clsPdfPlcVariable _clsPdfPlcVariable = new clsPdfPlcVariable();
                        _clsPdfPlcVariable.TimeIn = item.TimeIn.ToShortTimeString();
                        _clsPdfPlcVariable.TimeOut = item.TimeOut.ToShortTimeString();
                        _clsPdfPlcVariable.StationNo = item.StationNo.ToString() + "-" + item.Description.ToString();
                        _clsPdfPlcVariable.Quality = item.Quality;
                        _clsPdfPlcVariable.SequenceRecipe = item.SequenceRecipe.ToString();
                        _clsPdfPlcVariable.EffectiveTime = item.EffectiveTime.ToString();
                        _clsPdfPlcVariable.TemperaturePV = item.TemperaturePV.ToString();
                        _clsPdfPlcVariable.UltrasonicBottomAPower = item.USonicBottomAPowerPV.ToString();
                        _clsPdfPlcVariable.UltrasonicSideAPower = item.USonicBottomBPowerPV.ToString();
                        _clsPdfPlcVariable.ConductivityPV = item.ConductivityPV.ToString();
                        _clsPdfPlcVariable.PumpFlow = item.PumpFlowPV.ToString();
                        clsPdfPlcVariableList.Add(_clsPdfPlcVariable);
                    }

                    clsPdfFullDataVariable.clsPdfPlcVariables = clsPdfPlcVariableList;

                    if (_clsPartMemory != null)
                    {
                        clsPdfFullDataVariable.clsPdfBarcodesPalletA = new clsPdfBarcodePalletA();
                        clsPdfFullDataVariable.clsPdfBarcodesPalletB = new clsPdfBarcodePalletB();
                        clsPdfFullDataVariable.clsPdfBarcodesPalletC = new clsPdfBarcodePalletC();
                        clsPdfFullDataVariable.clsPdfBarcodesPalletD = new clsPdfBarcodePalletD();

                        if (!string.IsNullOrEmpty(_clsPartMemory.PalletA))
                        {                          
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA = _clsPartMemory.PalletA;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO1 = _clsPartMemory.PalletA_WO1;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO2 = _clsPartMemory.PalletA_WO2;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO3 = _clsPartMemory.PalletA_WO3;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO4 = _clsPartMemory.PalletA_WO4;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO5 = _clsPartMemory.PalletA_WO5;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO6 = _clsPartMemory.PalletA_WO6;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO7 = _clsPartMemory.PalletA_WO7;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO8 = _clsPartMemory.PalletA_WO8;
                        }
                        if (!string.IsNullOrEmpty(_clsPartMemory.PalletB))
                        {
                            
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB = _clsPartMemory.PalletB;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO1 = _clsPartMemory.PalletB_WO1;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO2 = _clsPartMemory.PalletB_WO2;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO3 = _clsPartMemory.PalletB_WO3;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO4 = _clsPartMemory.PalletB_WO4;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO5 = _clsPartMemory.PalletB_WO5;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO6 = _clsPartMemory.PalletB_WO6;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO7 = _clsPartMemory.PalletB_WO7;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO8 = _clsPartMemory.PalletB_WO8;
                        }
                        if (!string.IsNullOrEmpty(_clsPartMemory.PalletC))
                        {
                            
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC = _clsPartMemory.PalletC;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO1 = _clsPartMemory.PalletC_WO1;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO2 = _clsPartMemory.PalletC_WO2;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO3 = _clsPartMemory.PalletC_WO3;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO4 = _clsPartMemory.PalletC_WO4;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO5 = _clsPartMemory.PalletC_WO5;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO6 = _clsPartMemory.PalletC_WO6;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO7 = _clsPartMemory.PalletC_WO7;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO8 = _clsPartMemory.PalletC_WO8;
                        }
                        if (!string.IsNullOrEmpty(_clsPartMemory.PalletD))
                        {
                            
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD = _clsPartMemory.PalletD;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO1 = _clsPartMemory.PalletD_WO1;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO2 = _clsPartMemory.PalletD_WO2;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO3 = _clsPartMemory.PalletD_WO3;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO4 = _clsPartMemory.PalletD_WO4;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO5 = _clsPartMemory.PalletD_WO5;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO6 = _clsPartMemory.PalletD_WO6;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO7 = _clsPartMemory.PalletD_WO7;
                            clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO8 = _clsPartMemory.PalletD_WO8;
                        }
                    }
                }

                clsLang clsLang = ReportLanguage(Lang);
                string language = "";
                if(Lang.ToLower() == "english")
                {
                    language = "EN";
                }
                else
                {
                    language = "GE";
                }

                SinglePDFFunction.ExportToPdf(LoadingNo.ToString(), clsPdfFullDataVariable, clsLang, clsSystemSetting, language);
                
            }
            catch
            {
                throw;
            }                      
        }
        public static void CSVGenerate(int LoadingNo)
        {
            plcCsvVariable _plcCsvVariable = new plcCsvVariable();
            DataTable dtMain;
            DataTable dtPLC;
            List<DataTable> DtList = new List<DataTable>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string Query = "SELECT * FROM mainvariable WHERE LoadingNo = @LoadingNo";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@LoadingNo", LoadingNo, DbType.Int32, ParameterDirection.Input);
                    clsMainVariable _clsMainVariable = connection.Query<clsMainVariable>(Query, parameters).FirstOrDefault();

                    Query = "Select * from plcvariable where ReferenceId = @ReferenceId";
                    parameters = new DynamicParameters();
                    parameters.Add("@ReferenceId", _clsMainVariable.Id, DbType.Int32, ParameterDirection.Input);
                    List<clsStationVariable> _clsPlcVariableList = connection.Query<clsStationVariable>(Query, parameters).ToList();

                    Query = @"SELECT * FROM partmemory WHERE ReferenceId = @ReferenceId";
                    parameters = new DynamicParameters();
                    parameters.Add("@ReferenceId", _clsMainVariable.Id, DbType.Int32, ParameterDirection.Input);
                    clsPartMemory _clsPartMemory = connection.Query<clsPartMemory>(Query, parameters).FirstOrDefault();


                    plcMainVariable clsCSVMainVariable = new plcMainVariable();
                    clsCSVMainVariable.Username = _clsMainVariable.Username;
                    clsCSVMainVariable.LoadingId = _clsMainVariable.LoadingId.ToString();
                    clsCSVMainVariable.UnloadingId = _clsMainVariable.UnloadingId.ToString();
                    clsCSVMainVariable.RecipeNo = _clsMainVariable.RecipeNo.ToString();
                    clsCSVMainVariable.RecipeDescription = _clsMainVariable.RecipeDescription.ToString();
                    clsCSVMainVariable.LoadingNo = _clsMainVariable.LoadingNo.ToString();
                    clsCSVMainVariable.LoadingTotalNo = _clsMainVariable.LoadingTotalNo.ToString();
                    clsCSVMainVariable.ProgrammeBarcode = _clsMainVariable.ProgrammeBarcode;
                    clsCSVMainVariable.ProgrammeNo = _clsMainVariable.ProgrammeNumber;
                    clsCSVMainVariable.BasketBarcode = _clsMainVariable.BasketBarcode.ToString();
                    clsCSVMainVariable.BasketNumber = _clsMainVariable.BasketNumber.ToString();

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

                    _plcCsvVariable.csvMainVariable = clsCSVMainVariable;

                    _plcCsvVariable.csvStationVariable = new List<clsCsvStation>();
                    foreach (var item in _clsPlcVariableList)
                    {
                        clsCsvStation clsCsvStation = new clsCsvStation();
                        clsCsvStation.CreatedOn = item.CreatedOn.ToShortTimeString();
                        clsCsvStation.StationNo = item.StationNo;
                        clsCsvStation.SequenceRecipe = item.SequenceRecipe;
                        clsCsvStation.SubRecipe = item.SubRecipe;
                        clsCsvStation.MinimumTime = item.MinimumTime;
                        clsCsvStation.MaximumTime = item.MaximumTime;
                        clsCsvStation.EffectiveTime = item.EffectiveTime;
                        clsCsvStation.TemperatureSV = item.TemperatureSV;
                        clsCsvStation.TemperaturePV = item.TemperaturePV;
                        clsCsvStation.USonicSideAPowerSV = item.USonicSideAPowerSV;
                        clsCsvStation.USonicSideAPowerPV = item.USonicSideAPowerPV;
                        clsCsvStation.USonicSideAFrequency = item.USonicSideAFrequency;
                        clsCsvStation.USonicSideBPowerSV = item.USonicSideBPowerSV;
                        clsCsvStation.USonicSideBPowerPV = item.USonicSideBPowerPV;
                        clsCsvStation.USonicSideBFrequency = item.USonicSideBFrequency;
                        clsCsvStation.USonicBottomAPowerSV = item.USonicBottomAPowerSV;
                        clsCsvStation.USonicBottomAPowerPV = item.USonicBottomAPowerPV;
                        clsCsvStation.USonicBottomAFrequency = item.USonicBottomAFrequency;
                        clsCsvStation.USonicBottomBPowerSV = item.USonicBottomBPowerSV;
                        clsCsvStation.USonicBottomBPowerPV = item.USonicBottomBPowerPV;
                        clsCsvStation.USonicBottomBFrequency = item.USonicBottomBFrequency;
                        clsCsvStation.ConductivityPV = item.ConductivityPV;
                        clsCsvStation.VacuumSV = item.VacuumSV;
                        clsCsvStation.VacuumPV = item.VacuumPV;
                        clsCsvStation.ResistivityPV = item.ResistivityPV;
                        clsCsvStation.PhPV = item.PhPV;
                        clsCsvStation.Quality = item.Quality;
                        clsCsvStation.ActualTime = item.ActualTime;

                        _plcCsvVariable.csvStationVariable.Add(clsCsvStation);
                    }

                    dtMain = CSVFunction.CreateMainDataCsvTable(_plcCsvVariable.csvMainVariable, "JKS_Report.Text.CsvColumnMain_Full.txt");
                    dtPLC = CSVFunction.CreatePLCDataCsvTable(_plcCsvVariable.csvStationVariable, "JKS_Report.Text.CsvColumn_SymbolPLC_Full.txt", "JKS_Report.Text.CsvColumn_NamePLC_Full.txt");

                    DtList.Add(dtMain);
                    DtList.Add(dtPLC);

                    CSVFunction.ToCSV(DtList, LoadingNo.ToString());
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
                if(lang.ToLower() == "english")
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
