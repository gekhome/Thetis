using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.DataAccess;

namespace Thetis.DataAccess
{
    /// <summary>
    /// Όι μέθοδοι της κλάσης αυτής υπολογίζουν, καταχωρούν τα μόρια
    /// στα αντίστοιχα πεδία του πίνακα ΑΙΤΗΣΗ. Οι τιμές προέρχονται
    /// από Views της ΒΔ, εκτός από τα συνολικά μόρια που υπολογίζονται.
    /// </summary>
    class AitisiModel
    {
        private ThetisDataContext db = new ThetisDataContext();
        private CommitModel cm = new CommitModel();

        /// <summary>
        /// Υπολογίζει και καταχωρεί τα συνολικά μόρια διδακτικής στο αντίστοιχο
        /// πεδίο του πίνακα ΑΙΤΗΣΗ.
        /// </summary>
        public void DidaktikiMoriaToAitisi()
        {
            MoriaModel mm = new MoriaModel();
            short teach_moria = 0;
            // get the teaching total moria from the View
            var teachquery = (from d in db.ΔΙΔΑΚΤΙΚΗ_ΣΥΝΟΛΟs
                             where d.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                             select new { d.ΜΟΡΙΑ }).FirstOrDefault();
            var aitisiquery = (from a in db.ΑΙΤΗΣΗs
                               where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                               select a).First();
            var prokquery = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == SelectedAitisi.AitisiProkirixi
                             select new { p.ΠΡΟΣΜ_ΕΤΗ }).FirstOrDefault();

            // έλεγχος αν υπάρχει διδακτική εμπειρία
            if (teachquery == null)
            {
                aitisiquery.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ = 0;
            }
            else
            {
                teach_moria = (short)teachquery.ΜΟΡΙΑ;
                // έλεγχος αν υπερβαίνουν τα μόρια το μέσγιτο επιτρεπτό 
                // των 896*15=13440
                short max_moria = (short)(mm.MaxYearHours() * prokquery.ΠΡΟΣΜ_ΕΤΗ);
                //store the value in the corresponding field of ΑΙΤΗΣΗ
                aitisiquery.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ = (teach_moria > max_moria) ? max_moria : teach_moria;               
            }
            
            // this must be in a try-catch
            cm.CommitData(db);
            //db.SubmitChanges();
        }

        /// <summary>
        /// Καταχωρεί τα συνολικά μόρια επαγγελματικής στο αντίστοιχο
        /// πεδίο του πίνακα ΑΙΤΗΣΗ.
        /// </summary>
        public void EpagelmatikiMoriaToAitisi()
        {
            short epagelmatiki_moria = 0;
            // get the teaching total moria from the View
            var epagelmatikiquery = (from d in db.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΣΥΝΟΛΟ_ΜΟΡΙΑs
                              where d.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                              select new { d.ΣΥΝΟΛΟ_ΜΕΡΕΣ }).FirstOrDefault();
            var aitisiquery = (from a in db.ΑΙΤΗΣΗs
                               where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                               select a).First();

            // έλεγχος αν υπάρχει επαγγελματική εμπειρία
            if (epagelmatikiquery == null)
            {
                aitisiquery.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ = 0;
            }
            else
            {
                epagelmatiki_moria = (short)epagelmatikiquery.ΣΥΝΟΛΟ_ΜΕΡΕΣ;
                //store the value in the corresponding field of ΑΙΤΗΣΗ            
                aitisiquery.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ = epagelmatiki_moria;
            }
            // this must be in a try-catch
            cm.CommitData(db);
            //db.SubmitChanges();
        }

        /// <summary>
        /// Καταχωρεί τα συνολικά μόρια από ελεύθερο επάγγελμα
        /// στο αντίστοιχο πεδίο του πίνακα ΑΙΤΗΣΗ.
        /// </summary>
        public void EpagelmaMoriaToAitisi()
        {
            short epagelma_moria = 0;
            // get the teaching total moria from the View
            var epagelmaquery = (from d in db.ΕΕΠΑΓΓΕΛΜΑ_ΣΥΝΟΛΟ_ΜΟΡΙΑs
                                     where d.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                                     select new { d.ΣΥΝΟΛΟ_ΜΟΡΙΑ }).FirstOrDefault();
            var aitisiquery = (from a in db.ΑΙΤΗΣΗs
                               where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                               select a).First();

            // έλεγχος αν υπάρχει εμπειρία ελ. επαγγέλματος
            if (epagelmaquery == null)
            {
                aitisiquery.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ = 0;
            }
            else
            {
                epagelma_moria = (short)epagelmaquery.ΣΥΝΟΛΟ_ΜΟΡΙΑ;
                //store the value in the corresponding field of ΑΙΤΗΣΗ
                aitisiquery.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ = epagelma_moria;
            }        
            // this must be in a try-catch
            cm.CommitData(db);
            //db.SubmitChanges();
        }

        /// <summary>
        /// Καταχωρεί τα μόρια μεταπτυχιακών και παιδαγωγικού στα
        /// αντίστοιχα πεδία του πίνακα ΑΙΤΗΣΗ.
        /// </summary>
        public void PostgradPaidagMoriaToAitisi()
        {
            short msc_moria = 0;
            short phd_moria = 0;
            short paidag_moria = 0;

            // Βρίσκουμε από τον πίνακα της Προκήρυξης τα μόρια που αντιστοιχούν
            // στα μεταπτυχιακά και παιδαγωγικά (εάν αυτά λαμβάνονται υπόψη)
            var prokquery = (from p in db.ΠΡΟΚΗΡΥΞΗs
                                  where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == SelectedAitisi.AitisiProkirixi
                                  select new { p.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ, p.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ, p.ΠΑΙΔΑΓΩΓΙΚΟ_ΠΡΟΣΜ, p.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ }).FirstOrDefault();

            int _msc_moria = Convert.ToInt32(prokquery.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ);
            int _phd_moria = Convert.ToInt32(prokquery.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ);
            int _paid_moria = Convert.ToInt32(prokquery.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ);

            msc_moria = (short)_msc_moria;
            phd_moria = (short)_phd_moria;
            paidag_moria = (short)_paid_moria;

            // store the values in the corresponding fields of ΑΙΤΗΣΗ
            var aitisiquery = (from a in db.ΑΙΤΗΣΗs
                               where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                               select a).First();

            aitisiquery.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ = (short)((aitisiquery.ΜΕΤΑΠΤΥΧΙΑΚΟ == true) ? msc_moria : 0);
            aitisiquery.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ = (short)((aitisiquery.ΔΙΔΑΚΤΟΡΙΚΟ == true) ? phd_moria : 0);

            if (prokquery.ΠΑΙΔΑΓΩΓΙΚΟ_ΠΡΟΣΜ == true)
            {
                aitisiquery.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ = (short)((aitisiquery.ΠΑΙΔΑΓΩΓΙΚΟ == true) ? paidag_moria : 0);
            }
            else aitisiquery.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ = 0;

            cm.CommitData(db);
            //db.SubmitChanges();
        }

        
        /*
         * -----------------
         * Ο υπολογισμός βασίζεται στις τιμές των πεδίων του πίνακα
         * ΑΙΤΗΣΗ. Γι'αυτό προϋποθέτει ότι έχουν προηγούμενα κληθεί
         * οι μέθοδοι καταχώρησης των μορίων από προϋπηρεσίες,
         * μεταπτυχιακά/παιδαγωγικά, ώστε να αντλήσει σωστά δεδομένα.
         * -----------------
         */
        /// <summary>
        /// Υπολογίζει και καταχωρεί τα συνολικά μόρια στο αντίστοιχο
        /// πεδίο του πίνακα ΑΙΤΗΣΗ.
        /// </summary>
        public void TotalMoriaToAitisi()
        {
            short total_moria = 0;
            int mscphd_moria = 0;

            var aitisiquery = (from a in db.ΑΙΤΗΣΗs
                               where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                               select a).First();

            // this is required to avoid null values which throw exception
            int didaktiki_moria = Convert.ToInt32(aitisiquery.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ);
            int epaggelma_moria = Convert.ToInt32(aitisiquery.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ);
            int freelance_moria = Convert.ToInt32(aitisiquery.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ);
            int postgrad_moria = Convert.ToInt32(aitisiquery.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ);
            int didaktor_moria = Convert.ToInt32(aitisiquery.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ);
            int paidagog_moria = Convert.ToInt32(aitisiquery.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ);

            if (postgrad_moria > 0) mscphd_moria = postgrad_moria;
            if (didaktor_moria > 0) mscphd_moria = didaktor_moria;

            total_moria = (short)(didaktiki_moria + epaggelma_moria + freelance_moria +
                                  mscphd_moria + paidagog_moria);

            aitisiquery.ΜΟΡΙΑ = total_moria;
            
            // this must be in a try-catch
            cm.CommitData(db);
            //db.SubmitChanges();
        }

        public void AddPreviousMoriaToAitisi()
        {
            short total_moria = 0;
            int mscphd_moria = 0;

            var aitisiquery = (from a in db.ΑΙΤΗΣΗs
                               where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                               select a).First();

            // this is required to avoid null values which throw exception
            int didaktiki_moria = Convert.ToInt32(aitisiquery.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ);
            int epaggelma_moria = Convert.ToInt32(aitisiquery.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ);
            int freelance_moria = Convert.ToInt32(aitisiquery.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ);
            int postgrad_moria = Convert.ToInt32(aitisiquery.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ);
            int didaktor_moria = Convert.ToInt32(aitisiquery.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ);
            int paidagog_moria = Convert.ToInt32(aitisiquery.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ);

            if (postgrad_moria > 0) mscphd_moria = postgrad_moria;
            if (didaktor_moria > 0) mscphd_moria = didaktor_moria;

            total_moria = (short)(didaktiki_moria + epaggelma_moria + freelance_moria +
                                  mscphd_moria + paidagog_moria);

            aitisiquery.ΜΟΡΙΑ = total_moria;

            int prev_prokirixi_id = GetPreviousProkirixiId();

            var prev_aitisi = (from pa in db.ΑΙΤΗΣΗs
                               where pa.ΠΡΟΚΗΡΥΞΗ == prev_prokirixi_id
                               && pa.ΑΦΜ == SelectedAitisi.TeacherAfm
                               && pa.ΙΕΚ_ΑΙΤΗΣΗΣ == SelectedAitisi.AitisiIekId
                               && pa.ΕΙΔΙΚΟΤΗΤΑ == SelectedAitisi.AitisiEidikotitaId
                               select pa).FirstOrDefault();

            if (prev_aitisi == null)
            {
                UserFunctions.ShowAdminMessage("Δεν βρέθηκε αντίστοιχη αίτηση σε προηγούμενη προκήρυξη.");
            }
            else
            {

                didaktiki_moria = Convert.ToInt32(aitisiquery.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ) + 
                                  Convert.ToInt32(prev_aitisi.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ);
                epaggelma_moria = Convert.ToInt32(aitisiquery.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ) +
                                  Convert.ToInt32(prev_aitisi.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ);
                freelance_moria = Convert.ToInt32(aitisiquery.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ) +
                                  Convert.ToInt32(prev_aitisi.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ);
                
                total_moria = (short)(didaktiki_moria + epaggelma_moria + freelance_moria +
                                  mscphd_moria + paidagog_moria);

                // must refresh (set) the Aitisi fields
                aitisiquery.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ = (short)didaktiki_moria;
                aitisiquery.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ = (short)epaggelma_moria;
                aitisiquery.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ = (short)freelance_moria;
                aitisiquery.ΜΟΡΙΑ = total_moria;
            }

            cm.CommitData(db);
        }

        public int GetPreviousProkirixiId()
        {
            int previous_schoolYear = SelectedAitisi.SchoolYear - 1;
            //UserFunctions.ShowAdminMessage("Προηγούμενο σχ.έτος=" + previous_schoolYear);

            var previous_prok_query = (from pp in db.ΠΡΟΚΗΡΥΞΗs
                                       where pp.ΣΧΟΛΙΚΟ_ΕΤΟΣ == previous_schoolYear
                                       select new { pp.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ }).FirstOrDefault();

            int previous_prok_id = previous_prok_query.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
            //UserFunctions.ShowAdminMessage("Προηγούμενη προκήρυξη:" + previous_prok_id);

            return previous_prok_id;
        }


        /// <summary>
        /// Ελέγχει αν υπάρχει ανοικτή προκήρυξη
        /// </summary>
        /// <returns>true,false</returns>

        public Boolean CheckOpenProkirixi()
        {
            var prok_query = (from p in db.ΠΡΟΚΗΡΥΞΗs
                              where p.ΚΑΤΑΣΤΑΣΗ == 1
                              select new { p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ }).FirstOrDefault();
            if (prok_query == null)
                return false;
            else
                return true;
        }

        /* -------------------
         * Η μέθοδος αυτή κάνει επιλεκτική φόρτωση των δεδομένων από τον πίνακα ΑΙΤΗΣΗ, 
         * ανάλογα με το ποιος χρήστης έχει κάνει σύνδεση στην εφαρμογή.
         * Δέχεται ως παράμετρο το UserKey το οποίο είναι γνωστό μετά το login από το
         * Userid (κλάση GlobalClass.Userid). Το UserKey ανακτάται από την ιδιότητα
         * GlobalClass.UserKey
         * DATE: 4/5/12
         * -------------------
         */

        /// <summary>
        /// Φορτώνει τις Αιτήσεις που ανήκουν στον τρέχοντα χρήστη.
        /// </summary>
        /// <returns></returns>
        public IList<ΑΙΤΗΣΗ> LoadAitisiData()
        {
            ThetisDataContext db = new ThetisDataContext();
            IList<ΑΙΤΗΣΗ> aitiseis = null;
            int prokirixi_id;
            int user_key = LoginClass.UserKey;

            // αναζήτηση ανοικτής προκήρυξης
            var prok_query = (from p in db.ΠΡΟΚΗΡΥΞΗs
                              where p.ΚΑΤΑΣΤΑΣΗ == 1
                              select new { p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ }).FirstOrDefault();
            
            // εάν δεν βρέθηκε ανοικτή προκήρυξη μήνυμα
            if (prok_query == null)
            {
                return null;
            }
            
            prokirixi_id = prok_query.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;                 

            if (user_key > 0)   // iek logged in
            {
                var aitiseis_data = from dba in db.ΑΙΤΗΣΗs
                                    where dba.ΙΕΚ_ΑΙΤΗΣΗΣ == user_key && dba.ΠΡΟΚΗΡΥΞΗ == prokirixi_id
                                    orderby dba.ΠΡΟΚΗΡΥΞΗ, dba.ΚΩΔ_ΑΙΤΗΣΗ
                                    select dba;
                
                aitiseis = aitiseis_data.ToList();

            }
            else                // admin logged in
            {
                var aitiseis_data = from dba in db.ΑΙΤΗΣΗs
                                    where dba.ΠΡΟΚΗΡΥΞΗ == prokirixi_id
                                    orderby dba.ΠΡΟΚΗΡΥΞΗ, dba.ΚΩΔ_ΑΙΤΗΣΗ
                                    select dba;
                aitiseis = aitiseis_data.ToList();
            }

            return aitiseis;
        }

        /// <summary>
        /// Φορτώνει τις Αιτήσεις που ανήκουν στον τρέχοντα χρήστη 
        /// και για συγκεκριμένο εκπαιδευτικό.
        /// </summary>
        /// <param name="afm"></param>
        /// <returns></returns>
        public IList<ΑΙΤΗΣΗ> LoadAitisiData(string afm)
        {
            ThetisDataContext db = new ThetisDataContext();
            IList<ΑΙΤΗΣΗ> aitiseis = null;
            int prokirixi_id;
            int user_key = LoginClass.UserKey;

            // αναζήτηση ανοικτής προκήρυξης
            var prok_query = (from p in db.ΠΡΟΚΗΡΥΞΗs
                              where p.ΚΑΤΑΣΤΑΣΗ == 1
                              select new { p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ }).FirstOrDefault();

            // εάν δεν βρέθηκε ανοικτή προκήρυξη μήνυμα
            if (prok_query == null)
            {
                return null;
            }

            prokirixi_id = prok_query.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;

            if (user_key > 0)   // iek logged in
            {
                var aitiseis_data = from dba in db.ΑΙΤΗΣΗs
                                    where dba.ΙΕΚ_ΑΙΤΗΣΗΣ == user_key && dba.ΠΡΟΚΗΡΥΞΗ == prokirixi_id && dba.ΑΦΜ == afm
                                    orderby dba.ΠΡΟΚΗΡΥΞΗ, dba.ΚΩΔ_ΑΙΤΗΣΗ
                                    select dba;
                aitiseis = aitiseis_data.ToList();
            }
            else                // admin logged in
            {
                var aitiseis_data = from dba in db.ΑΙΤΗΣΗs
                                    where dba.ΑΦΜ == afm && dba.ΠΡΟΚΗΡΥΞΗ == prokirixi_id
                                    orderby dba.ΠΡΟΚΗΡΥΞΗ, dba.ΚΩΔ_ΑΙΤΗΣΗ
                                    select dba;
                aitiseis = aitiseis_data.ToList();
            }

            return aitiseis;
        }

        /// <summary>
        /// Κτίζει τον κωδικό Αίτησης που είναι και το πρωτεύον κλειδί.
        /// </summary>
        /// <param name="aitisi">object ΑΙΤΗΣΗ</param>
        /// <returns>string AitisiCode</returns>
        public string BuildAitisiCode(ΑΙΤΗΣΗ aitisi)
        {
            int iProkirixi = Convert.ToInt32(aitisi.ΠΡΟΚΗΡΥΞΗ);
            int iIek = Convert.ToInt32(aitisi.ΙΕΚ_ΑΙΤΗΣΗΣ);
            int iProtocol = Convert.ToInt32(aitisi.ΠΡΩΤΟΚΟΛΛΟ);

            // if any of these is 0, then we have invalid inputs
            // return empty string so caller can handle it.
            if (iProkirixi == 0 || iIek == 0 || iProtocol == 0) return "";


            // define the corresponding strings
            string sProkirixi = String.Format("{0:00}", iProkirixi);
            string sIek = String.Format("{0:00}", iIek);
            string sProtocol = String.Format("{0:0000}", iProtocol);

            string sAitisiCode = sProkirixi + "-" + sIek + "-" + sProtocol;

            return sAitisiCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aitisiID"></param>
        /// <returns></returns>
        public bool ExistsAitisiCode(string aitisiID)
        {
            using (ThetisDataContext db = new ThetisDataContext())
            {
                var recs_aitiseis = (from a in db.ΑΙΤΗΣΗs
                               where a.ΚΩΔ_ΑΙΤΗΣΗ == aitisiID
                               select a).Count();

                if (recs_aitiseis == 0) return false;
                else
                {
                    // εδώ δείχνουμε στον χρήστη ποιος έχει τον ίδιο
                    // κωδικό αίτησης,
                    var aitisi = (from a in db.ΑΙΤΗΣΗs
                                  where a.ΚΩΔ_ΑΙΤΗΣΗ == aitisiID
                                  select new { a.ΑΦΜ, a.ΠΡΩΤΟΚΟΛΛΟ }).FirstOrDefault();

                    string safm = aitisi.ΑΦΜ;
                    int ap = aitisi.ΠΡΩΤΟΚΟΛΛΟ;

                    // βρίσκουμε τα στοιχεία του εκπαιδευτή
                    var teacher = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                                   where t.ΑΦΜ == safm
                                   select new { t.ΕΠΩΝΥΜΟ, t.ΟΝΟΜΑ }).FirstOrDefault();
                    
                    // build the info string
                    string info_msg;

                    info_msg = String.Format("Ο αρθμός πρωτοκόλλου {0} έχει ήδη δοθεί στον εκπαιδευτικό\n", ap);
                    info_msg = String.Format("{0} {1} {2}", info_msg, teacher.ΕΠΩΝΥΜΟ, teacher.ΟΝΟΜΑ);

                    UserFunctions.ShowAdminMessage(info_msg);

                    return true;
                }
            }
                        
        } // ExistsAitisiCode

        public bool ChangedAitisiCode(string old_value, string new_value)
        {
            if (old_value != new_value)
            {
                string err_msg;
                err_msg = "Δεν μπορει να γίνει αλλαγή του Αρ.Πρωτοκόλλου.\n";
                err_msg += "Διαγράψτε την αίτηση και εισάγετε νέα αίτηση.";
                UserFunctions.ShowAdminMessage(err_msg);
                return true;
            }
            else return false;
        }
    }

    /*
     * ------------------------
     * Η κλάση αυτή έχει ιδιότητες που αφορούν την επιλεγμένη
     * αίτηση. Ο κωδ.αιτήσης γίνεται set από το UI και οι
     * υπόλοιπες ιδιότητες είναι μόνο get και αφορούν στοιχεία
     * της αίτησης (τα βασικά και τα μόρια) και των καθηγητών.
     * ------------------------
     */

    /// <summary>
    /// Οι ιδιότητες της κλάσης αντλούν δεδομένα που
    /// σχετίζονται με την επιλεγμένη αίτηση.
    /// </summary>
    public static class SelectedAitisi
    {
        private static ThetisDataContext db = new ThetisDataContext();

        #region Class Fields
        // aitisi details
        private static string aitisi_id;
        private static string aitisi_protocol;
        private static string aitisi_date;
        private static string aitisi_iek;
        private static int aitisi_iek_id;
        private static string aitisi_eidikotita;
        private static int aitisi_eidikotita_id;
        private static int aitisi_prokirixi;
        
        // teacher details
        private static string fullname;
        private static string afm;
        private static string sex;
        private static string age;
        private static string phone;
        // moria details
        private static short postgrad_moria;
        private static short phd_moria;
        private static short paidagogiko_moria;
        private static short teach_moria;
        private static short prof_moria;
        private static short freelance_moria;
        private static short total_moria;
        // prokirixi details
        private static int school_year;

        #endregion

        #region Aitisi region

        public static string AitisiId
        {
            get { return aitisi_id; }
            set { aitisi_id = value; }
        }

        public static int SchoolYear
        {
            get
            {
                var prokquery = (from p in db.ΠΡΟΚΗΡΥΞΗs
                                 where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == SelectedAitisi.AitisiProkirixi
                                 select new { p.ΣΧΟΛΙΚΟ_ΕΤΟΣ }).FirstOrDefault();

                school_year = Convert.ToInt32(prokquery.ΣΧΟΛΙΚΟ_ΕΤΟΣ);
                return school_year;
            }
        }

        public static int AitisiProkirixi
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΠΡΟΚΗΡΥΞΗ }).FirstOrDefault();

                aitisi_prokirixi = Convert.ToInt32(aitisi.ΠΡΟΚΗΡΥΞΗ);

                return aitisi_prokirixi;
            }
            set { aitisi_prokirixi = value; }
        }

        public static string AitisiProtocol
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΠΡΩΤΟΚΟΛΛΟ }).FirstOrDefault();
                
                aitisi_protocol = aitisi.ΠΡΩΤΟΚΟΛΛΟ.ToString();
                
                return aitisi_protocol;
            }
            set { }
        }

        public static string AitisiDate
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΗΜΕΡΟΜΗΝΙΑ }).FirstOrDefault();
                DateTime dateOnly = (aitisi.ΗΜΕΡΟΜΗΝΙΑ).Date; // this does not work. why?                            
                
                aitisi_date = Convert.ToString(dateOnly);
                aitisi_date = dateOnly.ToString("dd/MM/yyyy");
                
                return aitisi_date;
            }
            set { }
        }

        public static string AitisiIek
        {
            get
            {
                int _aitisi_iek;
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΙΕΚ_ΑΙΤΗΣΗΣ }).FirstOrDefault();
                
                _aitisi_iek = Convert.ToInt32(aitisi.ΙΕΚ_ΑΙΤΗΣΗΣ);

                if (_aitisi_iek > 0)
                {
                    var iek = (from i in db.ΙΕΚs
                               where i.ΙΕΚ_ΚΩΔ == _aitisi_iek
                               select new { i.ΙΕΚ_ΟΝΟΜΑΣΙΑ }).FirstOrDefault();
                    aitisi_iek = Convert.ToString(iek.ΙΕΚ_ΟΝΟΜΑΣΙΑ);
                }
                else
                {
                    aitisi_iek = "Δεν καταχωρήθηκε";
                }
                return aitisi_iek;
            }
            set { }
        }

        public static int AitisiIekId
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΙΕΚ_ΑΙΤΗΣΗΣ }).FirstOrDefault();

                aitisi_iek_id = Convert.ToInt32(aitisi.ΙΕΚ_ΑΙΤΗΣΗΣ);

                return aitisi_iek_id;
            }
            set { }
        }


        public static string AitisiEidikotita
        {
            get
            {
                int _aitisi_eidikotita;
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΕΙΔΙΚΟΤΗΤΑ }).FirstOrDefault();
                _aitisi_eidikotita = Convert.ToInt32(aitisi.ΕΙΔΙΚΟΤΗΤΑ);

                // check for null value (στους αποκλειόμενους δεν έχει καταχωρηθεί ειδικότητα)
                if (_aitisi_eidikotita > 0)
                {
                    var eidikotita = (from e in db.ΕΙΔΙΚΟΤΗΤΑs
                                      where e.ΚΛΑΔΟΣ_ΚΩΔ == _aitisi_eidikotita
                                      select new { e.ΚΛΑΔΟΣ }).FirstOrDefault();
                    aitisi_eidikotita = Convert.ToString(eidikotita.ΚΛΑΔΟΣ);
                }
                else
                {
                    aitisi_eidikotita = "Δεν καταχωρήθηκε";
                }
                return aitisi_eidikotita;
            }
        }

        public static int AitisiEidikotitaId
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΕΙΔΙΚΟΤΗΤΑ }).FirstOrDefault();
                aitisi_eidikotita_id = Convert.ToInt32(aitisi.ΕΙΔΙΚΟΤΗΤΑ);
             
                return aitisi_eidikotita_id;
            }
        }

        #endregion

        #region Teacher region

        public static string TeacherName
        {
            get
            {
                string _afm;
                var aitisi_afm = (from t in db.ΑΙΤΗΣΗs
                                  where t.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                                  select new { t.ΑΦΜ }).FirstOrDefault();
                _afm = aitisi_afm.ΑΦΜ;
                var teacher_name = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                                    where t.ΑΦΜ == _afm
                                    select new { t.ΕΠΩΝΥΜΟ, t.ΟΝΟΜΑ }).FirstOrDefault();
                fullname = teacher_name.ΕΠΩΝΥΜΟ + " " + teacher_name.ΟΝΟΜΑ;
                return fullname;
            }
        }
        public static string TeacherAfm
        {
            get
            {
                string _afm;
                var aitisi_afm = (from t in db.ΑΙΤΗΣΗs
                                  where t.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                                  select new { t.ΑΦΜ }).FirstOrDefault();
                _afm = aitisi_afm.ΑΦΜ;
                var teacher_afm = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                                   where t.ΑΦΜ == _afm
                                   select new { t.ΑΦΜ }).FirstOrDefault();
                afm = teacher_afm.ΑΦΜ;
                return afm;
            }
        }
        public static string TeacherAge
        {
            get
            {
                var aitisi_age = (from t in db.ΑΙΤΗΣΗs
                                  where t.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                                  select new { t.ΗΛΙΚΙΑ }).FirstOrDefault();

                if (aitisi_age.ΗΛΙΚΙΑ > 0)
                {
                    age = Convert.ToString(aitisi_age.ΗΛΙΚΙΑ);
                }
                else
                {
                    age = "Άκυρη ηλικία";
                }
                return age;
            }
        }
        public static string TeacherPhone
        {
            get
            {
                string _afm;
                var aitisi_afm = (from t in db.ΑΙΤΗΣΗs
                                  where t.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                                  select new { t.ΑΦΜ }).FirstOrDefault();
                _afm = aitisi_afm.ΑΦΜ;
                var teacher_phone = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                                     where t.ΑΦΜ == _afm
                                     select new { t.ΤΗΛΕΦΩΝΟ1 }).FirstOrDefault();
                phone = teacher_phone.ΤΗΛΕΦΩΝΟ1;
                return phone;
            }
        }
        public static string TeacherSex
        {
            get
            {
                string _afm;
                int _sex;
                var aitisi_afm = (from t in db.ΑΙΤΗΣΗs
                                  where t.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                                  select new { t.ΑΦΜ }).FirstOrDefault();
                _afm = aitisi_afm.ΑΦΜ;
                var teacher_sex = (from t in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                                   where t.ΑΦΜ == _afm
                                   select new { t.ΦΥΛΟ }).FirstOrDefault();
                _sex = Convert.ToInt32(teacher_sex.ΦΥΛΟ);

                if (_sex > 0)
                {
                    var tsex = (from f in db.ΦΥΛΑs
                                where f.ΚΩΔ_ΦΥΛΟ == _sex
                                select new { f.ΦΥΛΟ }).FirstOrDefault();
                    sex = tsex.ΦΥΛΟ;
                }
                else { sex = "Άγνωστο"; }
                return sex;
            }
        }

        #endregion

        #region Moria region

        public static short PostgradMoria
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ }).FirstOrDefault();

                // required, otherwise exception is thrown for null value
                int pmoria = Convert.ToInt32(aitisi.ΜΕΤΑΠΤΥΧΙΑΚΟ_ΜΟΡΙΑ); // null to zero

                postgrad_moria = (short)pmoria;
                return postgrad_moria;
            }
        }
        public static short PhdMoria
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ }).FirstOrDefault();

                // required, otherwise exception is thrown for null value
                int pmoria = Convert.ToInt32(aitisi.ΔΙΔΑΚΤΟΡΙΚΟ_ΜΟΡΙΑ); // null to zero

                phd_moria = (short)pmoria;
                return phd_moria;
            }
        }
        public static short PaidagogikoMoria
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ }).FirstOrDefault();

                // required, otherwise exception is thrown for null value
                int pmoria = Convert.ToInt32(aitisi.ΠΑΙΔΑΓΩΓΙΚΟ_ΜΟΡΙΑ); // null to zero

                paidagogiko_moria = (short)pmoria;
                return paidagogiko_moria;
            }
        }
        public static short DidaktikiMoria
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ }).FirstOrDefault();

                // required, otherwise exception is thrown for null value
                int tmoria = Convert.ToInt32(aitisi.ΔΙΔΑΚΤΙΚΗ_ΕΜΠΕΙΡΙΑ);

                teach_moria = (short)tmoria;
                return teach_moria;
            }
        }
        public static short EpagelmatikiMoria
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ }).FirstOrDefault();

                // required, otherwise exception is thrown for null value
                int pmoria = Convert.ToInt32(aitisi.ΕΠΑΓΓΕΛΜΑΤΙΚΗ_ΕΜΠΕΙΡΙΑ);

                prof_moria = (short)pmoria;
                return prof_moria;
            }
        }
        public static short EpagelmaMoria
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ }).FirstOrDefault();

                // required, otherwise exception is thrown for null value
                int fmoria = Convert.ToInt32(aitisi.ΕΛΕΥΘΕΡΟ_ΕΠΑΓΓΕΛΜΑ);
                
                freelance_moria = (short)fmoria;
                return freelance_moria;
            }
        }
        public static short TotalMoria
        {
            get
            {
                var aitisi = (from a in db.ΑΙΤΗΣΗs
                              where a.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.aitisi_id
                              select new { a.ΜΟΡΙΑ }).FirstOrDefault();

                // required, otherwise exception is thrown for null value
                int moria = Convert.ToInt32(aitisi.ΜΟΡΙΑ);

                total_moria = (short)moria;
                return total_moria;
            }
        }

        #endregion
    }

    public static class SelectedTeacher
    {
        private static ThetisDataContext db = new ThetisDataContext();
        private static string afm;

        public static string TeacherAFM
        {
            get { return afm; }
            set { afm = value; }
        }
    }

    public class SelectedProkirixi
    {
        private static ThetisDataContext db = new ThetisDataContext();

        public short GetProkirixiStatus(int prokirixi_id)
        {

            var prok_status = (from p in db.ΠΡΟΚΗΡΥΞΗs
                               where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == prokirixi_id
                               select new { p.ΚΑΤΑΣΤΑΣΗ }).FirstOrDefault();

            return (short)prok_status.ΚΑΤΑΣΤΑΣΗ;

        }
    }

}
