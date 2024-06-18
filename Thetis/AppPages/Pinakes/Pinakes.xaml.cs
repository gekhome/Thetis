using System;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Windows.Controls;
using Thetis.Controls;
using Thetis.Utilities;
using Telerik.Windows.Controls;

namespace Thetis.AppPages.Pinakes
{
    /// <summary>
    /// Interaction logic for Pinakes.xaml
    /// </summary>
    public partial class Pinakes : Page
    {
        static Loading loadingWin;
        static bool isLoadingWinCreated;
        
        public Pinakes()
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


        // keep it for possible change in the event handler
        private void Item_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
        }

        #region TreeViewItem Events

        private void item01_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask1);
        }

        private void item02_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask2);
        }

        private void item03_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask3);
        }

        private void item04_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask4);
        }

        private void item05_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask5);
        }

        private void item06_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask6);
        }

        private void item07_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask7);
        }

        private void item08_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask8);
        }

        private void item09_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask9);
        }

        private void item10_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask10);
        }

        private void item11_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask11);
        }

        private void item12_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask12);
        }

        private void item13_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask13);
        }

        private void item14_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask14);
        }

        private void item15_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TabOpen(tabItemExTask15);
        }

        private void TabOpen(ClosableTabItem tabItem)
        {
            ((UIElement)tabItem.Content).Visibility = Visibility.Visible; // show its contents
            tabItem.Visibility = Visibility.Visible; // show the tab itself
            tabItem.IsSelected = true; // select it
        }

        #endregion TreeViewItem Events


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
