using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Charting;
using Telerik.Windows.Controls;

namespace Thetis.AppPages.Statistics.ChartViewModel
{
    public class GenderData
    {
        private string _gender;
        private string _kladosname;
        private double _count;
        private int _prokirixi;
        private string _iekname;

        public GenderData(int prokirixi, string iekname, string gender, string kladosname, double count)
        {
            this._prokirixi = prokirixi;
            this._iekname = iekname;
            this._gender = gender;
            this._kladosname = kladosname;
            this._count = count;
        }

        public string GenderName
        {
            get { return this._gender; }
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

    }   // class GenderData

}
