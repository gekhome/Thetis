using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Thetis.Model;
using Thetis.Controls;

using Thetis.Utilities;
using Thetis.DataAccess;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Thetis.AppPages.Auxiliary.Teachers
{
    /// <summary>
    /// Interaction logic for AitisiPage.xaml
    /// </summary>
    public partial class AitisiPage : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        private SelectedProkirixi sp = new SelectedProkirixi();
        private string selected_aitisi = null;
        Loading loadingWin = null;
        bool isLoadingWinCreated = false;

        public AitisiPage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            // this has changed to load only the combos data sources
            // for speed and clarity.
            // Date: 6-6-12

            //source for parent grid
            var trainers = from dbp in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                           orderby dbp.ΕΠΩΝΥΜΟ, dbp.ΟΝΟΜΑ
                           select dbp;
            
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
            var iek_query = from iek in db.ΙΕΚs
                            select iek;           
            var school_years = from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                               select s;        
            var etos = from e in db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs
                       select e;
            var ergasia = from e in db.ΕΚΠ_ΕΡΓΑΣΙΑs
                          select e;
                      
            //data sources for children grids
            //var aitiseis = from dba in db.ΑΙΤΗΣΗs
            //               orderby dba.ΠΡΟΚΗΡΥΞΗ
            //               select dba;
            //var didaktiki = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
            //                orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending
            //                select d;
            //var epagelmatiki = from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
            //                   select e;
            //var epagelma = from e in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
            //               orderby e.ΟΙΚ_ΕΤΟΣ
            //               select e;
            //var other_iek = from o in db.ΕΚΠ_ΙΕΚs
            //                select o;


            teacherGrid.ItemsSource = null; // trainers.ToList();
            aitisiGrid.ItemsSource = null; // aitiseis.ToList();
            teachGrid.ItemsSource = null; // didaktiki.ToList();
            profGrid.ItemsSource = null; // epagelmatiki.ToList();
            freeGrid.ItemsSource = null; // epagelma.ToList();
            iekGrid.ItemsSource = null; // other_iek.ToList();

            cboErgasia.ItemsSource = ergasia.ToList();
            cboOikEtos.ItemsSource = etos.ToList();
            cboSchoolYear.ItemsSource = school_years.ToList();
            cboIek.ItemsSource = iek_query.ToList();
            cboiek.ItemsSource = ieks.ToList();
            cboeidikotitita.ItemsSource = eidikotites.ToList();
            cboprok.ItemsSource = prokirixis.ToList();
        }


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

                teacherGrid.ItemsSource = trainers.ToList();
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

            teacherGrid.ItemsSource = null; // trainers.ToList();
            // select 1st row and not a random default
            //teacherGrid.SelectedItem = teacherGrid.Items[0];
        }

        #endregion

        private void teacherGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //teacherGrid.SelectedItem = teacherGrid.Items[0];
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

                // DO NOT set the public property to the AFM of the current teacher
                //SelectedTeacher.TeacherAFM = afm;

                var aitisi_qry = from a in db.ΑΙΤΗΣΗs
                                 where a.ΑΦΜ == afm
                                 select a;

                ObservableCollection<ΑΙΤΗΣΗ> oca = new ObservableCollection<ΑΙΤΗΣΗ>(aitisi_qry.ToList());
                aitisiGrid.ItemsSource = oca;
                
                // reset the experience grids to avoid showing data from previously selected aitisi
                teachGrid.ItemsSource = null;
                profGrid.ItemsSource = null;
                freeGrid.ItemsSource = null;
                iekGrid.ItemsSource = null;
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


        private void ShowTeacherInfo(ΕΚΠΑΙΔΕΥΤΙΚΟΣ selectedTeacher)
        {
            var teacher = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                           where t.ΑΦΜ == selectedTeacher.ΑΦΜ
                           select new { t.ΑΦΜ, t.ΕΠΩΝΥΜΟ, t.ΟΝΟΜΑ }).FirstOrDefault();

            txtTeacherInfo.Text = "ΑΦΜ: " + teacher.ΑΦΜ + " - Ονοματεπώνυμο: " + teacher.ΕΠΩΝΥΜΟ + " " + teacher.ΟΝΟΜΑ;
        }

        private void aitisiGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            ΑΙΤΗΣΗ sel_aitisi = aitisiGrid.SelectedItem as ΑΙΤΗΣΗ;

            if (sel_aitisi == null) return;     // avoid null exception

            selected_aitisi = sel_aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

            // synchronize all grids (Διδακτική, Επαγγελματική, Ελ.Επάγγελμα, ΙΕΚ προτίμησης
            var didaktiki = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                            where d.ΚΩΔ_ΑΙΤΗΣΗ == selected_aitisi
                            orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending
                            select d;
            teachGrid.ItemsSource = didaktiki.ToList();

            var epagelmatiki = from ep in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                               where ep.ΚΩΔ_ΑΙΤΗΣΗ == selected_aitisi
                               select ep;
            profGrid.ItemsSource = epagelmatiki.ToList();

            var epagelma = from ee in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                           where ee.ΚΩΔ_ΑΙΤΗΣΗ == selected_aitisi
                           orderby ee.ΟΙΚ_ΕΤΟΣ
                           select ee;
            freeGrid.ItemsSource = epagelma.ToList();

            var other_iek = from o in db.ΕΚΠ_ΙΕΚs
                            where o.ΚΩΔ_ΑΙΤΗΣΗ == selected_aitisi
                            select o;
            iekGrid.ItemsSource = other_iek.ToList();
        }

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
}
