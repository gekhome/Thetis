using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Linq;
using System.Text;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.DataAccess;
using Thetis.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Windows;
using System.Windows.Controls;



namespace Thetis.DataAccess
{
    
    class Primus
    {
        private ThetisDataContext db = new ThetisDataContext();
        //private ObservableCollection<ΕΚΠ_ΔΙΔΑΚΤΙΚΗ> didaktiki2Add;
        private CommitModel cm = new CommitModel();

        /// <summary>
        /// Επιστρέφει την ημερομηνία λήξης της προκήρυξης. Χρησιμοποιείται
        /// για τους υπολογισμούς 15ετίας προϋπηρεσιών.
        /// </summary>
        /// <param name="prokirixi_id"></param>
        /// <returns></returns>
        public DateTime RefDate(int prokirixi_id)
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

        /// <summary>
        /// Επιστρέφει την έγκυρη ημερομηνία έναρξης προσμέτρησης των προϋπηρεσιών
        /// Αφαιρούνται Ν έτη (σύμφωνα με την προκήρυξη) από την ημερομηνία λήξης 
        /// της προκήρυξης.
        /// </summary>
        /// <param name="prokirixi_id"></param>
        /// <param name="years"></param>
        /// <returns>limitDate</returns>
        public DateTime LimitDate(int prokirixi_id)
        {
            DateTime refDate = new DateTime(1, 1, 1);
            DateTime limitDate = new DateTime(1,1,1);
            
            var prokirixi = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == prokirixi_id
                             select new { p.ΗΜΝΙΑ_ΛΗΞΗ, p.ΠΡΟΣΜ_ΕΤΗ }).FirstOrDefault();
            
            var years = (short)prokirixi.ΠΡΟΣΜ_ΕΤΗ;

            try
            {
                refDate = Convert.ToDateTime(prokirixi.ΗΜΝΙΑ_ΛΗΞΗ);
            }
            catch (System.NullReferenceException)
            {
                UserFunctions.ShowAdminMessage("Δεν βρέθηκε προκήρυξη ή ημερομηνία λήξης.");
                refDate = new DateTime(1, 1, 1);
            }

            limitDate = refDate.AddYears(-years);
            return limitDate;
        }

        /// <summary>
        /// Επιστρέφει τον κωδικό της ανοικτής προκήρυξης
        /// </summary>
        /// <returns></returns>
        public int ProkirixiID()
        {
            int prokirixi_id;
            var prokirixis = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΚΑΤΑΣΤΑΣΗ == 1
                             orderby p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ
                             select new { p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ }).FirstOrDefault();
            
            prokirixi_id = prokirixis.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ;
            return prokirixi_id;
        }

        /// <summary>
        /// Ελέγχει αν το διάστημα της επαγγελματικής προϋπηρεσίας είναι
        /// έγκυρο, δηλ. μέσα στα χρονικά όρια της προκήρυξης. Επιστρέφει
        /// 0 ή 1 ή 2 ανάλογα με το αποτέλεσμα ελέγχου.
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public int ValidateDates(DateTime d1, DateTime d2)
        {
            int state=0;
            int prokirixi_id = ProkirixiID();
            DateTime refDate = RefDate(prokirixi_id);
            DateTime limitDate = LimitDate(prokirixi_id);

            if (d1 >= limitDate && d2 <= refDate)
                state = 0;  //dates validate, both inside valid timespan
            else if (d1 < limitDate && d2 < limitDate)
                state = 1; // dates totally outside valid timespan
            else if (d1 < limitDate && d2 > limitDate) 
                state = 2; // dates partially outside valid timespan

            return state;
        }

        /// <summary>
        /// Επιστρέφει τον αριθμό προσμετρήσιμων ετών για
        /// την ανοικτή προκήρυξη.
        /// </summary>
        /// <returns></returns>
        public short YearsToCount()
        {
            int prokirixi_id = ProkirixiID();
            var prokirixi = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == prokirixi_id
                             select new {p.ΠΡΟΣΜ_ΕΤΗ}).FirstOrDefault();

            var years = (short)prokirixi.ΠΡΟΣΜ_ΕΤΗ;
            return years;
        }

        /// <summary>
        /// Ελέγχει εάν ένα έτος είναι εντός του χρονικού ορίου (-Ν έτη) της προκήρυξης.
        /// </summary>
        /// <param name="the_year"></param>
        /// <returns></returns>
        public bool ValidateYear(short the_year)
        {
            short limitYear;
            short prokirixiYear;
            int prokirixi_id = ProkirixiID();
            var prokirixi = (from p in db.ΠΡΟΚΗΡΥΞΗs
                             where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == prokirixi_id
                             select new { p.ΠΡΟΣΜ_ΕΤΗ, p.ΗΜΝΙΑ_ΕΝΑΡΞΗ }).FirstOrDefault();
            
            // εδώ πρέπει να εξαχθεί το έτος της προκήρυξης
            DateTime prokirixiStartDate = (DateTime)prokirixi.ΗΜΝΙΑ_ΕΝΑΡΞΗ;
            prokirixiYear = (short)prokirixiStartDate.Year;

            short years = (short)prokirixi.ΠΡΟΣΜ_ΕΤΗ;
            limitYear = (short)(prokirixiYear - years);

            if (the_year < limitYear) return false;
            else return true;
            
        }

        /// <summary>
        /// Εξετάζει αν υπάρχουν προηγούμενες αιτήσεις για μεταφορά,
        /// σύμφωνα με τα κριτήρια του Primus.
        /// </summary>
        /// <returns></returns>
        public bool Aitiseis2TransferExist()
        {
            bool transfer_status = false;
            int current_prokirixi = SelectedAitisi.AitisiProkirixi;
            string safm = SelectedAitisi.TeacherAfm;
            int iek = SelectedAitisi.AitisiIekId;
            int eidikotita = SelectedAitisi.AitisiEidikotitaId;

            var aitiseis = (from a in db.ΑΙΤΗΣΗs
                            where a.ΠΡΟΚΗΡΥΞΗ != current_prokirixi && 
                            (a.ΑΦΜ == safm && a.ΙΕΚ_ΑΙΤΗΣΗΣ==iek && a.ΕΙΔΙΚΟΤΗΤΑ==eidikotita)
                            select a).Count();

            if (aitiseis == 0) transfer_status = false;
            else transfer_status = true;

            return transfer_status;
        }

        /// <summary>
        /// Εξετάζει αν στην τρέχουσα προκήρυξη ισχύει η μεταφορά προϋπηρεσιών.
        /// </summary>
        /// <returns></returns>
        public bool MustTransfer()
        {
            var prokquery = (from p in db.ΠΡΟΚΗΡΥΞΗs
                            where p.ΚΑΤΑΣΤΑΣΗ == 1
                            select new { p.ΥΠΟΒΟΛΗ }).FirstOrDefault();

            if (prokquery.ΥΠΟΒΟΛΗ == true) return true;
            else
            {
                string msg = "Για την τρέχουσα προκήρυξη υποβάλλονται όλα τα δικαιολογητικά.\n";
                msg += "Η λειτουργία μεταφοράς προϋπηρεσιών ακυρώθηκε.";

                UserFunctions.ShowAdminMessage(msg);
                return false;
            }
        }

        public bool ResetTransferStatus(ThetisDataContext db)
        {
            string current_aitisiID = SelectedAitisi.AitisiId;
            bool doit = true;

            var sel_aitisi = (from a in db.ΑΙΤΗΣΗs
                                 where a.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                 select a).FirstOrDefault();

            string strMsg = "Η μεταφορά έχει ήδη γίνει.\n";
            strMsg += "Θέλετε να γίνει εκ νέου μεταφορά ;";

            if (MessageBox.Show(strMsg, "Μεταφορά Προϋπηρεσιών", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            { doit = false; }
            else
            {
                ΑΙΤΗΣΗ Aitisi = new ΑΙΤΗΣΗ();

                sel_aitisi.ΜΕΤΑΦΕΡΘΗΚΕ = false;
                Aitisi.ΜΕΤΑΦΕΡΘΗΚΕ = false;
                cm.CommitData(db);
                doit = true;
                UserFunctions.RadWindowAlert("Μεταφέρθηκε = " + Aitisi.ΜΕΤΑΦΕΡΘΗΚΕ);
             }
            return doit;
        }


        /// <summary>
        /// Επιστρέφει τον αριθμό (λεκτικό) της αντίστοιχης προκήρυξης για μια Αίτηση.
        /// </summary>
        /// <param name="aitisiID"></param>
        /// <returns></returns>
        public string GetProkirixiName(string aitisiID)
        {
            var prokirixi = (from a in db.ΑΙΤΗΣΗs
                             where a.ΚΩΔ_ΑΙΤΗΣΗ == aitisiID
                             select new { a.ΠΡΟΚΗΡΥΞΗ }).FirstOrDefault();

            var prokname = (from p in db.ΠΡΟΚΗΡΥΞΗs
                            where p.ΠΡΟΚΗΡΥΞΗ_ΚΩΔ == prokirixi.ΠΡΟΚΗΡΥΞΗ
                            select new { p.ΠΡΟΚΗΡΥΞΗ_ΑΡ }).FirstOrDefault();

            string prokirixi_name = prokname.ΠΡΟΚΗΡΥΞΗ_ΑΡ;
            return prokirixi_name;
        }

        /// <summary>
        /// Μεταφέρει (προσθέτει) προηγούμενες προϋπηρεσίες διδακτικής σε αυτές της τρέχουσας Αίτησης.
        /// </summary>
        public void TransferDidaktikes(ThetisDataContext db)
        {
            int current_prokirixi = SelectedAitisi.AitisiProkirixi;
            string current_aitisiID = SelectedAitisi.AitisiId;
            string safm = SelectedAitisi.TeacherAfm;
            int iek = SelectedAitisi.AitisiIekId;
            int eidikotita = SelectedAitisi.AitisiEidikotitaId;

            var aitiseis = from a in db.ΑΙΤΗΣΗs
                           where a.ΠΡΟΚΗΡΥΞΗ != current_prokirixi &&
                           (a.ΑΦΜ == safm && a.ΙΕΚ_ΑΙΤΗΣΗΣ == iek && a.ΕΙΔΙΚΟΤΗΤΑ == eidikotita)
                           select a;

            if (Aitiseis2TransferExist() == false)
            {
                string msg = "Δεν βρέθηκαν αντίστοιχες αιτήσεις για μεταφορά,\n";
                msg += "ή έχει γίνει ήδη μεταφορά.";
                UserFunctions.ShowAdminMessage(msg);
                return;
            }

            object sel_aitisi = (from a in db.ΑΙΤΗΣΗs
                                 where a.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                 select a).FirstOrDefault();

            //for each aitisi of the current teacher find his didaktikes
            foreach (var aitisi in aitiseis)
            {
                // get the old aitisiID
                string old_aitisiID = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

                //find the didaktikes of the current (old) aitisi
                var old_didaktikes = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                                     where d.ΚΩΔ_ΑΙΤΗΣΗ == old_aitisiID
                                     select d;
                
                var new_didaktikes = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                                     where d.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                     select d;

                if (old_didaktikes.ToList().Count() != 0)
                {
                    foreach (var didaktiki in old_didaktikes)
                    {
                        // change the temporary didaktiki to new aitisiID
                        ΕΚΠ_ΔΙΔΑΚΤΙΚΗ tempDidaktiki = new ΕΚΠ_ΔΙΔΑΚΤΙΚΗ()
                        {
                            ΑΙΤΗΣΗ = (ΑΙΤΗΣΗ)sel_aitisi,
                            ΕΝΑΡΞΗ = didaktiki.ΕΝΑΡΞΗ,
                            ΕΡΓΑΣΙΑ = didaktiki.ΕΡΓΑΣΙΑ,
                            ΚΛΑΔΟΣ = didaktiki.ΚΛΑΔΟΣ,
                            ΚΩΔ_ΑΙΤΗΣΗ = current_aitisiID,
                            ΛΗΞΗ = didaktiki.ΛΗΞΗ,
                            ΠΕΡΙΓΡΑΦΗ = didaktiki.ΠΕΡΙΓΡΑΦΗ,
                            ΣΥΝ_ΩΡΕΣ = didaktiki.ΣΥΝ_ΩΡΕΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = didaktiki.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΩΡΕΣ_ΕΒΔ = didaktiki.ΩΡΕΣ_ΕΒΔ,
                            ΩΡΕΣ_ΣΥΝ = didaktiki.ΩΡΕΣ_ΣΥΝ
                        };

                        // add the old didaktikes to the original collection
                        new_didaktikes.ToList().Add(tempDidaktiki);
                    } // foreach didaktiki loop

                } // end if
                else
                {                    
                    string prokirixi_name = GetProkirixiName(old_aitisiID);
                    UserFunctions.ShowAdminMessage("Δεν βρέθηκαν διδακτικές προϋπηρεσίες για την προκήρυξη: " + prokirixi_name);
                }
            } // foreach aitisi loop
            //var didaktikes = new ObservableCollection<ΕΚΠ_ΔΙΔΑΚΤΙΚΗ>(new_didaktikes.ToList());
        }

        /// <summary>
        /// Μεταφέρει (προσθέτει) προηγούμενες προϋπηρεσίες επαγγελματικής σε αυτές της τρέχουσας Αίτησης.
        /// </summary>
        /// <param name="db"></param>
        public void TransferEpagelmatikes(ThetisDataContext db)
        {
            int current_prokirixi = SelectedAitisi.AitisiProkirixi;
            string current_aitisiID = SelectedAitisi.AitisiId;
            string safm = SelectedAitisi.TeacherAfm;
            int iek = SelectedAitisi.AitisiIekId;
            int eidikotita = SelectedAitisi.AitisiEidikotitaId;

            var aitiseis = from a in db.ΑΙΤΗΣΗs
                           where a.ΜΕΤΑΦΕΡΘΗΚΕ == false && a.ΠΡΟΚΗΡΥΞΗ != current_prokirixi &&
                           (a.ΑΦΜ == safm && a.ΙΕΚ_ΑΙΤΗΣΗΣ == iek && a.ΕΙΔΙΚΟΤΗΤΑ == eidikotita)
                           select a;
            
            if (Aitiseis2TransferExist() == false)
            {
                string msg = "Δεν βρέθηκαν αντίστοιχες αιτήσεις για μεταφορά,\n";
                msg += "ή έχει γίνει ήδη μεταφορά.";
                UserFunctions.ShowAdminMessage(msg);
                return;
            }
            object sel_aitisi = (from a in db.ΑΙΤΗΣΗs
                                 where a.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                 select a).FirstOrDefault();

            //for each aitisi of the current teacher find his epagelmatikes
            foreach (var aitisi in aitiseis)
            {
                // get the old aitisiID
                string old_aitisiID = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

                //find the epagelmatikes of the current (old) aitisi
                var old_epagelmatikes = from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                                     where e.ΚΩΔ_ΑΙΤΗΣΗ == old_aitisiID
                                     select e;

                var new_epagelmatikes = from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                                     where e.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                     select e;

                if (old_epagelmatikes.ToList().Count() != 0)
                {
                    foreach (var epagelmatiki in old_epagelmatikes)
                    {
                        // change the temporary epagelmatiki to new aitisiID
                        ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ tempEpagelmatiki = new ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ()
                        {
                            ΑΙΤΗΣΗ = (ΑΙΤΗΣΗ)sel_aitisi,
                            ΚΩΔ_ΑΙΤΗΣΗ = current_aitisiID,
                            ΕΝΑΡΞΗ = epagelmatiki.ΕΝΑΡΞΗ,
                            ΛΗΞΗ = epagelmatiki.ΛΗΞΗ,
                            ΠΕΡΙΓΡΑΦΗ = epagelmatiki.ΠΕΡΙΓΡΑΦΗ,
                            ΗΜΕΡΕΣ = epagelmatiki.ΗΜΕΡΕΣ
                        };

                        // add the old epagelmatikes to the original collection
                        new_epagelmatikes.ToList().Add(tempEpagelmatiki);
                    } // foreach epagelmatiki loop

                } // end if
                else
                {
                    string prokirixi_name = GetProkirixiName(old_aitisiID);
                    UserFunctions.ShowAdminMessage("Δεν βρέθηκαν επαγγελματικές προϋπηρεσίες για την προκήρυξη: " + prokirixi_name);
                }
            } // foreach aitisi loop
            //var epagelmatikes = new ObservableCollection<ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ>(new_epagelmatikes.ToList());
        }

        /// <summary>
        /// Μεταφέρει (προσθέτει) προηγούμενες προϋπηρεσίες ελ. επαγγέλματος σε αυτές της τρέχουσας Αίτησης.
        /// </summary>
        /// <param name="db"></param>
        public void TransferFreelance(ThetisDataContext db)
        {
            int current_prokirixi = SelectedAitisi.AitisiProkirixi;
            string current_aitisiID = SelectedAitisi.AitisiId;
            string safm = SelectedAitisi.TeacherAfm;
            int iek = SelectedAitisi.AitisiIekId;
            int eidikotita = SelectedAitisi.AitisiEidikotitaId;

            var aitiseis = from a in db.ΑΙΤΗΣΗs
                           where a.ΜΕΤΑΦΕΡΘΗΚΕ == false && a.ΠΡΟΚΗΡΥΞΗ != current_prokirixi &&
                           (a.ΑΦΜ == safm && a.ΙΕΚ_ΑΙΤΗΣΗΣ == iek && a.ΕΙΔΙΚΟΤΗΤΑ == eidikotita)
                           select a;

            if (Aitiseis2TransferExist() == false)
            {
                string msg = "Δεν βρέθηκαν αντίστοιχες αιτήσεις για μεταφορά,\n";
                msg += "ή έχει γίνει ήδη μεταφορά.";
                UserFunctions.ShowAdminMessage(msg);
                return;
            }

            object sel_aitisi = (from a in db.ΑΙΤΗΣΗs
                                 where a.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                 select a).FirstOrDefault();

            //for each aitisi of the current teacher find his freelance
            foreach (var aitisi in aitiseis)
            {
                // get the old aitisiID
                string old_aitisiID = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

                //find the freelances of the current (old) aitisi
                var old_freelance = from f in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                                        where f.ΚΩΔ_ΑΙΤΗΣΗ == old_aitisiID
                                        select f;

                var new_freelance = from f in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                                        where f.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                        select f;

                if (old_freelance.ToList().Count() != 0)
                {
                    foreach (var freelance in old_freelance)
                    {
                        // change the temporary freelance to new aitisiID
                        ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ tempFreelance = new ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ()
                        {
                            ΑΙΤΗΣΗ = (ΑΙΤΗΣΗ)sel_aitisi,
                            ΚΩΔ_ΑΙΤΗΣΗ = current_aitisiID,
                            ΟΙΚ_ΕΤΟΣ = freelance.ΟΙΚ_ΕΤΟΣ,
                            ΕΤΟΣ_ΧΡΗΣΗ = freelance.ΕΤΟΣ_ΧΡΗΣΗ,
                            ΕΙΣΟΔΗΜΑ = freelance.ΕΙΣΟΔΗΜΑ,
                            ΝΟΜΙΣΜΑ = freelance.ΝΟΜΙΣΜΑ,
                            ΠΕΡΙΓΡΑΦΗ = freelance.ΠΕΡΙΓΡΑΦΗ,
                            ΕΕ_ΜΟΡΙΑ = freelance.ΕΕ_ΜΟΡΙΑ
                        };

                        // add the old freelance to the original collection
                        new_freelance.ToList().Add(tempFreelance);
                    } // foreach freelance loop

                } // end if
                else
                {
                    string prokirixi_name = GetProkirixiName(old_aitisiID);
                    UserFunctions.ShowAdminMessage("Δεν βρέθηκαν επαγγελματικές προϋπηρεσίες για την προκήρυξη: " + prokirixi_name);
                }
            } // foreach aitisi loop
            //var freelance = new ObservableCollection<ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ>(new_freelance.ToList());
        }


        /// <summary>
        /// Μεταφέρει όλες τις προϋπηρεσίες στην τρέχουσα Αίτηση και 
        /// μαρκάρει τις αντίστοιχες αιτήσεις ότι μεταφέρθηκαν.
        /// </summary>
        /// <param name="db"></param>
        public void TransferAllBatch(ThetisDataContext db)
        {
            // Αν η προκήρυξη δεν προβλέπει μεταφορά μην κάνεις τίποτα.
            if (MustTransfer() == false) 
            {
                return;
            }

            int current_prokirixi = SelectedAitisi.AitisiProkirixi;
            string current_aitisiID = SelectedAitisi.AitisiId;
            string safm = SelectedAitisi.TeacherAfm;
            int iek = SelectedAitisi.AitisiIekId;
            int eidikotita = SelectedAitisi.AitisiEidikotitaId;

            var aitiseis = (from a in db.ΑΙΤΗΣΗs
                           where a.ΠΡΟΚΗΡΥΞΗ != current_prokirixi &&
                           (a.ΑΦΜ == safm && a.ΙΕΚ_ΑΙΤΗΣΗΣ == iek && a.ΕΙΔΙΚΟΤΗΤΑ == eidikotita)
                           select a).ToList();
            ObservableCollection<ΑΙΤΗΣΗ> aitiseis2Transfer = new ObservableCollection<ΑΙΤΗΣΗ>(aitiseis);

            object sel_aitisi = (from a in db.ΑΙΤΗΣΗs
                                 where a.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                 select a).FirstOrDefault();

            //for each aitisi of the current teacher find his didaktikes
            foreach (var aitisi in aitiseis)
            {
                BatchTransferDidaktikes(aitisi, sel_aitisi, current_aitisiID);
                
                BatchTransferEpagelmatikes(aitisi, sel_aitisi, current_aitisiID);

                BatchTransferFreelance(aitisi, sel_aitisi, current_aitisiID);
                
                aitisi.ΜΕΤΑΦΕΡΘΗΚΕ = true;
                cm.CommitData(db);
            } // foreach aitisi loop
        }

        public void BatchTransferDidaktikes(ΑΙΤΗΣΗ aitisi, object sel_aitisi, string current_aitisiID)
        {
            // get the old aitisiID
            string old_aitisiID = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

            //TransferDidaktikes
            //find the didaktikes of the current (old) aitisi
            var old_didaktikes = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                                 where d.ΚΩΔ_ΑΙΤΗΣΗ == old_aitisiID
                                 select d;

            var new_didaktikes = from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                                 where d.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                 select d;

            if (old_didaktikes.ToList().Count() > 0)
            {
                foreach (var didaktiki in old_didaktikes)
                {
                    // change the temporary didaktiki to new aitisiID
                    ΕΚΠ_ΔΙΔΑΚΤΙΚΗ tempDidaktiki = new ΕΚΠ_ΔΙΔΑΚΤΙΚΗ()
                    {
                        ΑΙΤΗΣΗ = (ΑΙΤΗΣΗ)sel_aitisi,
                        ΕΝΑΡΞΗ = didaktiki.ΕΝΑΡΞΗ,
                        ΕΡΓΑΣΙΑ = didaktiki.ΕΡΓΑΣΙΑ,
                        ΚΛΑΔΟΣ = didaktiki.ΚΛΑΔΟΣ,
                        ΚΩΔ_ΑΙΤΗΣΗ = current_aitisiID,
                        ΛΗΞΗ = didaktiki.ΛΗΞΗ,
                        ΠΕΡΙΓΡΑΦΗ = didaktiki.ΠΕΡΙΓΡΑΦΗ,
                        ΣΥΝ_ΩΡΕΣ = didaktiki.ΣΥΝ_ΩΡΕΣ,
                        ΣΧΟΛΙΚΟ_ΕΤΟΣ = didaktiki.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        ΩΡΕΣ_ΕΒΔ = didaktiki.ΩΡΕΣ_ΕΒΔ,
                        ΩΡΕΣ_ΣΥΝ = didaktiki.ΩΡΕΣ_ΣΥΝ
                    };

                    // add the old didaktikes to the original collection
                    new_didaktikes.ToList().Add(tempDidaktiki);
                } // foreach didaktiki loop

            } // end if
       
        }

        public void BatchTransferEpagelmatikes(ΑΙΤΗΣΗ aitisi, object sel_aitisi, string current_aitisiID)
        {
            // get the old aitisiID
            string old_aitisiID = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

            //find the epagelmatikes of the current (old) aitisi
            var old_epagelmatikes = from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                                    where e.ΚΩΔ_ΑΙΤΗΣΗ == old_aitisiID
                                    select e;

            var new_epagelmatikes = from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                                    where e.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                    select e;

            if (old_epagelmatikes.ToList().Count() != 0)
            {
                foreach (var epagelmatiki in old_epagelmatikes)
                {
                    // change the temporary epagelmatiki to new aitisiID
                    ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ tempEpagelmatiki = new ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ()
                    {
                        ΑΙΤΗΣΗ = (ΑΙΤΗΣΗ)sel_aitisi,
                        ΚΩΔ_ΑΙΤΗΣΗ = current_aitisiID,
                        ΕΝΑΡΞΗ = epagelmatiki.ΕΝΑΡΞΗ,
                        ΛΗΞΗ = epagelmatiki.ΛΗΞΗ,
                        ΠΕΡΙΓΡΑΦΗ = epagelmatiki.ΠΕΡΙΓΡΑΦΗ,
                        ΗΜΕΡΕΣ = epagelmatiki.ΗΜΕΡΕΣ
                    };

                    // add the old epagelmatikes to the original collection
                    new_epagelmatikes.ToList().Add(tempEpagelmatiki);
                } // foreach epagelmatiki loop

            } // end if
        }

        public void BatchTransferFreelance(ΑΙΤΗΣΗ aitisi, object sel_aitisi, string current_aitisiID)
        {
            // get the old aitisiID
            string old_aitisiID = aitisi.ΚΩΔ_ΑΙΤΗΣΗ;

            //find the freelances of the current (old) aitisi
            var old_freelance = from f in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                                where f.ΚΩΔ_ΑΙΤΗΣΗ == old_aitisiID
                                select f;

            var new_freelance = from f in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                                where f.ΚΩΔ_ΑΙΤΗΣΗ == current_aitisiID
                                select f;

            if (old_freelance.ToList().Count() != 0)
            {
                foreach (var freelance in old_freelance)
                {
                    // change the temporary freelance to new aitisiID
                    ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ tempFreelance = new ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ()
                    {
                        ΑΙΤΗΣΗ = (ΑΙΤΗΣΗ)sel_aitisi,
                        ΚΩΔ_ΑΙΤΗΣΗ = current_aitisiID,
                        ΟΙΚ_ΕΤΟΣ = freelance.ΟΙΚ_ΕΤΟΣ,
                        ΕΤΟΣ_ΧΡΗΣΗ = freelance.ΕΤΟΣ_ΧΡΗΣΗ,
                        ΕΙΣΟΔΗΜΑ = freelance.ΕΙΣΟΔΗΜΑ,
                        ΝΟΜΙΣΜΑ = freelance.ΝΟΜΙΣΜΑ,
                        ΠΕΡΙΓΡΑΦΗ = freelance.ΠΕΡΙΓΡΑΦΗ,
                        ΕΕ_ΜΟΡΙΑ = freelance.ΕΕ_ΜΟΡΙΑ
                    };

                    // add the old freelance to the original collection
                    new_freelance.ToList().Add(tempFreelance);
                } // foreach freelance loop

            } // end if

        }

        #region Μέθοδοι επικύρωσης των προϋπηρεσιών
        /// <summary>
        /// Κάνει έλεγχο αν οι διδακτικές είναι έγκυρες, δηλ. υπακούουν στους 3 κανόνες επικύρωσης.
        /// Καλείται πάντα μέσα σε loop από τον caller.
        /// </summary>
        /// <param name="didaktiki"></param>
        /// <returns></returns>
        public bool ValidateDidaktiki(ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktiki)
        {
            int state = 0;
            int error_status = 0; //assume all correct
            bool validates = false;  // assume errors

            int school_year = Convert.ToInt32(didaktiki.ΣΧΟΛΙΚΟ_ΕΤΟΣ);  // a null is converted to 0
            string date1 = didaktiki.ΕΝΑΡΞΗ.ToString();
            string date2 = didaktiki.ΛΗΞΗ.ToString();
            
            // check for null values (throws exception)
            if (didaktiki.ΕΝΑΡΞΗ == null || didaktiki.ΛΗΞΗ == null) return true;

             DateTime startDate = (DateTime)didaktiki.ΕΝΑΡΞΗ;
             DateTime finalDate = (DateTime)didaktiki.ΛΗΞΗ;
            
            short hours = Convert.ToInt16(didaktiki.ΣΥΝ_ΩΡΕΣ);

            // Rule 1: Οι ημερομηνίες Έναρξης, Λήξης πρέπει να είναι εντός του σχολικού έτους
            if (Dates.InSchoolYear(date1, date2, school_year) == false)
            {
                int error_type = 1;
                error_status += 1;
                ErrorSaveDidaktiki(didaktiki, error_type);
            }

            // Rule 2: Οι ημερομηνίες πρέπει να είναι εντός του χρονικού ορίου (π.χ. 15ετία).
            if ((state = ValidateDates(startDate, finalDate)) > 0)
            {
                int error_type = 2;
                error_status += state;
                ErrorSaveDidaktiki(didaktiki, error_type);
            }
            
            // Rule 3: Πιθανές διπλοεγγραφές (ίδιες ημερομηνίες και ώρες)
            if ((state = FindDuplicatesDidaktiki(startDate, finalDate, hours)) > 0)
            {
                int error_type = 3;
                error_status += state;
                ErrorSaveDidaktiki(didaktiki, error_type);
            }

            if (error_status == 0)
            {
                ErrorSaveDidaktiki(didaktiki, error_status);
                validates = true;
            }
            else validates = false;
            return validates;
        } // ValidateDidaktiki

        /// <summary>
        /// Κάνει έλεγχο αν οι επαγγελματικές είναι έγκυρες, δηλ. υπακούουν στους 2 κανόνες επικύρωσης.
        /// </summary>
        /// <param name="epagelmatiki"></param>
        /// <returns></returns>
        public bool ValidateEpagelmatiki(ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ epagelmatiki)
        {
            if (epagelmatiki.ΕΝΑΡΞΗ == null || epagelmatiki.ΛΗΞΗ == null) return true;

            DateTime startDate = (DateTime)epagelmatiki.ΕΝΑΡΞΗ;
            DateTime finalDate = (DateTime)epagelmatiki.ΛΗΞΗ;
            int days = (int)epagelmatiki.ΗΜΕΡΕΣ;

            int state = 0;
            int error_status = 0; //assume all correct
            bool validates = false;  // assume errors

            // Rule 1: Οι ημερομηνίες πρέπει να είναι εντός του χρονικού ορίου (π.χ. 15ετία).
            if ((state = ValidateDates(startDate, finalDate)) > 0)
            {
                int error_type = 1;
                error_status += state;
                ErrorSaveEpagelmatiki(epagelmatiki, error_type);
            }

            // Rule 2: Πιθανές διπλοεγγραφές (ίδιες ημερομηνίες και ημέρες)
            if ((state = FindDuplicatesEpagelmatiki(startDate, finalDate, days)) > 0)
            {
                int error_type = 2;
                error_status += state;
                ErrorSaveEpagelmatiki(epagelmatiki, error_type);
            }
            
            if (error_status == 0) validates = true;
            else validates = false;
            return validates;
        } // ValidateEpagelmatiki

        /// <summary>
        /// Κάνει έλεγχο αν οι προϋπηρεσίες από ελ.επάγγελμα είναι έγκυρες, δηλ. υπακούουν στους 2 κανόνες επικύρωσης.
        /// </summary>
        /// <param name="eepagelma"></param>
        /// <returns></returns>
        public bool ValidateFreelance(ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ eepagelma)
        {
            // check for null values (throws exception)
            if (eepagelma.ΕΤΟΣ_ΧΡΗΣΗ == null || eepagelma.ΕΙΣΟΔΗΜΑ == null) return true;

            int state = 0;
            short year_use = (short)eepagelma.ΕΤΟΣ_ΧΡΗΣΗ;
            float income = (float)eepagelma.ΕΙΣΟΔΗΜΑ;

            int error_status = 0; //assume all correct
            bool validates = true;  // assume no errors

            // Rule 1: Το έτος χρήσης πρέπει να είναι εντός του χρονικού ορίου (π.χ. 15ετία).
            if (ValidateYear(year_use) == false)
            {
                int error_type = 1;
                error_status += 1;
                ErrorSaveEpagelma(eepagelma, error_type);
            } 
            
            // Rule 2: Πιθανές διπλοεγγραφές (ίδιες ημερομηνίες και ώρες)
            if((state = FindDuplicatesFreelance(year_use, income)) > 0)
            {
                int error_type = 2;
                error_status += 1;
                ErrorSaveEpagelma(eepagelma, error_type);
            }

            if (error_status == 0) validates = true;
            else validates = false;
            return validates;
        } // ValidateFreelance

        /// <summary>
        /// Ελέγχει αν υπάρχουν πιθανές διπλοεγγραφές διδακτικής, βάσει των τιμών ΕΝΑΡΞΗ, ΛΗΞΗ, ΣΥΝ_ΩΡΕΣ.
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateFinal"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public int FindDuplicatesDidaktiki(DateTime dateStart, DateTime dateFinal, short hours)
        {
            int error_status=0;

            var didaktiki_qry = (from d in db.ΕΚΠ_ΔΙΔΑΚΤΙΚΗs
                                 where d.ΕΝΑΡΞΗ == dateStart && d.ΛΗΞΗ == dateFinal && d.ΣΥΝ_ΩΡΕΣ == hours
                                        && d.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                                 select d).Count();

            int duplicates = didaktiki_qry;
            if (duplicates > 1) 
            {
                error_status = 1;
            }
            else error_status = 0;
            return error_status;
        }

        /// <summary>
        /// Ελέγχει αν υπάρχουν πιθανές διπλοεγγραφές επαγγελματικής, βάσει των τιμών ΕΝΑΡΞΗ, ΛΗΞΗ, ΗΜΕΡΕΣ.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="finalDate"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public int FindDuplicatesEpagelmatiki(DateTime startDate, DateTime finalDate, int days)
        {
            int error_status = 0;

            var epagelmatiki_qry = (from e in db.ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗs
                                 where e.ΕΝΑΡΞΗ == startDate && e.ΛΗΞΗ == finalDate && e.ΗΜΕΡΕΣ == days
                                        && e.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                                 select e).Count();

            int duplicates = epagelmatiki_qry;
            if (duplicates > 1)
            {
                error_status = 1;
            }
            else error_status = 0;
            return error_status;
        }

        /// <summary>
        /// Ελέγχει αν υπάρχουν πιθανές διπλοεγγραφές επαγγελματικής, βάσει των τιμών ΕΤΟΣ_ΧΡΗΣΗ, ΕΙΣΟΔΗΜΑ.
        /// </summary>
        /// <param name="year_use"></param>
        /// <param name="income"></param>
        /// <returns></returns>
        public int FindDuplicatesFreelance(short year_use, float income)
        {
            int error_status = 0;

            var epagelma_qry = (from e in db.ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑs
                                    where e.ΕΤΟΣ_ΧΡΗΣΗ == year_use && e.ΕΙΣΟΔΗΜΑ == income
                                           && e.ΚΩΔ_ΑΙΤΗΣΗ == SelectedAitisi.AitisiId
                                    select e).Count();

            int duplicates = epagelma_qry;
            if (duplicates > 1)
            {
                error_status = 1;
            }
            else error_status = 0;
            return error_status;

        }

        /// <summary>
        /// Καταχωρεί σε μια εγγραφή διδακτικής την (πιθανή αιτία σφάλματος)
        /// </summary>
        /// <param name="didaktikiRecord"></param>
        /// <param name="error_type"></param>
        public void ErrorSaveDidaktiki(ΕΚΠ_ΔΙΔΑΚΤΙΚΗ didaktikiRecord, int error_type)
        {
            string error_text = "";

            switch (error_type)
            {
                case 1:
                    error_text = "Οι ημερομηνίες έναρξης, λήξης δεν συμβαδίζουν με το σχολικό έτος";
                    break;
                case 2:
                    error_text = "Οι ημερομηνίες είναι μερικώς ή ολικώς εκτός χρονικού ορίου (π.χ.15ετία)";
                    break;
                case 3:
                    error_text = "Πιθανή διπλοεγγραφή (ίδιες ημ/νίες και ώρες)";
                    break;
                default:
                    error_text = "";
                    break;
            }

            // need a change here to reset message in error column if errors corrected and no errors found
            if (error_type == 0)
            {
                string clear_error = "";
                didaktikiRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ = clear_error;
                db.SubmitChanges();
                return;
            }

            string strvalue = didaktikiRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ;

            // store the value in the database
            if (strvalue == null) strvalue = error_text;
            else
            {
                // add the error text only if not already there
                if (strvalue.IndexOf(error_text) == -1) strvalue = strvalue + "," + error_text;
            }
            // remove last character ',' if exists
            string tempstr = "";

            if (strvalue[strvalue.Length - 1] == ',') tempstr = strvalue.TrimEnd(strvalue[strvalue.Length - 1]);
            else tempstr = strvalue;

            const int MaxLength = 300; // set in database
            // make sure no exception is thrown due to string overflow
            if (tempstr.Length > MaxLength) tempstr = tempstr.Substring(0, MaxLength);

            didaktikiRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ = tempstr;
            db.SubmitChanges();
        }

        /// <summary>
        /// Καταχωρεί σε μια εγγραφή επαγγελματικής την (πιθανή αιτία σφάλματος)
        /// </summary>
        /// <param name="didaktikiRecord"></param>
        /// <param name="error_type"></param>
        public void ErrorSaveEpagelmatiki(ΕΚΠ_ΕΠΑΓΓΕΛΜΑΤΙΚΗ epagelmatikiRecord, int error_type)
        {
            string error_text = "";

            switch (error_type)
            {
                case 1:
                    error_text = "Οι ημερομηνίες είναι μερικώς ή ολικώς εκτός χρονικού ορίου (π.χ.15ετία)";
                    break;
                case 2:
                    error_text = "Πιθανή διπλοεγγραφή (ίδιες ημ/νίες και ημέρες)";
                    break;
                default:
                    error_text = "";
                    break;
            }

            // need a change here to reset message in error column if errors corrected and no errors found
            if (error_type == 0)
            {
                string clear_error = "";
                epagelmatikiRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ = clear_error;
                db.SubmitChanges();
                return;
            }


            string strvalue = epagelmatikiRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ;

            // store the value in the database
            if (strvalue == null) strvalue = error_text;
            else
            {
                // add the error text only if not already there
                if (strvalue.IndexOf(error_text) == -1) strvalue = strvalue + "," + error_text;
            }

            // remove last character ',' if exists
            string tempstr = "";
            if (strvalue[strvalue.Length - 1] == ',') tempstr = strvalue.TrimEnd(strvalue[strvalue.Length - 1]);
            else tempstr = strvalue;
            
            const int MaxLength = 300; // set in database
            // make sure no exception is thrown due to string overflow
            if (tempstr.Length > MaxLength) tempstr = tempstr.Substring(0, MaxLength);

            epagelmatikiRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ = tempstr;
            db.SubmitChanges();
        }

        /// <summary>
        /// Καταχωρεί σε μια εγγραφή ελ. επαγγέλματος την (πιθανή αιτία σφάλματος)
        /// </summary>
        /// <param name="didaktikiRecord"></param>
        /// <param name="error_type"></param>
        public void ErrorSaveEpagelma(ΕΚΠ_ΕΕΠΑΓΓΕΛΜΑ epagelmaRecord, int error_type)
        {
            string error_text = "";

            switch (error_type)
            {
                case 1:
                    error_text = "Το έτος χρήσης είναι εκτός χρονικού ορίου (π.χ. 15ετία)";
                    break;
                case 2:
                    error_text = "Πιθανή διπλοεγγραφή (ίδιο έτος χρήσης και εισόδημα)";
                    break;
                default:
                    error_text = "";
                    break;
            }

            // need a change here to reset message in error column if errors corrected and no errors found
            if (error_type == 0)
            {
                string clear_error = "";
                epagelmaRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ = clear_error;
                db.SubmitChanges();
                return;
            }

            string strvalue = epagelmaRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ;

            // store the value in the database
            if (strvalue == null) strvalue = error_text;
            else
            {
                // add the error text only if not already there
                if (strvalue.IndexOf(error_text) == -1) strvalue = strvalue + "," + error_text;
            }

            // remove last character ',' if exists
            string tempstr = "";
            if (strvalue[strvalue.Length - 1] == ',') tempstr = strvalue.TrimEnd(strvalue[strvalue.Length - 1]);
            else tempstr = strvalue;
            
            const int MaxLength = 300; // set in database
            // make sure no exception is thrown due to string overflow
            if (tempstr.Length > MaxLength) tempstr = tempstr.Substring(0, MaxLength);

            epagelmaRecord.ΣΦΑΛΜΑ_ΑΙΤΙΕΣ = tempstr;
            db.SubmitChanges();
        }

        #endregion

    } // Class Primus
} // namespace
