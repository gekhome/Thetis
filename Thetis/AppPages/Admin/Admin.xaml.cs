using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using Thetis.Utilities;
using Thetis.DataAccess;
using Thetis.Controls;
using Telerik.Windows.Controls;


namespace Thetis.AppPages.Admin
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        static Loading loadingWin;
        static bool isLoadingWinCreated;

        private CommitModel cm = new CommitModel();

        public Admin()
        {
            InitializeComponent();
            EventManager.RegisterClassHandler(typeof(RadTabItem), RoutedEventHelper.CloseTabEvent, new RoutedEventHandler(OnCloseClicked));
        }

        public void OnCloseClicked(object sender, RoutedEventArgs args)
        {
            ClosableTabItem tabItem = (ClosableTabItem)args.Source; // get chosen tab
            ((UIElement)tabItem.Content).Visibility = Visibility.Collapsed; // collapse tab contents
            tabItem.Visibility = Visibility.Collapsed; // collapse tab

            //tabItem = sender as RadTabItem;
            //// Remove the item from the collection the control is bound to
            //tabItem.Visibility = Visibility.Collapsed;
            //tabItem.Content = null;
        }


        #region menuTipEvents

        private void menuTipShow(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;
            lblMenu.FontSize = 18;
            menutip.Visibility = Visibility.Visible;
            switch (lblMenu.Name)
            {
                case "adm01":
                    menutip.ContentText = "Με την επιλογή αυτή εμφανίζεται πίνακας των Αιτήσεων " +
                                          "εκπαιδευτικών για ενέργειες όπως αναζήτηση, προβολή " +
                                          "και μεταβολή των στοιχείων τους.";
                    break;
                case "adm02":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τα στοιχεία " +
                                          "των εκπαιδευτικών. Μπορεί να γίνει " +
                                          "διόρθωση, προσθήκη και διαγραφή στοιχείων.";
                    break;
                case "adm03":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τους " +
                                          "κλάδους των εκπαιδευτικών, προκειμένου να " +
                                          "γίνει διόρθωση, προσθήκη και διαγραφή στοιχείων.";
                    break;
                case "adm04":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλονται οι ειδικότητες " +
                                          "σχετικές με τους εκπαιδευτικούς. Μπορούν να γίνουν " +
                                          "προσθήκες, μεταβολές και διαγραφές των στοιχείων.";
                    break;
                case "adm05":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τις " +
                                          "αιτιολογίες αποκλεισμού. " +
                                          "Μπορούν να γίνουν μεταβολές των στοιχείων του πίνακα.";
                    break;
                
                default:
                    break;
            }

        } //menuTipShow

        private void menuTipHide(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;
            lblMenu.FontSize = 14;
            menutip.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region TabNavigation

        private void panelLabel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;

            switch (lblMenu.Name)
            {
                case "adm01":
                    // άνοιγμα του tab με αιτήσεις των εκπαιδευτικών.
                    LoadTab(lblMenu);
                    break;
                case "adm02":
                    // άνοιγμα του tab με τα στοιχεία των εκπαιδευτικών.
                    LoadTab(lblMenu);
                    break;
                case "adm03":
                    // άνοιγμα του tab με κλάδους των εκπαιδευτικών.
                    LoadTab(lblMenu);
                    break;
                case "adm04":
                    // άνοιγμα του tab με τις ειδικότητες των εκπαιδευτικών.
                    LoadTab(lblMenu);
                    break;
                case "adm05":
                    // άνοιγμα του tab με τις αιτιολογίες αποκλεισμού.
                    LoadTab(lblMenu);
                    break;

                default:
                    break;
            } //end switch

        } // panelLabel_Click

        private void LoadTab(System.Windows.Controls.Label menuChoice)
        {
            switch (menuChoice.Name)
            {
                case "adm01":
                    TabOpen(tabItemExTask1);
                    break;
                case "adm02":
                    TabOpen(tabItemExTask2);
                    break;
                case "adm03":
                    TabOpen(tabItemExTask3);
                    break;
                case "adm04":
                   TabOpen(tabItemExTask4);
                    break;
                case "adm05":
                    TabOpen(tabItemExTask5);
                    break;

                default:
                    break;
            } // switch
        } // LoadTab

        private void TabOpen(ClosableTabItem tabItem)
        {
            ((UIElement)tabItem.Content).Visibility = Visibility.Visible; // show its contents
            tabItem.Visibility = Visibility.Visible; // show the tab itself
            tabItem.IsSelected = true; // select it
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
    
    } // class Admin
}
