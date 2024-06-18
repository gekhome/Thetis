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


namespace Thetis.Controls
{
    /// <summary>
    /// Interaction logic for W8Message.xaml
    /// </summary>
    public partial class W8Message : Window
    {
        public W8Message()
        {
            InitializeComponent();
            DisplayMessage.Text = "";
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public string Message
        {
            get { return DisplayMessage.Text; }
            set { DisplayMessage.Text = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            this.Left = Convert.ToDouble(0);
            this.Top = Convert.ToDouble(screenHeight / 2.0 - 90.0);
            this.Width = screenWidth;
            this.Height = 200;

        }

    }
}
