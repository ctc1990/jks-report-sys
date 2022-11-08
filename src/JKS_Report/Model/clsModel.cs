using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace JKS_Report.Model
{
    public class clsModel
    {

    }

    public class clsMainVariable
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public int LoadingId { get; set; }
        public int UnloadingId { get; set; }
        public string BasketNumber { get; set; }
        public int RecipeNo { get; set; }
        public string RecipeDescription { get; set; }
        public int LoadingNo { get; set; }
        public string ProgrammeBarcode { get; set; }
        public string ProgrammeNumber { get; set; }
        public string BasketBarcode { get; set; }
        public int LoadingTotalNo { get; set; }
        public int NumberOfBasket { get; set; }     
        public DateTime CreatedOn { get; set; }
    }
    public class clsStationVariable
    {
        public long ReferenceID { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string RefLoadingNo {get;set;}
        public int StationNo { get; set; }
        public string Description { get; set; }
        public string SequenceRecipe { get; set; }
        public string SubRecipe { get; set; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public int EffectiveTime { get; set; }
        public int TemperatureSV { get; set; }
        public float TemperaturePV { get; set; }
        public int USonicSideAPowerSV { get; set; }
        public int USonicSideAPowerPV { get; set; }
        public int USonicSideAFrequency { get; set; }
        public int USonicSideBPowerSV { get; set; }
        public int USonicSideBPowerPV { get; set; }
        public int USonicSideBFrequency { get; set; }
        public int USonicBottomAPowerSV { get; set; }
        public int USonicBottomAPowerPV { get; set; }
        public int USonicBottomAFrequency { get; set; }
        public int USonicBottomBPowerSV { get; set; }
        public int USonicBottomBPowerPV { get; set; }
        public int USonicBottomBFrequency { get; set; }
        public int VacuumSV { get; set; }
        public float VacuumPV { get; set; }
        public float ConductivityPV { get; set; }
        public float PumpFlowPV { get; set; }
        public float ResistivityPV { get; set; }
        public float PhPV { get; set; }
        public string Quality { get; set; }
        public int ActualTime { get; set; }       
        public DateTime CreatedOn { get; set; }
    }
    public class clsPartMemory
    {
        public long ReferenceID { get; set; }
        public string PalletA { get; set; }
        public string PalletA_WO1 { get; set; }
        public string PalletA_WO2 { get; set; }
        public string PalletA_WO3 { get; set; }
        public string PalletA_WO4 { get; set; }
        public string PalletA_WO5 { get; set; }
        public string PalletA_WO6 { get; set; }
        public string PalletA_WO7 { get; set; }
        public string PalletA_WO8 { get; set; }
        public string PalletB { get; set; }
        public string PalletB_WO1 { get; set; }
        public string PalletB_WO2 { get; set; }
        public string PalletB_WO3 { get; set; }
        public string PalletB_WO4 { get; set; }
        public string PalletB_WO5 { get; set; }
        public string PalletB_WO6 { get; set; }
        public string PalletB_WO7 { get; set; }
        public string PalletB_WO8 { get; set; }
        public string PalletC { get; set; }
        public string PalletC_WO1 { get; set; }
        public string PalletC_WO2 { get; set; }
        public string PalletC_WO3 { get; set; }
        public string PalletC_WO4 { get; set; }
        public string PalletC_WO5 { get; set; }
        public string PalletC_WO6 { get; set; }
        public string PalletC_WO7 { get; set; }
        public string PalletC_WO8 { get; set; }
        public string PalletD { get; set; }
        public string PalletD_WO1 { get; set; }
        public string PalletD_WO2 { get; set; }
        public string PalletD_WO3 { get; set; }
        public string PalletD_WO4 { get; set; }
        public string PalletD_WO5 { get; set; }
        public string PalletD_WO6 { get; set; }
        public string PalletD_WO7 { get; set; }
        public string PalletD_WO8 { get; set; }
    }
    public class plcMainVariable
    {
        public string Username { get; set; }
        public string LoadingId { get; set; }
        public string UnloadingId { get; set; }
        public string BasketNumber { get; set; }
        public string RecipeNo { get; set; }
        public string RecipeDescription { get; set; }
        public string LoadingNo { get; set; }
        public string LoadingTotalNo { get; set; }
        public string ProgrammeBarcode { get; set; }       
        public string ProgrammeNo { get; set; }
        public string BasketBarcode { get; set; }        
        public string PalletA { get; set; }
        public string PalletA_WO1 { get; set; }
        public string PalletA_WO2 { get; set; }
        public string PalletA_WO3 { get; set; }
        public string PalletA_WO4 { get; set; }
        public string PalletA_WO5 { get; set; }
        public string PalletA_WO6 { get; set; }
        public string PalletA_WO7 { get; set; }
        public string PalletA_WO8 { get; set; }
        public string PalletB { get; set; }
        public string PalletB_WO1 { get; set; }
        public string PalletB_WO2 { get; set; }
        public string PalletB_WO3 { get; set; }
        public string PalletB_WO4 { get; set; }
        public string PalletB_WO5 { get; set; }
        public string PalletB_WO6 { get; set; }
        public string PalletB_WO7 { get; set; }
        public string PalletB_WO8 { get; set; }
        public string PalletC { get; set; }
        public string PalletC_WO1 { get; set; }
        public string PalletC_WO2 { get; set; }
        public string PalletC_WO3 { get; set; }
        public string PalletC_WO4 { get; set; }
        public string PalletC_WO5 { get; set; }
        public string PalletC_WO6 { get; set; }
        public string PalletC_WO7 { get; set; }
        public string PalletC_WO8 { get; set; }
        public string PalletD { get; set; }
        public string PalletD_WO1 { get; set; }
        public string PalletD_WO2 { get; set; }
        public string PalletD_WO3 { get; set; }
        public string PalletD_WO4 { get; set; }
        public string PalletD_WO5 { get; set; }
        public string PalletD_WO6 { get; set; }
        public string PalletD_WO7 { get; set; }
        public string PalletD_WO8 { get; set; }
    }
    public class plcStationVariable
    {
        public int StationNo { get; set; }
        public string StationWithDesc { get; set; }
        public string LoadingNo { get; set; }
        public string RecipeNo { get; set; }
        public string SequenceRecipe { get; set; }
        public string SubRecipe { get; set; }
        public string MinimumTime { get; set; }
        public string MaximumTime { get; set; }
        public string EffectiveTime { get; set; }
        public string TemperatureSV { get; set; }
        public string TemperaturePV { get; set; }
        public string USonicSideAPowerSV { get; set; }
        public string USonicSideAPowerPV { get; set; }
        public string USonicSideAFrequency { get; set; }
        public string USonicSideBPowerSV { get; set; }
        public string USonicSideBPowerPV { get; set; }
        public string USonicSideBFrequency { get; set; }
        public string USonicBottomAPowerSV { get; set; }
        public string USonicBottomAPowerPV { get; set; }
        public string USonicBottomAFrequency { get; set; }
        public string USonicBottomBPowerSV { get; set; }
        public string USonicBottomBPowerPV { get; set; }
        public string USonicBottomBFrequency { get; set; }
        public string VacuumSV { get; set; }
        public string VacuumPV { get; set; }
        public string ConductivityPV { get; set; }
        public string PumpFlowPV { get; set; }
        public string ResistivityPV { get; set; }
        public string PhPV { get; set; }
        public string Quality { get; set; }
        public string ActualTime { get; set; } 
        
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
    }
    public class plcMainStationVariable
    {
        public plcMainVariable plcMainVariable { get; set; }
        public List<plcStationVariable> plcStationVariables { get; set; }
    }
    public class clsPdfFullDataVariable
    {
        public clsPdfMainVariable clsPdfMainVariable { get; set; }
        public List<clsPdfPlcVariable> clsPdfPlcVariables { get; set; }
        public clsPdfBarcodePalletA clsPdfBarcodesPalletA { get; set; }
        public clsPdfBarcodePalletB clsPdfBarcodesPalletB { get; set; }
        public clsPdfBarcodePalletC clsPdfBarcodesPalletC { get; set; }
        public clsPdfBarcodePalletD clsPdfBarcodesPalletD { get; set; }
    }
    public class clsPdfMainVariable
    {
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string Username { get; set; }
        public string LoadingId { get; set; }
        public string UnloadingId { get; set; }
        public string BasketNumber { get; set; }
        public string RecipeNo { get; set; }
        public string RecipeDescription { get; set; }
        public string LoadingNo { get; set; }
        public string NumberOfBasket { get; set; }

        public string BasketBarcode { get; set; }
    }
    public class clsPdfPlcVariable
    {
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }     
        public string StationNo { get; set; }    
        public string StationDescription { get; set; }
        public string EffectiveTime { get; set; }
        public string TemperaturePV { get; set; }
        public string ConductivityPV { get; set; }
        public string Quality { get; set; }
        public string SequenceRecipe { get; set; }
        public string UltrasonicBottomAPower { get; set; }
        public string UltrasonicSideAPower { get; set; }
        public string PumpFlow { get; set; }
    }
    public class clsPdfBarcodePalletA
    {
        public string PalletA { get; set; }
        public string PalletA_WO1 { get; set; }
        public string PalletA_WO2 { get; set; }
        public string PalletA_WO3 { get; set; }
        public string PalletA_WO4 { get; set; }
        public string PalletA_WO5 { get; set; }
        public string PalletA_WO6 { get; set; }
        public string PalletA_WO7 { get; set; }
        public string PalletA_WO8 { get; set; }
    }
    public class clsPdfBarcodePalletB
    {
        public string PalletB { get; set; }
        public string PalletB_WO1 { get; set; }
        public string PalletB_WO2 { get; set; }
        public string PalletB_WO3 { get; set; }
        public string PalletB_WO4 { get; set; }
        public string PalletB_WO5 { get; set; }
        public string PalletB_WO6 { get; set; }
        public string PalletB_WO7 { get; set; }
        public string PalletB_WO8 { get; set; }
    }
    public class clsPdfBarcodePalletC
    {
        public string PalletC { get; set; }
        public string PalletC_WO1 { get; set; }
        public string PalletC_WO2 { get; set; }
        public string PalletC_WO3 { get; set; }
        public string PalletC_WO4 { get; set; }
        public string PalletC_WO5 { get; set; }
        public string PalletC_WO6 { get; set; }
        public string PalletC_WO7 { get; set; }
        public string PalletC_WO8 { get; set; }
    }
    public class clsPdfBarcodePalletD
    {
        public string PalletD { get; set; }
        public string PalletD_WO1 { get; set; }
        public string PalletD_WO2 { get; set; }
        public string PalletD_WO3 { get; set; }
        public string PalletD_WO4 { get; set; }
        public string PalletD_WO5 { get; set; }
        public string PalletD_WO6 { get; set; }
        public string PalletD_WO7 { get; set; }
        public string PalletD_WO8 { get; set; }
    }
    public class plcCsvVariable
    {
        public clsCsvMainVariableSingle csvMainVariable { get; set; }
        public List<clsCsvStation> csvStationVariable { get; set; }
        public clsPdfBarcodePalletA csvBarcodePalletA { get; set; }
        public clsPdfBarcodePalletB csvBarcodePalletB { get; set; }
        public clsPdfBarcodePalletC csvBarcodePalletC { get; set; }
        public clsPdfBarcodePalletD csvBarcodePalletD { get; set; }
    }
    public class plcCsvMasterVariable
    {
        public plcMainVariable csvMainVariable { get; set; }
        public List<clsCsvMasterStation> csvStationVariable { get; set; }
    }

    public class clsCsvMainVariableSingle
    {
        public string Username { get; set; }
        public string LoadingId { get; set; }
        public string UnloadingId { get; set; }
        public string BasketNumber { get; set; }
        public string RecipeNo { get; set; }
        public string RecipeDescription { get; set; }
        public string LoadingNo { get; set; }
        public string LoadingTotalNo { get; set; }
        public string ProgrammeBarcode { get; set; }
        public string ProgrammeNo { get; set; }
        public string BasketBarcode { get; set; }
        public string CreatedOn { get; set; }
    }
    public class clsCsvStation
    {
        public string CreatedOn { get; set; }
        public int StationNo { get; set; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public int EffectiveTime { get; set; }
        public int TemperatureSV { get; set; }
        public float TemperaturePV { get; set; }
        public int USonicSideAPowerSV { get; set; }
        public int USonicSideAPowerPV { get; set; }
        public int USonicSideBPowerSV { get; set; }
        public int USonicSideBPowerPV { get; set; }
        public int USonicBottomAPowerSV { get; set; }
        public int USonicBottomAPowerPV { get; set; }
        public int USonicBottomBPowerSV { get; set; }
        public int USonicBottomBPowerPV { get; set; }

        public float PumpFlowPV { get; set; }
        public float ConductivityPV { get; set; }
        public string Quality { get; set; }
    }
    public class clsCsvMasterStation
    {
        public string CreatedOn { get; set; }
        public int StationNo { get; set; }
        public string SequenceRecipe { get; set; }
        public string SubRecipe { get; set; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public int EffectiveTime { get; set; }
        public int TemperatureSV { get; set; }
        public float TemperaturePV { get; set; }
        public int USonicSideAPowerSV { get; set; }
        public int USonicSideAPowerPV { get; set; }
        public int USonicSideAFrequency { get; set; }
        public int USonicSideBPowerSV { get; set; }
        public int USonicSideBPowerPV { get; set; }
        public int USonicSideBFrequency { get; set; }
        public int USonicBottomAPowerSV { get; set; }
        public int USonicBottomAPowerPV { get; set; }
        public int USonicBottomAFrequency { get; set; }
        public int USonicBottomBPowerSV { get; set; }
        public int USonicBottomBPowerPV { get; set; }
        public int USonicBottomBFrequency { get; set; }
        public int VacuumSV { get; set; }
        public float VacuumPV { get; set; }
        public float ConductivityPV { get; set; }
        public float PumpFlowPV { get; set; }
        public float ResistivityPV { get; set; }
        public float PhPV { get; set; }
        public string Quality { get; set; }
        public int ActualTime { get; set; }
    }
    public class clsSystemSetting
    {
        public string ReferenceKey { get; set; }
        public string Machine { get; set; }
        public string Name { get; set; }
        public string Software { get; set; }
        public string CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class clsStnUpdate
    {
        public string RecipeNo { get; set; }
        public string LoadingNo { get; set; }
        public string TimeOut { get; set; }
    }

    public class clsReportAutoGenerate
    {
        public int LoadingNo { get; set; }
        public string Language { get; set; }
    }

    public class clsCsvMainVariable
    {
        public clsCsvMainVariable1 _clsCsvMainVariable1 { get; set; }
        public clsCsvMainVariable2 _clsCsvMainVariable2 { get; set; }
    }
    public class clsCsvMainVariable1
    {
        public string Machine { get; set; }
        public string Name { get; set; }
        public string Software { get; set; }
        public string Date { get; set; }
    }

    public class clsCsvMainVariable2
    {
        public string LoadNumber { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string RecipeNumber { get; set; }
        public string RecipeDescription { get; set; }
        public string LoadingId { get; set; }
        public string UnloadingId { get; set; }
        public string Operator { get; set; }
        public string BasketNumber { get; set; }
        public string NumberOfBasket { get; set; }
    }

    public class clsCsvPlcVariable
    {        
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string StationDescription { get; set; }
        public string Quality { get; set; }
        public string SequenceRecipe { get; set; }
        public string EffectiveTime { get; set; }
        public string TemperaturePV { get; set; }                  
        public string UltrasonicBottomAPower { get; set; }
        public string UltrasonicSideAPower { get; set; }
        public string ConductivityPV { get; set; }
        public string PumpFlow { get; set; }
    }

}
