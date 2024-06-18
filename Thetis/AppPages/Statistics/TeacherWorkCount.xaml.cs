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
    /// Interaction logic for TeacherWorkCount.xaml
    /// </summary>
    public partial class TeacherWorkCount : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private static string _work = null;
        private static bool _anergos = false;
        private static string _kladosname = null;
        private static double _count = 0;
        private static int _prokirixi = 0;
        private static string _iekname = null;

        private WorkData wd = new WorkData(_prokirixi, _iekname, _work, _anergos, _kladosname, _count );
        public ObservableCollection<string> names = new ObservableCollection<string>();

        public TeacherWorkCount()
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
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ΠΡΟΚΗΡΥΞΗ prokirixi = cboProkirixiSelection.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ iek = cboIekSelection.SelectedItem as ΙΕΚ;
            WorkViewModel wvm = DataContext as WorkViewModel;

            if (prokirixi == null && iek == null)
            {
                wvm.wd.Prokirixi = 0;
                wvm.wd.IekName = null;
                wvm.RefreshViewModel();
            }
            else if (prokirixi != null && iek == null)
            {
                int selected_prokirixi = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                wvm.wd.Prokirixi = selected_prokirixi;
                wvm.wd.IekName = null;
                wvm.RefreshViewModel();
            }
            else if (prokirixi != null && iek != null)
            {
                int selected_prokirixi = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                string selected_iek = iek.ΙΕΚ_ΟΝΟΜΑΣΙΑ;
                wvm.wd.Prokirixi = selected_prokirixi;
                wvm.wd.IekName = selected_iek;
                wvm.RefreshViewModel();
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

            WorkViewModel wvm = DataContext as WorkViewModel;
            wvm.wd.Prokirixi = 0;
            wvm.wd.IekName = null;
            wvm.RefreshViewModel();
        }

        private void cboCombineMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkViewModel wvm = DataContext as WorkViewModel;

            if (cboCombineMode.SelectedValue != null)
            {
                int index = cboCombineMode.SelectedIndex;

                if (index == 0)
                {
                    cboCombineMode.SelectedValue = "Συστοιχία";
                    wvm.BarCombineMode = Telerik.Charting.ChartSeriesCombineMode.Cluster;
                }
                if (index == 1)
                {
                    cboCombineMode.SelectedValue = "Στοίβα";
                    wvm.BarCombineMode = Telerik.Charting.ChartSeriesCombineMode.Stack;
                }
                if (index == 2)
                {
                    cboCombineMode.SelectedValue = "Στοίβα (%)";
                    wvm.BarCombineMode = Telerik.Charting.ChartSeriesCombineMode.Stack100;
                }
            }
        }

    }   // class TeacherWorkCount
}
