using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thetis.Controls
{
    /// <summary>
    /// Represents a transparent layer that intercepts mouse messages and knows how 
    /// to hide itself in response to the Close routed command being executed.
    /// </summary>
    public partial class GlassPane : Border
    {
        public GlassPane()
        {
            InitializeComponent();

            base.CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Close,
                (sender, e) =>
                {
                    base.Visibility = Visibility.Collapsed;
                    e.Handled = true;
                },
                (sender, e) =>
                { 
                    e.CanExecute = true;
                    e.Handled = true;
                }));
        }
    }
}