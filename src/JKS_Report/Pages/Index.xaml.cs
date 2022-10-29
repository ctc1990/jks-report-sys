using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JKS_Report.Pages
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        bool notifier = true;
        bool trigger = false;
        public static Index AppWindow;
        public Index()
        {
            try
            {
                InitializeComponent();
                AppWindow = this;               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton thisButton = (ToggleButton)sender;
            if (thisButton.IsChecked.Value)
            {
                MenuContainer.Visibility = Visibility.Visible;
                Storyboard SB = (Storyboard)FindResource("ShowMenu");
                SB.Begin();
                trigger = true;
            }
            else
            {
                Storyboard SB = (Storyboard)FindResource("HideMenu");
                SB.Begin();
                trigger = false;
            }
        }

        private void HideMenu()
        {
            if (trigger)
            {
                Storyboard SB = (Storyboard)FindResource("HideMenu");
                SB.Begin();
            }
        }

        public void HidePage()
        {
            Storyboard SB = (Storyboard)FindResource("HideFrame");
            SB.Begin();
        }

        private void MMSelections_Checked(object sender, RoutedEventArgs e)
        {
            string page = "";
            RadioButton radioButton = (RadioButton)sender;

            switch (radioButton.Name)
            {
                case "MMHome":
                    page = "Pages/Home.xaml";
                    break;
                case "MMReport":
                    page = "Pages/Report.xaml";
                    break;
                default:
                    Globals.MENU_URL = page;
                    HidePage();
                    HideMenu();
                    break;
            }
            Console.WriteLine("Pass");
           
            MainMenu.IsChecked = false;
            MainMenu_Click(MainMenu, null);
        }
    }
}
