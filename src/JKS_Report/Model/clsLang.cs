using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKS_Report.Model
{
    public class clsLang
    {
        
            public string Machine { get; set; }
            public string Name { get; set; }
            public string Page { get; set; }
            public string Software { get; set; }
            public string Time { get; set; }
            public string LoadNumber { get; set; }
            public string TimeStart { get; set; }
            public string TimeEnd { get; set; }
            public string RecipeNumber { get; set; }
            public string RecipeDesc { get; set; }
            public string LoadingID { get; set; }
            public string UnLoadingID { get; set; }
            public string Operator { get; set; }
            public string BasketNumber { get; set; }       
            public Station station { get; set; }
            public Barcode barcode { get; set; }
         
    }

    public class Station
    {
        public string tProgramSequence { get; set; }
        public string NumberOfBasket { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string StationName { get; set; }
        public string Quality { get; set; }
        public string ProgramSequence { get; set; }
        public string EffectiveTime { get; set; }
        public string Temperature { get; set; }
        public string USBottomA { get; set; }
        public string USSideA { get; set; }
        public string Conductivity { get; set; }
        public string Pressure { get; set; }
    }

    public class Barcode
    {
        public string Orders { get; set; }
        public string No { get; set; }
        public string PalletA { get; set; }
        public string PalletB { get; set; }
        public string PalletC { get; set; }
        public string PalletD { get; set; }
    }
}
