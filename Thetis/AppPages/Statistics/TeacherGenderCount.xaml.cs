using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Chart;
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls.ChartView;
using Thetis.AppPages.Statistics.ChartViewModel;
using Thetis.Controls;
using Thetis.Model;
using Thetis.Utilities;


namespace Thetis.AppPages.Statistics
{
    /// <summary>
    /// Interaction logic for TeacherGenderCount.xaml
    /// </summary>
    public partial class TeacherGenderCount : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private static int _prokirixi = 0;
        private static string _iekname = null;
        private static string _gender = null;
        private static string _klados = null;
        private static double _count = 0;

        private GenderData gd = new GenderData(_prokirixi, _iekname, _gender, _klados, _count);
        public ObservableCollection<string> names = new ObservableCollection<string>();

        public TeacherGenderCount()
        {
            InitializeComponent();
            LoadComboData();
        }

        private void LoadComboData()
        {
            var prok = from p in db.ΠΡΟΚΗΡΥΞΗs
                       orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ descending
                       select p;

            var iek = from i in db.ΙΕΚs
                      orderby i.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                      select i;

            cboProkirixiSelection.ItemsSource = prok.ToList();
            cboIekSelection.ItemsSource = iek.ToList();

            names.Add("Συστοιχία");
            names.Add("Στοίβα");
            names.Add("Στοίβα (%)");

            //cboCombineMode.ItemsSource = names;
            //cboCombineMode.SelectedIndex = 0;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ΠΡΟΚΗΡΥΞΗ prokirixi = cboProkirixiSelection.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ iek = cboIekSelection.SelectedItem as ΙΕΚ;
            GenderViewModel gvm = DataContext as GenderViewModel;

            if (prokirixi == null && iek == null) 
            {
                gvm.gd.Prokirixi = 0;
                gvm.gd.IekName = null;
                gvm.RefreshViewModel();
            }
            else if (prokirixi != null && iek == null)
            {
                int selected_prokirixi = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                gvm.gd.Prokirixi = selected_prokirixi;
                gvm.gd.IekName = null;
                gvm.RefreshViewModel();
            }
            else if (prokirixi != null && iek != null)
            {
                int selected_prokirixi = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                string selected_iek = iek.ΙΕΚ_ΟΝΟΜΑΣΙΑ;
                gvm.gd.Prokirixi = selected_prokirixi;
                gvm.gd.IekName = selected_iek;
                gvm.RefreshViewModel();
            }
            else if (prokirixi == null && iek != null)
            {
                UserFunctions.ShowAdminMessage("Πρέπει να επιλέξετε και προκήρυξη.");
            }

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cboIekSelection.SelectedIndex = -1;
            cboProkirixiSelection.SelectedIndex = -1;

            GenderViewModel gvm = DataContext as GenderViewModel;
            gvm.gd.Prokirixi = 0;
            gvm.gd.IekName = null;
            gvm.RefreshViewModel();
        }

        private void cboCombineMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenderViewModel gvm = DataContext as GenderViewModel;
 
            if (cboCombineMode.SelectedValue != null)
            {
                int index = cboCombineMode.SelectedIndex;
                
                if (index == 0)
                {
                    cboCombineMode.SelectedValue = "Συστοιχία";
                    gvm.BarCombineMode = Telerik.Charting.ChartSeriesCombineMode.Cluster;
                }
                if (index == 1)
                {
                    cboCombineMode.SelectedValue = "Στοίβα";
                    gvm.BarCombineMode = Telerik.Charting.ChartSeriesCombineMode.Stack;
                }
                if (index == 2)
                {
                    cboCombineMode.SelectedValue = "Στοίβα (%)";
                    gvm.BarCombineMode = Telerik.Charting.ChartSeriesCombineMode.Stack100;
                }
            }
        }




    } // class TeacherGenderCount
}
