using System.Linq;
using System.ComponentModel;
using Telerik.Windows.Data;
using Thetis.Model;

namespace Thetis.AppPages.Aitiseis
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
       

        

    }
}
