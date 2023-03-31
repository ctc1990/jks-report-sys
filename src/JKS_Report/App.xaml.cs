using JKS_Report.Function.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace JKS_Report
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public class Globals
    {       
        public static string processGlobalName = "";
       
        public static bool CONNECTION = false; // Modifiable
        public static String PAGE_URL = null; // Modifiable
        public static String MENU_URL = null; // Modifiable
        public static String SUBPAGE_URL = null; // Modifiable
        // public static String MENUPAGE_URL = null; // Modifiable
        public static String POPUP_URL = null; // Modifiable
        public static Int32 MENU_SELECTION = 0;

        public static string currentPage;   // Current Active Page
        public static object passingParameters; // Current Passing Parameters

        public static string currentRecipe = "";    // Current Selected Recipe
        public static string currentPart = "";      // Current Selected Part

        public static string Language;
        public static void PAGE_REQUEST()
        {
            if (PAGE_URL != null)
            {
                JKS_Report.MainWindow.AppWindow.FrameContainer.Source = new Uri(PAGE_URL, UriKind.RelativeOrAbsolute);
                Console.WriteLine("PAGE_REQUEST : Completed.");
            }
            else
            {
                Console.WriteLine("PAGE_REQUEST : PAGE_URL is not defined.");
            }
        }

    }

    

    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            ErrorHelper.LogError("Application Unexpected Closed", "", errorMessage);
            e.Handled = true; // Set Handled to true to prevent the application from crashing
        }
    }
}
