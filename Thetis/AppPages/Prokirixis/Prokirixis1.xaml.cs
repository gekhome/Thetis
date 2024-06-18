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
using Microsoft.Win32;
using Prototype.Utilities;
using MessageBoxUtils;
using Prototype.Model;

namespace Prototype.AppPages.Prokirixis
{
    /// <summary>
    /// Interaction logic for Prokirixis.xaml
    /// </summary>
    public partial class Prokirixis1 : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private CollectionView View;

        public Prokirixis1()
        {
            InitializeComponent();
            this.DataContext = db.ΠΡΟΚΗΡΥΞΗs;
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //fill the combo for school years and prokirixi status
            var school_years = from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                               orderby s.ΣΧ_ΕΤΟΣ descending
                               select s;
            var prok_status = from ps in db.PROK_STATUS
                              select ps;

            cboSchoolYear.ItemsSource = school_years.ToList();
            cboProkirixiStatus.ItemsSource = prok_status.ToList();

            this.View = (CollectionView)(CollectionViewSource.GetDefaultView(this.db.ΠΡΟΚΗΡΥΞΗs));
        }

        #region Record Navigation

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {   
            this.View.MoveCurrentToFirst();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if ((this.View.CurrentPosition > 0))
            {
                this.View.MoveCurrentToPrevious();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((this.View.CurrentPosition < (this.View.Count - 1)))
            {
                this.View.MoveCurrentToNext();
            }
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            this.View.MoveCurrentToLast();
        }

        #endregion

        #region Record Operations

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((this.View.CurrentPosition > -1))
            {
                ΠΡΟΚΗΡΥΞΗ prokDel = this.View.CurrentItem as ΠΡΟΚΗΡΥΞΗ;
                var proks = from dbp in db.ΠΡΟΚΗΡΥΞΗs
                            orderby dbp.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                            select dbp;

                var ocp = new ObservableCollection<ΠΡΟΚΗΡΥΞΗ>(proks.ToList());
                
                // verification of delete with custom or built-in dialog
                if ((WPFMessageBox.Show("Είστε σίγουροι ότι θέλετε διαγραφή αυτής της εγγραφής;", this.Title, 
                     MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
                {
                    ocp.Remove(prokDel);
                    //this.View = (CollectionView)(CollectionViewSource.GetDefaultView(ocp));
                    db.ΠΡΟΚΗΡΥΞΗs.DeleteOnSubmit(prokDel);
                    db.SubmitChanges();
                    View.Refresh();
                    View.MoveCurrentToLast();
                }
                
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ΠΡΟΚΗΡΥΞΗ prok = new ΠΡΟΚΗΡΥΞΗ();

            var proks = from dbp in db.ΠΡΟΚΗΡΥΞΗs
                              orderby dbp.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                              select dbp;

            var ocp = new ObservableCollection<ΠΡΟΚΗΡΥΞΗ>(proks.ToList());
            ocp.Add(prok);
            this.View.MoveCurrentTo(prok);
            db.ΠΡΟΚΗΡΥΞΗs.InsertOnSubmit(prok);
            db.SubmitChanges();
            //this.View = (CollectionView)(CollectionViewSource.GetDefaultView(ocp));
            //this.View.Refresh();
            //this.View.MoveCurrentToLast();
        }

        private void btnRevert_Click(object sender, RoutedEventArgs e)
        {
 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            db.SubmitChanges();
            WPFMessageBox.Show("Η αποθήκευση έγινε.", "Μήνυμα", MessageBoxButton.OK, MessageBoxImage.Information);    
        }

        #endregion

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            /*
            // this calls the pdf viewer (XAML Window hosting Windows Form)
            // Create OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog()
            { //Set filter for file extension, default file extension and default directory
                DefaultExt = ".pdf",
                Filter = "Αρχεία PDF (.pdf)|*.pdf",
                Title = "Επιλέξτε ένα αρχείο PDF",
                InitialDirectory = ""
            };

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            string filename = dlg.FileName;
            */
            // Get the selected file name (full path) and create object only if a file is selected
            string filename;
            string DefaultExt = ".pdf";
            string Filter = "Αρχεία PDF (.pdf)|*.pdf";
            string Title = "Επιλέξτε ένα αρχείο PDF";
            string InitialDirectory = "";

            filename = Prototype.Utilities.UserFunctions.OpenFile(DefaultExt,Filter,Title,InitialDirectory);

            if (! String.IsNullOrEmpty(filename))
            {
                pdfviewer pdfWin = new pdfviewer(filename);
                pdfWin.Show();
            }
            
            /*    
            if (result == true && filename != null)
            {
                pdfviewer pdfWin = new pdfviewer(filename);
                pdfWin.Show();
            }        
            */
        }

    } // Class Prokirixis
} // namespace
