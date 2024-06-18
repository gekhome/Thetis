using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using MessageBoxUtils;
using Prototype.Model;
using Prototype.DataSources.thetisDBDataSetTableAdapters;


namespace Prototype.AppPages.Auxiliary
{
    /// <summary>
    /// Interaction logic for Kladoi.xaml
    /// </summary>
    public partial class Kladoi : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        //private ObservableCollection<ΚΛΑΔΟΣ> ocp = new ObservableCollection<ΚΛΑΔΟΣ>();
        //private ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ> occ = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>();


        public Kladoi()
        {
            InitializeComponent();
            LoadData();

        }
        public void LoadData()
        {

            //source for parent grid
            var kladoi = from dbp in db.ΚΛΑΔΟΣs
                         orderby dbp.ΚΩΔ_ΚΛΑΔΟΣ
                         select dbp;
            
            //source for child grid
            var eidikotites = from dbc in db.ΕΙΔΙΚΟΤΗΤΑs
                              orderby dbc.ΚΛΑΔΟΣ_ΚΩΔ
                              select dbc;

            //convert to ObservableCollection<T> where T is a type (class)
            var ocp = new ObservableCollection<ΚΛΑΔΟΣ>(kladoi.ToList());
            var occ = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>(eidikotites.ToList());

            parentGrid.ItemsSource = ocp;
            //childGrid.DataSource = occ; // we do not this and it doesn't work
            
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
            string checkMessage = "Η παρακάτω εγγραφή(ές) θα διαγραφεί(ούν): " + "\n";

            foreach (var row in parentGrid.SelectedItems)
            {
                //ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ eisodimata = row as ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ;
                //checkMessage += eisodimata.ΟΙΚ_ΕΤΟΣ + ",";
            }
            checkMessage = Regex.Replace(checkMessage, ",$", "");
            if (WPFMessageBox.Show(checkMessage, "Διαγραφή", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { return; }

            // proceed with deletion process
            foreach (var row in parentGrid.SelectedItems)
            {
                //ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ eisodimata = row as ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ;
                //db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs.DeleteOnSubmit(eisodimata);
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
                //ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ eisodimata = row.Item as ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ;           // cast it to object ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑ

                // these two methods do the database udpating
                //db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs.InsertOnSubmit(eisodimata);            // insert new row into collection
            }
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            // load the existing collection without new changes (no submit is done unless Save is pressed)
            LoadData();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string checkMessage = "Να γίνει αποθήκευση των μεταβολών; " + "\n";
            if (WPFMessageBox.Show(checkMessage, "Αποθήκευση", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            { return; }

            db.SubmitChanges();
            LoadData();
        }


    }
}
