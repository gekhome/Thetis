using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Model;
using Thetis.DataAccess;
using Thetis.Utilities;
using Thetis.Controls;



namespace Thetis.AppPages.Admin
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class SearchAitiseis : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private CommitModel cm = new CommitModel();
        Loading loadingWin = null;
        bool isLoadingWinCreated = false;

        public SearchAitiseis()
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

            var aitiseis = from a in db.ΑΙΤΗΣΗs
                           orderby a.ΠΡΟΚΗΡΥΞΗ, a.ΙΕΚ_ΑΙΤΗΣΗΣ, a.ΠΡΩΤΟΚΟΛΛΟ
                           select a;

            var eidikotites = from e in db.ΕΙΔΙΚΟΤΗΤΑs
                              select e;

            var oca = new ObservableCollection<ΑΙΤΗΣΗ>(aitiseis.ToList());

            cboProkirixiSelection.ItemsSource = prokirixis.ToList();
            cboiekSelection.ItemsSource = ieks.ToList();
            cboEidikotis.ItemsSource = eidikotites.ToList();
            dataGrid.ItemsSource = oca;
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
                    var gridData = from gd in db.ΑΙΤΗΣΗs
                                   where gd.ΠΡΟΚΗΡΥΞΗ == prok
                                   orderby gd.ΙΕΚ_ΑΙΤΗΣΗΣ, gd.ΠΡΩΤΟΚΟΛΛΟ
                                   select gd;
                    var oca = new ObservableCollection<ΑΙΤΗΣΗ>(gridData.ToList());
                    dataGrid.ItemsSource = oca;
                }
                else
                {
                    var gridData = from gd in db.ΑΙΤΗΣΗs
                                   where gd.ΠΡΟΚΗΡΥΞΗ == prok && gd.ΙΕΚ_ΑΙΤΗΣΗΣ == iek
                                   orderby gd.ΠΡΩΤΟΚΟΛΛΟ
                                   select gd;
                    var oca = new ObservableCollection<ΑΙΤΗΣΗ>(gridData.ToList());
                    dataGrid.ItemsSource = oca;
                }
            }

        } // ShowSelectedData

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.BeginInsert();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.BeginEdit();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (dataGrid.SelectedItems.Count == 0) { return; }
            // verify deletion from user
            string checkMessage = "Να γίνει διαγραφή των επιλεγμένων εγγραφών; " + "\n";

            if (MessageBox.Show(checkMessage, "Διαγραφή", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { return; }

            // proceed with deletion process
            foreach (var row in dataGrid.SelectedItems)
            {
                ΑΙΤΗΣΗ aitiseis = row as ΑΙΤΗΣΗ;
                db.ΑΙΤΗΣΗs.DeleteOnSubmit(aitiseis);
            }
        }

        private void dataGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            // this event is raised when row editing is done in both insert and edit modes
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                var row = e.Row as GridViewRow;
                ΑΙΤΗΣΗ aitiseis = row.Item as ΑΙΤΗΣΗ;           // cast it to object ΑΙΤΗΣΗ

                // these two methods do the database udpating
                db.ΑΙΤΗΣΗs.InsertOnSubmit(aitiseis);            // insert new row into collection
            }
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            // load the existing collection without new changes (no submit is done unless Save is pressed)
            LoadData();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cm.UserCanUpdate() == false)
            {
                LoadData();
                return;
            }
            cm.CommitData(db);
            LoadData();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        #region Loading thread
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Close the indicator window and shut down the STA thread with its dispatcher
            if (loadingWin != null) loadingWin.CloseForm();
            //busyIndicator.IsBusy = false;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                loadingWin = new Loading();
                isLoadingWinCreated = true;
                loadingWin.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (!isLoadingWinCreated) ;
        }

        #endregion

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var aitiseis = from a in db.ΑΙΤΗΣΗs
                           where a.ΜΕΤΑΦΕΡΘΗΚΕ == true || !a.ΜΕΤΑΦΕΡΘΗΚΕ.HasValue
                           select a;

            foreach (ΑΙΤΗΣΗ aitisi in aitiseis)
            {
                aitisi.ΜΕΤΑΦΕΡΘΗΚΕ = false;
            }
            cm.CommitData(db);
        }
    } // class Search
}
