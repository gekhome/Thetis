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
    public class GenderViewModel : ViewModelBase
    {
        ThetisDataContext db = new ThetisDataContext();

        private List<GenderData> _g1;
        private List<GenderData> _g2;
        private ChartPalette _palette;
        private List<ChartPalette> _palettes;
        private String _name;
        public List<String> _names;
        

        private ChartSeriesCombineMode _barCombineMode = ChartSeriesCombineMode.Cluster;
        private Orientation _chartOrientation = Orientation.Vertical;
        private bool _isShowLabelsEnabled = true;
        private bool _showLabels = false;

        private double _gapLength = 0.4d;
        private double _axisMaxValue = 10000d;
        private double _axisStep = 500d;
        private string _axisTitle = "";
        private string _axisLabelFormat = "N0";
        private GridLineVisibility _majorLinesVisibility = GridLineVisibility.Y;

        private static int _prokirixi = 0;
        private static string _iekname = null;
        private static string _gender = null;
        private static string _klados = null;
        private static double _count = 0;
        public GenderData gd;

        public GenderViewModel()
        {
            this.InitializePalettePresets();
            this.InitializeComboNames();
            gd = new GenderData(_prokirixi, _iekname, _gender, _klados, _count);
        }

        private void LoadData1()
        {
            if (gd.Prokirixi == 0 && gd.IekName == null)
            {
                var qry1 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΦΥΛΟ_ALLs
                       where d.ΦΥΛΟ == "Άνδρας"
                       select d;
                var data_source1 = qry1.ToList();
                this._g1 = new List<GenderData>();
                foreach (var row in data_source1)
                {
                    this._g1.Add(new GenderData(0, null, row.ΦΥΛΟ, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            else if (gd.Prokirixi > 0 && gd.IekName == null)
            {
                var qry1 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΦΥΛΟ_ΠΡΟΚΗΡΥΞΗs
                       where d.ΦΥΛΟ == "Άνδρας" && d.ΠΡΟΚΗΡΥΞΗ == gd.Prokirixi
                       select d;
                var data_source1 = qry1.ToList();
                this._g1 = new List<GenderData>();
                foreach (var row in data_source1)
                {
                    this._g1.Add(new GenderData((int)row.ΠΡΟΚΗΡΥΞΗ, null, row.ΦΥΛΟ, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };

            }
            else if (gd.Prokirixi > 0 && gd.IekName != null)
            {
                var qry1 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΦΥΛΟs
                       where d.ΦΥΛΟ == "Άνδρας" && d.ΠΡΟΚΗΡΥΞΗ == gd.Prokirixi && d.ΙΕΚ_ΟΝΟΜΑΣΙΑ == gd.IekName
                       select d;
                var data_source1 = qry1.ToList();
                this._g1 = new List<GenderData>();
                foreach (var row in data_source1)
                {
                    this._g1.Add(new GenderData((int)row.ΠΡΟΚΗΡΥΞΗ, row.ΙΕΚ_ΟΝΟΜΑΣΙΑ, row.ΦΥΛΟ, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                }; 
            }
            G1 = _g1;
            AxisMaxScale();
        }   // LoadData1

        private void LoadData2()
        {
            if (gd.Prokirixi == 0 && gd.IekName == null)
            {
                var qry2 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΦΥΛΟ_ALLs
                           where d.ΦΥΛΟ == "Γυναίκα"
                           select d;
                var data_source2 = qry2.ToList();
                this._g2 = new List<GenderData>();
                foreach (var row in data_source2)
                {
                    this._g2.Add(new GenderData(0, null, row.ΦΥΛΟ, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            else if (gd.Prokirixi > 0 && gd.IekName == null)
            {
                var qry2 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΦΥΛΟ_ΠΡΟΚΗΡΥΞΗs
                           where d.ΦΥΛΟ == "Γυναίκα" && d.ΠΡΟΚΗΡΥΞΗ == gd.Prokirixi
                           select d;
                var data_source2 = qry2.ToList();
                this._g2 = new List<GenderData>();
                foreach (var row in data_source2)
                {
                    this._g2.Add(new GenderData((int)row.ΠΡΟΚΗΡΥΞΗ, null, row.ΦΥΛΟ, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };

            }
            else if (gd.Prokirixi > 0 && gd.IekName != null)
            {
                var qry2 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΦΥΛΟs
                           where d.ΦΥΛΟ == "Γυναίκα" && d.ΠΡΟΚΗΡΥΞΗ == gd.Prokirixi && d.ΙΕΚ_ΟΝΟΜΑΣΙΑ == gd.IekName
                           select d;
                var data_source2 = qry2.ToList();
                this._g2 = new List<GenderData>();
                foreach (var row in data_source2)
                {
                    this._g2.Add(new GenderData((int)row.ΠΡΟΚΗΡΥΞΗ, row.ΙΕΚ_ΟΝΟΜΑΣΙΑ, row.ΦΥΛΟ, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            G2 = _g2;
            AxisMaxScale();
        }   // LoadData2

        public void RefreshViewModel()
        {
            LoadData1();
            LoadData2();
        }

        private void AxisMaxScale()
        {
            double g1_max = Convert.ToDouble(G1.Max(x => x.Count));
            double g2_max = Convert.ToDouble(G2.Max(x => x.Count));
            double g_max = g1_max + g2_max;
            int maxCount = 0;

            if (_barCombineMode == ChartSeriesCombineMode.Cluster)  // cluster
            {
                maxCount = UserFunctions.Max((int)g1_max, (int)g2_max);
                AxisMaxValue = (int)(maxCount + 0.25 * maxCount);

            }
            else if (_barCombineMode == ChartSeriesCombineMode.Stack) // stack
            {
                maxCount = (int)g_max;
                AxisMaxValue = (int)(maxCount + 0.25 * maxCount);
            }
            else if (_barCombineMode == ChartSeriesCombineMode.Stack100)
            {
                AxisMaxValue = 1d;

            }

        }

        #region Gender Data

        public List<GenderData> G1
        {
            get
            {
                if (this._g1 == null)
                {
                    LoadData1();      
                }
                return this._g1;
            }
            set
            {
                this._g1 = value;
                OnPropertyChanged("G1");
            }
        }

        public List<GenderData> G2
        {
            get
            {
                if (this._g2 == null)
                {
                    LoadData2();
                }
                return this._g2;
            }
            set
            {
                this._g2 = value;
                OnPropertyChanged("G2");
            }
        }

        #endregion

        #region Chart Series Properties

        private void InitializePalettePresets()
        {
            List<ChartPalette> palettes = new List<ChartPalette>();
            palettes.Add(ChartPalettes.Arctic);
            palettes.Add(ChartPalettes.Autumn);
            palettes.Add(ChartPalettes.Cold);
            palettes.Add(ChartPalettes.Flower);
            palettes.Add(ChartPalettes.Forest);
            palettes.Add(ChartPalettes.Grayscale);
            palettes.Add(ChartPalettes.Ground);
            palettes.Add(ChartPalettes.Lilac);
            palettes.Add(ChartPalettes.Natural);
            palettes.Add(ChartPalettes.Pastel);
            palettes.Add(ChartPalettes.Rainbow);
            palettes.Add(ChartPalettes.Spring);
            palettes.Add(ChartPalettes.Summer);
            palettes.Add(ChartPalettes.Warm);
            palettes.Add(ChartPalettes.Windows8);

            this.Palettes = palettes;
            this.Palette = ChartPalettes.Windows8;
        }

        public void InitializeComboNames()
        {
            List<String> names = new List<String>();
            names.Add("Συστοιχία");
            names.Add("Στοίβα");
            names.Add("Στοίβα (%)");

            this.CombineModeNames = names;
            this.CombineModeName = "Συστοιχία";
        }


        public String CombineModeName
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
                    this.OnPropertyChanged("CombineModeName");
                }
            }
        }

        public List<String> CombineModeNames
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
                    this.OnPropertyChanged("CombineModeNames");
                }
            }
        }

        public ChartSeriesCombineMode ConvertNameToMode()
        {
            if (CombineModeName == "Συστοιχία")
            {
                _barCombineMode = ChartSeriesCombineMode.Cluster;
            }
            if (CombineModeName == "Στοίβα")
            {
                _barCombineMode = ChartSeriesCombineMode.Stack;
            }
            if (CombineModeName == "Στοίβα (%)")
            {
                _barCombineMode = ChartSeriesCombineMode.Stack100;
            }
            return _barCombineMode;
        }


        public ChartPalette Palette
        {
            get
            {
                return this._palette;
            }
            set
            {
                if (this._palette != value)
                {
                    this._palette = value;
                    this.OnPropertyChanged("Palette");
                }
            }
        }

        public List<ChartPalette> Palettes
        {
            get
            {
                return this._palettes;
            }
            set
            {
                if (this._palettes != value)
                {
                    this._palettes = value;
                    this.OnPropertyChanged("Palettes");
                }
            }
        }

        public ChartSeriesCombineMode BarCombineMode
        {
            get
            {
                this._barCombineMode = ConvertNameToMode();
                return this._barCombineMode;
            }
            set
            {
                if (this._barCombineMode != value)
                {
                    value = ConvertNameToMode();
                    this._barCombineMode = value;
                    this.OnPropertyChanged("BarCombineMode");

                    this.UpdateLabelsConfiguration(this._barCombineMode);
                    this.UpdateAxisConfiguration(this._barCombineMode);
                }
            }
        }

        public bool ShowLabels
        {
            get
            {
                return this._showLabels;
            }
            set
            {
                if (this._showLabels != value)
                {
                    this._showLabels = value;
                    this.OnPropertyChanged("ShowLabels");
                }
            }
        }

        public bool IsShowLabelsEnabled
        {
            get
            {
                return this._isShowLabelsEnabled;
            }
            set
            {
                if (this._isShowLabelsEnabled != value)
                {
                    this._isShowLabelsEnabled = value;
                    this.OnPropertyChanged("IsShowLabelsEnabled");
                }
            }
        }

        public Orientation ChartOrientation
        {
            get
            {
                return this._chartOrientation;
            }
            set
            {
                if (this._chartOrientation != value)
                {
                    this._chartOrientation = value;
                    this.OnPropertyChanged("ChartOrientation");

                    this.UpdateMajorLinesVisibility(this._chartOrientation);
                }
            }
        }

        public double GapLength
        {
            get
            {
                return this._gapLength;
            }
            set
            {
                if (this._gapLength != value)
                {
                    this._gapLength = value;
                    this.OnPropertyChanged("GapLength");
                }
            }
        }

        public double AxisMaxValue
        {
            get
            {
                return this._axisMaxValue;
            }
            set
            {
                if (this._axisMaxValue != value)
                {
                    this._axisMaxValue = value;
                    this.OnPropertyChanged("AxisMaxValue");
                }
            }
        }

        public double AxisStep
        {
            get
            {
                return this._axisStep;
            }
            set
            {
                if (this._axisStep != value)
                {
                    this._axisStep = value;
                    this.OnPropertyChanged("AxisStep");
                }
            }
        }

        public string AxisTitle
        {
            get
            {
                return this._axisTitle;
            }
            set
            {
                if (this._axisTitle != value)
                {
                    this._axisTitle = value;
                    this.OnPropertyChanged("AxisTitle");
                }
            }
        }

        public string AxisLabelFormat
        {
            get
            {
                return this._axisLabelFormat;
            }
            set
            {
                if (this._axisLabelFormat != value)
                {
                    this._axisLabelFormat = value;
                    this.OnPropertyChanged("AxisLabelFormat");
                }
            }
        }

        #region Chart configuration

        public GridLineVisibility MajorLinesVisibility
        {
            get
            {
                return this._majorLinesVisibility;
            }
            set
            {
                if (this._majorLinesVisibility != value)
                {
                    this._majorLinesVisibility = value;
                    this.OnPropertyChanged("MajorLinesVisibility");
                }
            }
        }

        private void UpdateMajorLinesVisibility(Orientation chartOrientation)
        {
            if (chartOrientation == Orientation.Horizontal)
                this.MajorLinesVisibility = GridLineVisibility.X;
            else
                this.MajorLinesVisibility = GridLineVisibility.Y;
        }

        private void UpdateLabelsConfiguration(ChartSeriesCombineMode mode)
        {
            this.ShowLabels = false;
            this.IsShowLabelsEnabled = mode == ChartSeriesCombineMode.Cluster;
        }

        private void UpdateAxisConfiguration(ChartSeriesCombineMode mode)
        {
            if (mode == ChartSeriesCombineMode.Cluster)
            {
                this.GapLength = 0.4d;
                AxisMaxScale();
                //this.AxisMaxValue = 20000d;
                //this.AxisStep = 5000d;

                this.AxisTitle = "ΠΛΗΘΟΣ ΑΝΔΡΩΝ, ΓΥΝΑΙΚΩΝ ΑΝΑ ΚΛΑΔΟ";
                this.AxisLabelFormat = "N0";
            }
            else if (mode == ChartSeriesCombineMode.Stack)
            {
                this.GapLength = 0.5d;
                AxisMaxScale();
                //this.AxisMaxValue = 70000d;
                //this.AxisStep = 16500d;

                this.AxisTitle = "ΠΛΗΘΟΣ ΑΝΔΡΩΝ, ΓΥΝΑΙΚΩΝ ΑΝΑ ΚΛΑΔΟ";
                this.AxisLabelFormat = "N0";
            }
            else if (mode == ChartSeriesCombineMode.Stack100)
            {
                this.GapLength = 0.5d;
                AxisMaxScale();

                //this.AxisMaxValue = 1d;
                //this.AxisStep = 0.25d;

                this.AxisTitle = "ΠΛΗΘΟΣ ΑΝΔΡΩΝ, ΓΥΝΑΙΚΩΝ ΑΝΑ ΚΛΑΔΟ (%)";
                this.AxisLabelFormat = "P0";
            }
        }

        #endregion

        #endregion

    }   // class GenderViewModel
}
