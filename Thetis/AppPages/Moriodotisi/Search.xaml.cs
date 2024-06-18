using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.Controls;



namespace Thetis.AppPages.Moriodotisi
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        public Search()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            var prokirixis = from p in db.ΠΡΟΚΗΡΥΞΗs
                             orderby p.ΗΜΝΙΑ_ΕΝΑΡΞΗ descending
                             select p;

            var ieks = from i in db.ΙΕΚs
                       orderby i.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                       select i;

            var aitiseis = from a in db.qryAITISI_TEACHERs
                           orderby a.ΠΡΟΚΗΡΥΞΗ, a.ΙΕΚ_ΑΙΤΗΣΗΣ, a.ΠΡΩΤΟΚΟΛΛΟ
                           select a;

            cboProkirixiSelection.ItemsSource = prokirixis.ToList();
            cboiekSelection.ItemsSource = ieks.ToList();
            aitisiGrid.ItemsSource = null;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {

            ΠΡΟΚΗΡΥΞΗ prokirixi = cboProkirixiSelection.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ iek = cboiekSelection.SelectedItem as ΙΕΚ;
            int iek_id;
            String checkMessage;

            if (prokirixi == null)
            {
                UserFunctions.ShowAdminMessage("Πρέπει να επιλέξετε τουλάχιστον προκήρυξη.");
                return;
            }

            if (iek == null)
            {
                checkMessage = "Δεν επιλέξατε ΙΕΚ. Θα γίνει προβολή όλων.\n";
                checkMessage = checkMessage + "Αυτό θα διαρκέσει μερικά λεπτά. Θέλετε να συνεχίσετε;";
                if (MessageBox.Show(checkMessage, "Ειδοποίηση", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                { return; }
                iek_id = 0;  // 0 means all ieks
            }
            else
            {
                iek_id = iek.ΙΕΚ_ΚΩΔ;
            }

            int prok_id = prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
            
            ShowSelectedData(prok_id, iek_id);
        }

        private void ShowSelectedData(int prok, int iek)
        {
            using (ThetisDataContext db = new ThetisDataContext())
            {
                if (iek == 0)
                {
                    var gridData = from gd in db.qryAITISI_TEACHERs
                                   where gd.ΠΡΟΚΗΡΥΞΗ == prok
                                   orderby gd.ΙΕΚ_ΟΝΟΜΑΣΙΑ, gd.ΠΡΩΤΟΚΟΛΛΟ
                                   select gd;
                    aitisiGrid.ItemsSource = gridData.ToList();
                }
                else
                {
                    var gridData = from gd in db.qryAITISI_TEACHERs
                                   where gd.ΠΡΟΚΗΡΥΞΗ == prok && gd.ΙΕΚ_ΑΙΤΗΣΗΣ == iek
                                   orderby gd.ΠΡΩΤΟΚΟΛΛΟ
                                   select gd;
                    aitisiGrid.ItemsSource = gridData.ToList();
                }
            }

        } // ShowSelectedData

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    } // class Search
}
