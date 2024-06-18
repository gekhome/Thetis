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
    /// Interaction logic for TeacherEnstasiCount.xaml
    /// </summary>
    public partial class TeacherEnstasiCount : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        BrushConverter bc = new BrushConverter();
        // Gauge and Panel fields
        int PECount, TECount, DECount, TotalEnstaseisCount, TotalAitiseisCount, MaxCount;
        int N1, N2, N3;
        bool withReduction = false;
        int toggleState = 1;
        string reductionState;

        public TeacherEnstasiCount()
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
                           where a.ΕΝΣΤΑΣΗ == true
                           orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                           select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        public void LoadGrid(int prokirixi_code)
        {
            var aitiseis = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                           where a.ΕΝΣΤΑΣΗ == true && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                           orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                           select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        public void LoadGrid(int prokirixi_code, string iek_name)
        {
            var aitiseis = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                           where a.ΕΝΣΤΑΣΗ == true && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code & a.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                           orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                           select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        #endregion

        #region Load Gauge and Panel data wrapper methods

        public void SetGaugeValues(double N, double p, double t, double d)
        {
            needlePE.Value = p;
            needleTE.Value = t;
            needleDE.Value = d;
            radialGaugePE.ToolTip = String.Format("Πλήθος {0}, από {1}", PECount, TotalEnstaseisCount);
            radialGaugeTE.ToolTip = String.Format("Πλήθος {0}, από {1}", TECount, TotalEnstaseisCount);
            radialGaugeDE.ToolTip = String.Format("Πλήθος {0}, από {1}", DECount, TotalEnstaseisCount);
        }

        public void SetPanelValues(double N, double p, double t, double d)
        {
            txtEnstasiCountPE.Text = Convert.ToString(PECount);
            txtEnstasiCountTE.Text = Convert.ToString(TECount);
            txtEnstasiCountDE.Text = Convert.ToString(DECount);
            txtTotalCountPE.Text = Convert.ToString(N1);
            txtTotalCountTE.Text = Convert.ToString(N2);
            txtTotalCountDE.Text = Convert.ToString(N3);
            txtPercentPE.Text = Convert.ToString(String.Format("{0:0.00}", p)) + " %";
            txtPercentTE.Text = Convert.ToString(String.Format("{0:0.00}", t)) + " %";
            txtPercentDE.Text = Convert.ToString(String.Format("{0:0.00}", d)) + " %";
            txtEnstasiCount.Text = Convert.ToString(TotalEnstaseisCount);
            txtTotalCount.Text = Convert.ToString(TotalAitiseisCount);
            double percent = (100.0 * TotalEnstaseisCount) / TotalAitiseisCount;
            txtPercent.Text = Convert.ToString(String.Format("{0:0.00}", percent)) + " %";
            txtReduction.Text = reductionState;
        }

        public void ClearGaugeValues()
        {
            needlePE.Value = 0;
            needleTE.Value = 0;
            needleDE.Value = 0;
            radialGaugePE.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
            radialGaugeTE.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
            radialGaugeDE.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
        }

        public void ClearPanelValues()
        {
            txtEnstasiCountPE.Text = "0";
            txtEnstasiCountTE.Text = "0";
            txtEnstasiCountDE.Text = "0";
            txtTotalCountPE.Text = "0";
            txtTotalCountTE.Text = "0";
            txtTotalCountDE.Text = "0";
            txtPercentPE.Text = "0" + " %";
            txtPercentTE.Text = "0" + " %";
            txtPercentDE.Text = "0" + " %";
            txtEnstasiCount.Text = "0";
            txtTotalCount.Text = "0";
            double percent = 0;
            txtPercent.Text = Convert.ToString(percent) + " %";

        }

        public void SetGaugeAndPanelData(bool reduction)
        {
            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;
            double NPE = (double)N1;
            double NTE = (double)N2;
            double NDE = (double)N3;

            radialScalePE.Min = 0;
            radialScalePE.Max = 100;

            radialScaleTE.Min = 0;
            radialScaleTE.Max = 100;

            radialScaleDE.Min = 0;
            radialScaleDE.Max = 100;

            if (reduction == false)
            {
                TotalEnstaseisCount = PECount + TECount + DECount;
                TotalAitiseisCount = N1 + N2 + N3;
                reductionState = "* Χωρίς αναγωγή";
                double N = (double)TotalEnstaseisCount;
                double p = ((100.0 * peN) / NPE);
                double t = ((100.0 * teN) / NTE);
                double d = ((100.0 * deN) / NDE);

                SetGaugeValues(N, p, t, d);     // set needle values
                SetPanelValues(N, p, t, d);     // set panel values
            }
            else if (reduction == true)
            {
                TotalEnstaseisCount = PECount + TECount + DECount;
                TotalAitiseisCount = MaxCount;
                reductionState = "* Με αναγωγή";
                double N = (double)TotalEnstaseisCount;
                double p = ((100.0 * peN) / TotalAitiseisCount);
                double t = ((100.0 * teN) / TotalAitiseisCount);
                double d = ((100.0 * deN) / TotalAitiseisCount);

                SetGaugeValues(N, p, t, d);     // set needle values     
                SetPanelValues(N, p, t, d);     // set panel values
            }

        } // SetGaugeAndPanelData

        #endregion

        #region Load Gauge Data ( 3 overloads)

        public void LoadGaugeData()
        {
            var aitiseisPE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΠΕ"
                             select a;
            var aitiseisTE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΤΕ"
                             select a;
            var aitiseisDE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΔΕ"
                             select a;

            var qryPE = from pe in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where pe.ΕΝΣΤΑΣΗ == true && pe.ΚΛΑΔΟΣ == "ΠΕ"
                        select pe;

            var qryTE = from te in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where te.ΕΝΣΤΑΣΗ == true && te.ΚΛΑΔΟΣ == "ΤΕ"
                        select te;

            var qryDE = from de in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where de.ΕΝΣΤΑΣΗ == true && de.ΚΛΑΔΟΣ == "ΔΕ"
                        select de;

            N1 = aitiseisPE.Count();
            N2 = aitiseisTE.Count();
            N3 = aitiseisDE.Count();

            MaxCount = UserFunctions.Max(N1, N2, N3);

            // absolute values of data
            PECount = qryPE.Count();
            TECount = qryTE.Count();
            DECount = qryDE.Count();

            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;
            double NPE = (double)N1;
            double NTE = (double)N2;
            double NDE = (double)N3;

            ClearGaugeValues();
            ClearPanelValues();
            SetGaugeAndPanelData(withReduction);

        } // LoadGaugeData

        public void LoadGaugeData(int prokirixi_code)
        {
            var aitiseisPE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΠΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                             select a;
            var aitiseisTE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΤΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                             select a;
            var aitiseisDE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΔΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                             select a;

            var qryPE = from pe in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where pe.ΕΝΣΤΑΣΗ == true && pe.ΚΛΑΔΟΣ == "ΠΕ" && pe.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select pe;

            var qryTE = from te in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where te.ΕΝΣΤΑΣΗ == true && te.ΚΛΑΔΟΣ == "ΤΕ" && te.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select te;

            var qryDE = from de in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where de.ΕΝΣΤΑΣΗ == true && de.ΚΛΑΔΟΣ == "ΔΕ" && de.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select de;

            int N1 = aitiseisPE.Count();
            int N2 = aitiseisTE.Count();
            int N3 = aitiseisDE.Count();

            MaxCount = UserFunctions.Max(N1, N2, N3);

            // absolute values of data
            PECount = qryPE.Count();
            TECount = qryTE.Count();
            DECount = qryDE.Count();

            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;
            double NPE = (double)N1;
            double NTE = (double)N2;
            double NDE = (double)N3;

            ClearGaugeValues();
            ClearPanelValues();
            SetGaugeAndPanelData(withReduction);

        }

        public void LoadGaugeData(int prokirixi_code, string iek_name)
        {
            var aitiseisPE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΠΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && a.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                             select a;
            var aitiseisTE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΤΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && a.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                             select a;
            var aitiseisDE = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΚΛΑΔΟΣ == "ΔΕ" && a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && a.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                             select a;

            var qryPE = from pe in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where pe.ΕΝΣΤΑΣΗ == true && pe.ΚΛΑΔΟΣ == "ΠΕ" && pe.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && pe.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                        select pe;

            var qryTE = from te in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where te.ΕΝΣΤΑΣΗ == true && te.ΚΛΑΔΟΣ == "ΤΕ" && te.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && te.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                        select te;

            var qryDE = from de in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where de.ΕΝΣΤΑΣΗ == true && de.ΚΛΑΔΟΣ == "ΔΕ" && de.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && de.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                        select de;


            int N1 = aitiseisPE.Count();
            int N2 = aitiseisTE.Count();
            int N3 = aitiseisDE.Count();

            MaxCount = UserFunctions.Max(N1, N2, N3);

            // absolute values of data
            PECount = qryPE.Count();
            TECount = qryTE.Count();
            DECount = qryDE.Count();

            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;
            double NPE = (double)N1;
            double NTE = (double)N2;
            double NDE = (double)N3;

            ClearGaugeValues();
            ClearPanelValues();
            SetGaugeAndPanelData(withReduction);

            }

        #endregion

        #region Button Filter events

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ΠΡΟΚΗΡΥΞΗ prokirixi = cboProkirixiSelection.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ iek = cboIekSelection.SelectedItem as ΙΕΚ;

            if (prokirixi == null && iek == null)
            {
                LoadData();
            }
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

        #region Button Reduction Events

        private void btnReduction_Click(object sender, RoutedEventArgs e)
        {
            btnReduction.Background = (Brush)bc.ConvertFrom("#550D49");
            btnReduction.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");

            if (toggleState == 1)
            {
                withReduction = true;
                toggleState = 2;
                btnReduction.Content = "Χωρίς Αναγωγή";
                //UserFunctions.ShowAdminMessage("toggleState = " + toggleState);
                //LoadGaugeData();
            }
            else if (toggleState == 2)
            {
                withReduction = false;
                btnReduction.Content = "Με Αναγωγή";
                toggleState = 1;
                //UserFunctions.ShowAdminMessage("toggleState = " + toggleState);
                //LoadGaugeData();
            }
        }

        private void btnReduction_Initialized(object sender, EventArgs e)
        {
            btnReduction.Content = "Με Αναγωγή";
            btnReduction.Background = (Brush)bc.ConvertFrom("#550D49");
            btnReduction.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            withReduction = false;
            toggleState = 1;
        }

        #endregion

    }   // TeacherEnstasiCount
}
