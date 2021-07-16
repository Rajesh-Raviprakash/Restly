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

namespace Restly.Model
{
    public class Datas
    {
        public int id { get; set; }
        public string title { get; set; }
        public string imageUrl { get; set; }
        public string description { get; set; }
        public double rating { get; set; }
        public double price { get; set; }
        public IList<string> allergens { get; set; }

    }
    public class MenuItemData
    {
        public IList<Datas> data { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
        public int activeIndex { get; set; }
        public int totalElementsCount { get; set; }

    }
    public class MenuItemsModel
    {
        public MenuItemData data { get; set; }
        public bool success { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }

    }
}