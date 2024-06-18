using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thetis.DataAccess;
using Thetis.Model;
using Telerik.Windows.Controls.GridView;

namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for OtherIek.xaml
    /// </summary>
    public partial class OtherIek : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private CommitModel cm = new CommitModel();

        public OtherIek()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            string aitisi_id;
            aitisi_id = SelectedAitisi.AitisiId;
            var other_iek = from o in db.ΕΚΠ_ΙΕΚs
                            where o.ΚΩΔ_ΑΙΤΗΣΗ==aitisi_id
                            select o;
            var iek_query = from iek in db.ΙΕΚs
                           select iek;

            cboIek.ItemsSource = iek_query.ToList();
            dataGrid.ItemsSource = other_iek.ToList();
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
                ΕΚΠ_ΙΕΚ iek = row as ΕΚΠ_ΙΕΚ;
                db.ΕΚΠ_ΙΕΚs.DeleteOnSubmit(iek);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.BeginEdit();
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            /*
            string checkMessage = "Να γίνει αποθήκευση των μεταβολών; " + "\n";
            if (MessageBox.Show(checkMessage, "Αποθήκευση", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            { return; }
            */
            cm.CommitData(db);
            LoadData();
        }

        private void dataGrid_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                var row = e.Row as GridViewRow;
                ΕΚΠ_ΙΕΚ iek = row.Item as ΕΚΠ_ΙΕΚ;
                // a null exception if not set, as ΚΩΔ_ΑΙΤΗΣΗ is PK
                string aitisiCode = SelectedAitisi.AitisiId;
                iek.ΚΩΔ_ΑΙΤΗΣΗ = aitisiCode;
                               
                db.ΕΚΠ_ΙΕΚs.InsertOnSubmit(iek);
            }
        }
    }
}
