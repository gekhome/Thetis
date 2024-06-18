using System;
using System.Collections.Generic;
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
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.Charting;
using Thetis.AppPages.Statistics.ChartViewModel;

namespace Thetis.AppPages.Statistics
{
    /// <summary>
    /// Interaction logic for TestPie.xaml
    /// </summary>
    public partial class TestPie : Page
    {
        public TestPie()
        {
            InitializeComponent();
            //typeCombo.SelectedIndex = 0;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Selector.Chart = PieChart;
            //this.Selector.SelectedIndex = 0;
        }





    }   // class TestPie
}
