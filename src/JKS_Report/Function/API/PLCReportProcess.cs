using JKS_Report.Function.DB;
using JKS_Report.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows;
using TwinCAT.Ads;

namespace JKS_Report.Function.API
{
    public class PLCReportProcess
    {
        static List<Mapping> mapping = new List<Mapping>();
        private static AdsStream adsDataStream;
        private static BinaryReader binRead;
        private static bool[] bitMap;
        private static TcAdsClient adsClient;
        private static int[] hconnect;

        private static string AMSNetID = WebConfigurationManager.AppSettings.Get("AMSNetID");
        private static string AMSPort = WebConfigurationManager.AppSettings.Get("AMSPort");

        struct Mapping
        {
            public string description;
            public string input;
        }

        public static void ReportingStart()
        {
            try
            {
                InitiateMapping();
                //File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + "mapping done" + Environment.NewLine);
                adsClient = new TcAdsClient();
                adsDataStream = new AdsStream(mapping.Count());
                hconnect = new int[mapping.Count()];
                bitMap = new bool[mapping.Count()];

                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + adsDataStream.ToString() + Environment.NewLine);

                binRead = new BinaryReader(adsDataStream, Encoding.ASCII);
                adsClient.Connect(AMSNetID,Convert.ToInt32(AMSPort));
                for (int i = 0; i < mapping.Count(); i++)
                {                   
                    hconnect[i] = adsClient.AddDeviceNotification(mapping[i].input, adsDataStream, i, 1, AdsTransMode.OnChange, 50, 0, null);
                    //File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + hconnect[i].ToString() + Environment.NewLine);
                }
                    


                adsClient.AdsNotification += new AdsNotificationEventHandler(StatusOnChange);

                if (adsClient.IsConnected)
                {
                    //MessageBoxResult result = MessageBox.Show("ADS Connected.", "Success");
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError("ADS Connect", ex.Source, ex.Message, ex.StackTrace);
                MessageBoxResult result = MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        private static void StatusOnChange(object sender, AdsNotificationEventArgs e)
        {
            plcMainVariable _plcMainVariable = null;
            clsMainVariable _clsMainVariable = null;
            clsPartMemory _clsPartMemory = null;
            plcStationVariable _plStationVariable = null;

            for (int i = 0; i < mapping.Count(); i++)
            {
                clsStationVariable _clsStationVariable = new clsStationVariable();

                if (e.NotificationHandle == hconnect[i])
                {
                    switch (mapping[i].input)
                    {
                        case "Log_Configuration.bfbBarcodeActivate_Loading":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "bfbBarcodeActivate_Loading" + Environment.NewLine);
                                _plcMainVariable = PLCMapping.PlcMainStationMapping(adsClient);

                                if (_plcMainVariable != null)
                                {
                                    _clsMainVariable = new clsMainVariable();
                                    _clsMainVariable.Username = string.IsNullOrEmpty(_plcMainVariable.Username) ? "" : _plcMainVariable.Username;
                                    _clsMainVariable.LoadingId = string.IsNullOrEmpty(_plcMainVariable.LoadingId) ? 0 : Convert.ToInt32(_plcMainVariable.LoadingId);
                                    _clsMainVariable.UnloadingId = string.IsNullOrEmpty(_plcMainVariable.UnloadingId) ? 0 : Convert.ToInt32(_plcMainVariable.UnloadingId);                                  
                                    _clsMainVariable.BasketNumber = string.IsNullOrEmpty(_plcMainVariable.Username) ? "" : _plcMainVariable.BasketNumber;                                   
                                    _clsMainVariable.RecipeNo = string.IsNullOrEmpty(_plcMainVariable.RecipeNo) ? 0 : Convert.ToInt32(_plcMainVariable.RecipeNo);
                                    _clsMainVariable.RecipeDescription = string.IsNullOrEmpty(_plcMainVariable.RecipeDescription) ? "" : _plcMainVariable.RecipeDescription;
                                    _clsMainVariable.LoadingNo = string.IsNullOrEmpty(_plcMainVariable.LoadingNo) ? 0 : Convert.ToInt32(_plcMainVariable.LoadingNo);
                                    _clsMainVariable.LoadingTotalNo = string.IsNullOrEmpty(_plcMainVariable.LoadingTotalNo) ? 0 : Convert.ToInt32(_plcMainVariable.LoadingTotalNo);
                                    _clsMainVariable.ProgrammeBarcode = string.IsNullOrEmpty(_plcMainVariable.ProgrammeBarcode) ? "" : _plcMainVariable.ProgrammeBarcode;
                                    _clsMainVariable.ProgrammeNumber = string.IsNullOrEmpty(_plcMainVariable.ProgrammeNo) ? "" : _plcMainVariable.ProgrammeNo;
                                    _clsMainVariable.BasketBarcode = string.IsNullOrEmpty(_plcMainVariable.BasketBarcode) ? "" : _plcMainVariable.BasketBarcode;
                                    _clsMainVariable.LoadingTotalNo = string.IsNullOrEmpty(_plcMainVariable.LoadingTotalNo) ? 0 : Convert.ToInt32(_plcMainVariable.LoadingTotalNo);
                                    _clsMainVariable.CreatedOn = DateTime.Now;
                                    _clsMainVariable.TimeIn = DateTime.Now.ToShortTimeString();

                                    _clsPartMemory = new clsPartMemory();
                                    _clsPartMemory.PalletA = string.IsNullOrEmpty(_plcMainVariable.PalletA) ? "" : _plcMainVariable.PalletA;
                                    _clsPartMemory.PalletA_WO1 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO1) ? "" : _plcMainVariable.PalletA_WO1;
                                    _clsPartMemory.PalletA_WO2 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO2) ? "" : _plcMainVariable.PalletA_WO2;
                                    _clsPartMemory.PalletA_WO3 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO3) ? "" : _plcMainVariable.PalletA_WO3;
                                    _clsPartMemory.PalletA_WO4 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO4) ? "" : _plcMainVariable.PalletA_WO4;
                                    _clsPartMemory.PalletA_WO5 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO5) ? "" : _plcMainVariable.PalletA_WO5;
                                    _clsPartMemory.PalletA_WO6 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO6) ? "" : _plcMainVariable.PalletA_WO6;
                                    _clsPartMemory.PalletA_WO7 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO7) ? "" : _plcMainVariable.PalletA_WO7;
                                    _clsPartMemory.PalletA_WO8 = string.IsNullOrEmpty(_plcMainVariable.PalletA_WO8) ? "" : _plcMainVariable.PalletA_WO8;

                                    _clsPartMemory.PalletB = string.IsNullOrEmpty(_plcMainVariable.PalletB) ? "" : _plcMainVariable.PalletB;
                                    _clsPartMemory.PalletB_WO1 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO1) ? "" : _plcMainVariable.PalletB_WO1;
                                    _clsPartMemory.PalletB_WO2 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO2) ? "" : _plcMainVariable.PalletB_WO2;
                                    _clsPartMemory.PalletB_WO3 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO3) ? "" : _plcMainVariable.PalletB_WO3;
                                    _clsPartMemory.PalletB_WO4 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO4) ? "" : _plcMainVariable.PalletB_WO4;
                                    _clsPartMemory.PalletB_WO5 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO5) ? "" : _plcMainVariable.PalletB_WO5;
                                    _clsPartMemory.PalletB_WO6 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO6) ? "" : _plcMainVariable.PalletB_WO6;
                                    _clsPartMemory.PalletB_WO7 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO7) ? "" : _plcMainVariable.PalletB_WO7;
                                    _clsPartMemory.PalletB_WO8 = string.IsNullOrEmpty(_plcMainVariable.PalletB_WO8) ? "" : _plcMainVariable.PalletB_WO8;

                                    _clsPartMemory.PalletC = string.IsNullOrEmpty(_plcMainVariable.PalletC) ? "" : _plcMainVariable.PalletC;
                                    _clsPartMemory.PalletC_WO1 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO1) ? "" : _plcMainVariable.PalletC_WO1;
                                    _clsPartMemory.PalletC_WO2 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO2) ? "" : _plcMainVariable.PalletC_WO2;
                                    _clsPartMemory.PalletC_WO3 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO3) ? "" : _plcMainVariable.PalletC_WO3;
                                    _clsPartMemory.PalletC_WO4 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO4) ? "" : _plcMainVariable.PalletC_WO4;
                                    _clsPartMemory.PalletC_WO5 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO5) ? "" : _plcMainVariable.PalletC_WO5;
                                    _clsPartMemory.PalletC_WO6 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO6) ? "" : _plcMainVariable.PalletC_WO6;
                                    _clsPartMemory.PalletC_WO7 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO7) ? "" : _plcMainVariable.PalletC_WO7;
                                    _clsPartMemory.PalletC_WO8 = string.IsNullOrEmpty(_plcMainVariable.PalletC_WO8) ? "" : _plcMainVariable.PalletC_WO8;

                                    _clsPartMemory.PalletD = string.IsNullOrEmpty(_plcMainVariable.PalletD) ? "" : _plcMainVariable.PalletD;
                                    _clsPartMemory.PalletD_WO1 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO1) ? "" : _plcMainVariable.PalletD_WO1;
                                    _clsPartMemory.PalletD_WO2 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO2) ? "" : _plcMainVariable.PalletD_WO2;
                                    _clsPartMemory.PalletD_WO3 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO3) ? "" : _plcMainVariable.PalletD_WO3;
                                    _clsPartMemory.PalletD_WO4 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO4) ? "" : _plcMainVariable.PalletD_WO4;
                                    _clsPartMemory.PalletD_WO5 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO5) ? "" : _plcMainVariable.PalletD_WO5;
                                    _clsPartMemory.PalletD_WO6 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO6) ? "" : _plcMainVariable.PalletD_WO6;
                                    _clsPartMemory.PalletD_WO7 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO7) ? "" : _plcMainVariable.PalletD_WO7;
                                    _clsPartMemory.PalletD_WO8 = string.IsNullOrEmpty(_plcMainVariable.PalletD_WO8) ? "" : _plcMainVariable.PalletD_WO8;
                                }

                                if (_clsMainVariable != null)
                                {
                                    int maindone = LibDBHelper.MainStationRecord(_clsMainVariable);

                                    if (maindone > 0)
                                    {
                                        _clsPartMemory.ReferenceID = maindone;
                                        int partdone = LibDBHelper.BarcodeMemory(_clsPartMemory);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb1_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb1_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("1", adsClient);
                                
                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);
                                    
                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb2_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb2_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("2", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb3_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb3_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("3", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb4_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb4_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("4", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb5_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb5_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("5", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb6_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb6_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("6", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb7_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb7_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("7", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb8_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb8_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("8", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb9_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb9_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("9", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb10_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb10_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("10", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb11_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb11_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("11", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;
                        case "Log_Configuration.fb12_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "fb12_dataLog" + Environment.NewLine);
                                _plStationVariable = PLCMapping.PlcSingleStationMapping("12", adsClient);

                                if (_plStationVariable != null)
                                {
                                    _clsStationVariable = stnPlcClsStationMapping(_plStationVariable);

                                    if (_clsStationVariable != null)
                                    {
                                        int done = LibDBHelper.SingleStationRecord(_clsStationVariable);
                                    }
                                }
                            }
                            break;                        
                        case "Log_BasketTimeInOut.ARbUldBasketInActivate[1]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARbUldBasketInActivate1" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcMasterReadUldEndTime(adsClient, 1);

                                if(clsStnUpdate != null)
                                {
                                    int done = LibDBHelper.UpdateMainTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo,clsStnUpdate.TimeOut);
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbUldBasketInActivate[2]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARbUldBasketInActivate2" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcMasterReadUldEndTime(adsClient, 2);

                                if (clsStnUpdate != null)
                                {
                                    int done = LibDBHelper.UpdateMainTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                }
                            }
                            break;                       
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[1]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[1]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 1);

                                if(clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if(record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[2]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[2]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 2);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[3]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[3]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 3);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[4]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[4]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 4);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[5]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[5]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 5);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[6]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[6]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 6);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[7]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[7]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 7);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[8]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[8]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 8);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[9]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[9]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 9);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[10]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[10]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 10);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[11]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[11]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 11);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        case "Log_BasketTimeInOut.ARbStnBasketOutActivate[12]":
                            if (binRead.ReadBoolean())
                            {
                                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "ARtsStnBasketOutTime[12]" + Environment.NewLine);
                                clsStnUpdate clsStnUpdate = PLCMapping.PlcStationReadEndTime(adsClient, 12);

                                if (clsStnUpdate != null)
                                {
                                    clsStationVariable record = LibDBHelper.GetStationRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo);

                                    if (record != null)
                                    {
                                        int done = LibDBHelper.UpdateStationTimeRecord(clsStnUpdate.RecipeNo, clsStnUpdate.LoadingNo, clsStnUpdate.TimeOut);
                                    }
                                }
                            }
                            break;
                        default:
                            binRead.ReadBoolean();
                            break;
                    }
                }
            }
        }
        private static clsStationVariable stnPlcClsStationMapping(plcStationVariable _plcStationVariable)
        {
            clsStationVariable result = new clsStationVariable();

            clsMainVariable mainRecord = LibDBHelper.getMainRecord(Convert.ToInt32(_plcStationVariable.RecipeNo), Convert.ToInt32(_plcStationVariable.LoadingNo));

            try
            {
                result.ReferenceID = mainRecord.Id;
                //result.ReferenceID = string.IsNullOrEmpty(_plcStationVariable.RecipeNo) ? 0 : Convert.ToUInt16(_plcStationVariable.RecipeNo);
                result.TimeIn = string.IsNullOrEmpty(_plcStationVariable.TimeIn) ? "0" : _plcStationVariable.TimeIn;
                result.RefLoadingNo = string.IsNullOrEmpty(_plcStationVariable.LoadingNo) ? "0" : _plcStationVariable.LoadingNo;

                result.StationNo = _plcStationVariable.StationNo < 0 ? 0 : _plcStationVariable.StationNo;
                result.Description = string.IsNullOrEmpty(_plcStationVariable.StationWithDesc) ? "" : _plcStationVariable.StationWithDesc;
                result.SequenceRecipe = string.IsNullOrEmpty(_plcStationVariable.SequenceRecipe) ? "" : _plcStationVariable.SequenceRecipe;
                result.SubRecipe = string.IsNullOrEmpty(_plcStationVariable.SubRecipe) ? "" : _plcStationVariable.SubRecipe;
                result.MaximumTime = string.IsNullOrEmpty(_plcStationVariable.MaximumTime) ? 0 : Convert.ToInt32(_plcStationVariable.MaximumTime);
                result.MinimumTime = string.IsNullOrEmpty(_plcStationVariable.MinimumTime) ? 0 : Convert.ToInt32(_plcStationVariable.MinimumTime);
                result.EffectiveTime = string.IsNullOrEmpty(_plcStationVariable.EffectiveTime) ? 0 : Convert.ToInt32(_plcStationVariable.EffectiveTime);
                result.TemperatureSV = string.IsNullOrEmpty(_plcStationVariable.TemperatureSV) ? 0 : Convert.ToInt32(_plcStationVariable.TemperatureSV);
                result.TemperaturePV = string.IsNullOrEmpty(_plcStationVariable.TemperaturePV) ? 0 : float.Parse(_plcStationVariable.TemperaturePV, CultureInfo.InvariantCulture.NumberFormat);
                result.USonicBottomAPowerSV = string.IsNullOrEmpty(_plcStationVariable.USonicBottomAPowerSV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicBottomAPowerSV);
                result.USonicBottomAPowerPV = string.IsNullOrEmpty(_plcStationVariable.USonicBottomAPowerPV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicBottomAPowerPV);
                result.USonicBottomAFrequency = string.IsNullOrEmpty(_plcStationVariable.USonicBottomAFrequency) ? 0 : Convert.ToInt32(_plcStationVariable.USonicBottomAFrequency);
                result.USonicBottomBPowerSV = string.IsNullOrEmpty(_plcStationVariable.USonicBottomBPowerSV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicBottomBPowerSV);
                result.USonicBottomBPowerPV = string.IsNullOrEmpty(_plcStationVariable.USonicBottomBPowerPV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicBottomBPowerPV);
                result.USonicBottomBFrequency = string.IsNullOrEmpty(_plcStationVariable.USonicBottomBFrequency) ? 0 : Convert.ToInt32(_plcStationVariable.USonicBottomBFrequency);
                result.USonicSideAPowerSV = string.IsNullOrEmpty(_plcStationVariable.USonicSideAPowerSV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicSideAPowerSV);
                result.USonicSideAPowerPV = string.IsNullOrEmpty(_plcStationVariable.USonicSideAPowerPV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicSideAPowerPV);
                result.USonicSideAFrequency = string.IsNullOrEmpty(_plcStationVariable.USonicSideAFrequency) ? 0 : Convert.ToInt32(_plcStationVariable.USonicSideAFrequency);
                result.USonicSideBPowerSV = string.IsNullOrEmpty(_plcStationVariable.USonicSideBPowerSV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicSideBPowerSV);
                result.USonicSideBPowerPV = string.IsNullOrEmpty(_plcStationVariable.USonicSideBPowerPV) ? 0 : Convert.ToInt32(_plcStationVariable.USonicSideBPowerPV);
                result.USonicSideBFrequency = string.IsNullOrEmpty(_plcStationVariable.USonicSideBFrequency) ? 0 : Convert.ToInt32(_plcStationVariable.USonicSideBFrequency);
                result.VacuumSV = string.IsNullOrEmpty(_plcStationVariable.VacuumSV) ? 0 : Convert.ToInt32(_plcStationVariable.VacuumSV);
                result.VacuumPV = string.IsNullOrEmpty(_plcStationVariable.VacuumPV) ? 0 : float.Parse(_plcStationVariable.VacuumPV, CultureInfo.InvariantCulture.NumberFormat);
                result.ConductivityPV = string.IsNullOrEmpty(_plcStationVariable.ConductivityPV) ? 0 : float.Parse(_plcStationVariable.ConductivityPV, CultureInfo.InvariantCulture.NumberFormat);
                result.PumpFlowPV = string.IsNullOrEmpty(_plcStationVariable.PumpFlowPV) ? 0 : float.Parse(_plcStationVariable.PumpFlowPV, CultureInfo.InvariantCulture.NumberFormat);
                result.ResistivityPV = string.IsNullOrEmpty(_plcStationVariable.ResistivityPV) ? 0 : float.Parse(_plcStationVariable.ResistivityPV, CultureInfo.InvariantCulture.NumberFormat);
                result.PhPV = string.IsNullOrEmpty(_plcStationVariable.PhPV) ? 0 : float.Parse(_plcStationVariable.PhPV);
                result.Quality = string.IsNullOrEmpty(_plcStationVariable.Quality) ? "" : _plcStationVariable.Quality;
                result.ActualTime = string.IsNullOrEmpty(_plcStationVariable.ActualTime) ? 0 : Convert.ToInt32(_plcStationVariable.ActualTime);
                result.CreatedOn = DateTime.Now;
            }
            catch
            {               
                throw;
            }
            return result;
        }
        public static void InitiateMapping()
        {
            //mapping.Add(new Mapping { description = "test", input = "Log_Configuration.test123" });
            mapping.Add(new Mapping { description = "LoadingTrigger", input = "Log_Configuration.bfbBarcodeActivate_Loading" });
            mapping.Add(new Mapping { description = "Stn1Trigger", input = "Log_Configuration.fb1_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn2Trigger", input = "Log_Configuration.fb2_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn3Trigger", input = "Log_Configuration.fb3_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn4Trigger", input = "Log_Configuration.fb4_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn5Trigger", input = "Log_Configuration.fb5_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn6Trigger", input = "Log_Configuration.fb6_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn7Trigger", input = "Log_Configuration.fb7_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn8Trigger", input = "Log_Configuration.fb8_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn9Trigger", input = "Log_Configuration.fb9_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn10Trigger", input = "Log_Configuration.fb10_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn11Trigger", input = "Log_Configuration.fb11_dataLog.bActivate" });
            mapping.Add(new Mapping { description = "Stn12Trigger", input = "Log_Configuration.fb12_dataLog.bActivate" });

            //mapping.Add(new Mapping { description = "RecipeTimeInTrigger", input = "Log_BasketTimeInOut.ARbLdBasketInActivate[1]" });
            mapping.Add(new Mapping { description = "RecipeTimeOutTrigger1", input = "Log_BasketTimeInOut.ARbUldBasketInActivate[1]" });
            mapping.Add(new Mapping { description = "RecipeTimeOutTrigger2", input = "Log_BasketTimeInOut.ARbUldBasketInActivate[2]" });

            mapping.Add(new Mapping { description = "BasketTimeOutTrigger1", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[1]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger2", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[2]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger3", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[3]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger4", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[4]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger5", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[5]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger6", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[6]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger7", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[7]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger8", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[8]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger9", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[9]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger10", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[10]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger11", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[11]" });
            mapping.Add(new Mapping { description = "BasketTimeOutTrigger12", input = "Log_BasketTimeInOut.ARbStnBasketOutActivate[12]" });
        }
        public static void PlcDispose()
        {
            if (adsClient != null)
            {
                adsClient.Disconnect();
                adsClient.Dispose();
                MessageBoxResult result = MessageBox.Show("ADS Disconnected and Dispose.", "Success");
            }
        }
    }
}
