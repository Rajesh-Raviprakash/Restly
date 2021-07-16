using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Restly.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restly.Controls
{
    class ProgressIndicator
    {
        public static ProgressDialog progress;
        /// <summary>
        /// starts progress indicator
        /// </summary>
        /// <param name="_context"></param>
        internal static void StartAnimation(Android.Content.Context _context)
        {
            progress = new Android.App.ProgressDialog(_context);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage(AppResource.PleaseWaitText);
            progress.SetCancelable(false);
            if (!progress.IsShowing)
            {
                progress.Show();
            }
        }
        /// <summary>
        /// stop progress indicator
        /// </summary>
        public static void StopAnimation()
        {
            progress.Dismiss();
        }
    }
}