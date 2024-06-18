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
    /// Interaction logic for TileTimeDate.xaml
    /// </summary>
    public partial class TileTimeDate : UserControl
    {
        public TileTimeDate()
        {
            InitializeComponent();
            LblDayOfWeek.Content = DateTime.Now.ToString("dddd");  //DateTime.Now.DayOfWeek φέρνει ημέρα στα αγγλικά (είναι enumerated).
            LblDayNumber.Content = DateTime.Now.Day;
            LblMonth.Content = Utilities.UserFunctions.month2GRstring(DateTime.Now.Month); //θέλουμε τον μήνα σε γενική πτώση.
            LblYear.Content = DateTime.Now.Year;
        }
    }
}
