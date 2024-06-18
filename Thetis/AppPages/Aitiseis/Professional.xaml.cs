using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thetis.Utilities;
using Thetis.Model;
using Thetis.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.DataAccess;
using Telerik.Windows.Controls;

namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for Professional.xaml
    /// </summary>
    public partial class Professional : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private MoriaModel mm = new MoriaModel();
        private CommitModel cm = new CommitModel();
        private Primus p = new Primus();
        //private bool transfer_complete = false;

        public Professional()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            //string aitisi_id;
            string aitisi_id = SelectedAitisi.AitisiId;
            var epagelmatiki = from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                               where e.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                               select e;

            dataGrid.ItemsSource = epagelmatiki.ToList();
            txtMoria.Text = mm.GetProfessionalTotalMoria(aitisi_id).ToString();
            txtInfo.Text = "Έναρξη καταχωρήσεων.";

        }

        /*
         * -------------------
         * This is required so that the first cell is selected
         * when adding a new item.
         * -------------------
         */
        private void dataGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            dataGrid.CurrentCellInfo = new GridViewCellInfo(dataGrid.CurrentItem, dataGrid.Columns["txtENARXI"]);
            dataGrid.Focus();

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.BeginInsert();
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
                ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ epagelmatiki = row as ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ;
                db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs.DeleteOnSubmit(epagelmatiki);
            }
            cm.CommitData(db);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.BeginEdit();
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

        /*
         * Ο αυτόματος υπολογισμός ημερών (μορίων) γίνεται μόνο στην προσθήκη 
         * νέας εγγραφής. Σε edit-mode ισχύει ότι έχει καταχωρηθεί, ώστε να 
         * δίνεται η δυνατότητα στον χρήστη να εισάγει δική του τιμή.
         */

        private void dataGrid_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            var row = e.Row as GridViewRow;

            // cast it to object ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ
            ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ epagelmatiki = row.Item as ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ;

            if (epagelmatiki.ΕΝΑΡΞΗ == null || epagelmatiki.ΛΗΞΗ == null) return;

            // this event is raised when row editing is done in both insert and edit modes
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }

            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                // suggest a value for days from the automatic calculation
                if (mm.UpdateProfessional(epagelmatiki) == false)
                {
                    LoadData(); //slow way but effective, otherwise empty rows appear in the collection on every Add
                    return;
                }
                db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs.InsertOnSubmit(epagelmatiki);//insert new row into collection
            }
            if (e.EditOperationType == GridViewEditOperationType.Edit)
            {
                // suggest a value for days from the automatic calculation
                if (mm.UpdateProfessional(epagelmatiki) == false)
                {
                    LoadData(); //slow way but effective, otherwise empty rows appear in the collection on every Add
                    return;
                }
            }
        }

        private void dataGrid_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            /*
             * Initializing to dummyDate in order to stop warnings before the user inputs a value
             */
            DateTime dummyDate = new DateTime(1, 1, 1);
            DateTime enarxi = dummyDate;
            DateTime lixi = dummyDate;

            if (e.Cell.Column.Name == "txtENARXI")
            {
                try
                {
                    enarxi = Convert.ToDateTime(e.NewValue.ToString());
                }
                catch (System.FormatException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Η ημερομηνία δεν έχει την σωστή μορφή.";
                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
            if (e.Cell.Column.Name == "txtLIXI")
            {
                try
                {
                    lixi = Convert.ToDateTime(e.NewValue.ToString());
                }
                catch (System.FormatException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Η ημερομηνία δεν έχει την σωστή μορφή.";
                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
        }

        private void dataGrid_RowValidating(object sender, Telerik.Windows.Controls.GridViewRowValidatingEventArgs e)
        {
            ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ epagelmatiki = e.Row.DataContext as ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ;
            int state;

            if (epagelmatiki.ΕΝΑΡΞΗ == epagelmatiki.ΛΗΞΗ)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "txtENARXI";
                validationResult.ErrorMessage = "Η έναρξη δεν μπορεί να έχει την ίδια τιμή με την λήξη.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }
            if (epagelmatiki.ΕΝΑΡΞΗ > epagelmatiki.ΛΗΞΗ)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "txtENARXI";
                validationResult.ErrorMessage = "Η έναρξη δεν μπορεί να είναι πιο μετά απο την λήξη.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }
            if (epagelmatiki.ΕΝΑΡΞΗ == null || epagelmatiki.ΛΗΞΗ == null)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "txtENARXI";
                validationResult.ErrorMessage = "Πρέπει να συμπληρωθούν και οι δύο ημερομηνίες.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
                return;
            }
            DateTime startDate = (DateTime)epagelmatiki.ΕΝΑΡΞΗ;
            DateTime finalDate = (DateTime)epagelmatiki.ΛΗΞΗ;
            state = p.ValidateDates(startDate, finalDate);
            switch (state)
            {
                case 0:
                    e.IsValid = true;
                    break;
                case 1:
                    UserFunctions.ShowAdminMessage("Η προϋπηρεσία είναι εκτός χρονικών ορίων της προκήρυξης.");
                    e.IsValid = false;                    
                    break;
                case 2:
                    int prokirixi_id = p.ProkirixiID();
                    string msg = "Η προϋπηρεσία είναι μερικώς εκτός χρονικών ορίων της προκήρυξης.\n";
                    msg = String.Format("{0}Η αρχική ημερομηνία πρέπει να αλλαχθεί σε: {1}", msg, p.LimitDate(prokirixi_id).ToString("dd/MM/yyyy"));
                    UserFunctions.ShowAdminMessage(msg);
                    e.IsValid = false;
                    break;
                default:
                    e.IsValid = true;
                    break;

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
                    p.TransferEpagelmatikes(db);
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
            var epagelmatikes = from d in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                             where d.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                             select d;

            foreach (var epagelmatiki in epagelmatikes)
            {
                if (p.ValidateEpagelmatiki(epagelmatiki) == false)
                {
                    error_status = true;
                    dataGrid.SelectedItems.Add(epagelmatiki);
                }
            }
            if (error_status == true)
            {
                string msg = "Βρέθηκαν εγγραφές, που εμφανίζονται επιλεγμένες, ";
                msg += "οι οποίες παραβιάζουν τους κανόνες επικύρωσης δεδομένων.\n";
                msg += "1. Οι ημ/νίες έναρξης ή/και λήξης είναι εκτός του χρονικού ορίου (π.χ.15ετία).\n";
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


    }
}
