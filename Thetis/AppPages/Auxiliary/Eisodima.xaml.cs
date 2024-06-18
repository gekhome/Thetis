using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.DataAccess;

namespace Thetis.AppPages.Auxiliary
{
    /// <summary>
    /// Interaction logic for Eisodima.xaml
    /// </summary>
    public partial class Eisodima : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ObservableCollection<ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ> oc = new ObservableCollection<ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ>();
        private CommitModel cm = new CommitModel();

        public Eisodima()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {

            var eisodimata = from eisodima in db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs
                             select eisodima;
            
            // these are the collection sources for the grid combos
            var nomismata = from nomisma in db.ΝΟΜΙΣΜΑΤΑs
                            select nomisma;
            var years = from etos in db.ΕΤΟΣs
                        orderby etos.ΕΤΟΣ1
                        select etos;

            //convert to ObservableCollection<T> where T is a type (class)
            var oc = new ObservableCollection<ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ>(eisodimata.ToList());

            cbooik_etos.ItemsSource = years.ToList();
            cbonomisma.ItemsSource = nomismata.ToList();

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = oc;
        }

        #region CRUD functions

        private void dataGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            dataGrid.CurrentCellInfo = new GridViewCellInfo(dataGrid.CurrentItem, dataGrid.Columns["cbooik_etos"]);
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
                ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ eisodimata = row as ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ;
                db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs.DeleteOnSubmit(eisodimata);
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
                ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ eisodimata = row.Item as ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ;           // cast it to object ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ

                // check for PK constraint before insert (sql exception)
                if (!ValidateInsertEisodima(eisodimata.ΟΙΚ_ΕΤΟΣ))
                {
                    LoadData(); // refresh the collection
                    return;     // do not insert
                }

                // these two methods do the database udpating
                db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs.InsertOnSubmit(eisodimata);            // insert new row into collection
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
            /*
            string checkMessage = "Να γίνει αποθήκευση των μεταβολών; " + "\n";
            if (MessageBox.Show(checkMessage, "Αποθήκευση", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            { return; }
            */
            cm.CommitData(db);
            LoadData();
        }

        #endregion

        
        #region Validation rules

        private void dataGrid_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.Name == "cbooik_etos")
            {
                try
                {
                    int oik_etos = Convert.ToInt32(e.NewValue.ToString());

                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
            if (e.Cell.Column.Name == "ΑΦΟΡΟΛΟΓΗΤΟ")
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
            // το νόμισμα καταχωρείται ως string (δηλ. το νόμισμα και όχι ο κωδικός, λόγω σχεδιασμού)
            if (e.Cell.Column.Name == "cbonomisma")
            {
                try
                {
                    string nomisma = e.NewValue.ToString();

                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
                
            }
            if (e.Cell.Column.Name == "moria")
            {
                try
                {
                    if (Convert.ToInt32(e.NewValue) <= 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Ο αριθμός πρέπει να είναι μεγαλύτερος του μηδενός.";
                    }
                }
                catch (System.FormatException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Ο αριθμός δεν έχει την σωστή μορφή.";
                }
                catch (System.OverflowException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Ο αριθμός είναι πολύ μεγάλος.";
                }
            }
        }

        private bool ValidateInsertEisodima(short oik_etos)
        {
            // έλεγχος αν το ΑΦΜ υπάρχει ήδη καταχωρημένο
            var recs = (from t in db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs
                        where t.ΟΙΚ_ΕΤΟΣ == oik_etos
                        select t).Count();

            if (recs != 0)
            {
                UserFunctions.ShowAdminMessage("Δεν μπορεί να γίνει εισαγωγή διότι το Οικ.έτος υπάρχει ήδη.");
                return false;
            }
            return true;
        }
        #endregion
    }
}
