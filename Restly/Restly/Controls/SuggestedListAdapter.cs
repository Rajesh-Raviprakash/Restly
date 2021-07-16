using Android.Graphics;
using Android.Support.Design.Card;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Restly.Controls;
using Restly.Model;
using System;

namespace Restly.Activities
{

    public class SuggestedViewHolder : RecyclerView.ViewHolder
    {
        public TextView suggestedItemTitle, suggestedItemPrice;
        public ImageView suggestedItemIcon;
        public CardView suggestedItemCardview;
        private IItemClickListener itemClickListener;

        public SuggestedViewHolder(View itemView) : base(itemView)
        {
            suggestedItemTitle = itemView.FindViewById<TextView>(Resource.Id.suggested_item_title);
            suggestedItemPrice = itemView.FindViewById<TextView>(Resource.Id.suggested_item_price);
            suggestedItemIcon = itemView.FindViewById<ImageView>(Resource.Id.suggested_item_icon);
            suggestedItemCardview = itemView.FindViewById<CardView>(Resource.Id.suggested_item_card);
            //suggestedItemCardview.SetOnClickListener(this);

            suggestedItemCardview.UseCompatPadding = true;
            suggestedItemCardview.Radius = 5;
        }
    }
    internal class SuggestedListAdapter : RecyclerView.Adapter
    {
        private ProductActivity productActivity;
        private FrequentlyBoughtProduct[] frequentlyBoughtProducts;
        private Bitmap[] suggestedIcons;

        public SuggestedListAdapter(ProductActivity productActivity, FrequentlyBoughtProduct[] frequentlyBoughtProducts, Bitmap[] suggestedIcons)
        {
            this.productActivity = productActivity;
            this.frequentlyBoughtProducts = frequentlyBoughtProducts;
            this.suggestedIcons = suggestedIcons;
        }

        public override int ItemCount
        {
            get
            {
                return frequentlyBoughtProducts.Length;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SuggestedViewHolder ovh = holder as SuggestedViewHolder;
            ovh.suggestedItemTitle.Text = frequentlyBoughtProducts[position].Name;
            ovh.suggestedItemPrice.Text = StringOperations.AddCurrencyText(frequentlyBoughtProducts[position].Price.ToString());
            ovh.suggestedItemIcon.SetImageBitmap(Bitmap.CreateScaledBitmap(suggestedIcons[position], 120, 120, false));
            //ovh.SetItemClickListener(this);
            ovh.suggestedItemCardview.Click += (sender, e) =>
            {
                ovh.suggestedItemCardview.SetCardBackgroundColor(Color.Rgb(44, 165, 111));
                ovh.suggestedItemTitle.SetTextColor(Color.White);
                ovh.suggestedItemPrice.SetTextColor(Color.White);
            };
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.suggested_items, parent, false);
            return new SuggestedViewHolder(itemView);
        }
    }
}