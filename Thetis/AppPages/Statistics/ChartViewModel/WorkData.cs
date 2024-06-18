using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Charting;
using Telerik.Windows.Controls;

namespace Thetis.AppPages.Statistics.ChartViewModel
{
    public class WorkData
    {
        private string _work;
        private bool _anergos;
        private string _kladosname;
        private double _count;
        private int _prokirixi;
        private string _iekname;

        public WorkData(int prokirixi, string iekname, string work, bool anergos, string kladosname, double count)
        {
            this._prokirixi = prokirixi;
            this._iekname = iekname;
            this._kladosname = kladosname;
            this._work = work;
            this._anergos = anergos;
            this._count = count;

        }

        public string WorkName
        {
            get { return this._work; }
        }

        public bool Anergos
        {
            get { return this._anergos; }
        }

        public string KladosName
        {
            get { return this._kladosname; }
        }

        public double Count
        {
            get { return this._count; }
        }

        public int Prokirixi
        {
            get { return this._prokirixi; }
            set { this._prokirixi = value; }
        }

        public string IekName
        {
            get { return this._iekname; }
            set { this._iekname = value; }
        }




    }   // WorkData
}
