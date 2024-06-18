using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Navigation;

namespace Thetis.Utilities
{
    public partial class ClosableTabItem : RadTabItem                     //System.Windows.Controls.TabItem
    {
        static ClosableTabItem()
        {
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClosableTabItem),
                new FrameworkPropertyMetadata(typeof(ClosableTabItem)));

            CommandManager.RegisterClassCommandBinding(typeof(ClosableTabItem),
                new CommandBinding(ClosableTabItem.StateChange, StateChangeExecuted));
        }

        public static readonly RoutedUICommand StateChange =
            new RoutedUICommand("State Change", "StateChange", typeof(ClosableTabItem));

        public static readonly RoutedEvent TabOpenEvent =
            EventManager.RegisterRoutedEvent("TabOpen", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ClosableTabItem));

        public static readonly RoutedEvent TabCloseEvent =
            EventManager.RegisterRoutedEvent("TabClose", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ClosableTabItem));

        private static void StateChangeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ClosableTabItem s = (ClosableTabItem)sender;
            bool parameter = (e.Parameter == null) ? false : (bool)e.Parameter;
            if (parameter)
                s.RaiseEvent(new RoutedEventArgs(ClosableTabItem.TabOpenEvent));
            else
                s.RaiseEvent(new RoutedEventArgs(ClosableTabItem.TabCloseEvent));
        }

        //public event RoutedEventHandler TabOpen
        //{
        //    add { AddHandler(TabOpenEvent, value); }
        //    remove { RemoveHandler(TabOpenEvent, value); }
        //}

        //public event RoutedEventHandler TabClose
        //{
        //    add { AddHandler(TabCloseEvent, value); }
        //    remove { RemoveHandler(TabCloseEvent, value); }
        //}

        /// <summary>
        /// Because of a bug I found in WPF, I cannot get a OneWay binding to the
        /// IsVisible property working, so this additional property is used instead.
        /// </summary>
        public Boolean CIsVisible
        {
            get { return (Boolean)GetValue(CIsVisibleProperty); }
            set { SetValue(CIsVisibleProperty, value); }
        }
        public static readonly DependencyProperty CIsVisibleProperty =
            DependencyProperty.Register("CIsVisible", typeof(Boolean), typeof(ClosableTabItem), new PropertyMetadata(true));
    }
}
