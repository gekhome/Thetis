using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using Thetis.Model;
using Thetis.Utilities;
using Thetis.Controls;

using System.Text;

namespace Thetis.DataAccess
{
    class CommitModel
    {
        //private ThetisDataContext db = new ThetisDataContext();

        /// <summary>
        /// Διαχειρίζεται την αποθήκευση δεδομένων στη Βάση.
        /// Παγιδεύει εξαιρέσεις γενικές ή ταυτοχρονισμού.
        /// </summary>
        public void CommitData(ThetisDataContext db)
        {
            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException)
            {
                foreach (ObjectChangeConflict o in db.ChangeConflicts)
                {
                    o.Resolve(RefreshMode.KeepChanges);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                string emsg = "Προέκυψε γενικό σφάλμα: " + "\n";
                emsg += ex.Message;
                UserFunctions.ShowAdminMessage(emsg);
            }
        }

        /// <summary>
        /// Ελέγχει αν ο συνδεδεμένος χρήστης έχει δικαίωμα ενημέρωσης δεδομένων
        /// </summary>
        /// <returns></returns>
        public bool UserCanUpdate()
        {
            int user_key = LoginClass.UserKey;
            bool can_update = false;

            if (LoginClass.UserKey > 0)
            {
                UserFunctions.ShowAdminMessage("Επεξεργασία, αποθήκευση δυνατή μόνο από διαχειριστές.");
                can_update = false;
            }
            else can_update = true;

            return can_update;

        }

        /// <summary>
        /// Ειδοποιεί τον χρήστη ότι δεν μπορεί να αλλάξει δεδομένα αν δεν είναι διαχειριστής
        /// </summary>
        public void AdminMessage()
        {
            if (LoginClass.UserKey > 0)
            {
                string msg = "Δεν μπορείτε να κάνετε αλλαγές στα δεδομένα της ";
                msg += "σελίδας αυτής διότι δεν έχετε κάνει είσοδο ως διαχειριστής.";
                //UserFunctions.ShowAdminMessage(msg);
                UserFunctions.ShowAdminMessage(msg);
            }
        }
        public bool PermitGrant()
        {
            if (LoginClass.UserKey > 0)
            {
                string msg = "Δεν μπορείτε να έχετε πρόσβαση στα δεδομένα της ";
                msg += "σελίδας αυτής διότι δεν έχετε κάνει είσοδο ως διαχειριστής.";
                //UserFunctions.ShowAdminMessage(msg);
                UserFunctions.ShowAdminMessage(msg);
                return false;
            }
            else return true;
        }

    }
}
