using JKS_Report.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using TwinCAT.Ads;

namespace JKS_Report.Function.API
{
    public class PLCMapping
    {
        private string AMSNetID = WebConfigurationManager.AppSettings.Get("AMSNetID");
        private string AMSPort = WebConfigurationManager.AppSettings.Get("AMSPort");
        public plcMainStationVariable PlcMapping(TcAdsClient adsClient)
        {
            plcMainStationVariable result = new plcMainStationVariable();
            plcMainVariable plcMainVariableDeclaration = new plcMainVariable();
            plcMainVariable plcMainVariableValue = new plcMainVariable();

            plcStationVariable[] plcStationVariableDeclaration = new plcStationVariable[13];
            List<plcStationVariable> plcStationVariableValue = new List<plcStationVariable>();

            int stationNo = 13;

            plcMainVariableDeclaration.Username = ".sUserName";
            plcMainVariableDeclaration.LoadingId = "1";
            plcMainVariableDeclaration.UnloadingId = ".DSA1fbPnpSequenceMemoryStore.DsA1inPnpSequenceMemory.iUldGroupNo";
            plcMainVariableDeclaration.RecipeNo = ".DSV2_BufferBasketInfo.iProductRecipeNo";
            plcMainVariableDeclaration.RecipeDescription = ".DSV2_BufferBasketInfo.sProgramDescription";
            plcMainVariableDeclaration.LoadingNo = ".DSV2_BufferBasketInfo.sReservedInt";
            plcMainVariableDeclaration.ProgrammeBarcode = ".DSV2_BufferBasketInfo.ARsBarcodeData[3]";
            plcMainVariableDeclaration.ProgrammeNo = ".DSV2_BufferBasketInfo.iProductRecipeNo";
            plcMainVariableDeclaration.BasketNumber = ".DSV2_BufferBasketInfo.sDisplayBasketNumber";
            plcMainVariableDeclaration.BasketBarcode = ".DSV2_BufferBasketInfo.iBaskeNo";

            

            plcMainVariableValue.Username = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.LoadingId = "1";
            plcMainVariableValue.UnloadingId = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.UnloadingId)).ToString();
            plcMainVariableValue.BasketNumber = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.RecipeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.RecipeDescription = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.LoadingNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.ProgrammeBarcode = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.ProgrammeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.BasketNumber = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
            plcMainVariableValue.BasketBarcode = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
           
            result.plcMainVariable = plcMainVariableValue;

            for (int j = 1; j < stationNo; j++)
            {
                plcStationVariableDeclaration[j] = new plcStationVariable();
                plcStationVariableDeclaration[j].StationNo = j;
                plcStationVariableDeclaration[j].SequenceRecipe = ".DSHmiStationDisplayInfo[" + j + "].sStationSequenceRecipeDescription";
                plcStationVariableDeclaration[j].SubRecipe = ".DSHmiStationDisplayInfo[" + j + "].sStationSubDescription";
                plcStationVariableDeclaration[j].MinimumTime = ".ARDsStnMinMaxEffTimeDisplay[" + j + "].iMinTime";
                plcStationVariableDeclaration[j].MaximumTime = ".ARDsStnMinMaxEffTimeDisplay[" + j + "].iMaxTime";
                plcStationVariableDeclaration[j].EffectiveTime = ".ARDsStnMinMaxEffTimeDisplay[" + j + "].iEffTime";
                plcStationVariableDeclaration[j].TemperatureSV = ".ARrStnTempSV[" + j + "]";
                plcStationVariableDeclaration[j].TemperaturePV = ".ARrStnTempPV[" + j + "]";
                plcStationVariableDeclaration[j].USonicBottomAPowerSV = ".ARDsStnSeqProcessCtrl[" + j + "].Out_DSStnSeqProOutput.i3ProTankBtmUsAPwrPercent";
                plcStationVariableDeclaration[j].USonicBottomAPowerPV = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].USonicBottomAFrequency = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].USonicBottomBPowerSV = ".ARDsStnSeqProcessCtrl[" + j + "].Out_DSStnSeqProOutput.i3ProTankBtmUsAPwrPercent";
                plcStationVariableDeclaration[j].USonicBottomBPowerPV = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].USonicBottomBFrequency = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].USonicSideAPowerSV = ".ARDsStnSeqProcessCtrl[" + j + "].Out_DSStnSeqProOutput.i3ProTankBtmUsAPwrPercent";
                plcStationVariableDeclaration[j].USonicSideAPowerPV = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].USonicSideAFrequency = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].USonicSideBPowerSV = ".ARDsStnSeqProcessCtrl[" + j + "].Out_DSStnSeqProOutput.i3ProTankBtmUsAPwrPercent";
                plcStationVariableDeclaration[j].USonicSideBPowerPV = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].USonicSideBFrequency = ".ARiStnUSBtmAPv[" + j + "]";
                plcStationVariableDeclaration[j].VacuumSV = "";
                plcStationVariableDeclaration[j].VacuumPV = "";
                plcStationVariableDeclaration[j].ConductivityPV = ".ARrStnConductivityPV[" + j + "]";
                plcStationVariableDeclaration[j].ResistivityPV = "";
                plcStationVariableDeclaration[j].PumpFlowPV = ".ARrStnFilterPressurePV[" + j + "]";
                plcStationVariableDeclaration[j].PhPV = ".ARrStnPhPV[" + j + "]";
                plcStationVariableDeclaration[j].Quality = ".DSStnBasketInfo[" + j + "].sProductQuality";
                plcStationVariableDeclaration[j].ActualTime = ".ARDsStnSeqProcessCtrl[" + j + "].Out_iCurrentProcessTime";
            }

            if (plcStationVariableDeclaration.Length > 0)
            {
                foreach (var a in plcStationVariableDeclaration)
                {
                    plcStationVariable plcStationVariable = new plcStationVariable();
                    plcStationVariable.StationNo = a.StationNo;
                    plcStationVariable.SequenceRecipe = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.SequenceRecipe)).ToString();
                    plcStationVariable.SubRecipe = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.SubRecipe)).ToString();
                    plcStationVariable.MinimumTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.MinimumTime)).ToString();
                    plcStationVariable.MaximumTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.MaximumTime)).ToString();
                    plcStationVariable.EffectiveTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.EffectiveTime)).ToString();
                    plcStationVariable.TemperatureSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.TemperatureSV)).ToString();
                    plcStationVariable.TemperaturePV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.TemperaturePV)).ToString();
                    plcStationVariable.USonicBottomAPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicBottomAPowerSV)).ToString();
                    plcStationVariable.USonicBottomAPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicBottomAPowerPV)).ToString();
                    plcStationVariable.USonicBottomAFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicBottomAFrequency)).ToString();
                    plcStationVariable.USonicBottomBPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicBottomBPowerSV)).ToString();
                    plcStationVariable.USonicBottomBPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicBottomBPowerPV)).ToString();
                    plcStationVariable.USonicBottomBFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicBottomBFrequency)).ToString();
                    plcStationVariable.USonicSideAPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicSideAPowerSV)).ToString();
                    plcStationVariable.USonicSideAPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicSideAPowerPV)).ToString();
                    plcStationVariable.USonicSideAFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicSideAFrequency)).ToString();
                    plcStationVariable.USonicSideBPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicSideBPowerSV)).ToString();
                    plcStationVariable.USonicSideBPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicSideBPowerPV)).ToString();
                    plcStationVariable.USonicSideBFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.USonicSideBFrequency)).ToString();
                    plcStationVariable.VacuumSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.VacuumSV)).ToString();
                    plcStationVariable.VacuumPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.VacuumPV)).ToString();
                    plcStationVariable.ConductivityPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.ConductivityPV)).ToString();
                    plcStationVariable.ResistivityPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.ResistivityPV)).ToString();
                    plcStationVariable.PumpFlowPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.PumpFlowPV)).ToString();
                    plcStationVariable.PhPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.PhPV)).ToString();
                    plcStationVariable.Quality = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.Quality)).ToString();
                    plcStationVariable.ActualTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(a.ActualTime)).ToString();

                    plcStationVariableValue.Add(plcStationVariable);
                }

                result.plcStationVariables = plcStationVariableValue;
            }

            return result;
        }
        public static plcStationVariable PlcSingleStationMapping(string stationNo, TcAdsClient adsClient)
        {
            plcStationVariable _plcStationVariable = new plcStationVariable();

            try
            {
                if (!string.IsNullOrEmpty(stationNo))
                {
                    File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "Single Station mapping start" + Environment.NewLine);
                    _plcStationVariable.StationNo = Convert.ToInt32(stationNo);
                    _plcStationVariable.StationWithDesc = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".a" + stationNo + "_datalink[2].svalue")).ToString();
                    //_plcStationVariable.LoadingNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".DSStnBasketInfo[" + stationNo + "].iBasketNo")).ToString();
                    _plcStationVariable.RecipeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".DSStnBasketInfo[" + stationNo + "].iProductRecipeNo")).ToString();

                    Regex regex = new Regex("([0-9]+(:[0-9]+)+)", RegexOptions.IgnoreCase);
                    Match match = regex.Match(adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARsStnBasketInTime[" + stationNo + "]")).ToString());
                    _plcStationVariable.TimeIn = match.Value;
                  
                    _plcStationVariable.LoadingNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARsStnBasketNoBuffer[" + stationNo + "]")).ToString();
                    _plcStationVariable.SequenceRecipe = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".DSHmiStationDisplayInfo[" + stationNo + "].sStationSequenceRecipeDescription")).ToString();
                    _plcStationVariable.SubRecipe = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".DSHmiStationDisplayInfo[" + stationNo + "].sStationSubDescription")).ToString();
                    _plcStationVariable.MinimumTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnMinMaxEffTimeDisplay[" + stationNo + "].iMinTime")).ToString();
                    _plcStationVariable.MaximumTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnMinMaxEffTimeDisplay[" + stationNo + "].iMaxTime")).ToString();
                    _plcStationVariable.EffectiveTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnMinMaxEffTimeDisplay[" + stationNo + "].iEffTime")).ToString();
                    _plcStationVariable.TemperatureSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARrStnTempSV[" + stationNo + "]")).ToString();
                    _plcStationVariable.TemperaturePV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARrStnTempPV[" + stationNo + "]")).ToString();
                    _plcStationVariable.USonicBottomAPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i3ProTankBtmUsAPwrPercent")).ToString();
                    _plcStationVariable.USonicBottomAPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARiStnUSBtmAPv[" + stationNo + "]")).ToString();
                    _plcStationVariable.USonicBottomAFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i8ProTankBtmUsAkHz")).ToString();
                    _plcStationVariable.USonicBottomBPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i4ProTankBtmUsBPwrPercent")).ToString();
                    _plcStationVariable.USonicBottomBPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARiStnUSBtmBPv[" + stationNo + "]")).ToString();
                    _plcStationVariable.USonicBottomBFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i9ProTankBtmUsBkHz")).ToString();
                    _plcStationVariable.USonicSideAPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i6ProTankSideUsAPwrPercent")).ToString();
                    _plcStationVariable.USonicSideAPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARiStnUSSideAPv[" + stationNo + "]")).ToString();
                    _plcStationVariable.USonicSideAFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i11ProTankSideUsAkHz")).ToString();
                    _plcStationVariable.USonicSideBPowerSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i7ProTankSideUsBPwrPercent")).ToString();
                    _plcStationVariable.USonicSideBPowerPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARiStnUSSideBPv[" + stationNo + "]")).ToString();
                    _plcStationVariable.USonicSideBFrequency = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_DSStnSeqProOutput.i12ProTankSideUsBkHz")).ToString();
                    _plcStationVariable.VacuumSV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].In_DsStnSeqProInput.iVacuumLevelPV")).ToString();
                    _plcStationVariable.VacuumPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].In_DsStnSeqProInput.iVacuumLevelPV")).ToString();
                    _plcStationVariable.ConductivityPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARrStnConductivityPV[" + stationNo + "]")).ToString();
                    _plcStationVariable.ResistivityPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARrStnResistivityPV[" + stationNo + "]")).ToString();
                    _plcStationVariable.PumpFlowPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARrStnFilterPressurePV[" + stationNo + "]")).ToString();
                    _plcStationVariable.PhPV = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARrStnPhPV[" + stationNo + "]")).ToString();
                    _plcStationVariable.Quality = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".DSStnBasketInfo[" + stationNo + "].sProductQuality")).ToString();
                    _plcStationVariable.ActualTime = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARDsStnSeqProcessCtrl[" + stationNo + "].Out_iCurrentProcessTime")).ToString();

                    File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "Single Station mapping end" + Environment.NewLine);
                }
            }
            catch
            {
                throw;
            }

            return _plcStationVariable;
        }
        public static plcMainVariable PlcMainStationMapping(TcAdsClient adsClient)
        {
            plcMainVariable result = new plcMainVariable();
            plcMainVariable plcMainVariableDeclaration = new plcMainVariable();

            try
            {
                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "Main Station mapping start" + Environment.NewLine);

                plcMainVariableDeclaration.Username = ".sUserName";
                plcMainVariableDeclaration.LoadingId = "1";
                plcMainVariableDeclaration.UnloadingId = ".AR2sDSCv_LdConfirmBasketInfo[1,5].iUnloadingID";
                plcMainVariableDeclaration.RecipeNo = ".DSV2_BufferBasketInfo.iProductRecipeNo";
                plcMainVariableDeclaration.RecipeDescription = ".DSV2_BufferBasketInfo.sProgramDescription";
                plcMainVariableDeclaration.LoadingNo = ".DSV2_BufferBasketInfo.sReservedInt";
                plcMainVariableDeclaration.ProgrammeBarcode = ".DSV2_BufferBasketInfo.ARsBarcodeData[3]";
                plcMainVariableDeclaration.ProgrammeNo = ".DSV2_BufferBasketInfo.iProductRecipeNo";
                plcMainVariableDeclaration.BasketNumber = ".DSV2_BufferBasketInfo.sDisplayBasketNumber";
                plcMainVariableDeclaration.BasketBarcode = ".DSV2_BufferBasketInfo.iBaskeNo";

                plcMainVariableDeclaration.PalletA = ".DSV2_BufferBasketInfo.PalletBarcodeData[1]";
                plcMainVariableDeclaration.PalletA_WO1 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,1]";
                plcMainVariableDeclaration.PalletA_WO2 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,2]";
                plcMainVariableDeclaration.PalletA_WO3 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,3]";
                plcMainVariableDeclaration.PalletA_WO4 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,4]";
                plcMainVariableDeclaration.PalletA_WO5 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,5]";
                plcMainVariableDeclaration.PalletA_WO6 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,6]";
                plcMainVariableDeclaration.PalletA_WO7 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,7]";
                plcMainVariableDeclaration.PalletA_WO8 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[1,8]";

                plcMainVariableDeclaration.PalletB = ".DSV2_BufferBasketInfo.PalletBarcodeData[2]";
                plcMainVariableDeclaration.PalletB_WO1 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,1]";
                plcMainVariableDeclaration.PalletB_WO2 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,2]";
                plcMainVariableDeclaration.PalletB_WO3 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,3]";
                plcMainVariableDeclaration.PalletB_WO4 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,4]";
                plcMainVariableDeclaration.PalletB_WO5 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,5]";
                plcMainVariableDeclaration.PalletB_WO6 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,6]";
                plcMainVariableDeclaration.PalletB_WO7 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,7]";
                plcMainVariableDeclaration.PalletB_WO8 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[2,8]";

                plcMainVariableDeclaration.PalletC = ".DSV2_BufferBasketInfo.PalletBarcodeData[3]";
                plcMainVariableDeclaration.PalletC_WO1 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,1]";
                plcMainVariableDeclaration.PalletC_WO2 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,2]";
                plcMainVariableDeclaration.PalletC_WO3 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,3]";
                plcMainVariableDeclaration.PalletC_WO4 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,4]";
                plcMainVariableDeclaration.PalletC_WO5 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,5]";
                plcMainVariableDeclaration.PalletC_WO6 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,6]";
                plcMainVariableDeclaration.PalletC_WO7 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,7]";
                plcMainVariableDeclaration.PalletC_WO8 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[3,8]";

                plcMainVariableDeclaration.PalletD = ".DSV2_BufferBasketInfo.PalletBarcodeData[4]";
                plcMainVariableDeclaration.PalletD_WO1 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,1]";
                plcMainVariableDeclaration.PalletD_WO2 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,2]";
                plcMainVariableDeclaration.PalletD_WO3 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,3]";
                plcMainVariableDeclaration.PalletD_WO4 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,4]";
                plcMainVariableDeclaration.PalletD_WO5 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,5]";
                plcMainVariableDeclaration.PalletD_WO6 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,6]";
                plcMainVariableDeclaration.PalletD_WO7 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,7]";
                plcMainVariableDeclaration.PalletD_WO8 = ".DSV2_BufferBasketInfo.PalletWOBarcodeData[4,8]";

                result.Username = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.Username)).ToString();
                result.LoadingId = "1";
                result.UnloadingId = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.UnloadingId)).ToString();
                result.RecipeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.RecipeNo)).ToString();
                result.RecipeDescription = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.RecipeDescription)).ToString();
                result.LoadingNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.LoadingNo)).ToString();
                result.ProgrammeBarcode = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.ProgrammeBarcode)).ToString();
                result.ProgrammeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.ProgrammeNo)).ToString();
                result.BasketNumber = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.BasketNumber)).ToString();
                result.BasketBarcode = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.BasketBarcode)).ToString();

                result.PalletA = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA)).ToString();
                result.PalletA_WO1 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO1)).ToString();
                result.PalletA_WO2 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO2)).ToString();
                result.PalletA_WO3 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO3)).ToString();
                result.PalletA_WO4 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO4)).ToString();
                result.PalletA_WO5 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO5)).ToString();
                result.PalletA_WO6 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO6)).ToString();
                result.PalletA_WO7 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO7)).ToString();
                result.PalletA_WO8 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletA_WO8)).ToString();

                result.PalletB = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB)).ToString();
                result.PalletB_WO1 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO1)).ToString();
                result.PalletB_WO2 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO2)).ToString();
                result.PalletB_WO3 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO3)).ToString();
                result.PalletB_WO4 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO4)).ToString();
                result.PalletB_WO5 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO5)).ToString();
                result.PalletB_WO6 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO6)).ToString();
                result.PalletB_WO7 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO7)).ToString();
                result.PalletB_WO8 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletB_WO8)).ToString();

                result.PalletC = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC)).ToString();
                result.PalletC_WO1 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO1)).ToString();
                result.PalletC_WO2 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO2)).ToString();
                result.PalletC_WO3 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO3)).ToString();
                result.PalletC_WO4 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO4)).ToString();
                result.PalletC_WO5 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO5)).ToString();
                result.PalletC_WO6 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO6)).ToString();
                result.PalletC_WO7 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO7)).ToString();
                result.PalletC_WO8 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletC_WO8)).ToString();

                result.PalletD = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD)).ToString();
                result.PalletD_WO1 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO1)).ToString();
                result.PalletD_WO2 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO2)).ToString();
                result.PalletD_WO3 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO3)).ToString();
                result.PalletD_WO4 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO4)).ToString();
                result.PalletD_WO5 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO5)).ToString();
                result.PalletD_WO6 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO6)).ToString();
                result.PalletD_WO7 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO7)).ToString();
                result.PalletD_WO8 = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(plcMainVariableDeclaration.PalletD_WO8)).ToString();

                File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "Main Station mapping End" + Environment.NewLine);
            }
            catch
            {
                throw;
            }

            return result;
        }

        public static clsStnUpdate PlcMasterReadUldEndTime1(TcAdsClient adsClient)
        {
            clsStnUpdate result = null;
            try
            {
                clsStnUpdate _clsStnUpdate = new clsStnUpdate();
                _clsStnUpdate.RecipeNo = ".ARiUldBasketProductRecipeBuffer[1]";
                _clsStnUpdate.LoadingNo = ".ARsUldBasketNoBuffer[1]";
                
                result = new clsStnUpdate();
                result.RecipeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(_clsStnUpdate.RecipeNo)).ToString();
                result.LoadingNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(_clsStnUpdate.LoadingNo)).ToString();
                Regex regex = new Regex("([0-9]+(:[0-9]+)+)", RegexOptions.IgnoreCase);
                Match match = regex.Match(adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARsUldBasketInTime[1]")).ToString());
                result.TimeOut = match.Value;

            }
            catch
            {
                throw;
            }
            return result;
        }

        public static clsStnUpdate PlcMasterReadUldEndTime2(TcAdsClient adsClient)
        {
            clsStnUpdate result = null;
            try
            {
                clsStnUpdate _clsStnUpdate = new clsStnUpdate();
                _clsStnUpdate.RecipeNo = ".ARiUldBasketProductRecipeBuffer[2]";
                _clsStnUpdate.LoadingNo = ".ARsUldBasketNoBuffer[2]";
                
                result = new clsStnUpdate();
                result.RecipeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(_clsStnUpdate.RecipeNo)).ToString();
                result.LoadingNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(_clsStnUpdate.LoadingNo)).ToString();
                
                Regex regex = new Regex("([0-9]+(:[0-9]+)+)", RegexOptions.IgnoreCase);
                Match match = regex.Match(adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARsUldBasketInTime[2]")).ToString());
                result.TimeOut = match.Value;

            }
            catch
            {
                throw;
            }
            return result;
        }

        public static clsStnUpdate PlcStationReadEndTime(TcAdsClient adsClient, int stationNo)
        {
            clsStnUpdate result = null;
            try
            {
                clsStnUpdate _clsStnUpdate = new clsStnUpdate();
                _clsStnUpdate.RecipeNo = ".ARiStnBasketProductRecipeBuffer[" + stationNo + "]";
                _clsStnUpdate.LoadingNo = ".ARsStnBasketNoBuffer[" + stationNo + "]";
              
                result = new clsStnUpdate();
                result.RecipeNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(_clsStnUpdate.RecipeNo)).ToString();
                result.LoadingNo = adsClient.ReadSymbol(adsClient.ReadSymbolInfo(_clsStnUpdate.LoadingNo)).ToString();

                Regex regex = new Regex("([0-9]+(:[0-9]+)+)", RegexOptions.IgnoreCase);
                Match match = regex.Match(adsClient.ReadSymbol(adsClient.ReadSymbolInfo(".ARsStnBasketOutTime[" + stationNo + "]")).ToString());
                result.TimeOut = match.Value;
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static clsReportAutoGenerate PlcReportAutoMapping(TcAdsClient adsClient)
        {
            clsReportAutoGenerate clsReportAutoGenerate = null;

            try
            {
                string sLoadingNo = ".AR2sDSCv_UldConfirmBasketInfo[1,1].sReservedInt";
                
                clsReportAutoGenerate = new clsReportAutoGenerate();
                clsReportAutoGenerate.LoadingNo = (int)adsClient.ReadSymbol(adsClient.ReadSymbolInfo(sLoadingNo));
                clsReportAutoGenerate.Language = Globals.Language;
            }
            catch
            {
                throw;
            }
            return clsReportAutoGenerate;
        }
    }
}
