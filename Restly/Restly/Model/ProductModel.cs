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

        public partial class ProductModel
        {
            public Data Data { get; set; }

            public bool Success { get; set; }

            public long StatusCode { get; set; }

            public object Message { get; set; }
        }

        public partial class Data
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public Uri ImageUrl { get; set; }

            public string Description { get; set; }

            public double Price { get; set; }

            public string[] Allergens { get; set; }

            public DataOption[] Options { get; set; }

            public FrequentlyBoughtProduct[] FrequentlyBoughtProducts { get; set; }
        }

        public partial class FrequentlyBoughtProduct
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public Uri ImageUrl { get; set; }

            public double Price { get; set; }

            public bool HasOptionsRequired { get; set; }
        }

        public partial class DataOption
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public OptionOption[] Options { get; set; }

            public long Type { get; set; }

            public long Min { get; set; }

            public long Max { get; set; }

            public bool IsRequired { get; set; }
        }

        public partial class OptionOption
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public double Price { get; set; }
        }

}