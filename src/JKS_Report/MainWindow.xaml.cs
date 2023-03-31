using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace JKS_Report
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        public MainWindow()
        {
            File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "Application Start." + Environment.NewLine);
            InitializeComponent();
            AppWindow = this;
        }

        private void FrameContainerLoaded(object sender, RoutedEventArgs e)
        {            
            Globals.PAGE_URL = "Pages/Report.xaml";
            Globals.PAGE_REQUEST();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.AppendAllText(@"C:\JKS\Setup\debug.txt", DateTime.Now.ToString() + " " + "Application Close." + Environment.NewLine);
        }
    }
}
