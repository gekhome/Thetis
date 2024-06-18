using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Telerik.Windows.Data;
using System.Collections.ObjectModel;
using Prototype.Model;

namespace Prototype.AppPages.Teachers
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
