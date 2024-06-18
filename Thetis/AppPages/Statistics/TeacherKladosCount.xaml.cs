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
    /// Interaction logic for TeacherKladosCount.xaml
    /// </summary>
    public partial class TeacherKladosCount : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        BrushConverter bc = new BrushConverter();
        // Gauge and Panel fields
        int PECount, TECount, DECount, TotalCount;

        public TeacherKladosCount()
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
                             orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                             select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        public void LoadGrid(int prokirixi_code)
        {
            var aitiseis = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                             where a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                             orderby a.ΠΡΟΚΗΡΥΞΗ descending, a.ΕΠΩΝΥΜΟ, a.ΟΝΟΜΑ
                             select a;

            aitisiGrid.ItemsSource = aitiseis.ToList();
        }

        public void LoadGrid(int prokirixi_code, string iek_name)
        {
            var aitiseis = from a in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                           where a.ΠΡΟΚΗΡΥΞΗ == prokirixi_code & a.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
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
            radialGaugePE.ToolTip = String.Format("Πλήθος {0}, από {1}", PECount, TotalCount);
            radialGaugeTE.ToolTip = String.Format("Πλήθος {0}, από {1}", TECount, TotalCount);
            radialGaugeDE.ToolTip = String.Format("Πλήθος {0}, από {1}", DECount, TotalCount);
        }

        public void SetPanelValues(double N, double p, double t, double d)
        {
            txtCountPE.Text = Convert.ToString(PECount);
            txtCountTE.Text = Convert.ToString(TECount);
            txtCountDE.Text = Convert.ToString(DECount);
            txtTotalCount1.Text = Convert.ToString(TotalCount);
            txtTotalCount2.Text = Convert.ToString(TotalCount);
            txtTotalCount3.Text = Convert.ToString(TotalCount);
            txtPercentPE.Text = Convert.ToString(String.Format("{0:0.00}", p)) + " %";
            txtPercentTE.Text = Convert.ToString(String.Format("{0:0.00}", t)) + " %";
            txtPercentDE.Text = Convert.ToString(String.Format("{0:0.00}", d)) + " %";
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
            txtCountPE.Text = "0";
            txtCountTE.Text = "0";
            txtCountDE.Text = "0";
            txtTotalCount1.Text = "0";
            txtTotalCount2.Text = "0";
            txtTotalCount3.Text = "0";
            txtPercentPE.Text = "0" + " %";
            txtPercentTE.Text = "0" + " %";
            txtPercentDE.Text = "0" + " %";
        }

        public void SetGaugeAndPanelData()
        {
            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;

            radialScalePE.Min = 0;
            radialScalePE.Max = 100;

            radialScaleTE.Min = 0;
            radialScaleTE.Max = 100;

            radialScaleDE.Min = 0;
            radialScaleDE.Max = 100;

            TotalCount = PECount + TECount + DECount;
            double N = (double)TotalCount;
            double p = ((100.0 * peN) / N);
            double t = ((100.0 * teN) / N);
            double d = ((100.0 * deN) / N);

            SetGaugeValues(N, p, t, d);     // set needle values
            SetPanelValues(N, p, t, d);     // set panel values

        } // SetGaugeAndPanelData

        #endregion

        #region Load Gauge Data ( 3 overloads)

        /// <summary>
        /// Loads gauge data for all aitiseis
        /// </summary>
        public void LoadGaugeData()
        {
            var qryPE = from pe in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where pe.ΚΛΑΔΟΣ == "ΠΕ"
                        select pe;

            var qryTE = from te in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where te.ΚΛΑΔΟΣ == "ΤΕ"
                        select te;

            var qryDE = from de in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where de.ΚΛΑΔΟΣ == "ΔΕ"
                        select de;

            // absolute values of data
            PECount = qryPE.Count();
            TECount = qryTE.Count();
            DECount = qryDE.Count();
            TotalCount = PECount + TECount + DECount;

            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;
            double N = (double)TotalCount;

            double p = (100.0 * peN) / N;
            double t = (100.0 * teN) / N;
            double d = (100.0 * deN) / N;

            SetGaugeAndPanelData();
        } // LoadGaugeData

        /// <summary>
        /// Loads gauge data for selected prokirixi
        /// </summary>
        /// <param name="prokirixi_code"></param>
        public void LoadGaugeData(int prokirixi_code)
        {
            var qryPE = from pe in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where pe.ΚΛΑΔΟΣ == "ΠΕ" && pe.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select pe;

            var qryTE = from te in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where te.ΚΛΑΔΟΣ == "ΤΕ" && te.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select te;

            var qryDE = from de in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where de.ΚΛΑΔΟΣ == "ΔΕ" && de.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select de;

            // absolute values of data
            PECount = qryPE.Count();
            TECount = qryTE.Count();
            DECount = qryDE.Count();
            TotalCount = PECount + TECount + DECount;

            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;
            double N = (double)TotalCount;

            double p = (100.0 * peN) / N;
            double t = (100.0 * teN) / N;
            double d = (100.0 * deN) / N;

            SetGaugeAndPanelData();
        }

        public void LoadGaugeData(int prokirixi_code, string iek_name)
        {
            var qryPE = from pe in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where pe.ΚΛΑΔΟΣ == "ΠΕ" && pe.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && pe.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                        select pe;

            var qryTE = from te in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where te.ΚΛΑΔΟΣ == "ΤΕ" && te.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && te.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                        select te;

            var qryDE = from de in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where de.ΚΛΑΔΟΣ == "ΔΕ" && de.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && de.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                        select de;

            // absolute values of data
            PECount = qryPE.Count();
            TECount = qryTE.Count();
            DECount = qryDE.Count();
            TotalCount = PECount + TECount + DECount;

            // percentage values of data
            double peN = (double)PECount;
            double teN = (double)TECount;
            double deN = (double)DECount;
            double N = (double)TotalCount;

            double p = (100.0 * peN) / N;
            double t = (100.0 * teN) / N;
            double d = (100.0 * deN) / N;

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
                LoadGaugeData(selected_prokirixi,selected_iek );
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

    }   // TeacherKladosCount
}
