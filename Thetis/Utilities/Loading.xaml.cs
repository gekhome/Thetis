using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Thetis.Utilities
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {

        public Loading()
        {
            InitializeComponent();
            this.Closed += new EventHandler(Loading_Closed);
            //progressBar = new ProgressBar();
            //Closed += Loading_Closed;

        }

        void Loading_Closed(object sender, EventArgs e)
        {
            this.Closed -= new EventHandler(Loading_Closed);
            this.Dispatcher.InvokeShutdown();

            //Closed -= Loading_Closed;
            //Dispatcher.InvokeShutdown();
        }

        public void CloseForm()
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(new Action(CloseForm));
            }
            else
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //double screenHeight = SystemParameters.PrimaryScreenHeight;
            //double screenWidth = SystemParameters.PrimaryScreenWidth;
            //this.Left = Convert.ToDouble(screenWidth / 2.0 - 50.0);
            //this.Top = Convert.ToDouble(screenHeight / 1.0 - 40.0);
        }
    }
}
