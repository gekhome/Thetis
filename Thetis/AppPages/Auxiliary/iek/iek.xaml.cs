using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Model;
using Thetis.DataAccess;

namespace Thetis.AppPages.Auxiliary.iek
{
    /// <summary>
    /// Interaction logic for iek.xaml
    /// </summary>
    public partial class iek : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ObservableCollection<ΙΕΚ> oc = new ObservableCollection<ΙΕΚ>();
        private CommitModel cm = new CommitModel();

        public iek()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            var iekdata = from qiek in db.ΙΕΚs
                          orderby qiek.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                          select qiek;

            // data sources for the combos
            var nomosdata = from nomos in db.ΝΟΜΟΣs
                            orderby nomos.ΝΟΜΟΣ1
                            select nomos;
            var perdata = from p in db.ΠΕΡΙΦΕΡΕΙΑΚΗs
                          orderby p.ΠΕΡΙΦΕΡΕΙΑΚΗ1
                          select p;


            //convert to ObservableCollection<T> where T is a type (class)
            var oc = new ObservableCollection<ΙΕΚ>(iekdata.ToList());
            cboperiferiaki.ItemsSource=perdata.ToList();
            cbonomos.ItemsSource = nomosdata.ToList();
            //this is the grid source
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = oc;
        }

        private void dataGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            dataGrid.CurrentCellInfo = new GridViewCellInfo(dataGrid.CurrentItem, dataGrid.Columns["ΙΕΚ_ΟΝΟΜΑΣΙΑ"]);
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
                ΙΕΚ iekdata = row as ΙΕΚ;
                db.ΙΕΚs.DeleteOnSubmit(iekdata);
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
                ΙΕΚ iekdata = row.Item as ΙΕΚ;           // cast it to object ΙΕΚ

                // these two methods do the database udpating
                db.ΙΕΚs.InsertOnSubmit(iekdata);            // insert new row into collection
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

        private void dataGrid_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.Name == "ΙΕΚ_ΟΝΟΜΑΣΙΑ")
            {
                string iek_name = e.NewValue.ToString();
                if (String.IsNullOrWhiteSpace(iek_name))
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί ονομασία.";
                }
            }
            if (e.Cell.Column.Name == "cboperiferiaki")
            {
                try
                {
                    int perifereia = Convert.ToInt32(e.NewValue.ToString());

                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
        } // CellValidating

    } // class iek
}
