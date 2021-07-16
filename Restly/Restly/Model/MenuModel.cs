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

namespace Restly
{
    public partial class MenuModel
    {
            public Data Data { get; set; }

            public bool Success { get; set; }

            public long StatusCode { get; set; }

            public object Message { get; set; }
        }

        public partial class Data
        {
            public MenuItemsFirstPage MenuItemsFirstPage { get; set; }

            public MenuCategory[] MenuCategories { get; set; }
        }

        public partial class MenuCategory
        {
            public long Id { get; set; }

            public string Title { get; set; }

            public Uri ImageUrl { get; set; }
        }

        public partial class MenuItemsFirstPage
        {
            public Datum[] Data { get; set; }

            public long TotalPages { get; set; }

            public long CurrentPage { get; set; }

            public long ActiveIndex { get; set; }

            public long TotalElementsCount { get; set; }
        }

        public partial class Datum
        {
            public long Id { get; set; }

            public string Title { get; set; }

            public Uri ImageUrl { get; set; }

            public string Description { get; set; }

            public double Rating { get; set; }

            public double Price { get; set; }

            public string[] Allergens { get; set; }
        }

}