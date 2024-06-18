using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thetis.DataAccess;

namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for AitisiInfo.xaml
    /// </summary>
    public partial class AitisiInfo : Page
    {
        private AitisiModel am = new AitisiModel();

        public AitisiInfo()
        {
            InitializeComponent();
            LoadAitisiInfo();
            LoadTeacherInfo();
            LoadMoriaInfo();
        }

        // loads data of AitisiPanel
        private void LoadAitisiInfo()
        {
            txtProtocol.Text = SelectedAitisi.AitisiProtocol;
            txtDate.Text = SelectedAitisi.AitisiDate;
            txtIek.Text = SelectedAitisi.AitisiIek;
            txtEidikotita.Text = SelectedAitisi.AitisiEidikotita;     
        }

        // loads data of TeacherPanel
        private void LoadTeacherInfo()
        {
            txtFullName.Text = SelectedAitisi.TeacherName;
            txtAFM.Text = SelectedAitisi.TeacherAfm;
            txtAge.Text = SelectedAitisi.TeacherAge;
            txtPhone.Text = SelectedAitisi.TeacherPhone;
            txtSex.Text = SelectedAitisi.TeacherSex;
        }

        // loads data of MoriaPanel
        private void LoadMoriaInfo()
        {
            txtPostgradMoria.Text = SelectedAitisi.PostgradMoria.ToString();
            txtPhdMoria.Text = SelectedAitisi.PhdMoria.ToString();
            txtPaidMoria.Text = SelectedAitisi.PaidagogikoMoria.ToString();
            txtDidaktiki.Text = SelectedAitisi.DidaktikiMoria.ToString();
            txtEpagelmatiki.Text = SelectedAitisi.EpagelmatikiMoria.ToString();
            txtEpagelma.Text = SelectedAitisi.EpagelmaMoria.ToString();
            txtTotalMoria.Text = SelectedAitisi.TotalMoria.ToString();
        }

        /*
         * -------------------
         * Με το συμβάν αυτό γίνονται τα εξής:
         * 1) Τα μόρια από τις προϋπηρεσίες καταχωρούνται στον πίνακα ΑΙΤΗΣΗ
         * 2) Υπολογίζει τα συνολικά μόρια από προϋπηρεσίες και μεταπτυχιακά, παιδαγωγικά
         * 3) Ανανεώνει το panel με τα αντίστοιχα μόρια
         * -------------------
         */
        private void btnMoria_Click(object sender, RoutedEventArgs e)
        {

            //UserFunctions.ShowAdminMessage("Not yet implemented");

            // Καταχώρηση μορίων από διδακτική στον πίνακα ΑΙΤΗΣΗ
            am.DidaktikiMoriaToAitisi();
            // Καταχώρηση μορίων από επαγγελματική στον πίνακα ΑΙΤΗΣΗ
            am.EpagelmatikiMoriaToAitisi();
            // Καταχώρηση μορίων από ελ. επάγγελμα στον πίνακα ΑΙΤΗΣΗ
            am.EpagelmaMoriaToAitisi();
            // Καταχώρηση μορίων από μεταπτυχιακά, παιδ. στον πίνακα ΑΙΤΗΣΗ
            am.PostgradPaidagMoriaToAitisi();
            // Υπολογισμός και καταχώρηση συνολικών μορίων στον πίνακα ΑΙΤΗΣΗ
            am.TotalMoriaToAitisi();

            /* -------------------------------------------------------------------------
             * Date: 26-01-2013
             * Ο μηχανισμός αυτός ακυρώνεται διότι δεν ελέγχει προϋπηρεσίες
             * πριν από 15ετία. Αντικαθίσταται από το σύστημα Primus.

            // προτροπή να προστεθούν τα μόρια αντίστοιχης αίτησης από
            // την προηγούμενη προκήρυξη.
            string checkMessage = "Να προστεθούν τα μόρια από την προηγούμενη προκήρυξη; " + "\n";
            if (MessageBox.Show(checkMessage, "Ειδοποίηση", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // do it here 
                am.AddPreviousMoriaToAitisi();
            }

             * -------------------------------------------------------------------------
             */

            // Ανανέωση των στοιχείων του MoriaPanel από τον πίνακα ΑΙΤΗΣΗ
            LoadMoriaInfo();

        }

        private void btnApp_Click(object sender, RoutedEventArgs e)
        {
            Button image = (Button)e.Source;
            string uri = "AppPages/Aitiseis/Teachers.xaml";

            if (uri != "")
            {
                NavigationService svc = NavigationService.GetNavigationService(image);
                if (svc != null) svc.Navigate(new Uri(uri, UriKind.Relative));
            }
        }
    }
}
