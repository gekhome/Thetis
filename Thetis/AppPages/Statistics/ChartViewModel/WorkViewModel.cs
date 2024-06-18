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
    public class WorkViewModel     : ViewModelBase
    {
        ThetisDataContext db = new ThetisDataContext();

        private List<WorkData> _w1;
        private List<WorkData> _w2;
        private List<WorkData> _w3;

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
        private static string _work = null;
        private static bool _anergos = false;
        private static string _klados = null;
        private static double _count = 0;
        public WorkData wd;

        public WorkViewModel()
        {
            this.InitializePalettePresets();
            this.InitializeComboNames();
            wd = new WorkData(_prokirixi, _iekname, _work, _anergos, _klados, _count);
        }

        // Δεδομένα για ιδιώτες
        private void LoadData1()
        {
            if (wd.Prokirixi == 0 && wd.IekName == null)
            {
                var qry1 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΕΡΓΑΣΙΑ_ΟΛΕΣs
                           where d.ΕΡΓΑΣΙΑ == "ΕΡΓΑΖΟΜΕΝΟΙ ΣΤΟΝ ΙΔΙΩΤΙΚΟ ΤΟΜΕΑ" && d.ΑΝΕΡΓΟΣ_12 == false
                           select d;

                var data_source1 = qry1.ToList();
                this._w1 = new List<WorkData>();
                foreach (var row in data_source1)
                {
                    this._w1.Add(new WorkData(0, null, row.ΕΡΓΑΣΙΑ, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            else if (wd.Prokirixi > 0 && wd.IekName == null)
            {
                var qry1 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΕΡΓΑΣΙΑ_ΠΡΟΚΗΡΥΞΗs
                           where d.ΕΡΓΑΣΙΑ == "ΕΡΓΑΖΟΜΕΝΟΙ ΣΤΟΝ ΙΔΙΩΤΙΚΟ ΤΟΜΕΑ" 
                           && d.ΑΝΕΡΓΟΣ_12 == false && d.ΠΡΟΚΗΡΥΞΗ == wd.Prokirixi
                           select d;
                var data_source = qry1.ToList();
                this._w1 = new List<WorkData>();
                foreach (var row in data_source)
                {
                    this._w1.Add(new WorkData((int)row.ΠΡΟΚΗΡΥΞΗ, null, row.ΕΡΓΑΣΙΑ, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };

            }
            else if (wd.Prokirixi > 0 && wd.IekName != null)
            {
                var qry1 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΕΡΓΑΣΙΑ_ΠΡΟΚ_ΙΕΚs
                           where d.ΠΡΟΚΗΡΥΞΗ == wd.Prokirixi && d.ΙΕΚ_ΟΝΟΜΑΣΙΑ == wd.IekName
                           select d;
                var data_source1 = qry1.ToList();
                this._w1 = new List<WorkData>();
                foreach (var row in data_source1)
                {
                    this._w1.Add(new WorkData((int)row.ΠΡΟΚΗΡΥΞΗ, row.ΙΕΚ_ΟΝΟΜΑΣΙΑ, row.ΕΡΓΑΣΙΑ, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            W1 = _w1;
            AxisMaxScale();

        }   // LoadData1

        // Δεδομένα για δημόσιους υπαλλήλους και συνταξιούχους
        private void LoadData2()
        {
            if (wd.Prokirixi == 0 && wd.IekName == null)
            {
                var qry2 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΕΡΓΑΣΙΑ_ΟΛΕΣs
                           where d.ΕΡΓΑΣΙΑ == "ΔΗΜΟΣΙΟΙ ΥΠΑΛΛΗΛΟΙ Ή ΣΥΝΤΑΞΙΟΥΧΟΙ"
                           select d;

                var data_source2 = qry2.ToList();
                this._w2 = new List<WorkData>();
                foreach (var row in data_source2)
                {
                    this._w2.Add(new WorkData(0, null, row.ΕΡΓΑΣΙΑ, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            else if (wd.Prokirixi > 0 && wd.IekName == null)
            {
                var qry2 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΕΡΓΑΣΙΑ_ΠΡΟΚΗΡΥΞΗs
                          where d.ΕΡΓΑΣΙΑ == "ΔΗΜΟΣΙΟΙ ΥΠΑΛΛΗΛΟΙ Ή ΣΥΝΤΑΞΙΟΥΧΟΙ" && d.ΠΡΟΚΗΡΥΞΗ == wd.Prokirixi
                          select d;
                var data_source2 = qry2.ToList();
                this._w2 = new List<WorkData>();
                foreach (var row in data_source2)
                {
                    this._w2.Add(new WorkData((int)row.ΠΡΟΚΗΡΥΞΗ, null, row.ΕΡΓΑΣΙΑ, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };

            }
            else if (wd.Prokirixi > 0 && wd.IekName != null)
            {
                var qry2 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΕΡΓΑΣΙΑ_ΠΡΟΚ_ΙΕΚs
                          where d.ΕΡΓΑΣΙΑ == "ΔΗΜΟΣΙΟΙ ΥΠΑΛΛΗΛΟΙ Ή ΣΥΝΤΑΞΙΟΥΧΟΙ" && d.ΠΡΟΚΗΡΥΞΗ == wd.Prokirixi && d.ΙΕΚ_ΟΝΟΜΑΣΙΑ == wd.IekName
                          select d;
                var data_source2 = qry2.ToList();
                this._w2 = new List<WorkData>();
                foreach (var row in data_source2)
                {
                    this._w2.Add(new WorkData((int)row.ΠΡΟΚΗΡΥΞΗ, row.ΙΕΚ_ΟΝΟΜΑΣΙΑ, row.ΕΡΓΑΣΙΑ, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            W2 = _w2;
            AxisMaxScale();

        }   // LoadData2

        // Δεδομένα για ανέργους
        private void LoadData3()
        {
            string ergasia = "ΕΡΓΑΖΟΜΕΝΟΙ ΣΤΟΝ ΙΔΙΩΤΙΚΟ ΤΟΜΕΑ";

            if (wd.Prokirixi == 0 && wd.IekName == null)
            {
                var qry3 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΑΝΕΡΓΟΙ_ΟΛΕΣs
                           where d.ΑΝΕΡΓΟΣ_12 == true
                           select d;
           
                var data_source3 = qry3.ToList();
                this._w3 = new List<WorkData>();
                foreach (var row in data_source3)
                {
                    this._w3.Add(new WorkData(0, null, ergasia, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            else if (wd.Prokirixi > 0 && wd.IekName == null)
            {
                var qry3 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΑΝΕΡΓΟΙ_ΠΡΟΚΗΡΥΞΗs
                          where d.ΑΝΕΡΓΟΣ_12 == true && d.ΠΡΟΚΗΡΥΞΗ == wd.Prokirixi
                          select d;
                var data_source3 = qry3.ToList();
                this._w3 = new List<WorkData>();
                foreach (var row in data_source3)
                {
                    this._w3.Add(new WorkData((int)row.ΠΡΟΚΗΡΥΞΗ, null, ergasia, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };

            }
            else if (wd.Prokirixi > 0 && wd.IekName != null)
            {
                var qry3 = from d in db.ViewΑΙΤΗΣΕΙΣ_ΚΛΑΔΟΣ_ΕΡΓΑΣΙΑ_ΠΡΟΚ_ΙΕΚs
                           where d.ΑΝΕΡΓΟΣ_12 == true && d.ΠΡΟΚΗΡΥΞΗ == wd.Prokirixi && d.ΙΕΚ_ΟΝΟΜΑΣΙΑ == wd.IekName
                          select d;

                var data_source3 = qry3.ToList();
                this._w3 = new List<WorkData>();
                foreach (var row in data_source3)
                {
                    this._w3.Add(new WorkData((int)row.ΠΡΟΚΗΡΥΞΗ, row.ΙΕΚ_ΟΝΟΜΑΣΙΑ, ergasia, row.ΑΝΕΡΓΟΣ_12, row.ΚΛΑΔΟΣ, (double)row.ΠΛΗΘΟΣ));
                };
            }
            W3 = _w3;
            AxisMaxScale();

        }   // LoadData3


        public void RefreshViewModel()
        {
            LoadData1();
            LoadData2();
            LoadData3();
        }

        private void AxisMaxScale()
        {
            double w1_max = Convert.ToDouble(W1.Max(x => x.Count));
            double w2_max = Convert.ToDouble(W2.Max(x => x.Count));
            double w3_max = Convert.ToDouble(W3.Max(x => x.Count));
            double w_max = w1_max + w2_max + w3_max;
            int maxCount = 0;

            if (_barCombineMode == ChartSeriesCombineMode.Cluster)  // cluster
            {
                maxCount = UserFunctions.Max((int)w1_max, (int)w2_max, (int)w3_max);
                AxisMaxValue = (int)(maxCount + 0.25 * maxCount);

            }
            else if (_barCombineMode == ChartSeriesCombineMode.Stack) // stack
            {
                maxCount = (int)w_max;
                AxisMaxValue = (int)(maxCount + 0.25 * maxCount);
            }
            else if (_barCombineMode == ChartSeriesCombineMode.Stack100)
            {
                AxisMaxValue = 1d;

            }
        }

        #region Work Data

        public List<WorkData> W1
        {
            get
            {
                if (this._w1 == null)
                {
                    LoadData1();
                }
                return this._w1;
            }
            set
            {
                this._w1 = value;
                OnPropertyChanged("W1");
            }
        }

        public List<WorkData> W2
        {
            get
            {
                if (this._w2 == null)
                {
                    LoadData2();
                }
                return this._w2;
            }
            set
            {
                this._w2 = value;
                OnPropertyChanged("W2");
            }
        }

        public List<WorkData> W3
        {
            get
            {
                if (this._w3 == null)
                {
                    LoadData3();
                }
                return this._w3;
            }
            set
            {
                this._w3 = value;
                OnPropertyChanged("W3");
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
            }
            else if (mode == ChartSeriesCombineMode.Stack)
            {
                this.GapLength = 0.5d;
                AxisMaxScale();
            }
            else if (mode == ChartSeriesCombineMode.Stack100)
            {
                this.GapLength = 0.5d;
                AxisMaxScale();
                this.AxisLabelFormat = "P0";
            }
        }

        #endregion

        #endregion











    }   // class WorkViewModel
}
