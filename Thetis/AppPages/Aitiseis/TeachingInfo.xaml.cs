using Telerik.Windows.Controls;
using Thetis.DataAccess;

namespace Thetis.AppPages.Aitiseis
{
    /// <summary>
    /// Interaction logic for TeachingInfo.xaml
    /// </summary>
    public partial class TeachingInfo : RadWindow
    {
        public TeachingInfo()
        {
            InitializeComponent();
            LoadData();
        }
        
        private void LoadData()
        {
            txtStartDate.Text = MoriaAnalysis.StartDate.ToString("dd/MM/yyyy");
            txtEndDate.Text = MoriaAnalysis.EndDate.ToString("dd/MM/yyyy");
            txtWeeklyHours.Text = MoriaAnalysis.WeeklyHours.ToString();
            txtTotalHours.Text = MoriaAnalysis.TotalHours.ToString();
            txtCalculatedTotalHours.Text = MoriaAnalysis.CalculatedTotal.ToString();

            // show calculated fields
            txtWorkingDays.Text = MoriaAnalysis.WorkingDays.ToString();
            txtChristmasDays.Text = MoriaAnalysis.ChristmasDays.ToString();
            txtEasterDays.Text = MoriaAnalysis.EasterDays.ToString();
            txtArgiesDays.Text = MoriaAnalysis.ArgiesDays.ToString();
            txtProperDays.Text = MoriaAnalysis.ProperDays.ToString();
        }
    }
}
