using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace JKS_Report.Function.ADS
{
    class ADS_ReadWrtite
    {
        public static bool ADS_WriteValue(TcAdsClient adsClient, string s_VariableName, string s_VariableValue, string s_DataType)
        {
            ITcAdsSymbol adsSymbol = adsClient.ReadSymbolInfo(s_VariableName);

            switch (s_DataType.ToLower())
            {
                case "bool":
                    adsClient.WriteSymbol(adsSymbol, Convert.ToBoolean(s_VariableValue));
                    break;
                case "int":
                    adsClient.WriteSymbol(adsSymbol, Convert.ToInt16(s_VariableValue));
                    break;
                case "dint":
                    adsClient.WriteSymbol(adsSymbol, Convert.ToInt32(s_VariableValue));
                    break;
                case "word":
                    adsClient.WriteSymbol(adsSymbol, Convert.ToUInt32(s_VariableValue));
                    break;
                case "real":
                    adsClient.WriteSymbol(adsSymbol, Convert.ToSingle(s_VariableValue));
                    break;
                case "lreal":
                    adsClient.WriteSymbol(adsSymbol, Convert.ToDouble(s_VariableValue));
                    break;
                case "string":
                    adsClient.WriteSymbol(adsSymbol, s_VariableValue);
                    break;
            }
            System.Threading.Thread.Sleep(8);

            return true;
        }

        public static string ADS_ReadValue(TcAdsClient adsClient, string s_VariableName)
        {
            ITcAdsSymbol adsSymbol = adsClient.ReadSymbolInfo(s_VariableName);
            System.Threading.Thread.Sleep(8);

            return adsClient.ReadSymbol(adsSymbol).ToString();
        }
    }
}
