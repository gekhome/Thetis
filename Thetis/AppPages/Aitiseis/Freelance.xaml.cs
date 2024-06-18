using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thetis.Utilities;
using Thetis.DataAccess;
using Thetis.Model;
using Thetis.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for Freelance.xaml
    /// </summary>
    public partial class Freelance : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private MoriaModel mm = new MoriaModel();
        private CommitModel cm = new CommitModel();
        private Primus p = new Primus();
        //private bool transfer_complete = false;
        
        public Freelance()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string aitisi_id;
            aitisi_id = SelectedAitisi.AitisiId;
            var epagelma = from e in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                           where e.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                           orderby e.ΟΙΚ_ΕΤΟΣ
                           select e;
            var etos = from e in db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs
                       select e;

            cboOikEtos.ItemsSource = etos.ToList();
            dataGrid.ItemsSource = epagelma.ToList();
            txtMoria.Text = mm.GetFreelanceTotalMoria(aitisi_id).ToString();
            txtInfo.Text = "Έναρξη καταχωρήσεων.";

        }

        #region CRUD Functions

        /*
         * -------------------
         * This is required so that the first cell is selected
         * when adding a new item. Otherwise, the last edited cell is
         * selected and in this case the column txtIncome which, since empty,
         * fires the validation exceptions.
         * -------------------
         */
        private void dataGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            dataGrid.CurrentCellInfo = new GridViewCellInfo(dataGrid.CurrentItem, dataGrid.Columns["cboOikEtos"]);
            dataGrid.Focus();
        }
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
                ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ eisodimata = row as ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ;
                db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs.DeleteOnSubmit(eisodimata);
            }
            cm.CommitData(db);
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            // load the existing collection without new changes (no submit is done unless Save is pressed)
            LoadData();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool errors_found;
            errors_found = SelectErroneousRows(dataGrid);
            if (errors_found == true)
            {
                string msg = "Η αποθήκευση ακυρώθηκε. ";
                msg += "Διορθώστε τα σφάλματα και δοκιμάστε ξανά.";
                UserFunctions.ShowAdminMessage(msg);
                txtInfo.Text += "\n" + msg;
                return;
            }
            cm.CommitData(db);
            txtInfo.Text = "Η αποθήκευση ολοκληρώθηκε.";
            LoadData();
        }

        //
        private void dataGrid_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            // this event is raised when row editing is done in both insert and edit modes
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                var row = e.Row as GridViewRow;
                ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ eisodimata = row.Item as ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ;     // cast it to object ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ
                
                // insert new row into collection
                if (mm.UpdateFreelance(eisodimata) == false)
                {
                    LoadData(); //slow way but effective, otherwise empty rows appear in the collection on every Add
                    return;
                }
                db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs.InsertOnSubmit(eisodimata);
            }
            if (e.EditOperationType == GridViewEditOperationType.Edit)
            {
                var row = e.Row as GridViewRow;
                ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ eisodimata = row.Item as ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ;     // cast it to object ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ

                if (mm.UpdateFreelance(eisodimata) == false)
                {
                    LoadData(); //slow way but effective, otherwise empty rows appear on every Add
                    return;
                }
            }
        }

        #endregion

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //needed to keep the combo from firing the EVENT before it is clicked
            if (!cboOikEtos.IsLoaded) return;

            var selectedIndex = ((RadComboBox)e.OriginalSource).SelectedIndex;
            //needed to keep the combo from firing the EVENT when you just click it
            if (selectedIndex == -1) return;

            var selectedValue = ((RadComboBox)e.OriginalSource).SelectedValue;
            //needed to keep the combo from firing the EVENT when you just click it
            if (selectedValue == null || Convert.ToString(selectedValue) == "") return;

            int oik_etos = Convert.ToInt32(selectedValue);
            int etos_use = Convert.ToInt32(oik_etos) - 1;

            //getting the selected row where the combobox belongs
            GridViewRow currentRow = ((UIElement)sender).ParentOfType<GridViewRow>();
            ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ eisodimata = currentRow.Item as ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ;  // cast it to object ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ

            eisodimata.ΝΟΜΙΣΜΑ = mm.GetNomisma(oik_etos);
            eisodimata.ΕΤΟΣ_ΧΡΗΣΗ = (short)etos_use;

        }

        private void dataGrid_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.Name == "txtIncome")
            {
                try
                {
                    if (Convert.ToInt32(e.NewValue) <= 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Το εισόδημα πρέπει να είναι μεγαλύτερο του μηδενός.";
                    }
                }
                catch (System.FormatException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Το εισόδημα δεν έχει την σωστή μορφή.";
                }
                catch (System.OverflowException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Το εισόδημα είναι πολύ μεγάλο.";
                }
            }

        } // CellValidating

        private void dataGrid_RowValidating(object sender, Telerik.Windows.Controls.GridViewRowValidatingEventArgs e)
        {
            ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ epagelma = e.Row.DataContext as ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ;
            short etos_use = (short)epagelma.ΕΤΟΣ_ΧΡΗΣΗ;

            if (p.ValidateYear(etos_use)) e.IsValid = true;
            else
            {
                e.IsValid = false;
                UserFunctions.ShowAdminMessage("Το έτος είναι εκτός χρονικών ορίων της προκήρυξης.");
            }
        }

        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (p.MustTransfer() == false) return;
            
            // Επιβεβαίωση με τον διάλογο MessageBox
            string message;
            MessageBoxResult dialog_result;
            message = "Να γίνει προσθήκη προηγούμενων προϋπηρεσιών στην τρέχουσα αίτηση;\n";
            message += "Η μεταφορά θα γίνει μόνο για αυτές του ίδιου ΙΕΚ και της ίδιας Ειδικότητας.";
            dialog_result = MessageBox.Show(message, "Επιβεβαίωση", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (dialog_result == MessageBoxResult.Cancel) { return; }
            else
            {
                if (p.ResetTransferStatus(db) == true)
                {
                    p.TransferFreelance(db);
                    cm.CommitData(db);
                    LoadData();
                }
            }
         
        } // btnTransfer_Click

        public bool SelectErroneousRows(RadGridView dataGrid)
        {
            string current_aitisiID = SelectedAitisi.AitisiId;
            bool error_status = false;
            // bring the collection of didaktikes for this aitisi
            var epagelmas = from e in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                                where e.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                select e;

            foreach (var epagelma in epagelmas)
            {
                if (p.ValidateFreelance(epagelma) == false)
                {
                    error_status = true;
                    dataGrid.SelectedItems.Add(epagelma);
                }
            }
            if (error_status == true)
            {
                string msg = "Βρέθηκαν εγγραφές, που εμφανίζονται επιλεγμένες, ";
                msg += "οι οποίες παραβιάζουν τους κανόνες επικύρωσης δεδομένων.\n";
                msg += "1. Το έτος χρήσης είναι εκτός του χρονικού ορίου (π.χ.15ετία).\n";
                msg += "2. Υπάρχουν (πιθανώς) διπλοεγγραφές, δηλ. ίδιες ημ/νίες και ίδιες ημέρες\n";

                //UserFunctions.ShowAdminMessage(msg);
                txtInfo.Text = msg;
                return error_status;
            }
            else return error_status;

        } // SelectErroneousRows

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Page_loaded(object sender, RoutedEventArgs e)
        {
            //dataGrid.AddHandler(RadComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(ComboBox_SelectionChanged));
            //this.AddHandler(RadComboBox.SelectionChangedEvent,
            //new Telerik.Windows.Controls.(ComboBox_SelectionChanged));


        }


    }
}
