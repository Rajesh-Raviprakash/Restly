using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Restly.Activities;
using Restly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restly.Controls
{

    public class SidesViewHolder : RecyclerView.ViewHolder
    {
        public RadioButton itemName;
        public EventHandler radioChecked;
        public TextView optionPrice;

        private IRadioClickListener radioClickListener;

        public SidesViewHolder(View itemView) : base(itemView)
        {
            itemName = itemView.FindViewById<RadioButton>(Resource.Id.option_radio_button);
            optionPrice = ItemView.FindViewById<TextView>(Resource.Id.option_Price);
            itemName.Click += ItemName_Click;
        }

        private void ItemName_Click(object sender, EventArgs e)
        {
            radioChecked.Invoke(sender, e);
        }
    }
    class SingleSelectionAdapter : RecyclerView.Adapter
    {
        private ProductActivity productActivity;
        public EventHandler<int> sidesOptionSelected;
        private DataOption dataOption;
        private int selectedPosition;

        public SingleSelectionAdapter(ProductActivity productActivity, DataOption dataOption, int selectedPosition)
        {
            this.productActivity = productActivity;
            this.dataOption = dataOption;
            this.selectedPosition = selectedPosition;
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
            SidesViewHolder ovh = holder as SidesViewHolder;
            if (position == selectedPosition)
            {
                ovh.itemName.Checked = true;
            }
            if (dataOption.Options[position].Price > 0)
            {
                if (dataOption.Options[position].Price % Convert.ToInt32(dataOption.Options[position].Price) == 0)
                {
                    ovh.optionPrice.Text = StringOperations.AddCurrencyDecimalText(dataOption.Options[position].Price.ToString());
                }
                else
                {
                    ovh.optionPrice.Text = StringOperations.AddCurrencyText(dataOption.Options[position].Price.ToString());
                }
            }
            else
            {
                ovh.optionPrice.Visibility = ViewStates.Gone;
            }
            ovh.itemName.Text = dataOption.Options[position].Name;

            ovh.radioChecked += (sender, ischecked) =>
            {
                sidesOptionSelected.Invoke(productActivity, position);
            };

            ovh.itemName.SetTypeface(MainActivity.typeface, TypefaceStyle.Normal);
            ovh.optionPrice.SetTypeface(MainActivity.typeface, TypefaceStyle.Normal);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.options_items, parent, false);
            return new SidesViewHolder(itemView);
        }
    }
}