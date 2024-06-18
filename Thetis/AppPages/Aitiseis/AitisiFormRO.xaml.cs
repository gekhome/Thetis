using System.Linq;
using System.Windows.Controls;
using Thetis.Model;
using Thetis.Utilities;

namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for AitisiDetail.xaml
    /// </summary>
    public partial class AitisiFormRO : UserControl
    {
        private ThetisDataContext db = new ThetisDataContext();
        public AitisiFormRO()
        {
            InitializeComponent();
            LoadData();
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
                         select k;
            var eidikotites = from e in db.ΕΙΔΙΚΟΤΗΤΑs
                              select e;
            var spoudes = from s in db.ΣΠΟΥΔΕΣs
                          select s;
            var apokleismoi = from a in db.ΑΠΟΚΛΕΙΣΜΟΣs
                              select a;
            var ergasia = from e in db.ΕΡΓΑΣΙΑs
                          select e;

            cboFamily.ItemsSource = marital_status.ToList();
            cboKlados.ItemsSource = kladoi.ToList();
            cboEidikotita.ItemsSource = eidikotites.ToList();
            cboSpoudes.ItemsSource = spoudes.ToList();
            cboApokleismos.ItemsSource = apokleismoi.ToList();
            cboErgasia.ItemsSource = ergasia.ToList();
            //txtAitisiDate.SelectedDate = DateTime.Now;
        }
    }
}
