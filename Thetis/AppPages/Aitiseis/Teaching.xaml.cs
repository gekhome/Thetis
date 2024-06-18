using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Thetis.Utilities;
using Thetis.Model;
using Thetis.DataAccess;
using Thetis.Controls;


namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for Teaching.xaml
    /// </summary>
    public partial class Teaching : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private MoriaModel mm = new MoriaModel();
        private CommitModel cm = new CommitModel();
        private Primus p = new Primus();
        //private bool transfer_complete = false;
        public ObservableCollection<ΕΚΠ_ΔΙΔΑΚΤΙΚΗ> ocDidaktiki = new ObservableCollection<ΕΚΠ_ΔΙΔΑΚΤΙΚΗ>();

        public Teaching()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string aitisi_id;
            aitisi_id = SelectedAitisi.AitisiId;
            var didaktiki = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                            where d.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                            orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending
                            select d;
            // data source for school year combo
            var school_years = from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                               select s;
            var ergasia = from e in db.ΕΚΠ_ΕΡΓΑΣΙΑs
                          select e;
            dataGrid.ItemsSource = didaktiki.ToList();
            cboSchoolYear.ItemsSource = school_years.ToList();
            cboErgasia.ItemsSource = ergasia.ToList();
            txtMoria.Text = mm.GetTeachingTotalMoria(aitisi_id).ToString();
            txtInfo.Text = "Έναρξη καταχωρήσεων.";
            // this is used for the transfer.
            //ocDidaktiki = new ObservableCollection<ΕΚΠ_ΔΙΔΑΚΤΙΚΗ>(didaktiki.ToList());
        }

        #region CRUD Events
        /*
         * -------------------
         * This is required so that the first cell is selected
         * when adding a new item. Otherwise, the last edited cell is selected.
         * -------------------
         */
        private void dataGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            dataGrid.CurrentCellInfo = new GridViewCellInfo(dataGrid.CurrentItem, dataGrid.Columns["cboSchoolYear"]);
            dataGrid.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.BeginInsert();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0) { return; }
            // verify deletion from user
            string checkMessage = "Να γίνει διαγραφή των επιλεγμένων εγγραφών;" + "\n";

            if (MessageBox.Show(checkMessage, "Διαγραφή", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { return; }

            // proceed with deletion process
            foreach (var row in dataGrid.SelectedItems)
            {
                ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktiki = row as ΕΚΠ_ΔΙΔΑΚΤΙΚΗ;
                db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs.DeleteOnSubmit(didaktiki);
            }
            cm.CommitData(db);
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
            bool errors_found;
            errors_found = SelectErroneousRows(dataGrid);
            if (errors_found == true)
            {
                string msg = "Η αποθήκευση ακυρώθηκε. ";
                msg += "Διορθώστε τα σφάλματα και δοκιμάστε ξανά.";
                UserFunctions.ShowAdminMessage(msg);
                txtInfo.Text += "\n" + msg;
                return;
            }
            cm.CommitData(db);
            txtInfo.Text = "Η αποθήκευση ολοκληρώθηκε.";
            LoadData();
        }

        private void dataGrid_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            // this event is raised when row editing is done in both insert and edit modes
            var row = e.Row as GridViewRow;
            ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktiki = row.Item as ΕΚΠ_ΔΙΔΑΚΤΙΚΗ;

            if (didaktiki.ΕΝΑΡΞΗ == null || didaktiki.ΛΗΞΗ == null) return;

            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                if (mm.CalculateTeachingMoria(didaktiki) == false)
                {
                    LoadData();
                    return;
                }
                db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs.InsertOnSubmit(didaktiki);            //insert new row into collection
            }
            if (e.EditOperationType == GridViewEditOperationType.Edit)
            {
                if (mm.CalculateTeachingMoria(didaktiki) == false)
                {
                    LoadData();
                    return;
                }
            }
        } // RowEditEnded

        #endregion

        #region Validations

        private void dataGrid_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            /*
             * Initializing to dummyDate in order to stop warnings before the user inputs a value
             */
            
            DateTime dummyDate = new DateTime(1, 1, 1);
            DateTime enarxi = dummyDate;
            DateTime lixi = dummyDate;

            if (e.Cell.Column.Name == "dcStartDate")
            {
                try
                {
                    enarxi = Convert.ToDateTime(e.NewValue.ToString());
                }
                catch (System.FormatException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Η ημερομηνία δεν έχει την σωστή μορφή.";
                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
            if (e.Cell.Column.Name == "dcFinalDate")
            {
                try
                {
                    lixi = Convert.ToDateTime(e.NewValue.ToString());
                }
                catch (System.FormatException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Η ημερομηνία δεν έχει την σωστή μορφή.";
                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
            if (e.Cell.Column.Name == "cboSchoolYear")
            {
                try
                {
                    int school_year = Convert.ToInt32(e.NewValue.ToString());
                    
                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }
            if (e.Cell.Column.Name == "cboErgasia")
            {
                try
                {
                    int ergasia = Convert.ToInt32(e.NewValue.ToString());

                }
                catch (System.NullReferenceException)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                    //return;
                }
            }


        } // CellValidating

        private void dataGrid_RowValidating(object sender, Telerik.Windows.Controls.GridViewRowValidatingEventArgs e)
        {
            ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktiki = e.Row.DataContext as ΕΚΠ_ΔΙΔΑΚΤΙΚΗ;
            int school_year = Convert.ToInt32(didaktiki.ΣΧΟΛΙΚΟ_ΕΤΟΣ);  // a null is converted to 0
            string date1 = didaktiki.ΕΝΑΡΞΗ.ToString();
            string date2 = didaktiki.ΛΗΞΗ.ToString();
            int state;

            if (!(school_year > 0))
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "cboSchoolYear";
                validationResult.ErrorMessage = "Δεν έχει καταχωρηθεί σχολικό έτος.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }

            if (didaktiki.ΕΝΑΡΞΗ == didaktiki.ΛΗΞΗ)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "dcStartDate";
                validationResult.ErrorMessage = "Η έναρξη δεν μπορεί να έχει την ίδια τιμή με την λήξη.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }
            if (didaktiki.ΕΝΑΡΞΗ > didaktiki.ΛΗΞΗ)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "dcStartDate";
                validationResult.ErrorMessage = "Η έναρξη δεν μπορεί να είναι μετά την λήξη.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }
            if (didaktiki.ΕΝΑΡΞΗ == null || didaktiki.ΛΗΞΗ == null)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "dcStartDate";
                validationResult.ErrorMessage = "Πρέπει να συμπληρωθούν και οι δύο ημερομηνίες.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
                return;
            }

            if (!Dates.InSchoolYear(date1, date2, school_year))
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "cboSchoolYear";
                validationResult.ErrorMessage = "Το σχολικό έτος δεν συμβαδίζει με τις ημερομηνίες.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }

            DateTime startDate = (DateTime)didaktiki.ΕΝΑΡΞΗ;
            DateTime finalDate = (DateTime)didaktiki.ΛΗΞΗ;
            state = p.ValidateDates(startDate, finalDate);
            switch (state)
            {
                case 0:
                    e.IsValid = true;
                    break;
                case 1:
                    UserFunctions.ShowAdminMessage("Η προϋπηρεσία είναι εκτός χρονικών ορίων της προκήρυξης.");
                    e.IsValid = false;
                    break;
                case 2:
                    int prokirixi_id = p.ProkirixiID();
                    string msg = "Η προϋπηρεσία είναι μερικώς εκτός χρονικών ορίων της προκήρυξης.\n";
                    msg = String.Format("{0}Η αρχική ημερομηνία πρέπει να αλλαχθεί σε: {1}", msg, p.LimitDate(prokirixi_id).ToString("dd/MM/yyyy"));
                    UserFunctions.ShowAdminMessage(msg);
                    e.IsValid = false;
                    break;
                default:
                    e.IsValid = true;
                    break;

            }

        } // RowValidating

        #endregion


        #region Info Window
        /* 
         * -----------------
         * Το συμβάν αυτό ανοίγει ένα RadWindow το οποίο περιέχει
         * πληροφορίες που αφορούν την ανάλυση των υπολογισμών
         * για τα μόρια διδακτικής προϋπηρεσίας. Εδώ γίνονται set
         * τα properties της κλάσης MoriaAnalysis.
         * -----------------
         */ 
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {           
            // these two lines (either of them) get the row on which the button is, when clicked.
            // the button inherits the datacontext of the row data object it belongs to.
            // this way, there is, also, always a selected value of didaktiki row

            //ΕΚΠ_ΔΙΔΑΚΤΙΚΗ sel_didaktiki = ((FrameworkElement)sender).DataContext as ΕΚΠ_ΔΙΔΑΚΤΙΚΗ;
            ΕΚΠ_ΔΙΔΑΚΤΙΚΗ sel_didaktiki = ((RadButton)sender).DataContext as ΕΚΠ_ΔΙΔΑΚΤΙΚΗ;
            
            //set the global class MoriaAnalysis
            SetAnalysisFields(sel_didaktiki);
            //show the analysis window
            ShowAnalysis();

        }

        /*
         * -------------------
         * Με τη συνάρτηση αυτή γίνονται set οι ιδιότητες της
         * κλάσης MoriaAnalysis, οι οποίες μπορούν να αναγνωστούν
         * από άλλες λειτουργικές μονάδες. Οι τιμές των ιδιοτήτων 
         * αφορούν κάθε φορά την επιλεγμένη διδ.προϋπηρεσία.
         * Η επιλογή γίνεται είτε από το UI ή από κώδικα.
         * Σημείωση:
         * Κανονικά πρέπει να είναι στο ViewModel.
         * -------------------
         */
        private void SetAnalysisFields(ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktiki)
        {
            //set the global class MoriaAnalysis
            MoriaAnalysis.DidaktikiId = didaktiki.ΚΩΔ_ΔΙΔΑΚΤΙΚΗ;
            try
            {
                MoriaAnalysis.StartDate = (DateTime)didaktiki.ΕΝΑΡΞΗ;
                MoriaAnalysis.EndDate = (DateTime)didaktiki.ΛΗΞΗ;
            }
            catch
            {
                UserFunctions.ShowAdminMessage("Δεν έχει εισαχθεί αρχική ή/και τελική ημερομηνία");
            }
            MoriaAnalysis.WeeklyHours = Convert.ToInt32(didaktiki.ΩΡΕΣ_ΕΒΔ);        //avoid direct cast due to possible null values
            MoriaAnalysis.TotalHours = Convert.ToInt32(didaktiki.ΩΡΕΣ_ΣΥΝ);         //avoid direct cast due to possible null values
            MoriaAnalysis.CalculatedTotal = Convert.ToInt32(didaktiki.ΣΥΝ_ΩΡΕΣ);    //avoid direct cast due to possible null values
        }

        /*
         * -----------------
         * Η μέθοδος αυτή ανοίγει το παράθυρο με την ανάλυση
         * των υπολογισμών για τις κανονικές διδακτικές μέρες.
         * Το παράθυρο είναι τύπου RadWindow, τα περιεχόμενα του
         * οποίου καθορίζονται από την μέθοδο SetAnalysisFields
         * και την κλάση MoriaAnalysis.
         * Σημείωση:
         * Κανονικά πρέπει να είναι στο ViewModel.
         * -----------------
         */
        private void ShowAnalysis()
        {
            TeachingInfo info_window = new TeachingInfo();
            info_window.Width = 440;
            info_window.Height = 300;

            info_window.Icon = new Image()
            {
                Source = new BitmapImage(new Uri("/Shell/Images/Info2.png", UriKind.Relative))
            };
            info_window.ResizeMode = ResizeMode.NoResize;   //hides min, max buttons
            info_window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            info_window.IsRestricted = true;                //prevent dragging off the screen    
            info_window.ShowDialog();                       //open it as modal window
        }

        #endregion

        #region Primus Functions

        /// <summary>
        /// Συμβάν με το οποίο προσθέτονται προηγούμενες προϋπηρεσίες διδακτικής
        /// (αν υπάρχουν) σε αυτές της τρέχουσας Αίτησης.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (p.MustTransfer() == false) return;
            
            // Επιβεβαίωση με τον διάλογο MessageBox
            string message;
            MessageBoxResult dialog_result;
            message = "Να γίνει προσθήκη προηγούμενων προϋπηρεσιών στην τρέχουσα αίτηση;\n";
            message += "Η μεταφορά θα γίνει μόνο για αυτές του ίδιου ΙΕΚ και της ίδιας Ειδικότητας.";
            dialog_result = MessageBox.Show(message, "Επιβεβαίωση", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (dialog_result == MessageBoxResult.Cancel) { return; }
            else
            {
                if (p.ResetTransferStatus(db) == true)
                {
                    p.TransferDidaktikes(db);
                    cm.CommitData(db);
                    LoadData();
                }
            }

        } // btnTransfer_Click

        /// <summary>
        /// Επιλέγει (μαρκάρει) τις γραμμές που δεν ικανοποιούν τα κριτήρια επικύρωσης.
        /// </summary>
        /// <param name="dataGrid"></param>
        public bool SelectErroneousRows(RadGridView dataGrid)
        {
            string current_aitisiID = SelectedAitisi.AitisiId;
            bool error_status = false;
            // bring the collection of didaktikes for this aitisi
            var didaktikes = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                             where d.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                             select d;

            foreach (var didaktiki in didaktikes)
            {
                if (p.ValidateDidaktiki(didaktiki) != true)     // was false
                {
                    error_status = true;
                    dataGrid.SelectedItems.Add(didaktiki);
                }
            }
            if (error_status == true)
            {
                string msg = "Βρέθηκαν εγγραφές, που εμφανίζονται επιλεγμένες, ";
                msg += "οι οποίες παραβιάζουν τους κανόνες επικύρωσης δεδομένων.\n";
                msg += "1. Οι ημ/νιες έναρξης, λήξης δεν συμβαδίζουν με το σχολικό έτος.\n";
                msg += "2. Οι ημ/νίες έναρξης ή/και λήξης είναι εκτός του χρονικού ορίου (π.χ.15ετία).\n";
                msg += "3. Υπάρχουν (πιθανώς) διπλοεγγραφές, δηλ. ίδιες ημ/νίες και ίδιες ώρες\n";

                //UserFunctions.ShowAdminMessage(msg);
                txtInfo.Text = msg;
                return error_status;
            }
            else
            {
                return error_status;
            }
        } // SelectErroneousRows

        #endregion

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}