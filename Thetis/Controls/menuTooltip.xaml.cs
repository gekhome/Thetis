using System.Windows;
using System.Windows.Controls;

namespace Thetis.Controls
{
    public partial class menuTooltip : UserControl
    {
        public menuTooltip()
        {
            InitializeComponent();

        }

        public string ContentText
        { 
            get {return this.messageText.Text;}
            set { this.messageText.Text = value; }
        }

        private void tipControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}