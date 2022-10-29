using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace CLEANXCEL2._2.Functions.EventNotifier
{
    class ADSEventNotification
    {
        public event Action ActionChange;
        private static AdsStream adsDataStream;
        private TcAdsClient tcAdsClient = new TcAdsClient();
        private BinaryReader binRead;
        private List<VariableInfo> local_variable = new List<VariableInfo>();
        private int[] hconnect;
        public int index;
        public object response;

        public void ADSEventGenerateNotif(TcAdsClient adsClient, List<VariableInfo> variable)
        {
            int index = 0;
            local_variable = variable;
            hconnect = new int[local_variable.Count()];
            tcAdsClient = adsClient;
            adsDataStream = new AdsStream(local_variable.Select(x=>Convert(x.variableType)).Sum());
            binRead = new BinaryReader(adsDataStream, Encoding.ASCII);

            try
            {
                for (int i = 0; i < local_variable.Count(); i++)
                {
                    int size = Convert(local_variable[i].variableType);
                    hconnect[i] = adsClient.AddDeviceNotification(local_variable[i].variableName, adsDataStream, index, size, AdsTransMode.OnChange, 100, 0, null);
                    index += size;
                }

                adsClient.AdsNotification += new AdsNotificationEventHandler(EventOnChange);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message.ToString());
            }
        }        

        private void EventOnChange(object sender, AdsNotificationEventArgs e)
        {
            try
            {
                for (int i = 0; i < hconnect.Count(); i++)
                {
                    if (hconnect[i] == e.NotificationHandle)
                    {
                        index = i;
                        response = Read(i);
                        ActionChange();
                    }
                }
            }
            catch
            {
                Console.WriteLine("ADSEventNotification Failed to Read");
            }
        }

        private string Read(int i)
        {
            switch(local_variable[i].variableType)
            {
                case "bool":
                    return binRead.ReadBoolean().ToString();
                case "sint":
                    return binRead.ReadSByte().ToString();
                case "int":
                    return binRead.ReadInt16().ToString();
                case "dint":
                    return binRead.ReadInt32().ToString();
                case "word":
                    return binRead.ReadInt32().ToString();
                case "real":
                    return binRead.ReadSingle().ToString();
                case "lreal":
                    return binRead.ReadDouble().ToString();
                case "string":
                    return binRead.ReadString().ToString();
                default:
                    return "";

            }
        }
        
        private int Convert(string DataType)
        {
            switch (DataType)
            {
                case "bool":
                    return 1;
                case "sint":
                    return 1;
                case "int":
                    return 2;
                case "dint":
                    return 4;
                case "word":
                    return 4;
                case "real":
                    return 4;
                case "lreal":
                    return 8;
                case "string":
                default:
                    return 99;

            }
        }

        public struct VariableInfo
        {
            public string variableName;
            public string variableType;
        }
    }
}
