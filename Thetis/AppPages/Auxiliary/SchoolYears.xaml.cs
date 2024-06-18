using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.DataAccess;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Thetis.AppPages.Auxiliary
{
    /// <summary>
    /// Interaction logic for SchoolYears.xaml
    /// </summary>
    public partial class SchoolYears : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private CommitModel cm = new CommitModel();

        public SchoolYears()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {

            var school_years = from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                               select s;

            /*
            try convert to ObservableCollection<T> where T is a type (class)
            this works but igDataGrid does not show "add" button, so switched
            to telerik GridView control which is easier to handle and has
            similar properties as the WPFToolkit DataGrid.
            */
            var oc = new ObservableCollection<ΣΧΟΛΙΚΟ_ΕΤΟΣ>(school_years.ToList());

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = oc; 
        }

        private void dataGrid_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            var row = e.Row as GridViewRow;
            ΣΧΟΛΙΚΟ_ΕΤΟΣ school_year = row.Item as ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                string syear = school_year.ΣΧ_ΕΤΟΣ;
                if (Dates.SchoolYearExists(syear))
                {
                    LoadData(); // clear the collection from the new row
                    return;
                }

                db.ΣΧΟΛΙΚΟ_ΕΤΟΣs.InsertOnSubmit(school_year);
            }
        }

        #region CRUD functions

        private void dataGrid_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            dataGrid.CurrentCellInfo = new GridViewCellInfo(dataGrid.CurrentItem, dataGrid.Columns["txtSchoolYear"]);
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
            string checkMessage = "Να γίνει διαγραφή των επιλεγμένων εγγραφών; " + "\n";

            if (MessageBox.Show(checkMessage, "Διαγραφή", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { return; }

            // proceed with deletion process
            foreach (var row in dataGrid.SelectedItems)
            {
                ΣΧΟΛΙΚΟ_ΕΤΟΣ school_years = row as ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                db.ΣΧΟΛΙΚΟ_ΕΤΟΣs.DeleteOnSubmit(school_years);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.BeginEdit();
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

        #endregion


        #region Validation rules

        private void dataGrid_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.Name == "txtSchoolYear")
            {
                string iek_name = e.NewValue.ToString();
                if (String.IsNullOrWhiteSpace(iek_name))
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Δεν έχει εισαχθεί τιμή.";
                }
            }
            
            //Initializing to dummyDate in order to stop warnings before the user inputs a value            
            DateTime dummyDate = new DateTime(1, 1, 1);
            DateTime enarxi = dummyDate;
            DateTime lixi = dummyDate;

            if (e.Cell.Column.Name == "txtStartDate")
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
                }
            }
            if (e.Cell.Column.Name == "txtEndDate")
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
        }

        private void dataGrid_RowValidating(object sender, GridViewRowValidatingEventArgs e)
        {
            ΣΧΟΛΙΚΟ_ΕΤΟΣ schoolYear = e.Row.DataContext as ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            
            string sy_string = schoolYear.ΣΧ_ΕΤΟΣ;
            DateTime date1 = (DateTime)schoolYear.ΣΧ_ΕΤΟΣ_ΕΝΑΡΞΗ;
            DateTime date2 = (DateTime)schoolYear.ΣΧ_ΕΤΟΣ_ΛΗΞΗ;

            if (schoolYear.ΣΧ_ΕΤΟΣ_ΕΝΑΡΞΗ == schoolYear.ΣΧ_ΕΤΟΣ_ΛΗΞΗ)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "txtStartDate";
                validationResult.ErrorMessage = "Η έναρξη δεν μπορεί να έχει την ίδια τιμή με την λήξη.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }
            if (schoolYear.ΣΧ_ΕΤΟΣ_ΕΝΑΡΞΗ > schoolYear.ΣΧ_ΕΤΟΣ_ΛΗΞΗ)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "txtStartDate";
                validationResult.ErrorMessage = "Η έναρξη δεν μπορεί να είναι πιο μετά απο την λήξη.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }
            if (schoolYear.ΣΧ_ΕΤΟΣ_ΕΝΑΡΞΗ == null || schoolYear.ΣΧ_ΕΤΟΣ_ΛΗΞΗ == null)
            {
                GridViewCellValidationResult validationResult = new GridViewCellValidationResult();
                validationResult.PropertyName = "txtStartDate";
                validationResult.ErrorMessage = "Πρέπει να συμπληρωθούν και οι δύο ημερομηνίες.";
                e.ValidationResults.Add(validationResult);
                e.IsValid = false;
            }
            if (!Dates.VerifySchoolYear(sy_string, date1, date2))
            {
                e.IsValid = false;
            }
        }

        #endregion Validation rules

    } // class SchoolYears
}
