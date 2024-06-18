using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thetis.Utilities;
using Thetis.Model;

namespace Thetis.Shell
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        static Loading loadingWin;
        static bool isLoadingWinCreated;

        #region Page Constructors

        public HomePage()
        {
            InitializeComponent();
            // we need this in the default constructor, otherwise
            // user is not shown when navigating back and forth to this page.
            txtLoginID.Text = LoginClass.Userid;
            GetUserInfo();
            //changeBackground("pack://application:,,,/Thetis;component/Shell/Wallpapers/windows8.jpg");
        }

        public HomePage(string inputString)
        {
            InitializeComponent();
            //changeBackground("pack://application:,,,/Thetis;component/Shell/Wallpapers/windows8.jpg");
            txtLoginID.Text = inputString;
            LoginClass.Userid = inputString; // set the global static variable to the username
            GetUserInfo();
        }

        #endregion


        #region ThemeChanging
        
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            Image currentButton = sender as Image;
            string themeFile = "";

            try
            {
                switch (currentButton.Name)
                {
                    //Blue Button Cases
                    case "btnBlue01":
                        themeFile = "blue01.jpg";
                        break;

                    case "btnBlue02":
                        themeFile = "blue02.jpg";
                        break;

                    case "btnBlue03":
                        themeFile = "blue03.jpg";
                        break;

                    case "btnBlue04":
                        themeFile = "blue04.jpg";
                        break;

                    //Green Button Cases
                    case "btnGreen01":
                        themeFile = "green01.jpg";
                        break;

                    case "btnGreen02":
                        themeFile = "green02.jpg";
                        break;

                    case "btnGreen03":
                        themeFile = "green03.jpg";
                        break;

                    case "btnGreen04":
                        themeFile = "green04.jpg";
                        break;

                    //Grey Button Cases
                    case "btnGrey01":
                        themeFile = "grey01.jpg";
                        break;

                    case "btnGrey02":
                        themeFile = "grey02.jpg";
                        break;

                    case "btnGrey03":
                        themeFile = "grey03.jpg";
                        break;

                    case "btnGrey04":
                        themeFile = "grey04.jpg";
                        break;

                    default:
                        break;
                }

                string path = "pack://application:,,,/Thetis;component/Shell/Wallpapers/" + themeFile;
                changeBackground(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα: " + ex.Message, "Εξαίρεση", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            Image currentButton = sender as Image;
            string themeString = "";

            try
            {
                switch (currentButton.Name)
                {
                    //Blue Button Cases
                    case "imgBlue01":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("33", "69", "115");
                        break;

                    case "imgBlue02":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("0", "105", "154");
                        break;

                    case "imgBlue03":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("119", "148", "165");
                        break;

                    case "imgBlue04":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("136", "170", "215");
                        break;

                    //Green Button Cases
                    case "imgGreen01":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("0", "85", "0");
                        break;

                    case "imgGreen02":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("0", "149", "109");
                        break;

                    case "imgGreen03":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("0", "109", "90");
                        break;

                    case "imgGreen04":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("42", "84", "79");
                        break;

                    //Grey Button Cases
                    case "imgGrey01":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("81", "81", "90");
                        break;

                    case "imgGrey02":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("102", "102", "109");
                        break;

                    case "imgGrey03":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("127", "127", "127");
                        break;

                    case "imgGrey04":
                        themeString = Utilities.UserFunctions.GetHexFromRGB("79", "79", "79");
                        break;

                    default:
                        break;
                }

                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(themeString));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα: " + ex.Message);
            }
        }

        private void changeBackground(string path)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path);
            image.EndInit();
            Background = new ImageBrush(image);
        }
        #endregion


        #region ProgressBar Threading

        private void Page_Initialized(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                ProgressBarShow();

                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (!isLoadingWinCreated) ;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadingWin != null)
            {
                loadingWin.CloseForm();
            }
            Cursor = Cursors.Arrow;
        }

        private static void ProgressBarShow()
        {
            loadingWin = new Loading()
            {
                Left = Convert.ToDouble(SystemParameters.PrimaryScreenWidth / 2.0 - 120.0),
                Top = Convert.ToDouble(SystemParameters.PrimaryScreenHeight - 240.0)
            };

            isLoadingWinCreated = true;
            loadingWin.Show();
        }

        #endregion


        private void GetUserInfo()
        {
            int userkey = LoginClass.UserKey;

            if(userkey == 0)
            {
                txtUserInfo.Text = "ΔΙΑΧΕΙΡΙΣΤΗΣ";
            }
            else
            {
                var iek_info = (from iek in db.ΙΕΚs
                                    where iek.ΙΕΚ_ΚΩΔ == userkey
                                    select new { iek.ΙΕΚ_ΟΝΟΜΑΣΙΑ, iek.ΙΕΚ_ΚΩΔ }).FirstOrDefault();

                    txtUserInfo.Text = iek_info.ΙΕΚ_ΟΝΟΜΑΣΙΑ;
            }

        } // GetUserInfo

    }
}
