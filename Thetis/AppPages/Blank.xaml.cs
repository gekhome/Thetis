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
using Thetis.Controls;
using Thetis.Utilities;

namespace Thetis.AppPages
{
    /// <summary>
    /// Interaction logic for Blank.xaml
    /// </summary>
    public partial class Blank : Page
    {
        public Blank()
        {
            InitializeComponent();
            //SHowMessageWindow();
        }

        private void SHowMessageWindow()
        {
            bool response = false;

            RadMsgWindow dialogWindow = new RadMsgWindow();
            dialogWindow.Message = "This is test Thank you!";
            dialogWindow.ShowDialog();

            if (dialogWindow.DialogResult == null)
            {
                response = false;
                //UserFunctions.ShowAdminMessage("User simply closed the window");
                return;
            }
            else
            {
                response = (bool)dialogWindow.DialogResult;

                //if (response == true) UserFunctions.ShowAdminMessage("User pressed OK");
                //else if (response == false) UserFunctions.ShowAdminMessage("User pressed Cancel");
            }
        }

    }
}
