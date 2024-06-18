using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Thetis.Model;


namespace Thetis.AppPages.Trainers
{
    /// <summary>
    /// Interaction logic for TeacherCard.xaml
    /// </summary>
    public partial class TrainerForm : UserControl
    {
        private ThetisDataContext db = new ThetisDataContext();
      
        public TrainerForm()
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
            
            cbosex.ItemsSource = sex.ToList();
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
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Thetis;component/Shell/Images/Other/person_male.png"));
            }
            else if (cbosex.SelectedIndex == 1)
            {
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Thetis;component/Shell/Images/Other/person_female.png"));
            }
            else
            {
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Thetis;component/Shell/Images/Other/person_unknown.png"));
            }
        }


    }
}
