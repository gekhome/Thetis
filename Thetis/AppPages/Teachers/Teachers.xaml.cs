using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prototype.Model;
using Prototype.Utilities;
using MessageBoxUtils;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.ComponentModel;
using Telerik.Windows.Data;
using Prototype.Model;


namespace Prototype.AppPages.Teachers
{
    /// <summary>
    /// Interaction logic for Teachers.xaml
    /// This is a heavy-duty page with lots of business rules and validation. 
    /// Προδιαγραφές λειτουργίας:
    /// 1) Οι CRUD λειτουργίες για κλειστές προκηρύξεις είναι ακυρωμένες (σημαντικό!).
    /// 2) Η εισαγωγή νέου εκπαιδευτικού γίνεται μόνον όταν το ΑΦΜ είναι έγκυρο.
    /// 3) Η σελίδα εκτελεί (το aitisiGrid) έχει όλες τις λειτουργίες για μοριοδότηση,
    /// από συμβάντα στα grids των RowDetails για τις προϋπηρεσίες.
    /// </summary>
    public partial class Teachers : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private bool isNewRec = false;
        
        public Teachers()
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

            DataContext = new TeacherViewModel();
            //convert to ObservableCollection<T> where T is a type (class)
            var ocp = new ObservableCollection<ΕΚΠΑΙΔΕΥΤΙΚΟΣ>(trainers.ToList());
            var oca = new ObservableCollection<ΑΙΤΗΣΗ>(aitiseis.ToList());
            var ociek = new ObservableCollection<ΙΕΚ>(ieks.ToList());
            var oce = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>(eidikotites.ToList());
            var ocprok = new ObservableCollection<ΠΡΟΚΗΡΥΞΗ>(prokirixis.ToList());

            //teacherGrid.ItemsSource = ocp;
            //teacherForm.ItemsSource = ocp;
            
            aitisiGrid.ItemsSource = oca;
            cboiek.ItemsSource = ociek;
            cboeidikotitita.ItemsSource = oce;
            cboprok.ItemsSource = ocprok;
        }

        private void teacherGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            isNewRec = true;
        }

        private void teacherGrid_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            // this event is raised when row editing is done in both insert and edit modes
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                var row = e.Row as GridViewRow;
                ΕΚΠΑΙΔΕΥΤΙΚΟΣ teacher = row.Item as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;
                db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs.InsertOnSubmit(teacher);
                // make this the selected item, otherwise exception is thrown in Save.
                teacherGrid.SelectedItem = row.Item;
            }
        }

        /*--------------
         * This is required. Clicking on the row details button does NOT
         * select the row and this causes exception thrown in Save event and
         * validation function. Save event looks for the SelectedItem.
         *--------------
         */
        private void teacherGrid_RowDetailsVisibilityChanged(object sender, GridViewRowDetailsEventArgs e)
        {
            teacherGrid.SelectedItem = e.Row.Item;
        }

        // synchronizes the Teacher grid with Aitisi grid
        private void teacherGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                ΕΚΠΑΙΔΕΥΤΙΚΟΣ sel_teacher = teacherGrid.SelectedItem as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;

                var aitiseis = from dba in db.ΑΙΤΗΣΗs
                               where dba.ΑΦΜ == sel_teacher.ΑΦΜ
                               orderby dba.ΠΡΟΚΗΡΥΞΗ
                               select dba;

                ObservableCollection<ΑΙΤΗΣΗ> oca = new ObservableCollection<ΑΙΤΗΣΗ>(aitiseis.ToList());
                aitisiGrid.ItemsSource = oca;
                ShowTeacherInfo(sel_teacher);
            }
            catch { }
        }

        private void ShowTeacherInfo(ΕΚΠΑΙΔΕΥΤΙΚΟΣ selectedTeacher)
        {
            var teacher = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                           where t.ΑΦΜ == selectedTeacher.ΑΦΜ
                           select new { t.ΑΦΜ, t.ΕΠΩΝΥΜΟ, t.ΟΝΟΜΑ }).FirstOrDefault();

            txtTeacherInfo.Text = "ΑΦΜ: " + teacher.ΑΦΜ + " - Ονοματεπώνυμο: " + teacher.ΕΠΩΝΥΜΟ + " " + teacher.ΟΝΟΜΑ;
        }



        #region CRUD button events with business logic functions

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            teacherGrid.BeginInsert();
            isNewRec = true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            teacherGrid.BeginEdit();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // delete is allowed only if there are no applications associated with the teacher.
            var teacher = teacherGrid.SelectedItem as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;
            string tdel_name = teacher.ΕΠΩΝΥΜΟ + " " + teacher.ΟΝΟΜΑ;
            string stitle = "Διαγραφή Εκπαιδευτικού";
            if ((WPFMessageBox.Show("Είστε σίγουροι ότι θέλετε διαγραφή της εγγραφής " + tdel_name + " ;", stitle,
                 MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
            {
                // here we must call a function that deletes all children first.
                db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs.DeleteOnSubmit(teacher);
            }
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ΕΚΠΑΙΔΕΥΤΙΚΟΣ teacher = teacherGrid.SelectedItem as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;

            if (teacher != null)
            {
                WPFMessageBox.Show(teacher.ΕΠΩΝΥΜΟ);

                if (ValidateTeacher(teacher))
                {
                    isNewRec = false;
                    db.SubmitChanges();
                    LoadData();
                }
            }
            else
            {
                WPFMessageBox.Show("Πρέπει να προηγηθεί επιλογή από τις γραμμές του πίνακα", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region filter functions

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnFilterOn_Click(sender,e);
            }
        }

        private void btnFilterOn_Click(object sender, RoutedEventArgs e)
        {

            if (!(txtSearch.Text == null || txtSearch.Text == ""))
            {
                var trainers = from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                               where t.ΕΠΩΝΥΜΟ.Contains(txtSearch.Text) || t.ΑΦΜ.Contains(txtSearch.Text)
                               select t;

                ICollectionView teacherSearch = null;
                teacherSearch = new QueryableCollectionView(trainers.ToList());
                teacherGrid.ItemsSource = teacherSearch;
                teacherForm.ItemsSource = teacherSearch;
            }
        }

        private void btnFilterOff_Click(object sender, RoutedEventArgs e)
        {
            
            /* reset the ICollectionView to the original source.
             * A QueryableCollectionView object is required, otherwise
             * the grid and form cease to be synchronized.
             */
            txtSearch.Text = null;
            var trainers = from dbp in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                           orderby dbp.ΕΠΩΝΥΜΟ, dbp.ΟΝΟΜΑ
                           select dbp;

            ICollectionView teacherAll = null;
            teacherAll = new QueryableCollectionView(trainers.ToList());
            teacherGrid.ItemsSource = teacherAll;
            teacherForm.ItemsSource = teacherAll;
        }

        #endregion


        #region LOB functions

        private bool ValidateTeacher(ΕΚΠΑΙΔΕΥΤΙΚΟΣ teacher)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Header = "Σφάλμα καταχώρησης";
            parameters.OkButtonContent = "Κλείσιμο";

            //exception here!
            string safm = teacher.ΑΦΜ;
            string birthdate = teacher.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ.ToString();

            if (afmRule.CheckAFM(safm) != true)
            {
                parameters.Content = "Το ΑΦΜ δεν είναι έγκυρο.";
                RadWindow.Alert(parameters);
                return false;
            }          
            if ((teacher.ΕΠΩΝΥΜΟ == null || teacher.ΕΠΩΝΥΜΟ == "") || (teacher.ΟΝΟΜΑ == null || teacher.ΟΝΟΜΑ==""))
            {
                parameters.Content = "Πρέπει να συμπληρωθεί ονοματεπώνυμο.";
                RadWindow.Alert(parameters);
                return false;
            }
            if (teacher.ΦΥΛΟ == null)
            {
                parameters.Content = "Πρέπει να επιλεχθεί φύλο.";
                RadWindow.Alert(parameters);
                return false;
            }
            if (birthdate == null || birthdate == "")
            {
                parameters.Content = "Πρέπει να συμπληρωθεί η ημερομηνία γέννησης.";
                RadWindow.Alert(parameters);
                return false;
            }
            else if (!BirthDateRule.ValidBirthDate(birthdate, 20, 66))
            {
                parameters.Content = "Η ημερομηνία γέννησης είναι εκτός έγκυρων ορίων.";
                RadWindow.Alert(parameters);
                return false;
            }
            return true;

        }

        #endregion
    }
}
