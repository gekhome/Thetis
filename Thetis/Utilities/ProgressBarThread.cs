using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Thetis.Utilities
{
    class ProgressBarThread
    {
        public static Loading loadingWin;
        public static bool isLoadingWinCreated;
        public static Thread thread;
        
        public static Thread InitializeThread()
        {
            Thread thread = new Thread(() =>
            {
                loadingWin = new Loading();
                isLoadingWinCreated = true;
                loadingWin.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            return thread;
        }

        public static void StartThread()
        {
            if (thread == null)
            {
                thread = InitializeThread();
                thread.Start();
            }
            while (!isLoadingWinCreated) ;
        }

        public static void StopThread()
        {
            if (loadingWin != null) loadingWin.CloseForm();
            //loadingWin = null;
            //Thread.Sleep(1000);
            if (thread == null) thread = InitializeThread();
            //thread.Abort();

        }


    } // class ProgressBarThread
}
