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
    /// Inw2raction logic for TeacherWorkTyw1Count.xaml
    /// </summary>
    public partial class TeacherWorkTypeCount : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        int W1Count, W2Count, W3Count, TotalCount;

        public TeacherWorkTypeCount()
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

        public void SetGaugeValues(double w1, double w2, double w3)
        {
            needleW1.Value = w1;
            needleW2.Value = w2;
            needleW3.Value = w3;
            radialGaugeW1.ToolTip = String.Format("Πλήθος {0}, από {1}", W1Count, TotalCount);
            radialGaugeW2.ToolTip = String.Format("Πλήθος {0}, από {1}", W2Count, TotalCount);
            radialGaugeW3.ToolTip = String.Format("Πλήθος {0}, από {1}", W3Count, TotalCount);
        }

        public void SetPanelValues(double w1, double w2, double w3)
        {
            txtCountW1.Text = Convert.ToString(W1Count);
            txtCountW2.Text = Convert.ToString(W2Count);
            txtCountW3.Text = Convert.ToString(W3Count);
            txtTotalCount1.Text = Convert.ToString(TotalCount);
            txtTotalCount2.Text = Convert.ToString(TotalCount);
            txtTotalCount3.Text = Convert.ToString(TotalCount);
            txtPercentW1.Text = Convert.ToString(String.Format("{0:0.00}", w1)) + " %";
            txtPercentW2.Text = Convert.ToString(String.Format("{0:0.00}", w2)) + " %";
            txtPercentW3.Text = Convert.ToString(String.Format("{0:0.00}", w3)) + " %";
        }

        public void ClearGaugeValues()
        {
            // set needle values
            needleW1.Value = 0; // (double)W1Count;
            needleW2.Value = 0; // (double)W2Count;
            needleW3.Value = 0; // (double)W3Count;

            radialGaugeW1.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
            radialGaugeW2.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
            radialGaugeW3.ToolTip = String.Format("Πλήθος {0}, από {1}", 0, 0);
        }

        public void ClearPanelValues()
        {
            txtCountW1.Text = "0";
            txtCountW2.Text = "0";
            txtCountW3.Text = "0";
            txtTotalCount1.Text = "0";
            txtTotalCount2.Text = "0";
            txtTotalCount3.Text = "0";
            txtPercentW1.Text = "0" + " %";
            txtPercentW2.Text = "0" + " %";
            txtPercentW3.Text = "0" + " %";
        }

        public void SetGaugeAndPanelData()
        {
            // percentage values of data
            double w1N = (double)W1Count;
            double w2N = (double)W2Count;
            double w3N = (double)W3Count;

            radialScaleW1.Min = 0;
            radialScaleW1.Max = 100;

            radialScaleW2.Min = 0;
            radialScaleW2.Max = 100;

            radialScaleW3.Min = 0;
            radialScaleW3.Max = 100;

            double N = (double)TotalCount;
            double w1 = ((100.0 * w1N) / N);
            double w2 = ((100.0 * w2N) / N);
            double w3 = ((100.0 * w3N) / N);

            SetGaugeValues(w1, w2, w3);     // set needle values
            SetPanelValues(w1, w2, w3);     // set panel values

        } // SetGaugeAndPanelData

        #endregion


        #region Load Gauge Data ( 3 overloads)

        /// <summary>
        /// Loads gauge data for all aitiseis
        /// </summary>
        public void LoadGaugeData()
        {
            var qryW1 = from w1 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w1.ΕΡΓΑΣΙΑ == "ΕΡΓΑΖΟΜΕΝΟΙ ΣΤΟΝ ΙΔΙΩΤΙΚΟ ΤΟΜΕΑ" && w1.ΑΝΕΡΓΟΣ_12 == false
                        select w1;

            var qryW2 = from w2 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w2.ΕΡΓΑΣΙΑ == "ΔΗΜΟΣΙΟΙ ΥΠΑΛΛΗΛΟΙ Ή ΣΥΝΤΑΞΙΟΥΧΟΙ"
                        select w2;

            var qryW3 = from w3 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w3.ΑΝΕΡΓΟΣ_12 == true
                        select w3;

            // absolute values of data
            W1Count = qryW1.Count();
            W2Count = qryW2.Count();
            W3Count = qryW3.Count();
            TotalCount = W1Count + W2Count + W3Count;

            SetGaugeAndPanelData();

        } // LoadGaugeData

        /// <summary>
        /// Loads gauge data for selecw2d prokirixi
        /// </summary>
        /// <param name="prokirixi_code"></param>
        public void LoadGaugeData(int prokirixi_code)
        {
            var qryW1 = from w1 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w1.ΚΛΑΔΟΣ == "ΠΕ" && w1.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select w1;

            var qryW2 = from w2 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w2.ΚΛΑΔΟΣ == "ΤΕ" && w2.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select w2;

            var qryW3 = from w3 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w3.ΚΛΑΔΟΣ == "ΔΕ" && w3.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select w3;

            // absoluw2 values of data
            W1Count = qryW1.Count();
            W2Count = qryW2.Count();
            W3Count = qryW3.Count();
            TotalCount = W1Count + W2Count + W3Count;

            SetGaugeAndPanelData();

        }

        public void LoadGaugeData(int prokirixi_code, string iek_name)
        {
            var qryW1 = from w1 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w1.ΚΛΑΔΟΣ == "ΠΕ" && w1.ΠΡΟΚΗΡΥΞΗ == prokirixi_code && w1.ΙΕΚ_ΟΝΟΜΑΣΙΑ == iek_name
                        select w1;

            var qryW2 = from w2 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w2.ΚΛΑΔΟΣ == "ΤΕ" && w2.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select w2;

            var qryW3 = from w3 in db.ViewΑΙΤΗΣΕΙΣ_ΣΥΝΟΨΗs
                        where w3.ΚΛΑΔΟΣ == "ΔΕ" && w3.ΠΡΟΚΗΡΥΞΗ == prokirixi_code
                        select w3;

            // absolute values of data
            W1Count = qryW1.Count();
            W2Count = qryW2.Count();
            W3Count = qryW3.Count();
            TotalCount = W1Count + W2Count + W3Count;

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



    }   // TeacherWorkTyw1Count
}
