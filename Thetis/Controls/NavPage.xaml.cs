using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Thetis.Controls
{
    /// <summary>
    /// Interaction logic for NavPage.xaml
    /// </summary>
    public partial class NavPage : UserControl
    {
        public NavPage()
        {
            InitializeComponent();
        }
        private void navButton_Click(object sender, RoutedEventArgs e)
        {
            Image btn = (Image)e.Source;
            string uri = "";
            NavigationService svc = NavigationService.GetNavigationService(btn);
            switch (btn.Name)
            {
                case "btnPrevious":
                    //Έγινε επιλογή πλοήγησης στην προηγούμενη σελίδα.
                    if (svc != null)
                    {
                        svc.GoBack();
                    }
                    break;
                case "btnHome":
                    //Έγινε επιλογή πλοήγησης στην αρχική σελίδα.
                    uri = "Shell/HomePage.xaml";
                    if (svc != null)
                    {
                        svc.Navigate(new Uri(uri, UriKind.Relative));
                    }
                    break;
                default:
                    break;
            } //end switch
   
        }
    }
}
