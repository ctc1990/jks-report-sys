using JKS_Report.Function.DB;
using JKS_Report.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using TwinCAT.Ads;

namespace JKS_Report.Function.API
{
    public class PLCReportProcess
    {
        List<Mapping> mapping = new List<Mapping>();
        private static AdsStream adsDataStream;
        private static BinaryReader binRead;
        private static bool[] bitMap;
        private TcAdsClient adsClient;
        private static int[] hconnect;

        private string AMSNetID = WebConfigurationManager.AppSettings.Get("AMSNetID");
        private string AMSPort = WebConfigurationManager.AppSettings.Get("AMSPort");

        struct Mapping
        {
            public string description;
            public string input;
        }

        public void ReportingStart()
        {
            try
            {
                InitiateMapping();
                adsClient = new TcAdsClient();
                adsDataStream = new AdsStream(mapping.Count());
                hconnect = new int[mapping.Count()];
                bitMap = new bool[mapping.Count()];

                binRead = new BinaryReader(adsDataStream, Encoding.ASCII);

                adsClient.Connect(Convert.ToInt32(AMSPort));
                for (int i = 0; i < mapping.Count(); i++)
                    hconnect[i] = adsClient.AddDeviceNotification(mapping[i].input, adsDataStream, i, 1, AdsTransMode.OnChange, 50, 0, null);

                adsClient.AdsNotification += new AdsNotificationEventHandler(StatusOnChange);

            }
            catch (Exception ex)
            {

            }
        }
        private void StatusOnChange(object sender, AdsNotificationEventArgs e)
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
                        case "Log_Configuration.fbBarcode_dataLog.bActivate":
                            if (binRead.ReadBoolean())
                            {
                                _plcMainVariable = PLCMapping.PlcMainStationMapping(adsClient);

                                if (_plcMainVariable != null)
                                {
                                    _clsMainVariable = new clsMainVariable();
                                    _clsMainVariable.Username = string.IsNullOrEmpty(_plcMainVariable.Username) ? "" : _plcMainVariable.Username;
                                    _clsMainVariable.BasketNumber = string.IsNullOrEmpty(_plcMainVariable.Username) ? 0 : Convert.ToInt32(_plcMainVariable.BasketNumber);
                                    _clsMainVariable.RecipeNo = string.IsNullOrEmpty(_plcMainVariable.RecipeNo) ? 0 : Convert.ToInt32(_plcMainVariable.RecipeNo);
                                    _clsMainVariable.LoadingNo = string.IsNullOrEmpty(_plcMainVariable.LoadingNo) ? 0 : Convert.ToInt32(_plcMainVariable.LoadingNo);
                                    _clsMainVariable.LoadingTotalNo = string.IsNullOrEmpty(_plcMainVariable.LoadingTotalNo) ? 0 : Convert.ToInt32(_plcMainVariable.LoadingTotalNo);
                                    _clsMainVariable.CreatedOn = DateTime.Now;

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

                                    if(maindone > 0)
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
                        default:
                            binRead.ReadBoolean();
                            break;
                    }
                }
            }
        }
        private clsStationVariable stnPlcClsStationMapping(plcStationVariable _plcStationVariable)
        {
            clsStationVariable result = new clsStationVariable();

            clsMainVariable mainRecord = LibDBHelper.getMainRecord(Convert.ToInt32(_plcStationVariable.RecipeNo), Convert.ToInt32(_plcStationVariable.LoadingNo));

            try
            {
                result.ReferenceID = mainRecord.Id;
                
                result.StationNo = _plcStationVariable.StationNo < 0 ? 0 : _plcStationVariable.StationNo;               
                result.SequenceRecipe = string.IsNullOrEmpty(_plcStationVariable.SequenceRecipe) ? "" : _plcStationVariable.SequenceRecipe;
                result.SubRecipe = string.IsNullOrEmpty(_plcStationVariable.SubRecipe) ? "" : _plcStationVariable.SubRecipe;
                result.MaximumTime = string.IsNullOrEmpty(_plcStationVariable.MaximumTime) ? 0 : Convert.ToInt32(_plcStationVariable.MaximumTime);
                result.MinimumTime = string.IsNullOrEmpty(_plcStationVariable.MinimumTime) ? 0 : Convert.ToInt32(_plcStationVariable.MinimumTime);
                result.EffectiveTime = string.IsNullOrEmpty(_plcStationVariable.EffectiveTime) ? 0 : Convert.ToInt32(_plcStationVariable.EffectiveTime);
                result.TemperatureSV = string.IsNullOrEmpty(_plcStationVariable.TemperatureSV) ? 0 : Convert.ToInt32(_plcStationVariable.TemperatureSV);
                result.TemperaturePV = string.IsNullOrEmpty(_plcStationVariable.TemperaturePV) ? 0 : Convert.ToInt32(_plcStationVariable.TemperaturePV);
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
                result.VacuumPV = string.IsNullOrEmpty(_plcStationVariable.VacuumPV) ? 0 : Convert.ToInt32(_plcStationVariable.VacuumPV);
                result.ConductivityPV = string.IsNullOrEmpty(_plcStationVariable.ConductivityPV) ? 0 : Convert.ToInt32(_plcStationVariable.ConductivityPV);
                result.PumpFlowPV = string.IsNullOrEmpty(_plcStationVariable.PumpFlowPV) ? 0 : Convert.ToInt32(_plcStationVariable.PumpFlowPV);
                result.ResistivityPV = string.IsNullOrEmpty(_plcStationVariable.ResistivityPV) ? 0 : Convert.ToInt32(_plcStationVariable.ResistivityPV);
                result.PhPV = string.IsNullOrEmpty(_plcStationVariable.PhPV) ? 0 : Convert.ToInt32(_plcStationVariable.PhPV);
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
        private void InitiateMapping()
        {
            mapping.Add(new Mapping { description = "LoadingTrigger", input = "Log_Configuration.fbBarcode_dataLog.bActivate" });
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
        }
    }
}
