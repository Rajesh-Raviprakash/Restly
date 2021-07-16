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
    class StringOperations
    {

        internal static string AddCurrencyText(string price)
        {
            return AppResource.CurrencyText + " " + price;
        }
        internal static string AddCurrencyDecimalText(string price)
        {
            return AppResource.CurrencyText + " " + price + ".0";
        }
    }
}