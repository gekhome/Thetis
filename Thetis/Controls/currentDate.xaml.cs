using System;
using System.Windows.Controls;

namespace Thetis.Controls
{
    /// <summary>
    /// Interaction logic for currentDate.xaml
    /// </summary>
    public partial class currentDate : UserControl
    {
        public currentDate()
        {
            InitializeComponent();
            LblDayOfWeek.Content = DateTime.Now.ToString("dddd");  //DateTime.Now.DayOfWeek φέρνει ημέρα στα αγγλικά (είναι enumerated).
            LblDayNumber.Content = DateTime.Now.Day;
            //LblMonth.Content = DateTime.Now.ToString("MMMM");    //φέρνει σωστά το locale string αλλά σε ονομαστική.
            int curMonth = DateTime.Now.Month;
            LblMonth.Content = Utilities.UserFunctions.month2GRstring(curMonth); //θέλουμε τον μήνα σε γενική πτώση.
        }
    }
}
