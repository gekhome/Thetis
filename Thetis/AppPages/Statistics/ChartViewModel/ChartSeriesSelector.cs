using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace Thetis.AppPages.Statistics.ChartViewModel
{
    public class ChartSeriesSelector : FrameworkElement
    {
        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register("Series",
            typeof(List<PieSeries>),
            typeof(ChartSeriesSelector),
            new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="Chart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register("Chart",
            typeof(RadPieChart),
            typeof(ChartSeriesSelector),
            new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex",
            typeof(int),
            typeof(ChartSeriesSelector),
            new PropertyMetadata(-1, OnSelectedIndexPropertyChanged));

        public ChartSeriesSelector()
        {
            this.Series = new List<PieSeries>();
        }

        public List<PieSeries> Series
        {
            get
            {
                return (List<PieSeries>)this.GetValue(SeriesProperty);
            }
            set
            {
                this.SetValue(SeriesProperty, value);
            }
        }

        public RadPieChart Chart
        {
            get
            {
                return (RadPieChart)this.GetValue(ChartProperty);
            }
            set
            {
                this.SetValue(ChartProperty, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return (int)this.GetValue(SelectedIndexProperty);
            }
            set
            {
                this.SetValue(SelectedIndexProperty, value);
            }
        }

        private static void OnSelectedIndexPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ChartSeriesSelector).OnSelectedIndexChanged((int)e.NewValue);
        }

        private void OnSelectedIndexChanged(int newIndex)
        {
            if (this.Chart == null || this.Series.Count <= newIndex)
            {
                return;
            }

            foreach (var series in this.Chart.Series)
            {
                series.Visibility = Visibility.Collapsed;
            }

            PieSeries selectedSeries = this.Series[newIndex];
            if (!this.Chart.Series.Contains(selectedSeries))
            {
                this.Chart.Series.Add(selectedSeries);
            }
            else
            {
                selectedSeries.Visibility = Visibility.Visible;
            }
        }
    }
}
