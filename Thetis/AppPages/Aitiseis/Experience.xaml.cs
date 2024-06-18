using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using Thetis.Utilities;
using Thetis.DataAccess;
using Thetis.Model;
using Thetis.Controls;

using Telerik.Windows.Controls;

namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for Experience.xaml
    /// </summary>
    public partial class Experience : Page
    {
        Loading loadingWin = null;
        bool isLoadingWinCreated = false;
        private Primus p = new Primus();
        private ThetisDataContext db = new ThetisDataContext();

        public Experience()
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
            lblMenu.FontSize = 14;
            menutip.Visibility = Visibility.Visible;
            switch (lblMenu.Name)
            {
                case "exp01":
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή, καταχώρηση " +
                                          "των διδακτικών προϋπηρεσιών που αφορούν την " +
                                          "επιλεγμένη αίτηση του υποψηφίου.";
                    break;
                case "exp02":
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή, καταχώρηση " +
                                          "των επαγγελματικών προϋπηρεσιών που αφορούν την " +
                                          "επιλεγμένη αίτηση του υποψηφίου.";
                    break;
                case "exp03":
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή, καταχώρηση " +
                                          "των προϋπηρεσιών από ελεύθερο επάγγελμα που αφορούν " +
                                          "την επιλεγμένη αίτηση του υποψηφίου.";
                    break;
                case "exp04":
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή και καταχώρηση " +
                                          "των ΙΕΚ, του ίδιου νομού, στα οποία επιθυμεί " +
                                          "αξιολόγηση ο υποψήφιος, για την ίδια ειδικότητα.";
                    break;
                
                case "exp05":
                    menutip.ContentText = "Μεταφέρει προηγούμενες προϋπηρεσίες (αυτές που δεν " +
                                          "έχουν ήδη μεταφερθεί) σε αυτές της τρέχουσας αίτησης." +
                                          "Δηλ. Διδακτική, Επαγγελματική και από Ελ. Επάγγελμα";
                    break;

                default:
                    break;
            }

        } //menuTipShow

        private void menuTipHide(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;
            lblMenu.FontSize = 12;
            menutip.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region TabNavigation

        private void panelLabel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;

            switch (lblMenu.Name)
            {
                case "exp01":
                    // άνοιγμα του tab με τον πίνακα του μητρώου εκπαιδευτικών.
                    TabOpen(tabItemExTask1);
                    break;
                case "exp02":
                    // άνοιγμα του tab με τους κλάδους και ειδικότητες των εκπαιδευτικών.
                    TabOpen(tabItemExTask2);
                    break;
                case "exp03":
                    // άνοιγμα του tab με τους κλάδους και ειδικότητες των εκπαιδευτικών.
                    TabOpen(tabItemExTask3);
                    break;
                case "exp04":
                    // άνοιγμα του tab με τις βαθμίδες σπουδών.
                    TabOpen(tabItemExTask4);
                    break;
                
                default:
                    break;
            } //end switch

        } // panelSystem.Windows.Controls.Label_Click

        private void TabOpen(ClosableTabItem tabItem)
        {
            ((UIElement)tabItem.Content).Visibility = Visibility.Visible; // show its contents
            tabItem.Visibility = Visibility.Visible; // show the tab itself
            tabItem.IsSelected = true; // select it
        }

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Close the indicator window and shut down the STA thread with its dispatcher
            if (loadingWin != null) loadingWin.CloseForm();
            //busyIndicator.IsBusy = false;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                loadingWin = new Loading();
                isLoadingWinCreated = true;
                loadingWin.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (!isLoadingWinCreated) ;
        }

        /* -------------------------
         * Με το συμβάν αυτό μεταφέρονται για την τρέχουσα Αίτηση όλες
         * οι αντίστοιχες προϋπηρεσίες προηγούμενων προκηρύξεων.
         * Καλεί την μέθοδο TransferAllBatch() της κλάσης Primus.
         * Date: 12-02-2013
         * Author: Lefteris
         * -------------------------
         */
        private void transferLabel_Click(object sender, MouseButtonEventArgs e)
        {
            string info_msg;

            info_msg = "Θα γίνει μεταφορά προηγούμενων προϋπηρεσιών ";
            info_msg += "διδακτικής, επαγγελματικής, ελ. επαγγέλματος.\n";
            info_msg += "Ανάλογα με το πλήθος προϋπηρεσιών η διαδικασία ";
            info_msg += "μπορεί να διαρκέσει λίγα λεπτά.\n";
            info_msg += "Η μεταφορά θα γινει για το ίδιο ΙΕΚ και ειδικότητα.";
            UserFunctions.ShowAdminMessage(info_msg);

            // do the transfer
            p.TransferAllBatch(db);

            string done_msg = "Η διαδικασία μεταφοράς ολοκληρώθηκε.";
            UserFunctions.ShowAdminMessage(done_msg);
        }

    }
}
