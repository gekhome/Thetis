using System.Windows;
using Telerik.Windows.Controls;

namespace Thetis.Controls
{
    /// <summary>
    /// Interaction logic for RadMsgWindow.xaml
    /// </summary>
    public partial class RadMsgWindow : RadWindow
    {
        public RadMsgWindow()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return DisplayMessage.Text; }
            set { DisplayMessage.Text = value; }
        }


        private void OnButtonAcceptClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void OnButtonCancelClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
