using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.DataAccess;

namespace Thetis.AppPages.Admin
{
    /// <summary>
    /// Interaction logic for SearchTrainers.xaml
    /// </summary>
    public partial class SearchTrainers : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private CommitModel cm = new CommitModel();

        public SearchTrainers()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // this loads the data of the grid and form.

            var trainers = from dbp in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                           orderby dbp.ΕΠΩΝΥΜΟ, dbp.ΟΝΟΜΑ
                           select dbp;
            teacherGrid.ItemsSource = trainers.ToList();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            teacherGrid.BeginInsert();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            teacherGrid.BeginEdit();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (teacherGrid.SelectedItems.Count == 0) { return; }
            // verify deletion from user
            string checkMessage = "Να γίνει διαγραφή των επιλεγμένων εγγραφών; " + "\n";

            if (MessageBox.Show(checkMessage, "Διαγραφή", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { return; }

            // proceed with deletion process
            foreach (var row in teacherGrid.SelectedItems)
            {
                ΕΚΠΑΙΔΕΥΤΙΚΟΣ trainer = row as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;

                if (ValidateDeleteTeacher(trainer.ΑΦΜ))
                {
                    db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs.DeleteOnSubmit(trainer);
                    cm.CommitData(db);
                    //ReloadData();       // refresh display
                }
                else
                {
                    string msg = "Δεν μπορεί να διαγραφεί ο/η " + trainer.ΕΠΩΝΥΜΟ + " " + trainer.ΟΝΟΜΑ + "\n";
                    msg += "διότι υπάρχουν συσχετισμένες αιτήσεις.";
                    UserFunctions.ShowAdminMessage(msg);
                    return;
                }
                db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs.DeleteOnSubmit(trainer);
            }
        }

        private void teacherGrid_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            // this event is raised when row editing is done in both insert and edit modes
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                var row = e.Row as GridViewRow;
                ΕΚΠΑΙΔΕΥΤΙΚΟΣ trainers = row.Item as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;           // cast it to object ΑΙΤΗΣΗ

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
            if (cm.UserCanUpdate() == false)
            {
                LoadData();
                return;
            }
            cm.CommitData(db);
            LoadData();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

       /* -------------------
        * Κάνει έλεγχο αν υπάρχουν αιτήσεις συσχετισμένες
        * με το ΑΦΜ που πρόκειται να διαγραφεί.
        * Εάν υπάρχουν η διαγραφή ακυρώνεται.
        * -------------------
        */
        private bool ValidateDeleteTeacher(string safm)
        {

            // έλεγχος αν υπάρχουν αιτήσεις για το συγκεκριμένο ΑΦΜ
            var ac = (from c in db.ΑΙΤΗΣΗs
                      where c.ΑΦΜ == safm
                      select c).Count();

            if (safm == null || safm == "")
            {
                return false;
            }
            if (ac != 0) return false;
            else return true;
        }

    }
}
