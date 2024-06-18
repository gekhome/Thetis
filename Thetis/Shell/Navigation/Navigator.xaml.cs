using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Media.Effects;
using Thetis.DataAccess;
using Thetis.Controls;
using Thetis.Utilities;


namespace Thetis.Shell.Navigation
{
    /// <summary>
    /// Interaction logic for Navigator.xaml
    /// </summary>
    public partial class Navigator : UserControl
    {
        #region Data
        readonly Thetis.Controls.GlassPane _glassPane = new Thetis.Controls.GlassPane();
        #endregion

        public static Brush defaultButtonBrush;
        public static Brush redButtonBrush;
        public static Brush greenButtonBrush;
        public static Brush yellowButtonBrush;
        public static Brush blueButtonBrush;
        public static Brush tealButtonBrush;
        public static Brush darkmagentaButtonBrush;

        private CommitModel cm = new CommitModel();

        public int userkey;

        public Navigator()
        {
            BrushConverter bc = new BrushConverter();
            defaultButtonBrush = (Brush)bc.ConvertFrom("#3C1235");
            redButtonBrush = (Brush)bc.ConvertFrom("#FF2E12");
            yellowButtonBrush = (Brush)bc.ConvertFrom("#FFBB00");
            greenButtonBrush = (Brush)bc.ConvertFrom("#7CBB00");
            blueButtonBrush = (Brush)bc.ConvertFrom("#3947DE");
            tealButtonBrush = (Brush)bc.ConvertFrom("#008080");
            darkmagentaButtonBrush = (Brush)bc.ConvertFrom("#8B008B");

            InitializeComponent();
        }


        #region Tile Events

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            userkey = LoginClass.UserKey;

            Border image = (Border)sender;
            string uri = "";
            switch (image.Name)
            {
                case "menu1":
                    //Έγινε επιλογή πλοήγησης στην σελίδα Μοριοποίησης.
                    uri = "AppPages/Moriodotisi/MoriaMenu.xaml";
                    break;
                case "menu2":
                    //Έγινε επιλογή πλοήγησης στην σελίδα Εκτυπώσεων.
                    uri = "AppPages/Pinakes/Pinakes.xaml";
                    break;
                case "menu3":
                    //Έγινε επιλογή πλοήγησης στην σελίδα Προκηρύξεων.
                    cm.AdminMessage();
                    uri = "AppPages/Prokirixis/Prokirixi.xaml";
                    break;
                case "menu4":
                    //Έγινε επιλογή πλοήγησης στην σελίδα Ρυθμίσεων.
                    cm.AdminMessage();
                    uri = "AppPages/Auxiliary/Auxiliary.xaml";
                    break;
                case "menu5":
                    //Έγινε επιλογή πλοήγησης στην σελίδα Διαχείρισης.
                    if (cm.PermitGrant() == true)
                        uri = "AppPages/Admin/Admin.xaml";
                    else
                        uri = "";
                    break;
                case "menu6":
                    //Έγινε επιλογή πλοήγησης στην σελίδα Στατιστικών.
                    uri = "AppPages/Statistics/Statistics.xaml";
                    break;


                default:
                    break;
            } //end switch
            if (uri != "")
            {
                NavigationService svc = NavigationService.GetNavigationService(image);
                if (svc != null)
                {
                    svc.Navigate(new Uri(uri, UriKind.Relative));

                }
            }

        }

        private static void ResetBorder(Border theBorder)
        {
            theBorder.BorderThickness = new Thickness(2, 2, 2, 2);
            theBorder.BorderBrush = Brushes.Transparent;
            BrushConverter bc = new BrushConverter();
            theBorder.Background = defaultButtonBrush;

            //theBorder.Height = theBorder.Height + 10;
            //theBorder.Width = theBorder.Width + 10;

        }

        private static void BorderClickEffect(Border theBorder)
        {
            theBorder.BorderThickness = new Thickness(4, 4, 4, 4);
            theBorder.BorderBrush = Brushes.White;
            //theBorder.Height = theBorder.Height - 10;
            //theBorder.Width = theBorder.Width - 10;
        }


        #endregion


        #region Tile Color Effects

        private void menu1_MouseEnter(object sender, MouseEventArgs e)
        {
            BorderClickEffect(menu1);
        }

        private void menu1_MouseLeave(object sender, MouseEventArgs e)
        {
            //menu1.Background = defaultButtonBrush;
            ResetBorder(menu1);
        }

        private void menu2_MouseEnter(object sender, MouseEventArgs e)
        {
            //menu2.Background = tealButtonBrush;
            BorderClickEffect(menu2);
        }

        private void menu2_MouseLeave(object sender, MouseEventArgs e)
        {
            //menu2.Background = defaultButtonBrush;
            ResetBorder(menu2);
        }

        private void menu3_MouseEnter(object sender, MouseEventArgs e)
        {
            //menu3.Background = darkmagentaButtonBrush;
            BorderClickEffect(menu3);
        }

        private void menu3_MouseLeave(object sender, MouseEventArgs e)
        {
            //menu3.Background = defaultButtonBrush;
            ResetBorder(menu3);
        }

        private void menu4_MouseEnter(object sender, MouseEventArgs e)
        {
            //menu4.Background = redButtonBrush;
            BorderClickEffect(menu4);
        }

        private void menu4_MouseLeave(object sender, MouseEventArgs e)
        {
            //menu4.Background = defaultButtonBrush;
            ResetBorder(menu4);
        }

        private void menu5_MouseEnter(object sender, MouseEventArgs e)
        {
            //menu5.Background = yellowButtonBrush;
            BorderClickEffect(menu5);
        }

        private void menu5_MouseLeave(object sender, MouseEventArgs e)
        {
            //menu5.Background = defaultButtonBrush;
            ResetBorder(menu5);
        }

        private void menu6_MouseEnter(object sender, MouseEventArgs e)
        {
            //menu6.Background = greenButtonBrush;
            BorderClickEffect(menu6);
        }

        private void menu6_MouseLeave(object sender, MouseEventArgs e)
        {
            menu6.Background = defaultButtonBrush;
            ResetBorder(menu6);
        }

        #endregion

        #region Tile Position Effects

        public Point GetLocation(Border element)
        {
            var location = element.PointToScreen(new Point(0, 0));
            string msg = "Position is: " + location.X + ", " + location.Y;
            //MessageBox.Show(msg);
            return location;
        }

        public void SetLocation(Border element)
        {
            var location = element.PointToScreen(new Point(0, 0));
            string msg = "Position is: " + location.X + ", " + location.Y;

            element.Margin = new Thickness(30);
        }

        #endregion

    }   //class Navigator
}