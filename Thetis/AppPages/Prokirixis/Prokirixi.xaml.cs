using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading;
using Telerik.Windows.Controls;
using Thetis.Utilities;
using Thetis.Model;
using Thetis.DataAccess;
using Thetis.Controls;


namespace Thetis.AppPages.Prokirixis
{
    /// <summary>
    /// Interaction logic for Prokirixi.xaml
    /// </summary>
    public partial class Prokirixi : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ObservableCollection<ΠΡΟΚΗΡΥΞΗ> ocProkirixi = new ObservableCollection<ΠΡΟΚΗΡΥΞΗ>();
        private CommitModel cm = new CommitModel();
        
        // loading window fields
        static Loading loadingWin;
        static bool isLoadingWinCreated;

        /*
         * this is required by the edit events of RadDataForm
         * to distinguish if editing concerns an existing record
         * or a new one. It is set in the _AddingNewItem event of the form.
        */
        private bool isNewRec = false;

        public Prokirixi()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            var prokirixis = from p in db.ΠΡΟΚΗΡΥΞΗs
                             orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ descending
                             select p;

            /*
            // these are the collection sources for the grid combos
            var school_years = from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                               orderby s.ΣΧ_ΕΤΟΣ descending
                               select s;
            var prok_status = from ps in db.PROK_STATUS
                              select ps;

            
            //cboSchoolYear.ItemsSource = school_years.ToList();
            //cboProkirixiStatus.ItemsSource = prok_status.ToList();
            */

            // similar loading with data grids
            var ocP = new ObservableCollection<ΠΡΟΚΗΡΥΞΗ>(prokirixis.ToList());

            DataFormProk.ItemsSource = ocP;
        }

        #region ProgressBar Threading

        private void Page_Initialized(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                ProgressBarShow();

                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (!isLoadingWinCreated) ;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadingWin != null)
            {
                loadingWin.CloseForm();
            }
            Cursor = Cursors.Arrow;
        }

        private static void ProgressBarShow()
        {
            loadingWin = new Loading()
            {
                Left = Convert.ToDouble(SystemParameters.PrimaryScreenWidth / 2.0 - 120.0),
                Top = Convert.ToDouble(SystemParameters.PrimaryScreenHeight - 240.0)
            };

            isLoadingWinCreated = true;
            loadingWin.Show();
        }

        #endregion


        #region Combos Data Sources

        private List<ΣΧΟΛΙΚΟ_ΕΤΟΣ> GetSchoolYears()
        {
            List<ΣΧΟΛΙΚΟ_ΕΤΟΣ> school_yearList = new List<ΣΧΟΛΙΚΟ_ΕΤΟΣ>();

            var school_years = from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                               orderby s.ΣΧ_ΕΤΟΣ descending
                               select s;

            return school_yearList = school_years.ToList();
        }
        private List<PROK_STATUS> GetProkStatus()
        {
            List<PROK_STATUS> prokStatusList = new List<PROK_STATUS>();

            var prok_status = from ps in db.PROK_STATUS
                              select ps;

            return prokStatusList = prok_status.ToList();
        }

        #endregion


        #region DataForm Event Handling

        private void DataFormProk_AutoGeneratingField(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
            //var prokirixi = this.DataFormProk.CurrentItem as ΠΡΟΚΗΡΥΞΗ;
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΠΡΟΚΗΡΥΞΗ_ΚΩΔ", "Α/Α Προκήρυξης");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΠΡΟΚΗΡΥΞΗ_ΑΡ", "Αρ. Προκήρυξης");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΦΕΚ", "Φ.Ε.Κ.");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΗΜΝΙΑ_ΕΝΑΡΞΗ", "Ημ/νία Έναρξης");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΗΜΝΙΑ_ΛΗΞΗ", "Ημ/νία Λήξης");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΣΧΟΛΙΚΟ_ΕΤΟΣ", "Σχολικό Έτος");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΦΟΡΕΑΣ", "Φορέας");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΔΙΟΙΚΗΤΗΣ", "Διοικητής");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΠΡΟΣΜ_ΕΤΗ", "Προσμετρήσιμα Έτη");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ", "Μόρια Μεταπτυχιακού");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ", "Μόρια Διδακτορικού");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ", "Μόρια Παιδαγωγικού");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΠΑΙΔΑΓΩΓΙΚΟ_ΠΡΟΣΜ", "Προσμέτρηση Παιδαγωγικού");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΜΟΡΙΑ_ΩΡΑ", "Διδακτική (μόρια/ώρα)");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΜΟΡΙΑ_ΗΜΕΡΑ", "Επαγγελμ. (μόρια/μέρα)");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΥΠΟΒΟΛΗ", "Μόνο πρόσθετα δικαιολογητικά");
            e.DataField.Label = ((string)e.DataField.Label).Replace("ΚΑΤΑΣΤΑΣΗ", "Καθεστώς Προκήρυξης");



            if (e.PropertyName == "ΠΡΟΚΗΡΥΞΗ_ΚΩΔ")
            {
                e.DataField.Visibility = Visibility.Hidden;
                //e.DataField.IsEnabled = false;
            }

            if (e.PropertyName == "ΣΧΟΛΙΚΟ_ΕΤΟΣ")
            {
                DataFormComboBoxField schoolYearField = new DataFormComboBoxField();
                schoolYearField.ItemsSource = GetSchoolYears();
                schoolYearField.Label = "Σχολικό Έτος";

                schoolYearField.DisplayMemberPath = "ΣΧ_ΕΤΟΣ";
                schoolYearField.SelectedValuePath = "ΣΧ_ΕΤΟΣ_ΚΩΔ";
                schoolYearField.DataMemberBinding = new Binding("ΣΧΟΛΙΚΟ_ΕΤΟΣ");
                e.DataField = schoolYearField;
            }

            if (e.PropertyName == "ΚΑΤΑΣΤΑΣΗ")
            {
                DataFormComboBoxField prokirixiField = new DataFormComboBoxField();
                prokirixiField.ItemsSource = GetProkStatus();
                prokirixiField.Label = "Καθεστώς Προκήρυξης";
                prokirixiField.Description = "Αλλαγή καθεστώτος μόνο από διαχειριστή";
                prokirixiField.DisplayMemberPath = "STATUS";
                prokirixiField.SelectedValuePath = "STATUS_ID";
                prokirixiField.DataMemberBinding = new Binding("ΚΑΤΑΣΤΑΣΗ");
                e.DataField = prokirixiField;
            }
        }

        private void DataFormProk_EditEnding(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndingEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Commit)
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Content = "Οι αλλαγές που έγιναν θα αποθηκευτούν.";
                parameters.Header = "Προειδοποίηση";
                parameters.OkButtonContent = "Κλείσιμο";
                RadWindow.Alert(parameters);

                if (isNewRec == true)
                {
                    var prokirixi = this.DataFormProk.CurrentItem as ΠΡΟΚΗΡΥΞΗ;
                    db.ΠΡΟΚΗΡΥΞΗs.InsertOnSubmit(prokirixi);
                    cm.CommitData(db);
                    //db.SubmitChanges();
                    // need to reset it otherwise InsertOnSubmit throws an exception if we edit immediately the new record
                    isNewRec = false;
                }
                else
                    cm.CommitData(db);
                    //db.SubmitChanges();

                CheckOpenProkirixis();
            }
        }//DataFormProk_EditEnding
        
        private void DataFormProk_AddingNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddingNewItemEventArgs e)
        {
            // check who is logged-in if not admin show message and cancel edit
            if (LoginClass.UserKey > 0)
            {
                UserFunctions.ShowAdminMessage("Επεξεργασία δυνατή μόνο από διαχειριστές.");
                e.Cancel = true;
                return;
            }
            isNewRec = true;
        }

        private void DataFormProk_DeletingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LoginClass.UserKey > 0)
            {
                UserFunctions.ShowAdminMessage("Επεξεργασία δυνατή μόνο από διαχειριστές.");
                e.Cancel = true;
                return;
            }

            var prokirixi = this.DataFormProk.CurrentItem as ΠΡΟΚΗΡΥΞΗ;
            if (prokirixi.ΚΑΤΑΣΤΑΣΗ == 2)
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Content = "Δεν μπορεί να γίνει διαγραφή κλειστής Προκήρυξης.";
                parameters.Header = "Προειδοποίηση";
                parameters.OkButtonContent = "Κλείσιμο";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else
            {
                if ((MessageBox.Show("Είστε σίγουροι ότι θέλετε διαγραφή αυτής της εγγραφής;", this.Title,
                     MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
                {
                    if (ValidateDeleteProkirixi(prokirixi.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ))
                    {
                        e.Cancel = false;
                        db.ΠΡΟΚΗΡΥΞΗs.DeleteOnSubmit(prokirixi);
                        cm.CommitData(db);
                    }
                    else
                    {
                        UserFunctions.ShowAdminMessage("Δεν μπορεί να γίνει διαγραφή διότι υπάρχουν συσχετισμένες αιτήσεις.");
                        e.Cancel = true;
                    }
                }
            }
        } //DataFormProk_DeletingItem


        private void DataFormProk_DeletedItem(object sender, Telerik.Windows.Controls.Data.DataForm.ItemDeletedEventArgs e)
        {
            //RadWindow.Alert("DeletedItem Event fired.");
        }

        private void DataFormProk_ValidatingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var prokirixi = this.DataFormProk.CurrentItem as ΠΡΟΚΗΡΥΞΗ;
            if (prokirixi == null)
            {
                e.Cancel = false;
                return;
            }

            // validate input fields
            string dateStart = Convert.ToString(prokirixi.ΗΜΝΙΑ_ΕΝΑΡΞΗ); //null value converted to ""
            string dateEnd = Convert.ToString(prokirixi.ΗΜΝΙΑ_ΛΗΞΗ);     //null value converted to ""
            int schoolYearID = Convert.ToInt32(prokirixi.ΣΧΟΛΙΚΟ_ΕΤΟΣ);
            // setup RadAlert parameters
            DialogParameters parameters = new DialogParameters();
            parameters.Header = "Σφάλμα";
            parameters.OkButtonContent = "Κλείσιμο";
            e.Cancel = false;
            
            if (dateStart == "" || dateEnd == "") 
            {
                parameters.Content = "Πρέπει να συμπληρωθούν και οι δύο ημερομηνίες";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (!Dates.ValidStartEndDates(dateStart, dateEnd)) 
            {
                parameters.Content = "Η αρχική ημερομηνία είναι μεγαλύτερη της τελικής";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (!Dates.InSameYear(dateStart, dateEnd)) 
            {
                parameters.Content = "Η αρχική και τελική ημερομηνία πρέπει να έχουν το ίδιο έτος";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΚΑΤΑΣΤΑΣΗ == null)
            {
                parameters.Content = "Πρέπει να επιλεγεί καθεστώς Προκήρυξης.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΠΡΟΚΗΡΥΞΗ_ΑΡ == null)
            {
                parameters.Content = "Πρέπει να συμπληρωθεί το αναγνωριστικό της Προκήρυξης.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΣΧΟΛΙΚΟ_ΕΤΟΣ == null)
            {
                parameters.Content = "Πρέπει να γίνει επιλογή σχολικού έτους.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (!Dates.InSchoolYear(dateStart, dateEnd, schoolYearID))
            {
                parameters.Content = "Οι ημερομηνίες προκήρυξης δεν συμβαδίζουν με το σχολικό έτος";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΠΡΟΣΜ_ΕΤΗ == 0 || prokirixi.ΠΡΟΣΜ_ΕΤΗ == null)
            {
                parameters.Content = "Άκυρη καταχώρηση προσμετρήσιμων ετών.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ == 0 || prokirixi.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ == null)
            {
                parameters.Content = "Άκυρη καταχώρηση μορίων μεταπτυχιακού.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ == 0 || prokirixi.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ == null)
            {
                parameters.Content = "Άκυρη καταχώρηση μορίων διδακτορικού.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ == 0 || prokirixi.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ == null)
            {
                parameters.Content = "Άκυρη καταχώρηση μορίων παιδαγωγικού.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΜΟΡΙΑ_ΩΡΑ == null || prokirixi.ΜΟΡΙΑ_ΩΡΑ == 0 || prokirixi.ΜΟΡΙΑ_ΩΡΑ > 5)
            {
                parameters.Content = "Άκυρη καταχώρηση μορίων/ώρα διδακτικής.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΜΟΡΙΑ_ΗΜΕΡΑ == null || prokirixi.ΜΟΡΙΑ_ΗΜΕΡΑ == 0 || prokirixi.ΜΟΡΙΑ_ΗΜΕΡΑ > 5)
            {
                parameters.Content = "Άκυρη καταχώρηση μορίων/ημέρα επαγγελματικής.";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            else if (prokirixi.ΥΠΟΒΟΛΗ == null)
            {
                parameters.Content = "Δεν έγινε επιλογή υποβολής δικαιολογητικών";
                RadWindow.Alert(parameters);
                e.Cancel = true;
            }
            
        } //DataFormProk_DeletedItem

        private void CheckOpenProkirixis()
        {
            // έλεγχος αν υπάρχει ΑΝΟΙΚΤΗ προκήρυξη
            // ή αν δεν είναι μόνο μία.
            var recs = (from p in db.ΠΡΟΚΗΡΥΞΗs
                        where p.ΚΑΤΑΣΤΑΣΗ == 1
                        select p).Count();

            if (recs == 0)
            {
                UserFunctions.ShowAdminMessage("Δεν βρέθηκαν ανοικτές Προκηρύξεις.");
            }
            else if (recs > 1)
            {
                UserFunctions.ShowAdminMessage("Βρέθηκαν περισσότερες από μία ανοικτές Προκηρύξεις.");
            }
        }


        #endregion


        #region PDF Viewer
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //// Get the selected file name (full path) and create object only if a file is selected
            //string filename;
            //string DefaultExt = ".pdf";
            //string Filter = "Αρχεία PDF (.pdf)|*.pdf";
            //string Title = "Επιλέξτε ένα αρχείο PDF";
            //string InitialDirectory = "";

            //filename = Thetis.Utilities.UserFunctions.OpenFile(DefaultExt, Filter, Title, InitialDirectory);

            //if (!String.IsNullOrEmpty(filename))
            //{
            //    pdfviewer pdfWin = new pdfviewer(filename);
            //    pdfWin.Show();
            //}
        }
        #endregion

        private void DataFormProk_BeginningEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check who is logged-in if not admin show message and cancel edit
            if (LoginClass.UserKey > 0)
            {
                UserFunctions.ShowAdminMessage("Επεξεργασία δυνατή μόνο από διαχειριστές.");
                e.Cancel = true;
            }
            
        }

        private bool ValidateDeleteProkirixi(int prokirixi_id)
        {
            var aitiseis = (from p in db.ΑΙΤΗΣΗs
                              where p.ΠΡΟΚΗΡΥΞΗ == prokirixi_id
                              select p).Count();

            if (aitiseis > 0) return false;
            else return true;

            
        }

    }
}
