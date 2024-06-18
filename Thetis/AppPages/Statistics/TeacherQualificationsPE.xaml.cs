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
using Thetis.Model;
using Thetis.Utilities;


namespace Thetis.AppPages.Statistics
{
    /// <summary>
    /// Interaction logic for TeacherQualificationsPE.xaml
    /// </summary>
    public partial class TeacherQualificationsPE : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        int PE1Count, PE2Count, PE3Count, TotalCount;

        public TeacherQualificationsPE()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {

            // Load school year combo
            var prok = from p in db.ΠΡΟΚΗΡΥΞΗs
                       orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ descending
                       select p;

            var iek = from i in db.ΙΕΚs
                      orderby i.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                      select i;

            cboProkirixiSelection.ItemsSource = prok.ToList();
            cboIekSelection.ItemsSource = iek.ToList();

            // Load datagrid
            LoadGrid();
            // Load Gauge data
            LoadGaugeData();

        } // LoadData


        #region Load Grid Data ( 3 overloads)

        public void LoadGrid()
        {
            var aitiseis = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                           where a.ΚΛΑΔΟΣ == "ΠΕ"
                           orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                           select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        public void LoadGrid(int prokirixi_code)
        {
            var aitiseis = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                           where a.ΚΛΑΔΟΣ == "ΠΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                           orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                           select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        public void LoadGrid(int prokirixi_code, string iek_name)
        {
            var aitiseis = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                           where a.ΚΛΑΔΟΣ == "ΠΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && a.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                           orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                           select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        #endregion

        #region Load Gauge and Panel data wrapper methods

        public void SetGaugeValues(double p1, double p2, double p3)
        {
            gauge1_marker.Value = p1;
            gauge2_marker.Value = p2;
            gauge3_marker.Value = p3;
            radGaugePE.ToolTip = String.Format("Πλήθος {0}, από {1}", PE1Count, TotalCount);
            radGaugeTE.ToolTip = String.Format("Πλήθος {0}, από {1}", PE2Count, TotalCount);
            radGaugeDE.ToolTip = String.Format("Πλήθος {0}, από {1}", PE3Count, TotalCount);
        }

        public void SetPanelValues(double p1, double p2, double p3)
        {
            txtCountPE1.Text = Convert.ToString(PE1Count);
            txtCountPE2.Text = Convert.ToString(PE2Count);
            txtCountPE3.Text = Convert.ToString(PE3Count);
            txtTotalCount1.Text = Convert.ToString(TotalCount);
            txtTotalCount2.Text = Convert.ToString(TotalCount);
            txtTotalCount3.Text = Convert.ToString(TotalCount);
            txtPercentPE1.Text = Convert.ToString(String.Format("{0:0.00}", p1)) + " %";
            txtPercentPE2.Text = Convert.ToString(String.Format("{0:0.00}", p2)) + " %";
            txtPercentPE3.Text = Convert.ToString(String.Format("{0:0.00}", p3)) + " %";
        }

        public void ClearGaugeValues()
        {
            gauge1_marker.Value = 0;
            gauge2_marker.Value = 0;
            gauge3_marker.Value = 0;
            radGaugePE.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
            radGaugeTE.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
            radGaugeDE.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
        }

        public void ClearPanelValues()
        {
            txtCountPE1.Text = "0";
            txtCountPE2.Text = "0";
            txtCountPE3.Text = "0";
            txtTotalCount1.Text = "0";
            txtTotalCount2.Text = "0";
            txtTotalCount3.Text = "0";
            txtPercentPE1.Text = "0" + " %";
            txtPercentPE2.Text = "0" + " %";
            txtPercentPE3.Text = "0" + " %";
        }

        public void SetGaugeAndPanelData()
        {
            // percentage values of data
            double pe1N = (double)PE1Count;
            double pe2N = (double)PE2Count;
            double pe3N = (double)PE3Count;
            double N = (double)TotalCount;

            double p1 = (100.0 * pe1N) / N;
            double p2 = (100.0 * pe2N) / N;
            double p3 = (100.0 * pe3N) / N;

            // set the values of the gauges
            N = (double)TotalCount;

            p1 = (100.0 * pe1N) / N;
            p2 = (100.0 * pe2N) / N;
            p3 = (100.0 * pe3N) / N;

            SetGaugeValues(p1, p2, p3);     // set needle values
            SetPanelValues(p1, p2, p3);     // set panel values

        } // SetGaugeAndPanelData

        #endregion


        #region Load Gauge Data ( 3 overloads)

        /// <summary>
        /// Loads gauge data for all aitiseis
        /// </summary>
        public void LoadGaugeData()
        {
            var qryPE1 = from pe1 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where pe1.ΚΛΑΔΟΣ == "ΠΕ" && (pe1.ΜΕΤΑΠΤΥΧΙΑΚΟ == false && pe1.ΔΙΔΑΚΤΟΡΙΚΟ == false)
                        select pe1;

            var qryPE2 = from pe2 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe2.ΚΛΑΔΟΣ == "ΠΕ" && (pe2.ΜΕΤΑΠΤΥΧΙΑΚΟ == true && pe2.ΔΙΔΑΚΤΟΡΙΚΟ == false)
                        select pe2;

            var qryPE3 = from pe3 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe3.ΚΛΑΔΟΣ == "ΠΕ" && (pe3.ΔΙΔΑΚΤΟΡΙΚΟ == true)
                        select pe3;

            // absolute values of data
            PE1Count = qryPE1.Count();
            PE2Count = qryPE2.Count();
            PE3Count = qryPE3.Count();
            TotalCount = PE1Count + PE2Count + PE3Count;

            SetGaugeAndPanelData();


        } // LoadGaugeData

        /// <summary>
        /// Loads gauge data for selected prokirixi
        /// </summary>
        /// <param name="prokirixi_code"></param>
        public void LoadGaugeData(int prokirixi_code)
        {
            var qryPE1 = from pe1 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe1.ΚΛΑΔΟΣ == "ΠΕ" && (pe1.ΜΕΤΑΠΤΥΧΙΑΚΟ == false && pe1.ΔΙΔΑΚΤΟΡΙΚΟ == false) 
                         && pe1.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                         select pe1;

            var qryPE2 = from pe2 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe2.ΚΛΑΔΟΣ == "ΠΕ" && (pe2.ΜΕΤΑΠΤΥΧΙΑΚΟ == true && pe2.ΔΙΔΑΚΤΟΡΙΚΟ == false)
                         && pe2.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                         select pe2;

            var qryPE3 = from pe3 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe3.ΚΛΑΔΟΣ == "ΠΕ" && (pe3.ΜΕΤΑΠΤΥΧΙΑΚΟ == false && pe3.ΔΙΔΑΚΤΟΡΙΚΟ == true)
                         && pe3.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                         select pe3;

            // absolute values of data
            PE1Count = qryPE1.Count();
            PE2Count = qryPE2.Count();
            PE3Count = qryPE3.Count();
            TotalCount = PE1Count + PE2Count + PE3Count;

            SetGaugeAndPanelData();

        }

        public void LoadGaugeData(int prokirixi_code, string iek_name)
        {
            var qryPE1 = from pe1 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe1.ΚΛΑΔΟΣ == "ΠΕ" && (pe1.ΜΕΤΑΠΤΥΧΙΑΚΟ == false && pe1.ΔΙΔΑΚΤΟΡΙΚΟ == false)
                         && pe1.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && pe1.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                         select pe1;

            var qryPE2 = from pe2 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe2.ΚΛΑΔΟΣ == "ΠΕ" && (pe2.ΜΕΤΑΠΤΥΧΙΑΚΟ == true && pe2.ΔΙΔΑΚΤΟΡΙΚΟ == false)
                         && pe2.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && pe2.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                         select pe2;

            var qryPE3 = from pe3 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                         where pe3.ΚΛΑΔΟΣ == "ΠΕ" && (pe3.ΜΕΤΑΠΤΥΧΙΑΚΟ == false && pe3.ΔΙΔΑΚΤΟΡΙΚΟ == true)
                         && pe3.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && pe3.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                         select pe3;

            // absolute values of data
            PE1Count = qryPE1.Count();
            PE2Count = qryPE2.Count();
            PE3Count = qryPE3.Count();
            TotalCount = PE1Count + PE2Count + PE3Count;

            SetGaugeAndPanelData();

        }


        #endregion


        #region Button Filter events

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ΠΡΟΚΗΡΥΞΗ prokirixi = cboProkirixiSelection.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ iek = cboIekSelection.SelectedItem as ΙΕΚ;

            if (prokirixi == null && iek == null) return;
            else if (prokirixi != null && iek == null)
            {
                int selected_prokirixi = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                LoadGaugeData(selected_prokirixi);
                LoadGrid(selected_prokirixi);
            }
            else if (prokirixi != null && iek != null)
            {
                int selected_prokirixi = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                string selected_iek = iek.ΙΕΚ_ΟΝΟΜΑΣΙΑ;
                LoadGaugeData(selected_prokirixi, selected_iek);
                LoadGrid(selected_prokirixi, selected_iek);
            }
            else if (prokirixi == null && iek != null)
            {
                UserFunctions.ShowAdminMessage("Πρέπει να επιλέξετε και προκήρυξη.");
                return;
            }

        }   //btnView_Click

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cboIekSelection.SelectedIndex = -1;
            cboProkirixiSelection.SelectedIndex = -1;

            LoadData();
        }

        #endregion


    } // TeacherQualificationsPE
}
