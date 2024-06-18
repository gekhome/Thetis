using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Thetis.Model;


namespace Thetis.AppPages.Auxiliary.Teachers
{
    /// <summary>
    /// Interaction logic for TeacherCard.xaml
    /// </summary>
    public partial class TeacherCard : UserControl
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ObservableCollection<ΦΥΛΑ> oc = new ObservableCollection<ΦΥΛΑ>();
        public TeacherCard()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            // data source for the combo
            var sex = from s in db.ΦΥΛΑs
                        orderby s.ΚΩΔ_ΦΥΛΟ
                        select s;
            var ocsex = new ObservableCollection<ΦΥΛΑ>(sex.ToList());
            cbosex.ItemsSource = ocsex;

            changeSexPhoto();
            
        }

        private void cbosex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSexPhoto();
        }

        private void changeSexPhoto()
        {
            if (cbosex.SelectedIndex == 0)
            {
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Thetis;component/Shell/Images/person_male.png"));
            }
            else if (cbosex.SelectedIndex == 1)
            {
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Thetis;component/Shell/Images/person_female.png"));
            }
            else
            {
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Thetis;component/Shell/Images/person_unknown.png"));
            }
        }

    }
}
