using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Thetis.Model;
using Thetis.Utilities;

namespace Thetis.AppPages.Statistics.ChartViewModel
{
    public class ChartCombineModeExtended : ViewModelBase
    {
        private ChartSeriesCombineMode _barCombineMode = ChartSeriesCombineMode.Cluster;
        private String _name;
        public List<String> _names;

        public ChartCombineModeExtended()
        {
            this.InitializeNames();
        }

        public void InitializeNames()
        {
            List<String> names = new List<String>();
            names.Add("Συστοιχία");
            names.Add("Στοίβα");
            names.Add("Στοίβα (%)");

            this._names = names;
            this._name = "Συστοιχία";
        }

        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        public List<String> Names
        {
            get
            {
                return this._names;
            }
            set
            {
                if (this._names != value)
                {
                    this._names = value;
                    this.OnPropertyChanged("Names");
                }
            }
        }

        public ChartSeriesCombineMode ModeValue
        {
            get
            {
                return this._barCombineMode;
            }
            set
            {
                if (this._barCombineMode != value)
                {
                    this._barCombineMode = value;
                    this.OnPropertyChanged("ModeValue");

                }
            }
        }

    }   // ChartCombineModeExtended
}
