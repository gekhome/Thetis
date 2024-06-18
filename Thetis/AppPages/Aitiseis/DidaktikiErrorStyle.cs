using System;
using System.Windows.Data;
using System.Windows.Media;
using Thetis.Model;
using Thetis.DataAccess;

namespace Thetis.AppPages.Aitiseis
{
    class DidaktikiErrorStyle : IValueConverter
    {
        private ThetisDataContext db = new ThetisDataContext();
        private Primus p = new Primus();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush error_color = new SolidColorBrush(Colors.Red);          
            ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktiki = (ΕΚΠ_ΔΙΔΑΚΤΙΚΗ)value;
            
            if (didaktiki != null)
            {
                if (p.ValidateDidaktiki(didaktiki) != true)     // was false
                {
                    error_color = new SolidColorBrush(Colors.Red); ;
                }
                else
                {
                    error_color = new SolidColorBrush(Colors.White);
                }
            }

            return error_color;
        } // Convert

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush(Colors.White);
        } // ConvertBack

    } // class
}
