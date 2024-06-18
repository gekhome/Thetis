using System;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.DataAccess;
using Thetis.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Data.DataForm;
using System.ComponentModel;

namespace Thetis.AppPages.Aitiseis
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
        private SelectedProkirixi sp = new SelectedProkirixi();
        AitisiModel am = new AitisiModel();
        MoriaModel mm = new MoriaModel();
        CommitModel cm = new CommitModel();
        
        private bool isNewRec = false;
        Loading loadingWin = null;
        bool isLoadingWinCreated = false;

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
                             where p.ΚΑΤΑΣΤΑΣΗ == 1
                             orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                             select p;

            //DataContext = new AitisiViewModel();
            teacherGrid.ItemsSource = null; // trainers.ToList();
            aitisiGrid.ItemsSource = null; // am.LoadAitisiData();
            aitisiForm.ItemsSource = null; // am.LoadAitisiData();

            cboiek.ItemsSource = ieks.ToList();
            cboeidikotitita.ItemsSource = eidikotites.ToList();
            cboprok.ItemsSource = prokirixis.ToList();

            //DisableFormEditDelete();
        }

        // NOT used
        private void DisableFormEditDelete()
        {
            /*
             * Ακύρωση δυνατότητας Edit/Delete στη φόρμα διότι εμφανίζονται
             * και Αιτήσεις κλειστής Προκήρυξης. Υπάρχει μόνο η δυνατότητα 
             * προσθήκης εγγραφής. Οι πλήρεις δυνατότητες CRUD ενεργοποιούνται
             * μόνο όταν επιλεγεί Αίτηση ανοικτής προκήρυξης.
             */
            aitisiForm.CommandButtonsVisibility = DataFormCommandButtonsVisibility.All
                                                  & ~DataFormCommandButtonsVisibility.Edit
                                                  & ~DataFormCommandButtonsVisibility.Delete;
        }

        #region Loading window thread

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

        #endregion

        #region Teacher DataGrid events

        private void teacherGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (teacherGrid.ItemsSource == null) return;
            
            teacherGrid.SelectedItem = teacherGrid.Items[0];
        }

        private void teacherGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            
                ΕΚΠΑΙΔΕΥΤΙΚΟΣ sel_teacher = teacherGrid.SelectedItem as ΕΚΠΑΙΔΕΥΤΙΚΟΣ;

                if (sel_teacher == null)
                {
                    UserFunctions.ShowAdminMessage("Δεν έχει επιλεγεί εκπαιδευτικός");
                    return;
                }
      
                string afm = sel_teacher.ΑΦΜ;               
                // set the property to the AFM of the current teacher
                SelectedTeacher.TeacherAFM = afm;

               //ObservableCollection<ΑΙΤΗΣΗ> oca = new ObservableCollection<ΑΙΤΗΣΗ>(am.LoadAitisiData(afm));
                aitisiGrid.ItemsSource = am.LoadAitisiData(afm);
                aitisiForm.ItemsSource = am.LoadAitisiData(afm);
                ShowTeacherInfo(sel_teacher);

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

        #endregion

        #region Teacher DataGrid filter functions 

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnFilterOn_Click(sender, e);
            }
        }

        private void btnFilterOn_Click(object sender, RoutedEventArgs e)
        {

            if (!(txtSearch.Text == null || txtSearch.Text == ""))
            {
                var trainers = from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                               where t.ΕΠΩΝΥΜΟ.Contains(txtSearch.Text) || t.ΑΦΜ.Contains(txtSearch.Text)
                               select t;
                if (trainers.Count() == 0) UserFunctions.ShowAdminMessage("Δεν βρέθηκε καταχώρηση.");
                teacherGrid.ItemsSource = trainers.ToList();
            }
        }

        private void btnFilterOff_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = null;
            
            // disable this to avoid delay of loading grid data
            //var trainers = from dbp in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
            //               orderby dbp.ΕΠΩΝΥΜΟ, dbp.ΟΝΟΜΑ
            //               select dbp;

            
            teacherGrid.ItemsSource = null; // trainers.ToList();
            //aitisiGrid.ItemsSource = null; // am.LoadAitisiData();
            //aitisiForm.ItemsSource = null; // am.LoadAitisiData();
            // select 1st row and not a random default
            //teacherGrid.SelectedItem = teacherGrid.Items[0];
        }

        #endregion

        #region Aitisi DataGrid and DataForm bindings

        // synchronizes the Teacher grid with Aitisi grid

        /* ----------------
         * Syncrhonizes the aitisiForm with the aitisiGrid
         * the ViewModel is useless here as we explicitly tie the row source with the
         * selected item of the parent grid. The navigation buttons on the form
         * are inactive since we force one selected item at a time.
         * 
         * Another important function performed is setting the global class
         * SelectedAitisi so that the aitisi_id can be used in the binding of
         * experiences pages by Experience.xaml. These pages retrieve the value
         * and subsequently set their data sources.
         * ----------------
         */
        private void aitisiGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                ΑΙΤΗΣΗ sel_aitisi = aitisiGrid.SelectedItem as ΑΙΤΗΣΗ;

                if (sel_aitisi == null) return;

                var aitiseis = from dba in db.ΑΙΤΗΣΗs
                               where dba.ΚΩΔ_ΑΙΤΗΣΗ == sel_aitisi.ΚΩΔ_ΑΙΤΗΣΗ
                               orderby dba.ΠΡΟΚΗΡΥΞΗ
                               select dba;

                //ObservableCollection<ΑΙΤΗΣΗ> oca = new ObservableCollection<ΑΙΤΗΣΗ>(aitiseis.ToList());
                aitisiForm.ItemsSource = aitiseis.ToList();
                // set the global value of aitisi_id so it can be used
                SelectedAitisi.AitisiId = sel_aitisi.ΚΩΔ_ΑΙΤΗΣΗ;
                //test
                //UserFunctions.ShowAdminMessage("Aitisi Code=" + SelectedAitisi.AitisiId);

                /*
                 * Εδώ γίνεται εργοποίηση δυνατότητας CRUD στη φόρμα,
                 * για αιτήσεις που αφορούν ανοικτή προκήρυξη.
                 */
                int prokirixi_id = (int)sel_aitisi.ΠΡΟΚΗΡΥΞΗ;
                short prok_status = sp.GetProkirixiStatus(prokirixi_id);

                if (prok_status == 1) aitisiForm.CommandButtonsVisibility = DataFormCommandButtonsVisibility.All;

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

        #endregion

        #region LOB functions

       /// <summary>
       /// Οι κανόνες επικύρωσης εδώ είναι πιο αυστηροί και σύνθετοι.
       /// Date: 18-3-2012
       /// </summary>
       /// <param name="aitisi"></param>
       /// <returns></returns>
        private bool ValidateAitisi(ΑΙΤΗΣΗ aitisi)
        {
            
            return true;

        }
        
        #endregion

        #region DataForm events and LOB functions

        private void aitisiForm_AddingNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddingNewItemEventArgs e)
        {
            isNewRec = true;
        }

        private void aitisiForm_BeginningEdit(object sender, CancelEventArgs e)
        {
            ΑΙΤΗΣΗ sel_aitisi = aitisiGrid.SelectedItem as ΑΙΤΗΣΗ;
            if (sel_aitisi == null)
            {
                UserFunctions.ShowAdminMessage("Πρέπει πρώτα να επιλέξετε αίτηση από τον πίνακα.");
                e.Cancel = true;
                return;
            }
        }

        private void aitisiForm_EditEnding(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndingEventArgs e)
        {
            
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Commit)
            { 
                //UserFunctions.ShowAdminMessage("Οι αλλαγές που έγιναν θα αποθηκευτούν.");
                var aitisi = this.aitisiForm.CurrentItem as ΑΙΤΗΣΗ;
                
                // Το πρωτεύον κλειδί της Αίτησης δημιουργείται από κώδικα και δεν είναι ορατό στο UI
                string aitisiCode = am.BuildAitisiCode(aitisi);
                
                if (String.IsNullOrEmpty(aitisiCode))
                {
                    UserFunctions.ShowAdminMessage("Άκυρος Κωδικός Αίτησης.Η αποθήκευση ακυρώθηκε.");
                    return;
                }

                if (isNewRec == true)
                {
                    // these are linking fields and are not set automatically.
                    // Εδώ γίνεται έλεγχος αν το aitisiCode υπάρχει (ουσιαστικά ο Αρ.Πρωτοκόλλου)
                    // ώστε να μην πετάει εξαίρεση που αναγκάζει επαναφόρτωση του UI.
                    if (am.ExistsAitisiCode(aitisiCode) == true)
                    {
                        RefreshGridData();
                        return;
                    }
                    aitisi.ΚΩΔ_ΑΙΤΗΣΗ = aitisiCode;             // set the value of the PK
                    aitisi.ΑΦΜ = SelectedTeacher.TeacherAFM;    // set the value of ΑΦΜ required!
                    aitisi.ΗΛΙΚΙΑ = CalculateAge(aitisi);       // required only for a new record
                    db.ΑΙΤΗΣΗs.InsertOnSubmit(aitisi);
                    cm.CommitData(db);
                    //LoadData(); //test
                    RefreshGridData();
                    // need to reset it otherwise InsertOnSubmit throws an exception if we edit immediately the new record
                    isNewRec = false;
                }
                else
                {
                    string old_aitisiCode = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;
                    string new_aitisiCode = aitisiCode;
                    // check if changed as exception is thrown on PK change
                    if (am.ChangedAitisiCode(old_aitisiCode, new_aitisiCode) == true)
                    {
                        RefreshGridData();
                        return;
                    }

                    //aitisi.ΚΩΔ_ΑΙΤΗΣΗ = aitisiCode;
                    cm.CommitData(db);
                    RefreshGridData();
                    //db.SubmitChanges();
                }               
            }
        }

        private void aitisiForm_DeletedItem(object sender, Telerik.Windows.Controls.Data.DataForm.ItemDeletedEventArgs e)
        {

        }

        private void aitisiForm_DeletingItem(object sender, CancelEventArgs e)
        {
            
            var sel_aitisi = this.aitisiForm.CurrentItem as ΑΙΤΗΣΗ;

            e.Cancel = true;
            if ((MessageBox.Show("Είστε σίγουροι ότι θέλετε διαγραφή αυτής της εγγραφής;", this.Title,
                     MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
            {                
                // here we must call a function that deletes all children first,
                // which concerns almost all tables in the schema.
                // best idea is to disable the delete, keep it only for testing
                if (CanDeleteAitisi(sel_aitisi))
                {
                    e.Cancel = false;
                    db.ΑΙΤΗΣΗs.DeleteOnSubmit(sel_aitisi);
                    cm.CommitData(db);
                    RefreshGridData();
                }                
            }
        }

        private void aitisiForm_ValidatingItem(object sender, CancelEventArgs e)
        {
            var aitisi = this.aitisiForm.CurrentItem as ΑΙΤΗΣΗ;

            if (aitisi == null)
            {
                //UserFunctions.RadWindowAlert("null Aitisi");
                e.Cancel = false;
                return;
            }

            DialogParameters parameters = new DialogParameters();
            parameters.Header = "Σφάλμα";
            parameters.OkButtonContent = "Κλείσιμο";

            // validation of primary fields (ΠΡΟΚΗΡΥΞΗ, ΙΕΚ_ΑΙΤΗΣΗΣ, ΠΡΩΤΟΚΟΛΛΟ, ΗΜΕΡΟΜΗΝΙΑ)
            string sdate = Convert.ToString(aitisi.ΗΜΕΡΟΜΗΝΙΑ);     // null value converted to ""
            int iProkirixi = Convert.ToInt32(aitisi.ΠΡΟΚΗΡΥΞΗ);     // null value converted to 0
            int iIek = Convert.ToInt32(aitisi.ΙΕΚ_ΑΙΤΗΣΗΣ);         // null value converted to 0
            int iProtocol = Convert.ToInt32(aitisi.ΠΡΩΤΟΚΟΛΛΟ);     // null value converted to 0

            // validation of secondary fields
            decimal degree = Convert.ToDecimal(aitisi.ΒΑΘΜΟΣ_ΠΤΥΧΙΟΥ);


            e.Cancel = false;   // it is set to true in case of errors

            if (sdate == "")
            {
                parameters.Content = "Πρέπει να συμπληρωθεί η ημερομηνία αίτησης";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (!Dates.ValidateAitisiDate(sdate))
            {
                parameters.Content = "Η ημερομηνία αίτησης είναι εκτός ορίων Προκήρυξης";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (iProkirixi == 0) 
            {
                parameters.Content = "Πρέπει να επιλεγεί Προκήρυξη.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (iIek == 0)
            {
                parameters.Content = "Πρέπει να επιλεγεί ΙΕΚ.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (iProtocol == 0 || iProtocol > 9999)
            {
                parameters.Content = "Άκυρος αριθμός Πρωτοκόλλου (> 0 και < 10000).";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (aitisi.ΕΡΓΑΣΙΑ == null)
            {
                parameters.Content = "Πρέπει να καταχωρηθεί Εργασία.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (aitisi.ΚΛΑΔΟΣ == null)
            {
                parameters.Content = "Πρέπει να καταχωρηθεί Κλάδος.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (aitisi.ΕΙΔΙΚΟΤΗΤΑ == null)
            {
                parameters.Content = "Πρέπει να καταχωρηθεί Ειδικότητα.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (degree == 0 || degree <= 0 || degree > 20)
            {
                parameters.Content = "Άκυρος βαθμός πτυχίου (> 0 και <= 20).";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            // πιο σύνθετος έλεγχος αποκλεισμού
            else if (aitisi.ΑΠΟΚΛΕΙΣΜΟΣ == true && aitisi.ΑΠΟΚΛΕΙΣΜΟΣ_ΑΙΤΙΑ == null)
            {
                parameters.Content = "Πρέπει να καταχωρηθεί αιτία αποκλεισμού.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (aitisi.ΑΠΟΚΛΕΙΣΜΟΣ == false && aitisi.ΑΠΟΚΛΕΙΣΜΟΣ_ΑΙΤΙΑ != null)
            {
                parameters.Content = "Πρέπει να χαρακτηριστεί αποκλεισμένος.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (String.IsNullOrEmpty(aitisi.ΤΙΤΛΟΣ_ΠΤΥΧΙΟΥ))
            {
                parameters.Content = "Πρέπει να συμπληρωθεί ο τίτλος του βασικού πτυχίου.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }




        } // AtisiForm_ValidatingItem

        #endregion


        // Πλοήγηση στη σελίδα προϋπηρεσιών και μοριοδότησης
        private void btnExp_Click(object sender, RoutedEventArgs e)
        {
            Experience exp;
            ΑΙΤΗΣΗ sel_aitisi = aitisiGrid.SelectedItem as ΑΙΤΗΣΗ;
            if (sel_aitisi == null)
            {
                UserFunctions.ShowAdminMessage("Πρέπει πρώτα να επιλέξετε αίτηση από τον πίνακα.");
                return;
            }
            else
            exp = new Experience();
            
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(exp);
        }

        // Calculate the Age field
        private short CalculateAge(ΑΙΤΗΣΗ aitisi)
        {
            DateTime RefDate = mm.GetRefDate((int)aitisi.ΠΡΟΚΗΡΥΞΗ);
            
            DateTime BirthDate = mm.GetBirthDate(aitisi.ΑΦΜ);
            
            short Age = (short)Dates.YearsDiff(BirthDate, RefDate);

            return Age;
        }

        private void RefreshGridData()
        {
            var aitisi = this.aitisiForm.CurrentItem as ΑΙΤΗΣΗ;
            string afm = SelectedTeacher.TeacherAFM;
            aitisiGrid.ItemsSource = am.LoadAitisiData(afm);
            aitisiForm.ItemsSource = am.LoadAitisiData(afm);            
        }

        private bool CanDeleteAitisi(ΑΙΤΗΣΗ aitisi)
        {
            string aitisi_id = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

            var didaktiki_recs = (from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                                  where d.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                                  select d).Count();

            var epagelmatiki_recs = (from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                                     where e.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                                     select e).Count();

            var eepagelma_recs = (from ee in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                                  where ee.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                                  select ee).Count();

            var iek_recs = (from i in db.ΕΚΠ_ΙΕΚs
                            where i.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                            select i).Count();

            if (didaktiki_recs > 0 || epagelmatiki_recs > 0 || eepagelma_recs > 0 || iek_recs > 0)
            {
                UserFunctions.ShowAdminMessage("Διαγράψτε πρώτα τις συσχετισμένες εγγραφές \n (προϋπηρεσίες, ΙΕΚ προτίμησης).");
                return false;
            }

            return true;
        }

        private void aitisiGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if(!am.CheckOpenProkirixi()) UserFunctions.ShowAdminMessage("Δεν βρέθηκε ανοικτή προκήρυξη.");
        }


    } // class Teachers
}
