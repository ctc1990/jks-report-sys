using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    public partial class Configuration : Page
    {
        string compartmentName = "";
        int noCompartment = 0;
        public Configuration()
        {
            InitializeComponent();
        }
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            compartmentName = TextBox1.Text;
            noCompartment = Convert.ToInt32(TextBox2.Text);

           
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Globals.PAGE_URL = "Pages/Home.xaml";
            Globals.PAGE_REQUEST();
        }
    }
}
