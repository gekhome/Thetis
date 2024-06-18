using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Thetis.Model;
using Thetis.Controls;
using System.Text.RegularExpressions;

namespace Thetis.Utilities
{
    class UserFunctions
    {

        #region ColorConversion
        public static string GetHexFromRGB(string R, string G, string B)
        {
            string a, b, c, d, e, f, z;
            a = GetHex(Math.Floor(double.Parse(R) / 16));
            b = GetHex(int.Parse(R) % 16);
            c = GetHex(Math.Floor(double.Parse(G) / 16));
            d = GetHex(int.Parse(G) % 16);
            e = GetHex(Math.Floor(double.Parse(B) / 16));
            f = GetHex(int.Parse(B) % 16);
            z = a + b + c + d + e + f;
            z = "#" + z;
            return z;
        } // GetHexFromRGB

        private static string GetHex(double Dec)
        {
            string Value = "";
            if (Dec == 10)
            {
                Value = "A";
            }
            else if (Dec == 11)
            {
                Value = "B";
            }
            else if (Dec == 12)
            {
                Value = "C";
            }
            else if (Dec == 13)
            {
                Value = "D";
            }
            else if (Dec == 14)
            {
                Value = "E";
            }
            else if (Dec == 15)
            {
                Value = "F";
            }
            else
            {
                Value = "" + Dec;
            }
            return Value;
        } //GetHex

        #endregion

        #region String Functions (equivalent to VB)

        public static string Right(string text, int numberCharacters)
        {
            return text.Substring(numberCharacters > text.Length ? 0 : text.Length - numberCharacters);
        }

        public static string Left(string text, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
            else if (length == 0 || text.Length == 0)
                return "";
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(0, length);
        }
        public static int Len(string text)
        {
            int _length;
            _length = text.Length;
            return _length;
        }
        public static byte Asc(string src)
        {
            return (System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(src + "")[0]);
        }
        public static char Chr(byte src)
        {
            return (System.Text.Encoding.GetEncoding("iso-8859-1").GetChars(new byte[] { src })[0]);
        }
        public static bool isNumber(string param)
        {
            Regex isNum = new Regex("[^0-9]");
            return !isNum.IsMatch(param);
        }

        #endregion

        #region Dates Functions
        public static string month2GRstring(int m)
        {
            string stGRmonth = "";

            switch (m)
            {
                case 1: stGRmonth = "Ιανουαρίου"; break;
                case 2: stGRmonth = "Φεβρουαρίου"; break;
                case 3: stGRmonth = "Μαρτίου"; break;
                case 4: stGRmonth = "Απριλίου"; break;
                case 5: stGRmonth = "Μαϊου"; break;
                case 6: stGRmonth = "Ιουνίου"; break;
                case 7: stGRmonth = "Ιουλίου"; break;
                case 8: stGRmonth = "Αυγούστου"; break;
                case 9: stGRmonth = "Σεπτεμβρίου"; break;
                case 10: stGRmonth = "Οκτωβρίου"; break;
                case 11: stGRmonth = "Νοεμβρίου"; break;
                case 12: stGRmonth = "Δεκεμβρίου"; break;
                default: break;
            }
            return stGRmonth;
        }
        #endregion

        #region FileOpenDialog Wrapper
        /// <summary>
        ///  Just a short wrapper of convenient use of OpenFileDialog
        ///  I could elaborate more on this, but I leave it to Lefteris to handle
        ///  oveloads and defaulting for instance to c:, and *.*
        /// </summary>
        /// <param name="_DefaultExt"></param>
        /// <param name="_Filter"></param>
        /// <param name="_Title"></param>
        /// <param name="_InitDir"></param>
        /// <returns></returns>
        public static string OpenFile(string _DefaultExt, string _Filter, string _Title, string _InitDir)
        {
            // Create OpenFileDialog object
            string filename;
            OpenFileDialog dlg = new OpenFileDialog()
            { /* Set filter for file extension, default file extension and default directory*/
                DefaultExt = _DefaultExt,
                Filter = _Filter,
                Title = _Title,
                InitialDirectory = _InitDir
            };

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();
            filename = dlg.FileName;

            return filename;
        }

        #endregion

        #region RadWindow.Alert Wrapper

        public static void RadWindowAlert(string user_message)
        {
            DialogParameters parameters = new DialogParameters() 
            { Content = user_message, Header = "Ειδοποίηση", OkButtonContent = "Κλείσιμο" };
            RadWindow.Alert(parameters);
        }

        public static void ShowAdminMessage(string the_message)
        {
            W8Message msgWIndow = new W8Message();
            msgWIndow.Message = the_message;
            msgWIndow.ShowDialog();
        }

        #endregion

        public static int Max(params int[] values)
        {
            return Enumerable.Max(values);
        }

    } // Class UserFunctions

    /// <summary>
    /// Custom class for dates functions handling
    /// and validating according to business logic.
    /// These methods accept strings as dates input.
    /// </summary>
    public class Dates
    {

        // checks if initial date is earlier than final date
        public static bool ValidStartEndDates(string dateStart, string dateEnd)
        {
            bool result;
            DateTime _dateStart = DateTime.Parse(dateStart);
            DateTime _dateEnd = DateTime.Parse(dateEnd);

            if (_dateStart > _dateEnd)
                result = false;
            else
                result = true;
            return result;

        }

        // checks if dates belong to the same year
        public static bool InSameYear(string date1, string date2)
        {
            bool result;
            DateTime _date1 = DateTime.Parse(date1);
            DateTime _date2 = DateTime.Parse(date2);

            if (_date1.Year != _date2.Year)
                result = false;
            else
                result = true;
            return result;
        }

        // checks if dates fall in the specified school year
        public static bool InSchoolYear(string dateStart, string dateEnd, int schoolYearID)
        {
            ThetisDataContext db = new ThetisDataContext();
            bool result = true;
            DateTime _date1 = DateTime.Parse(dateStart);
            DateTime _date2 = DateTime.Parse(dateEnd);

            var schoolYear = (from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                              where s.ΣΧ_ΕΤΟΣ_ΚΩΔ == schoolYearID
                              select new { s.ΣΧ_ΕΤΟΣ_ΕΝΑΡΞΗ, s.ΣΧ_ΕΤΟΣ_ΛΗΞΗ }).FirstOrDefault();

            DateTime syStart = DateTime.Parse(Convert.ToString(schoolYear.ΣΧ_ΕΤΟΣ_ΕΝΑΡΞΗ));
            DateTime syEnd = DateTime.Parse(Convert.ToString(schoolYear.ΣΧ_ΕΤΟΣ_ΛΗΞΗ));

            if (_date1 < syStart || _date2 > syEnd)
                result = false;

            return result;
        }

        /// <summary>
        /// Κάνει επικύρωση των δεδομένων απου αφορούν το σχολικό έτος
        /// και τις ημερομηνίες έναρξης-λήξης.
        /// </summary>
        /// <param name="syear"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool VerifySchoolYear(string syear, DateTime d1, DateTime d2)
        {

            if (syear.IndexOf('-') == -1)
            {
                UserFunctions.ShowAdminMessage("Το σχολικό έτος πρέπει να είναι στη μορφή έτος1-έτος2.");
                return false;
            }

            string[] split = syear.Split(new Char[] { '-' });
            string sy1 = Convert.ToString(split[0]);
            string sy2 = Convert.ToString(split[1]);

            if (!UserFunctions.isNumber(sy1) || !UserFunctions.isNumber(sy2))
            {
                UserFunctions.ShowAdminMessage("Τα έτη δεν είναι αριθμοί.");
                return false;
            }
            else
            {
                int y1 = Convert.ToInt32(sy1);
                int y2 = Convert.ToInt32(sy2);

                if (y2 - y1 > 1 || y2 - y1 <= 0)
                {
                    UserFunctions.ShowAdminMessage("Τα έτη δεν είναι σωστά.");
                    return false;
                }
                if (d1.Year != y1 || d2.Year != y2)
                {
                    UserFunctions.ShowAdminMessage("Οι ημερομηνίες δεν συμφωνούν με τα έτη.");
                    return false;
                }
            }
            // at this point everything is ok
            return true;
        }

        public static bool SchoolYearExists(string syear)
        {
            ThetisDataContext db = new ThetisDataContext();
            var syear_recs = (from s in db.ΣΧΟΛΙΚΟ_ΕΤΟΣs
                              where s.ΣΧ_ΕΤΟΣ == syear
                              select s).Count();

            if (syear_recs != 0)
            {
                UserFunctions.ShowAdminMessage("Το σχολικό έτος υπάρχει ήδη.");
                return true;
            }

            return false;
        }

        public static bool ValidateAitisiDate(string date)
        {
            ThetisDataContext db = new ThetisDataContext();
            DateTime aitisi_date = DateTime.Parse(date);
            int margin_days = 20;

            var prokquery = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΚΑΤΑΣΤΑΣΗ == 1
                             select new { p.ΗΜΝΙΑ_ΕΝΑΡΞΗ, p.ΗΜΝΙΑ_ΛΗΞΗ }).FirstOrDefault();

            DateTime pStart = DateTime.Parse(Convert.ToString(prokquery.ΗΜΝΙΑ_ΕΝΑΡΞΗ));
            DateTime pEnd = DateTime.Parse(Convert.ToString(prokquery.ΗΜΝΙΑ_ΛΗΞΗ));
            DateTime limitDate = pEnd.AddDays(margin_days);

            if (aitisi_date < pStart || aitisi_date > limitDate) return false;

            return true;
        }

        /// <summary>
        /// Υπολογίζει τα έτη (στρογγυλοποιημένα) μεταξύ δύο ημερομηνιών
        /// </summary>
        /// <param name="sdate">αρχική ημερομηνία</param>
        /// <param name="edate">τελική ημερομηνία</param>
        /// <returns></returns>
        public static int YearsDiff(DateTime sdate, DateTime edate)
        {
            TimeSpan ts = edate - sdate;
            int days = ts.Days;

            double _years = days / 365;

            int years = Convert.ToInt32(Math.Ceiling(_years));

            return years;
        }


    } // class Dates

    /// <summary>
    /// This class performs the validation of the Tax Reg.No
    /// </summary>
    public class afmRule : ValidationRule
    {
        public afmRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string safm = "";
            safm = (string)value;

            if (safm == null || safm == "")
            {
                return new ValidationResult(false, "Καταχωρήστε έγκυρους χαρακτήρες για το ΑΦΜ.");
            }
            else
            {
                if (!CheckAFM(safm))
                    return new ValidationResult(false, "Ο αριθμός ΑΦΜ δεν είναι έγκυρος.");
                else
                    return new ValidationResult(true, null);
            }
        }

        #region AFM check functions
        /// <summary>
        /// The fisrt two methods do not work, although
        /// the ValidAfm2 has been translated from my working
        /// version in VB. The second, ValidAfm fails to verify all VAT numbers.
        /// The working version is CheckAFM and is currently used after many tests.
        /// Date: 8-3-2012
        /// </summary>
        /// <param name="safm"></param>
        /// <returns></returns>
        public static bool ValidAfm2(string safm)
        {
            int dSum = 0;
            int dRem = 0;
            int pol = 0;
            int i;

            for (i = 0; i <= UserFunctions.Len(safm) - 1; i++)
            {
                if (!UserFunctions.isNumber(safm))
                {
                    MessageBox.Show("Μη έγκυροι χαρακτήρες", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return false;
                }
            }
            for (i = 1; i <= UserFunctions.Len(safm)-1; i++)
            {
                pol = 2 ^ (UserFunctions.Len(safm) - i);
                dSum = dSum + int.Parse(safm.Substring(i, 1)) * pol;
            }
            if (dSum == 0)
            {
                //MessageBox.Show("dSum is zero.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            else
            {
                dRem = dSum % 11;
                if ((dRem == int.Parse(UserFunctions.Right(safm, 1))) || (dRem == 10 && int.Parse(UserFunctions.Right(safm, 1)) == 0))
                {
                    return true;
                }
                else
                {
                    int slen = safm.Length;
                    //MessageBox.Show("dRem not verified." + slen + " " + dRem, "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return false;
                }
            }

        } // ValidAfm

        public static bool ValidAfm(string safm)
        {
            if (UserFunctions.isNumber(safm))
            {
                try
                {
                    int telestis = 2;
                    int sum = 0;
                    string afm = safm;
                    for (int i = 8; i >= 0; i--) // was 7
                    {
                        sum += int.Parse(afm[i].ToString()) * telestis;
                        telestis = telestis * 2;
                    }
                    double check;
                    check = sum / 11;
                    check = check * 11;
                    if (sum == check)
                    {
                        MessageBox.Show("Έγκυρο ΑΦΜ", "www.digitalnews.gr");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Μη έγκυρο ΑΦΜ", "www.digitalnews.gr");
                        return false;
                    }
                }
                catch
                {
                    //MessageBox.Show("Λάθος στην εισαγωγή δεδομένων", "www.digitalnews.gr");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Παρακαλώ δώστε μόνο αριθμούς", "www.digitalnews.gr");
                return false;
            }
        }

        /// --------------------------------------------
        /// CheckAFM: Ελέγχει αν ένα ΑΦΜ είναι σωστό
        /// Το ΑΦΜ που θα ελέγξουμε
        /// true = ΑΦΜ σωστό, false = ΑΦΜ Λάθος
        /// Αυτή είναι η χρησιμοποιούμενη μεθοδος.
        /// Προσθήκη: Αποκλεισμός όταν όλα τα ψηφία = 0 (ο αλγόριθμος τα δεχεται!)
        /// Ημ/νια: 12/3/2013
        /// --------------------------------------------

        public static bool CheckAFM(string cAfm)
        {
            int nExp = 1;
            // Ελεγχος αν περιλαμβάνει μόνο γράμματα
            try { long nAfm = Convert.ToInt64(cAfm); }

            catch (Exception) { return false; }

            // Ελεγχος μήκους ΑΦΜ
            if (string.IsNullOrWhiteSpace(cAfm))
            {
                return false;
            }

            cAfm = cAfm.Trim();
            int nL = cAfm.Length;
            if (nL != 9) return false;

            // Έλεγχος αν όλα τα ψηφία είναι 0
            var count = cAfm.Count(x => x == '0');
            if (count == cAfm.Length) return false;

            //Υπολογισμός αν το ΑΦΜ είναι σωστό

            int nSum = 0;
            int xDigit = 0;
            int nT = 0;

            for (int i = nL - 2; i >= 0; i--)
            {
                xDigit = int.Parse(cAfm.Substring(i, 1));
                nT = xDigit * (int)(Math.Pow(2, nExp));
                nSum += nT;
                nExp++;
            }

            xDigit = int.Parse(cAfm.Substring(nL - 1, 1));

            nT = nSum / 11;
            int k = nT * 11;
            k = nSum - k;
            if (k == 10) k = 0;
            if (xDigit != k) return false;

            return true;

        }
    }
        #endregion


    public class BirthDateRule : ValidationRule
    {
        private int _minAge;
        private int _maxAge;
        
        public int MinAge
        {
            get { return _minAge; }
            set { _minAge = value; }
        }

        public int MaxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; }
        }
        public BirthDateRule()
        {
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value ==null)
                return new ValidationResult(false, "Πρέπει να καταχωρηθεί μία ημερομηνία.");
            else
            {
                DateTime birthdate = (DateTime)value;
                string sbirthdate = birthdate.ToString();

                if (!ValidBirthDate(sbirthdate, MinAge, MaxAge))
                    return new ValidationResult(false, "Η ημερομηνία γέννησης είναι εκτός έγκυρων ορίων.");
                else
                    return new ValidationResult(true, null);
            }
        }
        // validates birthdates according to age-limits - specific for thetis
        public static bool ValidBirthDate(string sbirthdate, int minAge, int maxAge)
        {
            bool result = true;
            //int maxAge = 66;
            //int minAge = 22;
            DateTime birthdate = DateTime.Parse(sbirthdate);
            DateTime minDate = DateTime.Today.Date.AddYears(-maxAge);
            DateTime maxDate = DateTime.Today.Date.AddYears(-minAge);

            if (birthdate >= minDate && birthdate <= maxDate)
                result = true;
            else
                result = false;
            return result;
        }

    } // class BirthDateRule

    #region Logged user info

    /// <summary>
    /// 
    /// </summary>
    static class LoginClass
    {
        private static ThetisDataContext db = new ThetisDataContext();
        private static string _username = "";
        private static int _userkey = 0;

        public static string Userid
        {
            get { return _username; }
            set { _username = value; }
        }

        public static int UserKey
        {
            get
            {
                var user = (from u in db.USERs
                            where u.USER_TYPE == LoginClass.Userid
                            select new { u.USER_KEY }).FirstOrDefault();
                _userkey = Convert.ToInt32(user.USER_KEY);
                return _userkey;
            }
            set { }
        }
    }

    #endregion


} // namespace
