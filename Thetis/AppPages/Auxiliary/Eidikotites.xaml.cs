using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Model;
using Thetis.DataAccess;
using Thetis.Utilities;
using Thetis.Controls;


namespace Thetis.AppPages.Auxiliary
{
    /// <summary>
    /// Interaction logic for Eidikotites.xaml
    /// </summary>
    public partial class Eidikotites : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        SelectedProkirixi sp = new SelectedProkirixi();
        private CommitModel cm = new CommitModel();

        public Eidikotites()
        {
            InitializeComponent();
            LoadData();
        }
        #region Data Load and Selection
        public void LoadData()
        {
            var prokirixis = from p in db.ΠΡΟΚΗΡΥΞΗs
                             orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                             select p;
            var ieks = from iek in db.ΙΕΚs
                       orderby iek.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                       select iek;
            var eidikotites = from e in db.ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑs
                              orderby e.ΚΩΔ_ΕΙΔΙΚΟΤΗΤΑ
                              select e;

            //data source for the combo
            var eidikotita = from e in db.ΕΙΔΙΚΟΤΗΤΑs
                              orderby e.ΒΑΘΜΙΔΑ, e.ΚΛΑΔΟΣ
                              select e;

            //convert to ObservableCollection<T> where T is a type (class)
            var ocp = new ObservableCollection<ΠΡΟΚΗΡΥΞΗ>(prokirixis.ToList());
            var oci = new ObservableCollection<ΙΕΚ>(ieks.ToList());
            var ocei = new ObservableCollection<ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ>(eidikotites.ToList());
            var oce = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>(eidikotita.ToList());

            prokGrid.ItemsSource = ocp;
            iekGrid.ItemsSource = oci;
            eidikotitesGrid.ItemsSource = ocei;
            cboEidikotis.ItemsSource = oce;
        }

        public void RefreshData()
        {

        }

        private void prokGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            ShowSelected();
        }

        private void iekGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            ShowSelected();
        }
        
        private void ShowSelected()
        {
            ΠΡΟΚΗΡΥΞΗ selected_prok = prokGrid.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ selected_iek = iekGrid.SelectedItem as ΙΕΚ;

            // make sure that something is selected in the two "filter grids", if the user hasn't made a selection,
            // otherwise we get a null exception.

            if (selected_iek == null)
            {
                selected_iek = ((ObservableCollection<ΙΕΚ>)iekGrid.ItemsSource).First();
            }
            else if (selected_prok == null)
            {
                selected_prok = ((ObservableCollection<ΠΡΟΚΗΡΥΞΗ>)prokGrid.ItemsSource).First();
            }

            var eidikotites = from dbe in db.ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑs
                              where dbe.ΚΩΔ_ΙΕΚ == selected_iek.ΙΕΚ_ΚΩΔ && dbe.ΚΩΔ_ΠΡΟΚΗΡΥΞΗ== selected_prok.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                              orderby dbe.ΚΩΔ_ΠΡΟΚΗΡΥΞΗ, dbe.ΚΩΔ_ΙΕΚ
                              select dbe;

            ObservableCollection<ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ> oce = new ObservableCollection<ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ>(eidikotites.ToList());
            eidikotitesGrid.ItemsSource = null;
            eidikotitesGrid.ItemsSource = oce;
        }

        private bool IsSelected()
        {
            ΠΡΟΚΗΡΥΞΗ selected_prok = prokGrid.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ selected_iek = iekGrid.SelectedItem as ΙΕΚ;

            if (selected_prok == null || selected_iek == null)
            {
                UserFunctions.ShowAdminMessage("Πρέπει να επιλεγεί Προκήρυξη και ΙΕΚ");
                return false;
            }
            else return true;
        }

        // CRUD λειτουργίες επιτρέπονται για ανοικτή προκήρυξη.
        private bool CanEdit()
        {
            ΠΡΟΚΗΡΥΞΗ selected_prok = prokGrid.SelectedItem as ΠΡΟΚΗΡΥΞΗ;

            if (selected_prok == null) return false;
            else
            {
                int prokirixi_id = selected_prok.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                short status = sp.GetProkirixiStatus(prokirixi_id);
                if (status == 2)
                {
                    UserFunctions.ShowAdminMessage("Πρέπει να επιλεγεί ανοικτή Προκήρυξη.");
                    return false; 
                }
                else return true;
            }            
        }

        private bool ValidateInsertEidikotita(ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ prok_iek_eidikotita)
        {
            var recs = (from p in db.ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑs
                        where p.ΚΩΔ_ΠΡΟΚΗΡΥΞΗ == prok_iek_eidikotita.ΚΩΔ_ΠΡΟΚΗΡΥΞΗ &&
                              p.ΚΩΔ_ΙΕΚ == prok_iek_eidikotita.ΚΩΔ_ΙΕΚ &&
                              p.ΚΩΔ_ΕΙΔΙΚΟΤΗΤΑ == prok_iek_eidikotita.ΚΩΔ_ΕΙΔΙΚΟΤΗΤΑ
                        select p).Count();
            if (recs != 0)
            {
                UserFunctions.ShowAdminMessage("Δεν μπορεί να γίνει εισαγωγή διότι η καταχώρηση υπάρχει ήδη.");
                return false;
            }
            return true;
        }

        #endregion


        #region CRUD functions

        // Οι CRUD λειτουργίες ενεργοποιούνται μόνο για ανοικτή προκήρυξη.
        private void eidikotitesGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            if (!IsSelected())
            {
                e.Cancel = true;
                return;
            }
            if (!CanEdit())
            {
                e.Cancel = true;
                return;
            }
        }
        // Οι CRUD λειτουργίες ενεργοποιούνται μόνο για ανοικτή προκήρυξη.
        private void eidikotitesGrid_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!IsSelected())
            {
                e.Cancel = true;
                return;
            }
            if (!CanEdit())
            {
                e.Cancel = true;
                return;
            }
        }
        // Οι CRUD λειτουργίες ενεργοποιούνται μόνο για ανοικτή προκήρυξη.
        private void eidikotitesGrid_Deleting(object sender, GridViewDeletingEventArgs e)
        {
            if (!IsSelected())
            {
                e.Cancel = true;
                return;
            }
            if (!CanEdit())
            {
                e.Cancel = true;
                return;
            }
        }

        private void eidikotitesGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            
            ΠΡΟΚΗΡΥΞΗ selected_prok = prokGrid.SelectedItem as ΠΡΟΚΗΡΥΞΗ;
            ΙΕΚ selected_iek = iekGrid.SelectedItem as ΙΕΚ;
                      
            // this event is raised when row editing is done in both insert and edit modes
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                var row = e.Row as GridViewRow;
                ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ eidikotites = row.Item as ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ;

                // set the PK values by code (ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ is an all-key relation)
                // these two values are not available in the UI.
                eidikotites.ΚΩΔ_ΠΡΟΚΗΡΥΞΗ = selected_prok.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
                eidikotites.ΚΩΔ_ΙΕΚ = selected_iek.ΙΕΚ_ΚΩΔ;

                // check for PK constraint before insert (sql exception)
                if (!ValidateInsertEidikotita(eidikotites))
                {
                    LoadData(); // refresh the collection
                    return;     // do not insert
                }

                // these two methods do the database udpating
                db.ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑs.InsertOnSubmit(eidikotites);
            }
        }

        #endregion

        // Οι CRUD λειτουργίες ενεργοποιούνται μόνο για ανοικτή προκήρυξη.
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSelected()) return;
            if (!CanEdit()) return;

            if (eidikotitesGrid.SelectedItems.Count == 0) { return; }
            // verify deletion from user
            string checkMessage = "Να γίνει διαγραφή των επιλεγμένων εγγραφών; " + "\n";

            if (MessageBox.Show(checkMessage, "Διαγραφή", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { return; }

            // proceed with deletion process
            foreach (var row in eidikotitesGrid.SelectedItems)
            {
                ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ eidikotites = row as ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑ;
                db.ΠΡΟΚ_ΙΕΚ_ΕΙΔΙΚΟΤΗΤΑs.DeleteOnSubmit(eidikotites);
            }
        }
        // Οι CRUD λειτουργίες ενεργοποιούνται μόνο για ανοικτή προκήρυξη.
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSelected()) return;
            if (!CanEdit()) return;
            eidikotitesGrid.BeginInsert();
            
        }
        // Οι CRUD λειτουργίες ενεργοποιούνται μόνο για ανοικτή προκήρυξη.
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSelected()) return;
            if (!CanEdit()) return;
            eidikotitesGrid.BeginEdit();
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            ShowSelected();
            //LoadData();
        }
        // Οι CRUD λειτουργίες ενεργοποιούνται μόνο για ανοικτή προκήρυξη.
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cm.UserCanUpdate() == false)
            {
                LoadData();
                return;
            }
            /*
            string checkMessage = "Να γίνει αποθήκευση των μεταβολών; " + "\n";
            if (MessageBox.Show(checkMessage, "Αποθήκευση", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            { return; }
            */
            cm.CommitData(db);
            ShowSelected();
        }

    }
}
