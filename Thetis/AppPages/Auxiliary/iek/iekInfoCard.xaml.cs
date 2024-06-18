using System.Linq;
using System.Windows.Controls;
using Thetis.Model;

namespace Thetis.AppPages.Auxiliary.iek
{
    /// <summary>
    /// Interaction logic for iekInfoCard.xaml
    /// </summary>
    public partial class iekInfoCard : UserControl
    {
        private ThetisDataContext db = new ThetisDataContext();
        public iekInfoCard()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            // data sources for the combos
            //var nomoi = from n in db.ΝΟΜΟΣs
            //            orderby n.ΝΟΜΟΣ1
            //            select n;
            var periferiakes = from p in db.ΠΕΡΙΦΕΡΕΙΑΚΗs
                               orderby p.ΠΕΡΙΦΕΡΕΙΑΚΗ1
                               select p;

            //cboNomos.ItemsSource = nomoi.ToList();
            cboPeriferiaki.ItemsSource = periferiakes.ToList();
        }
    }
}
