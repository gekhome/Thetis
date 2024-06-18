using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Thetis.Localization;

namespace Thetis
{
    public partial class App : Application
    {
 
        public App()
        {
            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            LocalizationManager.Manager = new LocalizationManager()
            {
                ResourceManager = RadControlsResources_gr.ResourceManager
            };

        }

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            // Uncomment the following code for testing purposes.
            //System.Threading.Thread.CurrentThread.CurrentCulture =
            //    System.Threading.Thread.CurrentThread.CurrentUICulture =
            //    new System.Globalization.CultureInfo("fr-FR");
#else
            this.VerifyPermissions();
#endif

            if (IsRunningInXBAP)
            {
                base.StartupUri = new Uri("Shell/LoginPage.xaml", UriKind.Relative);
            }
            else
            {
                base.StartupUri = new Uri("Shell/MainWindow.xaml", UriKind.Relative);
            }

            base.OnStartup(e);
        }

        void VerifyPermissions()
        {
            if (!IsRunningInXBAP)
                return;

            try
            {
                new System.Windows.Media.Effects.DropShadowBitmapEffect();
                System.Diagnostics.Trace.Fail("When creating a production build, be sure to adjust the project's permissions level to 'Internet'.");
            }
            catch
            {
            }
        }

        /// <summary>
        /// Shows the friendly exception message to the user.
        /// </summary>
        /// <param name="ex"></param>
        /// <remarks>In Debug mode, it will provide the exception details through its ToString implementation; in Release, it will only show the message.</remarks>
        public static void HandleException(Exception ex)
        {
            if (ex == null)
                return;
            string displayMessage = Thetis.Properties.Resources.GeneralExceptionPrompt + Environment.NewLine + Environment.NewLine;
            while ((ex is TargetInvocationException || ex is XamlParseException) && ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
#if DEBUG
            displayMessage += ex.ToString();
#else
            displayMessage += ex.Message;
#endif
            MessageBox.Show(displayMessage, Thetis.Properties.Resources.GeneralExceptionTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Returns true if the application is running in the browser as an XBAP,
        /// or false if it is running on the desktop in a top-level window.
        /// </summary>
        public static bool IsRunningInXBAP
        {
            get { return System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted; }
        }

        /// <summary>
        /// An overload that all samples should use to retrieve assembly resources.
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static Stream GetResourceStream(string resourcePath)
        {
            Uri uri = new Uri(resourcePath, UriKind.Relative);
            return Application.GetResourceStream(uri).Stream;
        }
    }
}
