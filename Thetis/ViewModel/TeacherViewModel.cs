using System.Linq;
using System.ComponentModel;
using Telerik.Windows.Data;
using Thetis.Model;

namespace Thetis.ViewModel
{
    public class TeacherViewModel
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ICollectionView teachers = null;
        public ICollectionView TeacherCollection
        {
            get
            {
                if (this.teachers == null)
                {
                    var trainers = from dbp in db.ΕΚΠΑΙΔΕΥΤΙΚΟΣs
                                   orderby dbp.ΕΠΩΝΥΜΟ, dbp.ΟΝΟΜΑ
                                   select dbp;
                    this.teachers = new QueryableCollectionView(trainers.ToList());
                }
                return this.teachers;
            }
            
        }   
    } // TeacherViewModel

    public class AitisiViewModel
    {
        private ThetisDataContext db = new ThetisDataContext();
        private ICollectionView aitiseis = null;
        public ICollectionView AitisiCollection
        {
            get
            {
                if (this.aitiseis == null)
                {
                    var aitiseis = from dba in db.ΑΙΤΗΣΗs
                                   orderby dba.ΚΩΔ_ΑΙΤΗΣΗ
                                   select dba;
                    this.aitiseis = new QueryableCollectionView(aitiseis.ToList());
                }
                return this.aitiseis;
            }
        }

    } // AitisiViewModel

}
