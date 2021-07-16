using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restly.Resources
{
    class AppResource
    {
        public static string AppName => Application.Context.GetString(Resource.String.app_name);
        public static string MenuText => Application.Context.GetString(Resource.String.Menu);
        public static string ClearText => Application.Context.GetString(Resource.String.clear);
        public static string AddToCartText => Application.Context.GetString(Resource.String.addtocart_text);
        public static string BaseUrl => Application.Context.GetString(Resource.String.baseUrl);
        public static string InitUrl => Application.Context.GetString(Resource.String.initUrl_string);
        public static string ProductByIdUrl => Application.Context.GetString(Resource.String.get_product_byid_ulr_string);
        public static string RestaurantId => Application.Context.GetString(Resource.String.restaurantID);
        public static string MenuItemsUrl => Application.Context.GetString(Resource.String.getmenulist_url_string);
        public static string PostMenuListParameter1 => Application.Context.GetString(Resource.String.getmenulist_parameter1);
        public static string PostMenuListParameter2 => Application.Context.GetString(Resource.String.getmenulist_parameter2);
        public static string PostMenuListParameter3 => Application.Context.GetString(Resource.String.getmenulist_parameter3);
        public static string PostMenuListParameter4 => Application.Context.GetString(Resource.String.getmenulist_parameter4);
        public static string PostMenuListParameter5 => Application.Context.GetString(Resource.String.getmenulist_parameter5);
        public static string YouMayAlsoLikeText => Application.Context.GetString(Resource.String.youMayAlsoLike_string);
        public static string SpecialPreferenceText => Application.Context.GetString(Resource.String.special_preference_string);
        public static string ItemsPerPage => Application.Context.GetString(Resource.String.items_per_page);
        public static string HelloText => Application.Context.GetString(Resource.String.hello_text);
        public static string NotificationDescription => Application.Context.GetString(Resource.String.notification_text);
        public static string CurrencyText => Application.Context.GetString(Resource.String.Currency_text);
        public static string OptionalText => Application.Context.GetString(Resource.String.optional_text);
        public static string PleaseWaitText => Application.Context.GetString(Resource.String.pleasewait_text);
    }
}