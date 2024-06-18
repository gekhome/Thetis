using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Model;
using Thetis.DataAccess;


namespace Thetis.AppPages.Auxiliary
{
    /// <summary>
    /// Interaction logic for Perifereies.xaml
    /// </summary>
    public partial class Perifereies : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private CommitModel cm = new CommitModel();

        public Perifereies()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {

            //source for parent grid
            var perifereies = from dbp in db.ΠΕΡΙΦΕΡΕΙΑs
                              orderby dbp.ΠΕΡΙΦΕΡΕΙΑ_ΚΩΔ
                              select dbp;

            //convert to ObservableCollection<T> where T is a type (class)
            var ocp = new ObservableCollection<ΠΕΡΙΦΕΡΕΙΑ>(perifereies.ToList());

            parentGrid.ItemsSource = ocp;

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            parentGrid.BeginInsert();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            parentGrid.BeginEdit();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (parentGrid.SelectedItems.Count == 0) { return; }
            // verify deletion from user
            string checkMessage = "Να γίνει διαγραφή των επιλεγμένων εγγραφών; " + "\n";

            if (MessageBox.Show(checkMessage, "Διαγραφή", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { return; }

            // proceed with deletion process
            foreach (var row in parentGrid.SelectedItems)
            {
                ΠΕΡΙΦΕΡΕΙΑ per = row as ΠΕΡΙΦΕΡΕΙΑ;
                db.ΠΕΡΙΦΕΡΕΙΑs.DeleteOnSubmit(per);
            }
        }

        private void parentGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            // this event is raised when row editing is done in both insert and edit modes
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                var row = e.Row as GridViewRow;
                ΠΕΡΙΦΕΡΕΙΑ per = row.Item as ΠΕΡΙΦΕΡΕΙΑ;           // cast it to object ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ

                // these two methods do the database udpating
                db.ΠΕΡΙΦΕΡΕΙΑs.InsertOnSubmit(per);            // insert new row into collection
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


    }
}
