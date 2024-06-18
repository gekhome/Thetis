using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Thetis.Controls;
using Thetis.Utilities;
using Thetis.DataAccess;
using Thetis.Model;

using Telerik.Windows.Controls;


namespace Thetis.AppPages.Statistics
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        static Loading loadingWin;
        static bool isLoadingWinCreated;

        private ThetisDataContext db = new ThetisDataContext();

        public Statistics()
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

        private void TabOpen(ClosableTabItem tabItem)
        {
            ((UIElement)tabItem.Content).Visibility = Visibility.Visible; // show its contents
            tabItem.Visibility = Visibility.Visible; // show the tab itself
            tabItem.IsSelected = true; // select it
        }

        #region menuTipEvents

        private void menuTipShow(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Label lblMenu = (System.Windows.Controls.Label)e.Source;
            lblMenu.FontSize = 16;
            menutip.Visibility = Visibility.Visible;
            switch (lblMenu.Name)
            {
                case "task01":  // Αιτήσεις ανά κλάδο
                    menutip.ContentText = "Εμφανίζει το πλήθος και ποσοστό των αιτήσεων, " +
                                          "επί του συνόλου, των εκπαιδευτικών για επιλεγμένη " +
                                          "προκήρυξη ή/και ΙΕΚ.";
                    break;
                case "task02":  // Αιτήσεις ΠΕ ανά τίτλους σπουδών
                    menutip.ContentText = "Εμφανίζει το πλήθος και ποσοστό των αιτήσεων, " +
                                          "επί του συνόλου, των εκπαιδευτικών ΠΕ για επιλεγμένη " +
                                          "προκήρυξη ή/και ΙΕΚ, με διάκριση τους τίτλους σπουδών.";
                    break;
                case "task03":  // Αιτήσεις ανά απασχόληση
                    menutip.ContentText = "Εμφανίζει το πλήθος και ποσοστό των αιτήσεων, " +
                                          "επί του συνόλου, των εκπαιδευτικών ως προς το " +
                                          "είδος απασχόλησης (ιδιώτες, δημ.υπ.+συντ., άνεργοι.";
                    break;
                case "task04":  // Αιτήσεις ανά φύλο
                    menutip.ContentText = "Εμφανίζει το πλήθος και ποσοστό των εκπαιδευτικών " +
                                          "ως προς το φύλο και ανά κλάδο εκπαιδευτικού.";
                    break;
                case "task05":  // Αποκλεισμοί
                    menutip.ContentText = "Εμφανίζει το πλήθος και ποσοστό των αιτήσεων, " + 
                                          "επί του συνόλου, που αποκλείονται ως προς τον " +
                                          "κλάδο των εκπαιδευτικών.";
                    break;
                case "task06":  // Ενστάσεις
                    menutip.ContentText = "Εμφανίζει το πλήθος και ποσοστό των εκπαιδευτικών " +
                                          "που υπέβαλαν ένσταση ανά κλάδο εκπαιδευτικών.";
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
                    TabOpen(tabItemTask1);
                    break;

                case "task02":
                    TabOpen(tabItemTask2);
                    break;

                case "task03":
                    TabOpen(tabItemTask3);
                    break;
                case "task04":
                    TabOpen(tabItemTask4);
                    break;
                case "task05":
                    TabOpen(tabItemTask5);
                    break;
                case "task06":
                    TabOpen(tabItemTask6);
                    break;
                
                default:
                    break;
            } // switch
        } // LoadTab

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
