using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using MessageBoxUtils;
using Prototype.Model;
using Prototype.Utilities;
using Prototype.DataSources.thetisDBDataSetTableAdapters;


namespace Prototype.AppPages.Teachers
{
    /// <summary>
    /// Interaction logic for TeacherCard.xaml
    /// </summary>
    public partial class TeacherDetail : UserControl
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ObservableCollection<ΦΥΛΑ> oc = new ObservableCollection<ΦΥΛΑ>();
        public TeacherDetail()
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
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Prototype;component/Shell/Images/person_male.png"));
            }
            else if (cbosex.SelectedIndex == 1)
            {
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Prototype;component/Shell/Images/person_female.png"));
            }
            else
            {
                SexPhoto.Source = new BitmapImage(new Uri(@"pack://application:,,,/Prototype;component/Shell/Images/person_unknown.png"));
            }
        }


    }
}
