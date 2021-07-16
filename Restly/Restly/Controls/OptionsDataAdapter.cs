using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Restly.Activities;
using Restly.Model;
using System;

namespace Restly.Controls
{

    public class OptionsDataHolder : RecyclerView.ViewHolder
    {
        public CheckBox itemName;
        public TextView extrasPrice;
        public OptionsDataHolder(View itemView) : base(itemView)
        {
            itemName = itemView.FindViewById<CheckBox>(Resource.Id.checkbox_button);
            extrasPrice = ItemView.FindViewById<TextView>(Resource.Id.extras_Price);
        }
    }


    internal class OptionsDataAdapter : RecyclerView.Adapter
    {
        private ProductActivity productActivity;
        private DataOption[] options;
        private DataOption dataOption;

        private bool[] checkedOptions;

        public OptionsDataAdapter(ProductActivity productActivity, DataOption dataOption, bool[] checkedOptions)
        {
            this.productActivity = productActivity;
            this.dataOption = dataOption;
            this.checkedOptions = checkedOptions;
        }

        public override int ItemCount
        {
            get
            {
                return dataOption.Options.Length;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OptionsDataHolder ovh = holder as OptionsDataHolder;

            ovh.itemName.Text = dataOption.Options[position].Name;
            
            if (dataOption.Options[position].Price > 0)
            {
                if (dataOption.Options[position].Price % Convert.ToInt32(dataOption.Options[position].Price) == 0)
                {
                    ovh.extrasPrice.Text = StringOperations.AddCurrencyDecimalText(dataOption.Options[position].Price.ToString());
                }
                else
                {
                    ovh.extrasPrice.Text = StringOperations.AddCurrencyText(dataOption.Options[position].Price.ToString());
                }
            }
            else
            {
                ovh.extrasPrice.Visibility = ViewStates.Gone;
            }

            ovh.itemName.SetTypeface(MainActivity.typeface, TypefaceStyle.Normal);
            ovh.extrasPrice.SetTypeface(MainActivity.typeface, TypefaceStyle.Normal);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.extras_items, parent, false);
            return new OptionsDataHolder(itemView);
        }
    }
}