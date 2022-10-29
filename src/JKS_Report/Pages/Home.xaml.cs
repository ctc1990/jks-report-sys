using JKS_Report.Function.API;
using JKS_Report.Function.DB;
using JKS_Report.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwinCAT.Ads;

namespace JKS_Report.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {       
        private TcAdsClient adsClient;
       
        private string AMSNetID = WebConfigurationManager.AppSettings.Get("AMSNetID");
        private string AMSPort = WebConfigurationManager.AppSettings.Get("AMSPort");

        public Home()
        {
            InitializeComponent();
        }
       
        private void Page_loaded(object sender, RoutedEventArgs e)
        {
            try
            {               
                PLCReportProcess.ReportingStart();
            }
            catch (Exception ex)
            {

            }
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PLCReportProcess.PlcDispose();
            }
            catch (Exception ex)
            {

            }
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            PLCReportProcess.PlcDispose();
        }
                    
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate("Index.xaml");
            Globals.PAGE_URL = "Pages/Index.xaml";
            Globals.PAGE_REQUEST();
        }
    }
}
