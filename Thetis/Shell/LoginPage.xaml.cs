using System.Windows.Controls;
using System.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.Controls;



namespace Thetis.Shell
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private ThetisDataContext db = new ThetisDataContext();
        //static Loading loadingWin = null;
        //static bool isLoadingWinCreated = false;

        public LoginPage()
        {
            InitializeComponent();
            txtUserName.Focus();
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Write code here to authenticate user
            // This will check the database to validate the user. This version is for admin access
            HideValidationImages();
            int error_code = ValidateUser(txtUserName.Text, txtPassword.Password);

            switch (error_code)
            {
                case 0: // username and password correct
                    imgUsrOK.Visibility = Visibility.Visible;
                    imgPwdOK.Visibility = Visibility.Visible;
                    NavigateToHomePage();
                    break;

                case 1: // username and password empty
                    imgUsrERR.Visibility = Visibility.Visible;
                    imgPwdERR.Visibility = Visibility.Visible;
                    txtUserName.Text = "";
                    txtPassword.Password = "";
                    txtUserName.Focus();
                    break;

                case 2: // username empty
                    imgUsrERR.Visibility = Visibility.Visible;
                    txtUserName.Text = "";
                    txtUserName.Focus();
                    break;

                case 3: // password empty
                    imgPwdERR.Visibility = Visibility.Visible;
                    txtPassword.Password = "";
                    txtPassword.Focus();
                    break;

                case 4: // incorrect username and correct password
                    imgUsrERR.Visibility = Visibility.Visible;
                    imgPwdOK.Visibility = Visibility.Visible;
                    txtUserName.Text = "";
                    txtUserName.Focus();
                    break;

                case 5: // incorrect username and password
                    imgUsrERR.Visibility = Visibility.Visible;
                    imgPwdERR.Visibility = Visibility.Visible;
                    txtUserName.Text = "";
                    txtPassword.Password = "";
                    txtUserName.Focus();
                    break;

                case 6: // correct username and incorrect password
                    imgUsrOK.Visibility = Visibility.Visible;
                    imgPwdERR.Visibility = Visibility.Visible;
                    txtPassword.Password = "";
                    txtPassword.Focus();
                    break;
                case -1:    // unable to connect to server (timeout)
                    Application.Current.Shutdown();
                    break;

                default:
                    break;

            }

        } // btnLogin_Click

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region User Validation


        private void HideValidationImages()
        {
            imgUsrOK.Visibility = Visibility.Collapsed;
            imgUsrERR.Visibility = Visibility.Collapsed;
            imgPwdOK.Visibility = Visibility.Collapsed;
            imgPwdERR.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Validates user login from database, table USERS
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        private int ValidateUser(string username, string password)
        {
            // first check if both passed parameters are empty
            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password)) return 1;
            // check username empty and password not empty
            if (string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)) return 2;
            // check username not empty and password empty
            if (!string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password)) return 3;

            // user entered both username and password, so safe to LINQ it
            try
            {
                var loginInfo = (from u in db.USERs
                                 where u.USER_TYPE == username && u.USER_PASSWORD == password
                                 select u).Count();
                // both username and password correct
                if (loginInfo != 0) return 0;
                else
                {
                    // if username is correct
                    if (!UsernameValidated(username) && PasswordValidated(password)) return 4; // incorrect username and password correct
                    else if (!UsernameValidated(username) && !PasswordValidated(password)) return 5; // both incorrect
                    else // correct username
                    {
                        // check if password is correct
                        if (!PasswordValidated(password)) return 6; // incorrect password
                        else return 0; //correct password (at this point both username and password correct)
                    }
                } // else
            }
            catch (Exception ex)
            {
                string ex_message = "Δεν κατέστη δυνατή η σύνδεση με τον Διακομιστή.\n";
                ex_message += "Παρακαλώ δοκιμάστε αργότερα. Η εφαρμογή θα κλείσει.";
                UserFunctions.ShowAdminMessage(ex_message);
                UserFunctions.RadWindowAlert(ex.Message);
                return -1;
            }


        } // ValidateUser

        private bool PasswordValidated(string password)
        {
            var pass = (from p in db.USERs
                        where p.USER_PASSWORD == password
                        select p).Count();

            if (pass != 0) return true;
            else return false;

        }

        private bool UsernameValidated(string username)
        {
            var user = (from u in db.USERs
                        where u.USER_TYPE == username
                        select u).Count();

            if (user != 0) return true;
            else return false;

        }

        #endregion

        private void NavigateToHomePage()
        {
            HomePage p1 = new HomePage(txtUserName.Text);
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(p1);

        }

        #region Loading

        private void Page_Initialized(object sender, EventArgs e)
        {
            //Thread thread = new Thread(() =>
            //{
            //    loadingWin = new Loading();
            //    isLoadingWinCreated = true;
            //    loadingWin.Show();
            //    System.Windows.Threading.Dispatcher.Run();
            //});
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();

            //while (!isLoadingWinCreated) ;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //if (loadingWin != null)
            //{
            //    loadingWin.CloseForm();
            //}
            //Cursor = Cursors.Arrow;
        }

        #endregion
    }
}
