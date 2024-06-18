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
    /// Interaction logic for AitisiDetail.xaml
    /// </summary>
    public partial class AitisiForm : UserControl
    {
        private ThetisDataContext db = new ThetisDataContext();
        private SelectedProkirixi sp = new SelectedProkirixi();

        //private bool first_time = true;
        public AitisiForm()
        {
            InitializeComponent();
            LoadData();
            //DisableFields();
        }

        // this loads just the combo data
        public void LoadData()
        {
            // η επιλογή περιορίζεται στην ανοικτή προκήρυξη
            var prokirixis = from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΚΑΤΑΣΤΑΣΗ == 1
                             orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                             select p;
            cboProkirixi.ItemsSource = prokirixis.ToList();

            // η επιλογή περιορίζεται στο login IEK (εκτός για admin)
            // Date: 16-5-12
            int user_key = LoginClass.UserKey;

            if (user_key > 0)
            {
                var ieks = from i in db.ΙΕΚs
                           where i.ΙΕΚ_ΚΩΔ == user_key
                           orderby i.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                           select i;
                cboiek.ItemsSource = ieks.ToList();
            }
            else
            {
                var ieks = from i in db.ΙΕΚs
                           orderby i.ΙΕΚ_ΟΝΟΜΑΣΙΑ
                           select i;
                cboiek.ItemsSource = ieks.ToList();
            }

            var marital_status = from ms in db.ΟΙΚΟΓΕΝΕΙΑs
                                 select ms;
            var kladoi = from k in db.ΚΛΑΔΟΣs
                         orderby k.ΚΩΔ_ΚΛΑΔΟΣ
                         select k;
            var eidikotites = from e in db.ΕΙΔΙΚΟΤΗΤΑs
                              orderby e.ΒΑΘΜΙΔΑ, e.ΚΛΑΔΟΣ
                              select e;
            var spoudes = from s in db.ΣΠΟΥΔΕΣs
                          select s;
            var apokleismoi = from a in db.ΑΠΟΚΛΕΙΣΜΟΣs
                              orderby a.ΑΙΤΙΑ
                              select a;
            var ergasia = from e in db.ΕΡΓΑΣΙΑs
                          select e;

            cboFamily.ItemsSource = marital_status.ToList();
            cboKlados.ItemsSource = kladoi.ToList();
            cboEidikotita.ItemsSource = eidikotites.ToList();
            cboSpoudes.ItemsSource = spoudes.ToList();
            cboApokleismos.ItemsSource = apokleismoi.ToList();
            cboErgasia.ItemsSource = ergasia.ToList();

        }

        // NOT used
        private void DisableFields()
        {
            if (SelectedAitisi.AitisiId == null) return;
            //if (SelectedAitisi.AitisiProkirixi == 0) return;
            //if (SelectedAitisi.AitisiProkirixi == -1) return;

            cboProkirixi.IsEnabled = false;
            cboiek.IsEnabled = false;
            txtProtocol.IsEnabled = false;
            dpAitisiDate.IsEnabled = false;

        }

        // this works but when the parent combo is changed after the first time, there is
        // a very long delay (30-45s) for the child data to show. (why ?)
        // It is solved by forcing dropdown of the combo !!!
        private void cboKlados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // we need this, otherwise the dropdown is fired even without user selection
            if (!cboKlados.IsLoaded) return;
            if (cboKlados.SelectedValue != null)
            {
                //cboEidikotita.ItemsSource = null;
                int selected_klados = (int)cboKlados.SelectedValue;

                var eidikotites = from t in db.ΕΙΔΙΚΟΤΗΤΑs
                                  where t.ΒΑΘΜΙΔΑ == selected_klados
                                  orderby t.ΒΑΘΜΙΔΑ, t.ΚΛΑΔΟΣ
                                  select t;
                //var oce = new ObservableCollection<ΕΙΔΙΚΟΤΗΤΑ>(eidikotites.ToList());
                cboEidikotita.IsDropDownOpen = true;
                cboEidikotita.ItemsSource = eidikotites.ToList();
                
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (txtAitisiID != null)
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == txtAitisiID.Text
                              select new {a.ΗΜΕΡΟΜΗΝΙΑ}).FirstOrDefault();

                //UserFunctions.RadWindowAlert("ID exists");
                if (aitisi != null)
                {
                    dpAitisiDate.CurrentDateTimeText = Convert.ToString(aitisi.ΗΜΕΡΟΜΗΝΙΑ);
                }
            }
        }

    }
}
