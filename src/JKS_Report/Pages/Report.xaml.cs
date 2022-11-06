using JKS_Report.Function.API;
using JKS_Report.Function.DB;
using JKS_Report.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JKS_Report.Pages
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Report : Page
    {
       
        public Report()
        {
            InitializeComponent();
        }       
        
              
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            clsSystemSetting record = new clsSystemSetting();
            record = LibDBHelper.getSystemSettings();
            bool RecordneedUpdate = false;
            int RecordUpdate = 0;
            clsSystemSetting PreUpdate = new clsSystemSetting();

            if (record != null)
            {
                string sTitle = string.IsNullOrEmpty(record.Name) ? "" : record.Name;
                string sMachine = string.IsNullOrEmpty(record.Machine) ? "" : record.Machine;
                string sSoftware = string.IsNullOrEmpty(record.Software) ? "" : record.Software;
                if (sTitle.ToLower() != Title.Text.ToLower() || sMachine.ToLower() != Machine.Text.ToLower() || sSoftware.ToLower() != Software.Text.ToLower())
                {
                    RecordneedUpdate = true;
                }

                if (RecordneedUpdate)
                {
                    PreUpdate.Machine = Machine.Text;
                    PreUpdate.Name = Title.Text;
                    PreUpdate.Software = Software.Text;
                    PreUpdate.ModifiedOn = DateTime.Now;
                    RecordUpdate = LibDBHelper.UpdateSystemSetting(PreUpdate);

                    if(RecordUpdate > 0)
                    {
                        System.Windows.MessageBox.Show("Saved Successful.", "Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string files = fbd.SelectedPath;
                                     
                    if(SavePath.Text != files)
                    {
                        clsSystemSetting clsSystemSetting = new clsSystemSetting();
                        clsSystemSetting.Name = files;
                        int record = LibDBHelper.UpdateFilePath(clsSystemSetting);

                        SavePath.Text = files;
                    }
                }
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                PLCReportProcess.PlcDispose();
            }
            catch(Exception ex)
            {
                ErrorHelper.LogError("Report_Check", ex.Source, ex.Message, ex.StackTrace);
            }
            
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                PLCReportProcess.ReportingStart();
            }
            catch(Exception ex)
            {
                ErrorHelper.LogError("Report_Uncheck", ex.Source, ex.Message, ex.StackTrace);
            }            
        }

        private void Page_loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PLCReportProcess.ReportingStart();
                clsSystemSetting _clsSystemSetting = LibDBHelper.getSystemSettings();
                clsSystemSetting _clsFilePath = LibDBHelper.getFilePath();
                Machine.Text = _clsSystemSetting.Machine;
                Software.Text = _clsSystemSetting.Software;
                Title.Text = _clsSystemSetting.Name;
                SavePath.Text = _clsFilePath.Name;
                Globals.Language = combo1.Text;
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError("Report_Load", ex.Source, ex.Message, ex.StackTrace);
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
                ErrorHelper.LogError("Report_Unload",ex.Source,ex.Message,ex.StackTrace);
            }
        }

        

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
