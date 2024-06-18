using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading;
using Thetis.Utilities;
using Thetis.DataAccess;
using Thetis.Controls;
using Telerik.Windows.Controls;


namespace Thetis.AppPages.Auxiliary
{
    /// <summary>
    /// Interaction logic for Auxiliary.xaml
    /// </summary>
    public partial class Auxiliary : Page
    {
        static Loading loadingWin;
        static bool isLoadingWinCreated;

        private CommitModel cm = new CommitModel();
          
        public Auxiliary()
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
            lblMenu.FontSize = 20;
            menutip.Visibility = Visibility.Visible;
            switch (lblMenu.Name)
            {
                case "aux01":
                    menutip.ContentText = "Με την επιλογή αυτή εμφανίζεται πίνακας του Μητρώου " +
                                          "εκπαιδευτικών για ενέργειες όπως αναζήτηση, προβολή " +
                                          "των στοιχείων τους καθώς και των αιτήσεών τους.";
                    break;
                case "aux02":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τους κλάδους " +
                                          "και ειδικότητες των εκπαιδευτικών. Μπορεί να γίνει " +
                                          "διόρθωση, προσθήκη και διαγραφή στοιχείων.";
                    break;
                case "aux03":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τις " +
                                          "ειδικότητες των εκπαιδευτικών, προκειμένου να " +
                                          "γίνει διόρθωση, προσθήκη και διαγραφή στοιχείων.";
                    break;
                case "aux04":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλονται οι βαθμίδες σπουδών " +
                                          "σχετικές με τους εκπαιδευτικούς. Μπορούν να γίνουν "   +
                                          "προσθήκες, μεταβολές και διαγραφές των στοιχείων.";
                    break;
                case "aux05":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τα μόρια που " +
                                          "αντιστοιχούν σε ελεύθερο επάγγελμα των εκπαιδευτικών.  " +
                                          "Μπορούν να γίνουν μεταβολές των στοιχείων του πίνακα.";
                    break;
                case "aux06":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τα κείμενα " +
                                          "που χαρακτηρίζουν την αιτιολογία αποκλεισμού από τη " + 
                                          "διαδικασία μοριοποίησης.";
                    break;
                case "aux07":
                    menutip.ContentText = "Προβολή και επεξεργασία των προκηρυσσόμενων ειδικοτήτων " +
                                          "σε κάθε ΙΕΚ και ανά Προκήρυξη.";
                    break;
                case "aux08":
                    menutip.ContentText = "Προβολή και επεξεργασία των στοιχείων των ΙΕΚ, όπως π.χ." +
                                          "διεύθυνση, τηλέφωνα, email, διευθυντής κλπ.";
                    break;
                case "aux09":
                    menutip.ContentText = "Προβολή και επεξεργασία των στοιχείων σχολικών ετών, " +
                                          "όπως π.χ. 2011-2012, ημερομηνίες έναρξης και λήξης.";
                    break;
                case "aux10":
                    menutip.ContentText = "Με την επιλογή αυτή εμφανίζονται οι πίνακες για " +
                                          "κάθε εκπαιδευτικό, που αφορούν τις προϋπηρεσίες " +
                                          "για κάθε αίτηση.";
                    break;

                default:
                    break;
            }

        } //menuTipShow

        private void menuTipHide(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;
            lblMenu.FontSize = 16;
            menutip.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region TabNavigation

        private void panelLabel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;
            
            switch (lblMenu.Name)
            {
                
                case "aux02":
                    // άνοιγμα του tab με τους κλάδους και ειδικότητες των εκπαιδευτικών.
                    TabOpen(tabItemExTask2);
                    break;
                case "aux03":
                    // άνοιγμα του tab με τους κλάδους και ειδικότητες των εκπαιδευτικών.
                    TabOpen(tabItemExTask3);
                    break;
                case "aux04":
                    // άνοιγμα του tab με τις βαθμίδες σπουδών.
                    TabOpen(tabItemExTask4);
                    break;
                case "aux05":
                    // άνοιγμα του tab με τα αφορολόγητα εισοδήματα ελ. επαγγελματιών.
                    TabOpen(tabItemExTask5);
                    break;
                case "aux06":
                    // άνοιγμα του tab με τις αιτιολογίες αποκλεισμού.
                    TabOpen(tabItemExTask6);
                    break;
                case "aux07":
                    // άνοιγμα του tab με τις προκηρυσσόμενες ειδικότητες ανά ΙΕΚ.
                    TabOpen(tabItemExTask7);
                    break;
                case "aux08":
                    // άνοιγμα του tab με τα στοιχεία των ΙΕΚ.
                    TabOpen(tabItemExTask8);
                    break;
                case "aux09":
                    // άνοιγμα του tab με τα στοιχεία των ΙΕΚ.
                    TabOpen(tabItemExTask9);
                    break;
                
                default:
                    break;
            } //end switch
            
        } // panelLabel_Click

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

    }
}
