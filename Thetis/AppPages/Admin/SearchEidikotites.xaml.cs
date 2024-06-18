using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Model;
using Thetis.DataAccess;

namespace Thetis.AppPages.Admin
{
    /// <summary>
    /// Interaction logic for SearchEidikotites.xaml
    /// </summary>
    public partial class SearchEidikotites : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ> oc = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>();
        private CommitModel cm = new CommitModel();
        //private GridViewRow Row;

        public SearchEidikotites()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {

            var eidikotites = from e in db.ΕΙΔΙΚΟΤΗΤΑs
                              orderby e.ΒΑΘΜΙΔΑ, e.ΚΛΑΔΟΣ
                              select e;
            var kladoi = from k in db.ΚΛΑΔΟΣs
                         select k;

            /*
             * oc is the datagrid source and ock the combo data source
            */
            var oc = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>(eidikotites.ToList());
            var ock = new ObservableCollection<ΚΛΑΔΟΣ>(kladoi.ToList());

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = oc;
            cbograde.ItemsSource = ock;
        }

        #region CRUD functions

        private void dataGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            dataGrid.CurrentCellInfo = new GridViewCellInfo(dataGrid.CurrentItem, dataGrid.Columns["ΚΛΑΔΟΣ"]);
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
                ΕΙΔΙΚΟΤΗΤΑ eidikotites = row as ΕΙΔΙΚΟΤΗΤΑ;
                db.ΕΙΔΙΚΟΤΗΤΑs.DeleteOnSubmit(eidikotites);
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
                ΕΙΔΙΚΟΤΗΤΑ eidikotites = row.Item as ΕΙΔΙΚΟΤΗΤΑ;           // cast it to object ΕΙΔΙΚΟΤΗΤΑ

                // these two methods do the database udpating
                db.ΕΙΔΙΚΟΤΗΤΑs.InsertOnSubmit(eidikotites);            // insert new row into collection
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

        private void dataGrid_GroupRowIsExpandedChanged(object sender, GroupRowEventArgs e)
        {
            /*
             * not here in another event of the grid as each time expander is clicked descriptors keep adding!
             * put it in the event Grouped, so if it's grouped refresh function or try loaded event of page
            CountFunction f = new CountFunction();
            f.Caption = "Αρ. Ειδικοτήτων: ";
            GroupDescriptor descriptor = new GroupDescriptor();
            //this.dataGrid.GroupDescriptors.Add(descriptor);

            descriptor.Member = "ΒΑΘΜΙΔΑ";
            //countryDescriptor.SortDirection = ListSortDirection.Ascending;
            descriptor.AggregateFunctions.Add(f);
            //this.dataGrid.GroupDescriptors.Add(descriptor);
            */
        }

        #region Validation events

        private void dataGrid_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.Name == "ΚΛΑΔΟΣ")
            {
                string iek_name = e.NewValue.ToString();
                if (String.IsNullOrWhiteSpace(iek_name))
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                }
            }
            if (e.Cell.Column.Name == "cbograde")
            {
                try
                {
                    int grade = Convert.ToInt32(e.NewValue.ToString());

                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
        }

        #endregion
    }
}
