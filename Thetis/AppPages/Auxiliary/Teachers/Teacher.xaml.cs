using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Model;
using Thetis.DataAccess;
using System.Threading;
using Thetis.Utilities;

namespace Thetis.AppPages.Auxiliary.Teachers
{
    /// <summary>
    /// Interaction logic for Teachers.xaml
    /// </summary>
    public partial class Teacher : Page
    {
        Loading loadingWin = null;
        bool isLoadingWinCreated = false;

        private ThetisDataContext db = new ThetisDataContext();
        private CommitModel cm = new CommitModel();

        public Teacher()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {

            //source for parent grid
            var trainers = from dbp in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                         orderby dbp.ΕΠΩΝΥΜΟ, dbp.ΟΝΟΜΑ
                         select dbp;
            //source for child grid
            var aitiseis = from dba in db.ΑΙΤΗΣΗs
                          orderby dba.ΠΡΟΚΗΡΥΞΗ
                          select dba;
            //sources for combos
            var ieks = from dbiek in db.ΙΕΚs
                       orderby dbiek.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                       select dbiek;
            var eidikotites = from dbe in db.ΕΙΔΙΚΟΤΗΤΑs
                              orderby dbe.ΚΛΑΔΟΣ
                              select dbe;
            var prokirixis = from p in db.ΠΡΟΚΗΡΥΞΗs
                             orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                             select p;
            var studies = from s in db.ΣΠΟΥΔΕΣs
                          orderby s.ΚΩΔ_ΣΠΟΥΔΕΣ
                          select s;
            var work = from w in db.ΕΡΓΑΣΙΑs
                       select w;
            var apokleismoi = from a in db.ΑΠΟΚΛΕΙΣΜΟΣs
                              select a;


            //convert to ObservableCollection<T> where T is a type (class)
            var ocp = new ObservableCollection<ΕΚΠΑΙΔΕΥΤΙΚΟΣ>(trainers.ToList());
            var oca = new ObservableCollection<ΑΙΤΗΣΗ>(aitiseis.ToList());
            var ociek = new ObservableCollection<ΙΕΚ>(ieks.ToList());
            var oce = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>(eidikotites.ToList());
            var ocprok = new ObservableCollection<ΠΡΟΚΗΡΥΞΗ>(prokirixis.ToList());

            parentGrid.ItemsSource = null; // ocp;
            //radDataPager.Source = ocp;
            childGrid.ItemsSource = null; // oca;
            cboiek.ItemsSource = ociek;
            cboeidikotitita.ItemsSource = oce;
            cboprok.ItemsSource = ocprok;
            cbostudies.ItemsSource = studies.ToList();
            cbowork.ItemsSource = work.ToList();
            cboapokleismos.ItemsSource = apokleismoi.ToList();
        }

        #region CRUD Events

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
                // disable it for now as it requires cascade delete the applications associated with the teacher
                //ΕΚΠΑΙΔΕΥΤΙΚΟΣ trainers = row as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;
                //db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs.DeleteOnSubmit(trainers);
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
                ΕΚΠΑΙΔΕΥΤΙΚΟΣ trainers = row.Item as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;

                // these two methods do the database udpating
                db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs.InsertOnSubmit(trainers);            // insert new row into collection
            }
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            // load the existing collection without new changes (no submit is done unless Save is pressed)
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

        #endregion

        private void parentGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            /*
             * the dataPager throws an exception here, so Lefteris
             * covers it up wth this try-catch.
             */
            try
            {
                ΕΚΠΑΙΔΕΥΤΙΚΟΣ sel_teacher = parentGrid.SelectedItem as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;

                if (sel_teacher == null) return;

                var aitiseis = from dba in db.ΑΙΤΗΣΗs
                               where dba.ΑΦΜ == sel_teacher.ΑΦΜ
                               orderby dba.ΠΡΟΚΗΡΥΞΗ
                               select dba;

                ObservableCollection<ΑΙΤΗΣΗ> oca = new ObservableCollection<ΑΙΤΗΣΗ>(aitiseis.ToList());
                childGrid.ItemsSource = oca;
            }
            catch { }

        }

        private void btnFilterOn_Click(object sender, RoutedEventArgs e)
        {
            if (!(txtSearch.Text == null || txtSearch.Text == ""))
            {
                var trainers = from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                               where t.ΕΠΩΝΥΜΟ.Contains(txtSearch.Text) || t.ΑΦΜ.Contains(txtSearch.Text)
                               select t;

                parentGrid.ItemsSource = trainers.ToList();
            }
        }

        private void btnFilterOff_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = null;
            var trainers = from dbp in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                           orderby dbp.ΕΠΩΝΥΜΟ, dbp.ΟΝΟΜΑ
                           select dbp;

            parentGrid.ItemsSource = null; // trainers.ToList();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnFilterOn_Click(sender, e);
            }
        }

        #region Loading window

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Close the indicator window and shut down the STA thread with its dispatcher
            if (loadingWin != null) loadingWin.CloseForm();
            //busyIndicator.IsBusy = false;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                loadingWin = new Loading();
                isLoadingWinCreated = true;
                loadingWin.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (!isLoadingWinCreated) ;
        }
    }
        #endregion Loading Window
}
