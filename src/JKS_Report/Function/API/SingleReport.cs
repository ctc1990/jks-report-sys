﻿using Dapper;
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
                    clsPdfMainVariable.TimeStart = _clsMainVariable.TimeIn;
                    clsPdfMainVariable.TimeEnd = _clsMainVariable.TimeOut;
                    clsPdfMainVariable.NumberOfBasket = _clsMainVariable.NumberOfBasket.ToString();


                    clsPdfFullDataVariable.clsPdfMainVariable = clsPdfMainVariable;

                    List<clsPdfPlcVariable> clsPdfPlcVariableList = new List<clsPdfPlcVariable>();

                    foreach (var item in _clsPlcVariableList)
                    {
                        clsPdfPlcVariable _clsPdfPlcVariable = new clsPdfPlcVariable();
                        _clsPdfPlcVariable.TimeIn = item.TimeIn;
                        _clsPdfPlcVariable.TimeOut = item.TimeOut;
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


                    clsCsvMainVariableSingle clsCSVMainVariable = new clsCsvMainVariableSingle();
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
                    clsCSVMainVariable.CreatedOn = _clsMainVariable.CreatedOn.ToShortDateString();

                    if (!string.IsNullOrEmpty(_clsPartMemory.PalletA))
                    {
                        _plcCsvVariable.csvBarcodePalletA = new clsPdfBarcodePalletA();
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO1 = _clsPartMemory.PalletA_WO1;
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO2 = _clsPartMemory.PalletA_WO2;
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO3 = _clsPartMemory.PalletA_WO3;
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO4 = _clsPartMemory.PalletA_WO4;
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO5 = _clsPartMemory.PalletA_WO5;
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO6 = _clsPartMemory.PalletA_WO6;
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO7 = _clsPartMemory.PalletA_WO7;
                        _plcCsvVariable.csvBarcodePalletA.PalletA_WO8 = _clsPartMemory.PalletA_WO8;
                    }

                    if (!string.IsNullOrEmpty(_clsPartMemory.PalletB))
                    {
                        _plcCsvVariable.csvBarcodePalletB = new clsPdfBarcodePalletB();
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO1 = _clsPartMemory.PalletB_WO1;
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO2 = _clsPartMemory.PalletB_WO2;
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO3 = _clsPartMemory.PalletB_WO3;
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO4 = _clsPartMemory.PalletB_WO4;
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO5 = _clsPartMemory.PalletB_WO5;
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO6 = _clsPartMemory.PalletB_WO6;
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO7 = _clsPartMemory.PalletB_WO7;
                        _plcCsvVariable.csvBarcodePalletB.PalletB_WO8 = _clsPartMemory.PalletB_WO8;
                    }

                    if (!string.IsNullOrEmpty(_clsPartMemory.PalletC))
                    {
                        _plcCsvVariable.csvBarcodePalletC = new clsPdfBarcodePalletC();
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO1 = _clsPartMemory.PalletC_WO1;
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO2 = _clsPartMemory.PalletC_WO2;
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO3 = _clsPartMemory.PalletC_WO3;
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO4 = _clsPartMemory.PalletC_WO4;
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO5 = _clsPartMemory.PalletC_WO5;
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO6 = _clsPartMemory.PalletC_WO6;
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO7 = _clsPartMemory.PalletC_WO7;
                        _plcCsvVariable.csvBarcodePalletC.PalletC_WO8 = _clsPartMemory.PalletC_WO8;
                    }

                    if (!string.IsNullOrEmpty(_clsPartMemory.PalletD))
                    {
                        _plcCsvVariable.csvBarcodePalletD = new clsPdfBarcodePalletD();
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO1 = _clsPartMemory.PalletD_WO1;
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO2 = _clsPartMemory.PalletD_WO2;
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO3 = _clsPartMemory.PalletD_WO3;
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO4 = _clsPartMemory.PalletD_WO4;
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO5 = _clsPartMemory.PalletD_WO5;
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO6 = _clsPartMemory.PalletD_WO6;
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO7 = _clsPartMemory.PalletD_WO7;
                        _plcCsvVariable.csvBarcodePalletD.PalletD_WO8 = _clsPartMemory.PalletD_WO8;
                    }


                    _plcCsvVariable.csvMainVariable = clsCSVMainVariable;

                    _plcCsvVariable.csvStationVariable = new List<clsCsvStation>();
                    foreach (var item in _clsPlcVariableList)
                    {
                        clsCsvStation clsCsvStation = new clsCsvStation();
                        clsCsvStation.CreatedOn = item.CreatedOn.ToShortTimeString();
                        clsCsvStation.StationNo = item.StationNo;
                        clsCsvStation.MinimumTime = item.MinimumTime;
                        clsCsvStation.MaximumTime = item.MaximumTime;
                        clsCsvStation.EffectiveTime = item.EffectiveTime;
                        clsCsvStation.TemperatureSV = item.TemperatureSV;
                        clsCsvStation.TemperaturePV = item.TemperaturePV;
                        clsCsvStation.USonicSideAPowerSV = item.USonicSideAPowerSV;
                        clsCsvStation.USonicSideAPowerPV = item.USonicSideAPowerPV;
                        clsCsvStation.USonicSideBPowerSV = item.USonicSideBPowerSV;
                        clsCsvStation.USonicSideBPowerPV = item.USonicSideBPowerPV;
                        clsCsvStation.USonicBottomAPowerSV = item.USonicBottomAPowerSV;
                        clsCsvStation.USonicBottomAPowerPV = item.USonicBottomAPowerPV;
                        clsCsvStation.USonicBottomBPowerSV = item.USonicBottomBPowerSV;
                        clsCsvStation.USonicBottomBPowerPV = item.USonicBottomBPowerPV;
                        clsCsvStation.ConductivityPV = item.ConductivityPV;
                        clsCsvStation.Quality = item.Quality;

                        _plcCsvVariable.csvStationVariable.Add(clsCsvStation);
                    }


                    if (_plcCsvVariable.csvMainVariable != null)
                    {
                        dtMain = CSVFunction.CreateSingleMainDataTable(_plcCsvVariable.csvMainVariable, "Record");
                        DtList.Add(dtMain);
                    }
                    if (_plcCsvVariable.csvStationVariable.Count > 0)
                    {
                        dtPLC = CSVFunction.CreateSinglePLCDataTable(_plcCsvVariable.csvStationVariable, "Record", "JKS_Report.Text.CsvColumnSymbol_Main_Single.txt");
                        DtList.Add(dtPLC);
                    }
                    if (_plcCsvVariable.csvBarcodePalletA != null)
                    {
                        dtPalletA = SinglePDFFunction.CreatePalletADataTable(_plcCsvVariable.csvBarcodePalletA);
                        dtBeforePalletList.Add(dtPalletA);
                    }
                    if (_plcCsvVariable.csvBarcodePalletB != null)
                    {
                        dtPalletB = SinglePDFFunction.CreatePalletBDataTable(_plcCsvVariable.csvBarcodePalletB);
                        dtBeforePalletList.Add(dtPalletB);
                    }
                    if (_plcCsvVariable.csvBarcodePalletC != null)
                    {
                        dtPalletC = SinglePDFFunction.CreatePalletCDataTable(_plcCsvVariable.csvBarcodePalletC);
                        dtBeforePalletListbc.Add(dtPalletC);
                    }
                    if (_plcCsvVariable.csvBarcodePalletD != null)
                    {
                        dtPalletD = SinglePDFFunction.CreatePalletDDataTable(_plcCsvVariable.csvBarcodePalletD);
                        dtBeforePalletListbc.Add(dtPalletD);
                    }

                    if (dtBeforePalletList.Count > 0)
                    {
                        dtAfterPalletMerge = SinglePDFFunction.DataTableMerge(dtBeforePalletList, "Record");

                        if (dtAfterPalletMerge != null)
                        {
                            DtList.Add(dtAfterPalletMerge);
                        }

                    }
                    if (dtBeforePalletListbc.Count > 0)
                    {
                        dtAfterPalletMergebc = SinglePDFFunction.DataTableMerge(dtBeforePalletListbc, "Record");
                        if (dtAfterPalletMergebc != null)
                        {
                            DtList.Add(dtAfterPalletMergebc);
                        }
                    }

                    if (DtList.Count > 0)
                    {
                        CSVFunction.ToCSV(DtList, LoadingNo.ToString(),false);
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
