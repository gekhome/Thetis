using System.Linq;
using System.Windows.Controls;
using Thetis.Model;

namespace Thetis.AppPages.Auxiliary.Teachers
{
    /// <summary>
    /// Interaction logic for AitisiDetail.xaml
    /// </summary>
    public partial class AitisiDetail : UserControl
    {
        private ThetisDataContext db = new ThetisDataContext();
        public AitisiDetail()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
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

        }
    }
}
