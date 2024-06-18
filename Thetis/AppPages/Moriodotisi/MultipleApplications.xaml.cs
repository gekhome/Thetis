using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.Controls;


namespace Thetis.AppPages.Moriodotisi
{
    /// <summary>
    /// Interaction logic for MultipleApplications.xaml
    /// </summary>
    public partial class MultipleApplications : Page
    {
        private ThetisDataContext db = new ThetisDataContext();

        public MultipleApplications()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            var prokirixis = from p in db.ΠΡΟΚΗΡΥΞΗs
                             orderby p.ΗΜΝΙΑ_ΕΝΑΡΞΗ descending
                             select p;

            var aitiseis = from a in db.qryMultipleAppsNomosBetas
                           orderby a.ΑΦΜ, a.ΙΕΚ_ΟΝΟΜΑΣΙΑ, a.ΝΟΜΟΣ
                           select a;

            cboProkirixiSelection.ItemsSource = prokirixis.ToList();
            aitisiGrid.ItemsSource = null;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ΠΡΟΚΗΡΥΞΗ prokirixi = cboProkirixiSelection.SelectedItem as ΠΡΟΚΗΡΥΞΗ;

            if (prokirixi == null)
            {
                UserFunctions.ShowAdminMessage("Πρέπει να επιλέξετε προκήρυξη.");
                return;
            }

            int prok_id = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;

            ShowSelectedData(prok_id);
        }

        private void ShowSelectedData(int prok)
        {
            using (ThetisDataContext db = new ThetisDataContext())
            {
                    var gridData = from gd in db.qryMultipleAppsNomosBetas
                                   where gd.ΠΡΟΚΗΡΥΞΗ == prok
                                   orderby gd.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                                   select gd;
                    aitisiGrid.ItemsSource = gridData.ToList();
                }
            }

        } // ShowSelectedData

    }
