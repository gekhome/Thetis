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
    /// Interaction logic for SearchApokleismoi.xaml
    /// </summary>
    public partial class SearchApokleismoi : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ObservableCollection<ΑΠΟΚΛΕΙΣΜΟΣ> oc = new ObservableCollection<ΑΠΟΚΛΕΙΣΜΟΣ>();
        private CommitModel cm = new CommitModel();

        public SearchApokleismoi()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {

            var apokleismoi = from s in db.ΑΠΟΚΛΕΙΣΜΟΣs
                              orderby s.ΑΙΤΙΑ
                              select s;

            /*
            try convert to ObservableCollection<T> where T is a type (class)
            this works but igDataGrid does not show "add" button, so switched
            to telerik GridView control which is easier to handle and has
            similar properties as the WPFToolkit DataGrid.
            */
            var oc = new ObservableCollection<ΑΠΟΚΛΕΙΣΜΟΣ>(apokleismoi.ToList());

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = oc;       //was apokleismoi.ToList()
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
                ΑΠΟΚΛΕΙΣΜΟΣ apokleismoi = row as ΑΠΟΚΛΕΙΣΜΟΣ;
                db.ΑΠΟΚΛΕΙΣΜΟΣs.DeleteOnSubmit(apokleismoi);
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
                ΑΠΟΚΛΕΙΣΜΟΣ apokleismoi = row.Item as ΑΠΟΚΛΕΙΣΜΟΣ;           // cast it to object ΑΠΟΚΛΕΙΣΜΟΣ

                // these two methods do the database udpating
                db.ΑΠΟΚΛΕΙΣΜΟΣs.InsertOnSubmit(apokleismoi);            // insert new row into collection
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
            if (e.Cell.Column.Name == "aitia")
            {
                string str_aitia = e.NewValue.ToString();
                if (String.IsNullOrWhiteSpace(str_aitia))
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                }
            }
        }

    }
}
