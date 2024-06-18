using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.Controls;


namespace Thetis.DataAccess
{
    public class MoriaModel
    {
        private ThetisDataContext db = new ThetisDataContext();

        /// <summary>
        /// Επιστρέφει για την τρέχουσα αίτηση τα μόρια/ώρα διδακτικής από την αντίστοιχη προκήρυξη.
        /// </summary>
        /// <returns></returns>
        public short GetTeachMoriaHour()
        {
            short moria_hour = 0;

            var prokquery = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == SelectedAitisi.AitisiProkirixi
                             select new { p.ΜΟΡΙΑ_ΩΡΑ }).FirstOrDefault();

            moria_hour = (short)prokquery.ΜΟΡΙΑ_ΩΡΑ;
            return moria_hour;
        }

        /// <summary>
        /// Επιστρέφει για την τρέχουσα αίτηση τα μόρια/μέρα επαγγελματικής από την αντίστοιχη προκήρυξη.
        /// </summary>
        /// <returns></returns>
        public short GetProfMoriaDay()
        {
            short moria_day = 0;

            var prokquery = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == SelectedAitisi.AitisiProkirixi
                             select new { p.ΜΟΡΙΑ_ΗΜΕΡΑ }).FirstOrDefault();

            moria_day = (short)prokquery.ΜΟΡΙΑ_ΗΜΕΡΑ;
            return moria_day;
        }


        /// <summary>
        /// Υπολογίζει τα μόρια για μια διδακτική προϋπηρεσία.
        /// Δέχεται ένα αντικείμενο ΕΚΠ_ΔΙΔΑΚΤΙΚΗ και θέτει τιμές στα πεδία μορίων.
        /// </summary>
        /// <remarks>Το αντικείμενο είναι ένα row από το View</remarks>
        /// <param name="didaktiki"></param>
        public bool CalculateTeachingMoria(ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktiki)
        {
            string date1 = didaktiki.ΕΝΑΡΞΗ.ToString();
            string date2 = didaktiki.ΛΗΞΗ.ToString();

            int school_year = Convert.ToInt32(didaktiki.ΣΧΟΛΙΚΟ_ΕΤΟΣ);  // a null is converted to 0

            if (!(school_year > 0) || String.IsNullOrEmpty(date1) || String.IsNullOrEmpty(date2))
            {
                UserFunctions.ShowAdminMessage("Μη έγκυρα δεδομένα εισόδου.");
                return false;
            }

            short calc_hours = 0;
            int weeklyHours, Hours, Years;
            weeklyHours = Convert.ToInt32(didaktiki.ΩΡΕΣ_ΕΒΔ);      // a null value will be converted to 0

            if (weeklyHours > 0)
            {
                ProperTeachHours(date1, date2, weeklyHours, out Hours, out Years);
                didaktiki.ΩΡΕΣ_ΣΥΝ = (short)Hours;
            }
            else // user has not entered a value of ΩΡΕΣ_ΕΒΔ
            {
                //verify that a value >0 exists for ΩΡΕΣ_ΣΥΝ which is now the input value
                short total_hours = (short)Convert.ToInt32(didaktiki.ΩΡΕΣ_ΣΥΝ);
                if ((total_hours == 0))
                {
                    UserFunctions.ShowAdminMessage("Οι Ώρες/Εβδ=0. Πρέπει τουλάχιστον να δoθούν οι συνολικές ώρες.");
                    return false;
                }
                double _Years = (double)Days360(date1, date2)/360;
                Years = Convert.ToInt32(Math.Ceiling(_Years));
            }
            // set the ΚΩΔ_ΑΙΤΗΣΗ as it is part of the PK
            didaktiki.ΚΩΔ_ΑΙΤΗΣΗ = SelectedAitisi.AitisiId;
            // determine the total hours to assign to the field ΣΥΝ_ΩΡΕΣ
            short MaxHours = (short)(MaxYearHours() * Years);
            short teachHours = (short)Convert.ToInt32(didaktiki.ΩΡΕΣ_ΣΥΝ);
            calc_hours = (teachHours > MaxHours) ? MaxHours : teachHours;

            short moria_hour = GetTeachMoriaHour();
            didaktiki.ΣΥΝ_ΩΡΕΣ = (short)(calc_hours * moria_hour);
            return true;
        }

        /// <summary>
        /// Κάνει την ενημέρωση κωδ.αίτησης και μορίων μιας γραμμής της συλλογής ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ,
        /// μετά από επικύρωση των δεδομένων. Επιστρέφει true ή false.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool UpdateFreelance(ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ row)
        {
            int oik_etos = Convert.ToInt32(row.ΟΙΚ_ΕΤΟΣ);
            int income = Convert.ToInt32(row.ΕΙΣΟΔΗΜΑ);

            if (oik_etos == 0 || income == 0)
            {
                UserFunctions.ShowAdminMessage("Μη έγκυρα δεδομένα εισόδου.");
                return false;
            }

            //set the moria corresponding to oik_etos and income
            row.ΕΕ_ΜΟΡΙΑ = GetFreelanceMoria(oik_etos, income);

            // set the value of ΚΩΔ_ΑΙΤΗΣΗ from the global class
            // this is required as ΚΩΔ_ΑΙΤΗΣΗ is part of the PK
            row.ΚΩΔ_ΑΙΤΗΣΗ = SelectedAitisi.AitisiId;
            return true;
        }

        /// <summary>
        /// Κάνει την ενημέρωση κωδ.αίτησης και μορίων μιας γραμμής της συλλογής ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ,
        /// μετά από επικύρωση των δεδομένων. Επιστρέφει true ή false.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool UpdateProfessional(ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ row)
        {
            string date1 = row.ΕΝΑΡΞΗ.ToString();
            string date2 = row.ΛΗΞΗ.ToString();

            if (String.IsNullOrEmpty(date1) || String.IsNullOrEmpty(date2))
            {
                UserFunctions.ShowAdminMessage("Μη έγκυρα δεδομένα εισόδου.");
                return false;
            }
            int meres = Days360(date1, date2);

            //set the value ΚΩΔ_ΑΙΤΗΣΗ to the parent Aitisi
            //this is required because ΚΩΔ_ΑΙΤΗΣΗ is part of the PK
            row.ΚΩΔ_ΑΙΤΗΣΗ = SelectedAitisi.AitisiId;

            int moria_day = GetProfMoriaDay();

            row.ΗΜΕΡΕΣ = meres * moria_day;
            return true;
        }

        /// <summary>
        /// Βρίσκει την ημερομηνία αναφοράς που είναι η ημερομηνία
        /// λήξης της Προκήρυξης. Χρησιμοποιείται για υπολογισμούς ηλικίας
        /// και στις προϋπηρεσίες.
        /// </summary>
        /// <param name="prokirixi_id"></param>
        /// <returns name="refDate"></returns>
        public DateTime GetRefDate(int prokirixi_id)
        {
            DateTime refDate = new DateTime(1, 1, 1);
            var prokirixi = (from p in db.ΠΡΟΚΗΡΥΞΗs
                              where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == prokirixi_id
                              select new { p.ΗΜΝΙΑ_ΛΗΞΗ }).FirstOrDefault();

            try
            {
                refDate = Convert.ToDateTime(prokirixi.ΗΜΝΙΑ_ΛΗΞΗ);
            }
            catch (System.NullReferenceException)
            {
                UserFunctions.ShowAdminMessage("Δεν βρέθηκε προκήρυξη ή ημερομηνία λήξης.");
                refDate = new DateTime(1, 1, 1);
            }
           
            return refDate;
        }

        public DateTime GetBirthDate(string afm)
        {
            var teacher_qry = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                               where t.ΑΦΜ == afm
                               select new { t.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ }).FirstOrDefault();

            DateTime birthdate = (DateTime)teacher_qry.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ;

            return birthdate;
        }




        /// <summary>
        /// Η μέθοδος αυτή επιστρέφει τα συνολικά μόρια για μια αίτηση
        /// που αφορούν το ελεύθερο επάγγελμα. Η τιμή προέρχεται από
        /// View του SQL Server.
        /// </summary>
        /// <param name="aitisi_id"></param>
        /// <returns name="totalmoria"></returns>
        public int GetFreelanceTotalMoria(string aitisi_id)
        {
            int totalmoria;
            var total_moria = (from t in db.ΕΕΠΑΓΓΕΛΜΑ_ΣΥΝΟΛΟ_ΜΟΡΙΑs
                               where t.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                               select new { t.ΣΥΝΟΛΟ_ΜΟΡΙΑ }).FirstOrDefault();

            try
            {
                totalmoria = Convert.ToInt32(total_moria.ΣΥΝΟΛΟ_ΜΟΡΙΑ);
            }
            catch (System.NullReferenceException)
            {
                totalmoria = 0;
            }
            return totalmoria;

        }

        /// <summary>
        /// Η μέθοδος αυτή επιστρέφει τα συνολικά μόρια για μια αίτηση
        /// που αφορούν την επαγγελματική προϋπηρεσία. Η τιμή προέρχεται
        /// από View του SQL Server.
        /// </summary>
        /// <param name="aitisi_id"></param>
        /// <returns name="totalmoria"></returns>
        public int GetProfessionalTotalMoria(string aitisi_id)
        {
            int totalmoria;
            // fetch total moria from view
            var total_moria = (from t in db.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΣΥΝΟΛΟ_ΜΟΡΙΑs
                               where t.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                               select new { t.ΣΥΝΟΛΟ_ΜΕΡΕΣ }).FirstOrDefault();

            try
            {
                totalmoria = Convert.ToInt32(total_moria.ΣΥΝΟΛΟ_ΜΕΡΕΣ);
            }
            catch (System.NullReferenceException)
            {
                totalmoria = 0;
            }

            return totalmoria;
            
        }

        /// <summary>
        /// Επιστρέφει τα συνολικά μόρια για μια αίτηση
        /// που αφορούν τη διδακτική προϋπηρεσία. Η τιμή προέρχεται
        /// από View του SQL Server.
        /// </summary>
        /// <param name="aitisi_id"></param>
        /// <returns></returns>
        public int GetTeachingTotalMoria(string aitisi_id)
        {
            int totalmoria;
            var total_moria = (from t in db.ΔΙΔΑΚΤΙΚΗ_ΣΥΝΟΛΟs
                               where t.ΚΩΔ_ΑΙΤΗΣΗ == aitisi_id
                               select new { t.ΜΟΡΙΑ }).FirstOrDefault();

            try
            {
                totalmoria = Convert.ToInt32(total_moria.ΜΟΡΙΑ);
            }
            catch (System.NullReferenceException)
            {
                totalmoria = 0;
            }

            return totalmoria;
        }

        /// <summary>
        /// Βρίσκει το νόμισμα που αντιστοιχεί σε ένα οικονομικό έτος.
        /// Επιστρέφει το λεκτικό του νομίσματος και όχι τον κωδικό.
        /// </summary>
        /// <param name="oik_etos"></param>
        /// <returns name="nom"></returns>
        public string GetNomisma(Int32 oik_etos)
        {
            var nomisma = (from n in db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs
                           where n.ΟΙΚ_ΕΤΟΣ == oik_etos
                           select new { n.ΝΟΜΙΣΜΑ }).FirstOrDefault();

            if (nomisma == null) return "";

            string nom = Convert.ToString(nomisma.ΝΟΜΙΣΜΑ);
            return nom;
        }

        /// <summary>
        /// Υπολογίζει τα μόρια που αντιστοιχούν στο οικ. έτος
        /// και εισόδημα που δηλώθηκε.
        /// </summary>
        /// <param name="oik_etos"></param>
        /// <param name="income"></param>
        /// <returns name="moria"></returns>
        public short GetFreelanceMoria(Int32 oik_etos, Int32 income)
        {
            short moria;
            var _aforologito = (from a in db.ΕΤΟΣ_ΑΦΕΙΣΟΔΗΜΑs
                               where a.ΟΙΚ_ΕΤΟΣ == oik_etos
                               select new { a.ΑΦΟΡΟΛΟΓΗΤΟ, a.Ε_ΜΟΡΙΑ }).FirstOrDefault();

            // this is important because it is called by the UI
            // and the user can do all sort of messy things
            if (_aforologito == null) return 0;

            var aforologito = Convert.ToInt32(_aforologito.ΑΦΟΡΟΛΟΓΗΤΟ);

            if (income > aforologito)
                moria = Convert.ToInt16(_aforologito.Ε_ΜΟΡΙΑ);
            else
                moria = 0;

            return moria;
        }

        /// <summary>
        /// Υπολογίζει τις ημέρες λογιστικού έτους μεταξύ δύο ημερομηνιών,
        /// προσομειώνοντας τη συνάρτηση Days360 του Excel.
        /// </summary>
        /// <param name="initial_date"></param>
        /// <param name="final_date"></param>
        /// <returns name="meres"></returns>
        public int Days360(string initial_date, string final_date)
        {
            DateTime date1 = DateTime.Parse(initial_date);
            DateTime date2 = DateTime.Parse(final_date);

            //UserFunctions.ShowAdminMessage(date1.ToString());
            //UserFunctions.ShowAdminMessage(date2.ToString());

            var y1 = date1.Year;
            var y2 = date2.Year;
            var m1 = date1.Month;
            var m2 = date2.Month;
            var d1 = date1.Day;
            var d2 = date2.Day;

            DateTime tempDate = date1.AddDays(1);
            if (tempDate.Day == 1 && date1.Month == 2)
            {
                d1 = 30;
            }
            if (d2 == 31 && d1 == 30)
            {
                d2 = 30;
            }

            double meres = (y2 - y1) * 360 + (m2 - m1) * 30 + (d2 - d1);
            meres = (meres/30)*25;
            meres = Math.Ceiling(meres);
            //UserFunctions.ShowAdminMessage(meres.ToString());

            return Convert.ToInt32(meres);
        }

        /// <summary>
        /// Υπολογίζει τις εργάσιμες ημέρες μεταξύ δύο ημερομηνιών,
        /// δηλ. χωρίς τα Σαββατοκύριακα.
        /// Δεχεται τις ημερομηνίες ως συμβολοσειρές.
        /// </summary>
        /// <param name="initial_date"></param>
        /// <param name="final_date"></param>
        /// <returns name="daycount"></returns>
        public int WorkingDays(string initial_date, string final_date)
        {
            int daycount = 0;

            DateTime date1 = DateTime.Parse(initial_date);
            DateTime date2 = DateTime.Parse(final_date);

            while (date1 <= date2)
            {
                switch (date1.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Saturday:
                        break;
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        daycount++;
                        break;
                    default:
                        break;
                }
                date1 = date1.AddDays(1);
            }
            return daycount;
        }

        /// <summary>
        /// Υπολογίζει την ημερομηνία του Ορθόδοξου Πάσχα
        /// για το συγκεκριμένο έτος της παραμέτρου εισόδου.
        /// Επιστρέφει την ημερομηνία ως DateTime.
        /// </summary>
        /// <param name="theyear"></param>
        /// <returns name="easterDate"></returns>
        public DateTime ComputeEaster(int theyear)
        {
            int r1, r2, r3, r4, r5;
            int days;
            DateTime easterDate;

            r1 = theyear % 4;
            r2 = theyear % 7;
            r3 = theyear % 19;
            r4 = (19 * r3 + 15) % 30;
            r5 = (2 * r1 + 4 * r2 + 6 * r4 + 6) % 7;
            days = r5 + r4 + 13;

            if (days > 39) 
            {
                days = days-39;
                easterDate = Convert.ToDateTime(days+"/"+5+"/"+theyear);
            }
            else if (days > 9)
            {
                days = days-9;
                easterDate = Convert.ToDateTime(days+"/"+4+"/"+theyear);
            }
            else
            {
                days = days+22;
                easterDate = (Convert.ToDateTime(days+"/"+3+"/"+theyear)).Date;
            }
            return easterDate;
        }

        /// <summary>
        /// Υπολογίζει τον αριθμό ημερών διακοπών του Πάσχα
        /// που πρέπει να αφαιρεθούν από τη διδακτική προϋπηρεσία.
        /// Δεν αφαιρεί ΣΚ διότι αυτά αφαιρούνται από την μέθοδο WorkingDays
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns name="days2remove"></returns>
        public int EasterDays(string date1, string date2)
        {
            int days2remove = 0;
            DateTime d1 = Convert.ToDateTime(date1);
            DateTime d2 = Convert.ToDateTime(date2);
            //UserFunctions.ShowAdminMessage("Years:" + d1.Year + "-" + d2.Year);
            
            if (d1.Year == d2.Year)
            {
                //UserFunctions.ShowAdminMessage("Same year:" + d1.Year + "-" + d2.Year);
                if (d1 <= ComputeEaster(d1.Year) && d2 >= ComputeEaster(d1.Year))
                {
                    string _d1 = Convert.ToString(ComputeEaster(d1.Year).AddDays(-6));
                    string _d2 = Convert.ToString(ComputeEaster(d1.Year).AddDays(7));
                    days2remove = WorkingDays(_d1, _d2);
                    //UserFunctions.ShowAdminMessage("Days to remove -same year:" + days2remove);
                }
            }
            else
            {
                //UserFunctions.ShowAdminMessage("different years:" + d1.Year + "-" + d2.Year);
                if (d1 <= ComputeEaster(d2.Year) && d2 >= ComputeEaster(d2.Year))
                {
                    string _d1 = Convert.ToString(ComputeEaster(d2.Year).AddDays(-6));
                    string _d2 = Convert.ToString(ComputeEaster(d2.Year).AddDays(7));
                    days2remove = WorkingDays(_d1, _d2);
                    //UserFunctions.ShowAdminMessage("Days to remove -different years:" + days2remove);
                }
            }   
            return days2remove;
        }

        /// <summary>
        /// Υπολογίζει τον αριθμό ημερών διακοπών Χριστουγέννων
        /// που πρέπει να αφαιρεθούν από τη διδακτική προϋπηρεσία.
        /// Δεν αφαιρεί ΣΚ διότι αυτά αφαιρούνται από την μέθοδο WorkingDays
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns name="days2remove"></returns>
        public int ChristmasDays(string date1, string date2)
        {
            int days2remove = 0;
            DateTime d1 = Convert.ToDateTime(date1);
            DateTime d2 = Convert.ToDateTime(date2);

            if (d1.Year != d2.Year)
            {
                string y1 = Convert.ToString(d1.Year);
                string y2 = Convert.ToString(d1.Year+1);
                days2remove = WorkingDays("23/12/" + y1, "7/1/" + y2);
            }
            //UserFunctions.ShowAdminMessage("Christmas Days to remove:" + days2remove);
            return days2remove;
        }

        /// <summary>
        /// Υπολογίζει τον αριθμό ημερών αργιών μεταξύ δύο ημερομηνιών
        /// ώστε αυτός να αφαιρεθεί από τις ημέρες διδακτικής προϋπηρεσίας.
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns name="argies_days"></returns>
        public int ArgiesDays(string date1, string date2)
        {
            int argies_days = 0;
            DateTime d1 = Convert.ToDateTime(date1);
            DateTime d2 = Convert.ToDateTime(date2);
            
            string[] ArgiesTable = { "27/2", "25/3", "1/5", "4/6", "15/8", "28/10", "17/11" };

            if (d1.Year == d2.Year)
            {
                for (int i = 0; i < ArgiesTable.Length; i++)
                {
                    DateTime argia = Convert.ToDateTime(ArgiesTable[i] + "/" + Convert.ToString(d1.Year));
                    if (argia >= d1 && argia <= d2)
                    {
                        if ((int)argia.DayOfWeek != 0 && (int)argia.DayOfWeek != 6) argies_days++;
                    }
                }
            }
            else
            {
                DateTime part1_date = Convert.ToDateTime("31/12/" + Convert.ToString(d1.Year));
                DateTime part2_date = Convert.ToDateTime("1/1/" + Convert.ToString(d2.Year));
                
                for (int i = 0; i < ArgiesTable.Length; i++)
                {
                    DateTime argia = Convert.ToDateTime(ArgiesTable[i] + "/" + Convert.ToString(part1_date.Year));
                    if (argia >= d1 && argia <= part1_date)
                    {
                        if ((int)argia.DayOfWeek != 0 && (int)argia.DayOfWeek != 6) argies_days++;
                    }
                }
                for (int i = 0; i < ArgiesTable.Length; i++)
                {
                    DateTime argia = Convert.ToDateTime(ArgiesTable[i] + "/" + Convert.ToString(part2_date.Year));
                    if (argia >= part2_date && argia <= d2)
                    {
                        if ((int)argia.DayOfWeek != 0 && (int)argia.DayOfWeek != 6) argies_days++;
                    }
                }
            }
            return argies_days;
        }

        /// <summary>
        /// Υπολογίζει τις καθαρές διδακτικές ημέρες μετα από αφαίρεση
        /// των αργιών (Χριστούγεννα, Πάσχα, Εθνικές αργίες).
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns name="work_days"></returns>
        public int ProperTeachDays(string date1, string date2)
        {
            int work_days = 0;

            work_days = WorkingDays(date1, date2);
            work_days = work_days - ArgiesDays(date1, date2);
            work_days = work_days - ChristmasDays(date1, date2);
            work_days = work_days - EasterDays(date1, date2);
            
            return work_days;
        }

        /// <summary>
        /// Υπολογίζει τις κανονικοποιημένες ώρες και έτη διδασκαλίας
        /// Επιστρέφει Hours, Years.
        /// Καλεί την ProperTeachDays για τον υπολογισμό των κανονικοποιημένων ημερών.
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="weekly_hours"></param>
        /// <param name="Hours"></param>
        /// <param name="Years"></param>
        public void ProperTeachHours(string date1, string date2, int weekly_hours, out int Hours, out int Years)
        {
            Hours = 0;
            Years = 0;

            // work_days must be double otherwise, division returns int (int/int)
            // and results are wrong.
            double work_days = ProperTeachDays(date1, date2);

            double _hours = work_days / 5 * weekly_hours;
            double _years = work_days / 360;

            Hours = Convert.ToInt32(Math.Ceiling(_hours));
            Years = Convert.ToInt32(Math.Ceiling(_years));
            //UserFunctions.ShowAdminMessage("Hours: " + Hours.ToString() + " Years: " + Years.ToString());

        }

        /// <summary>
        /// Επιστρέφει τις μέγιστες υπολογίσιμες διδακτικές ώρες ενός έτους
        /// </summary>
        /// <returns></returns>
        public int MaxYearHours()
        {
            int max_yearly_hours = 896;
            return max_yearly_hours;
        }    
    }

    public static class MoriaAnalysis
    {
        private static ThetisDataContext db = new ThetisDataContext();
        //class fields
        private static int didaktiki_id;
        private static DateTime start_date;
        private static DateTime end_date;
        private static int weekly_hours;
        private static int total_hours;
        private static int calculated_total;
        // class fields (calculated values)
        private static int working_days;
        private static int christmas_days;
        private static int easter_days;
        private static int argies_days;
        private static int proper_days;

        public static int DidaktikiId
        {
            get { return didaktiki_id; }
            set { didaktiki_id = value; }
        }
        public static DateTime StartDate
        {
            get { return start_date; }
            set { start_date = value; }
        }
        public static DateTime EndDate
        {
            get { return end_date; }
            set { end_date = value; }
        }
        public static int WeeklyHours
        {
            get { return weekly_hours; }
            set { weekly_hours = value; }
        }
        public static int TotalHours
        {
            get { return total_hours; }
            set { total_hours = value; }
        }
        public static int CalculatedTotal
        {
            get { return calculated_total; }
            set { calculated_total = value; }
        }
        // these properties are calculated.
        public static int WorkingDays
        {
            get
            {
                MoriaModel mm = new MoriaModel();
                working_days = mm.WorkingDays(StartDate.ToString(), EndDate.ToString());
                return working_days;
            }
        }
        public static int ChristmasDays
        {
            get
            {
                MoriaModel mm = new MoriaModel();
                christmas_days = mm.ChristmasDays(StartDate.ToString(), EndDate.ToString());
                return christmas_days;
            }
        }
        public static int EasterDays
        {
            get
            {
                MoriaModel mm = new MoriaModel();
                easter_days = mm.EasterDays(StartDate.ToString(), EndDate.ToString());
                return easter_days;
            }
        }
        public static int ArgiesDays
        {
            get
            {
                MoriaModel mm = new MoriaModel();
                argies_days = mm.ArgiesDays(StartDate.ToString(), EndDate.ToString());
                return argies_days;
            }
        }
        public static int ProperDays
        {
            get
            {
                MoriaModel mm = new MoriaModel();
                proper_days = mm.ProperTeachDays(StartDate.ToString(), EndDate.ToString());
                return proper_days;
            }
        }
    }
}
