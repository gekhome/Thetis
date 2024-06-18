using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading;
using Thetis.Controls;
using Thetis.Utilities;
using Telerik.Windows.Controls;

namespace Thetis.AppPages.Moriodotisi
{
    /// <summary>
    /// Interaction logic for MoriaMenu.xaml
    /// </summary>
    public partial class MoriaMenu : Page
    {
        static Loading loadingWin;
        static bool isLoadingWinCreated;

        public MoriaMenu()
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
            lblMenu.FontSize = 16;
            menutip.Visibility = Visibility.Visible;
            switch (lblMenu.Name)
            {
                case "task01":
                    menutip.ContentText = "Με την επιλογή αυτή εμφανίζεται πίνακας του Μητρώου " +
                                          "εκπαιδευτικών για ενέργειες όπως αναζήτηση, προβολή " +
                                          "και μεταβολή των ατομικών στοιχείων τους.";
                    break;
                case "task02":
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή και καταχώρηση " +
                                          "των αιτήσεων των εκπαιδευτικών για την τρέχουσα ανοικτή " +
                                          "προκήρυξη και το ΙΕΚ που έχει συνδεθεί στο σύστημα.";
                    break;
                case "task03":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλονται οι αιτήσεις των " +
                                          "εκπαιδευτικών καθώς κα τα ατομικά τους στοιχεία για την  " +
                                          "επιλεγμένη προκήρυξη και ΙΕΚ. Η αναζήτηση μπορεί να γίνει " +
                                          "με οποιοδήποτε πεδίο του πίνακα αιτήσεων. ";
                    break;
                case "task04":
                    menutip.ContentText = "Με την επιλογή αυτή προβάλλεται πίνακας με τις " +
                                          "αιτήσεις και προϋπηρεσίες των εκπαιδευτικών, που " +
                                          "έχουν υποβάλει σε όλες τις προκηρύξεις και όλα τα ΙΕΚ.";
                    break;
                case "task05":  // Πίνακας Α
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή, εκτύπωση και " +
                                          "αποθήκευση του πίνακα αξιολόγησης Α για ανάρτηση, " +
                                          "που αφορά τους εργαζόμενους στον ιδιωτικό τομέα.";
                    break;
                case "task06":  // Πίνακας Β
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή, εκτύπωση και " +
                                          "αποθήκευση του πίνακα αξιολόγησης Β για ανάρτηση, " +
                                          "που αφορά τους δημόσιους υπαλλήλους ή συνταξιούχους.";;
                    break;
                case "task07":  // Πίνακας Γ
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή, εκτύπωση και " +
                                          "αποθήκευση του πίνακα αξιολόγησης Γ για ανάρτηση, " +
                                          "που αφορά τους ανέργους χωρίς προϋπηρεσίες.";;
                    break;
                case "task08":  // Πίνακας Δ
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή, εκτύπωση και " +
                                          "αποθήκευση του πίνακα αξιολόγησης Δ για ανάρτηση, " +
                                          "που αφορά τους αποκλειόμενους από τη μοριοποίηση.";;
                    break;
                case "task09":  // Πίνακας Δ
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται προβολή του πίνακα " +
                                          "των υποψηφίων εκπαιδευτικών που έχουν κάνει, " +
                                          "πολλαπλές αιτήσεις."; ;
                    break;
                case "task10":  // Πίνακας Δ
                    menutip.ContentText = "Με την επιλογή αυτή γίνεται εκτύπωση του πίνακα " +
                                          "των υποψηφίων εκπαιδευτικών που έχουν κάνει, " +
                                          "πολλαπλές αιτήσεις."; ;
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
                case "task01":
                    // άνοιγμα του tab με τα στοιχεία των εκπαιδευτικών.
                    LoadTab(lblMenu);
                    break;
                case "task02":
                    // άνοιγμα του tab με τις αιτήσεις των εκπαιδευτικών.
                    LoadTab(lblMenu);
                    break;
                case "task03":
                    // άνοιγμα του tab με την αναζήτηση αιτήσεων.
                    LoadTab(lblMenu);
                    break;
                case "task04":
                    // άνοιγμα του tab με το μητρώο αιτήσεων.
                    LoadTab(lblMenu);
                    break;
                case "task05":
                    // άνοιγμα του tab με την προβολή του Πίνακα Α.
                    LoadTab(lblMenu);
                    break;
                case "task06":
                    // άνοιγμα του tab με την προβολή του Πίνακα Β.
                    LoadTab(lblMenu);
                    break;
                case "task07":
                    // άνοιγμα του tab με την προβολή του Πίνακα Γ.
                    LoadTab(lblMenu);
                    break;
                case "task08":
                    // άνοιγμα του tab με την προβολή του Πίνακα Δ.
                    LoadTab(lblMenu);
                    break;
                case "task09":
                    // άνοιγμα του tab με την προβολή πολλαπλών αιτήσεων.
                    LoadTab(lblMenu);
                    break;
                case "task10":
                    // άνοιγμα του tab με την εκτύπωση πολλαπλών αιτήσεων.
                    LoadTab(lblMenu);
                    break;
                default:
                    break;
            } //end switch

        }

        private void LoadTab(System.Windows.Controls.Label menuChoice)
        {
            switch (menuChoice.Name)
            {
                case "task01":
                    TabOpen(tabItemExTask1);
                    break;

                case "task02":
                    TabOpen(tabItemExTask2);
                    break;

                case "task03":
                    TabOpen(tabItemExTask3);
                    break;
                case "task04":
                    TabOpen(tabItemExTask4);
                    break;
                case "task05":
                    TabOpen(tabItemExTask5);
                    break;
                case "task06":
                    TabOpen(tabItemExTask6);
                    break;
                case "task07":
                    TabOpen(tabItemExTask7);
                    break;
                case "task08":
                    TabOpen(tabItemExTask8);
                    break;
                case "task09":
                    TabOpen(tabItemExTask9);
                    break;
                case "task10":
                    TabOpen(tabItemExTask10);
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



    }
}
