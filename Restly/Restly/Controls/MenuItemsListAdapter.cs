using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Restly.Controls;
using Restly.Model;
using System;

namespace Restly
{

    public class MenuItemsViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        public TextView itemName,itemDescription,itemRatings, itemPrice;
        public ImageView itemImage;
        public CardView itemCardview;
        private IItemClickListener itemClickListener;
        public RecyclerView allergenseRecyclerView;

        public MenuItemsViewHolder(View itemView) : base(itemView)
        {
            itemName = itemView.FindViewById<TextView>(Resource.Id.menuItemName);
            itemDescription = itemView.FindViewById<TextView>(Resource.Id.itemDescription);
            itemRatings = itemView.FindViewById<TextView>(Resource.Id.itemRatings);
            itemPrice = itemView.FindViewById<TextView>(Resource.Id.itemPrice);
            itemImage = itemView.FindViewById<ImageView>(Resource.Id.menuImage);
            itemCardview = itemView.FindViewById<CardView>(Resource.Id.menu_item_card);
            allergenseRecyclerView = itemView.FindViewById<RecyclerView>(Resource.Id.allergensRecyclerView);
            itemCardview.SetOnClickListener(this);
        }

        public void SetItemClickListener(IItemClickListener itemClickListener)
        {
            this.itemClickListener = itemClickListener;
        }
        public void OnClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, false, itemCardview, itemName);
        }
    }


    internal class MenuItemsListAdapter : RecyclerView.Adapter , IItemClickListener
    {
        private MainActivity mainActivity;
        private Bitmap[] menuIcons;

        public EventHandler<int> menuItemClicked;
        private MenuItemData data;

        public MenuItemsListAdapter(MainActivity mainActivity, MenuItemData data, Bitmap[] menuIcons)
        {
            this.mainActivity = mainActivity;
            this.data = data;
            this.menuIcons = menuIcons;
        }

        public override int ItemCount
        {
            get
            {
                return data.data.Count;
            }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MenuItemsViewHolder ovh = holder as MenuItemsViewHolder;
            ovh.itemName.Text = data.data[position].title;
            ovh.itemDescription.Text = data.data[position].description;
            ovh.itemPrice.Text = StringOperations.AddCurrencyText(data.data[position].price.ToString());
            ovh.itemRatings.Text = data.data[position].rating.ToString();
            ovh.itemImage.SetImageBitmap(menuIcons[position]);

            var allergense_LayoutManager = new LinearLayoutManager(mainActivity);
            allergense_LayoutManager.Orientation = LinearLayoutManager.Horizontal;

            ovh.allergenseRecyclerView.SetLayoutManager(allergense_LayoutManager);

            var allergenseAdapter = new AllergenseAdapter(mainActivity, data.data[position].allergens);
            ovh.allergenseRecyclerView.SetAdapter(allergenseAdapter);
            ovh.SetItemClickListener(this);
            ovh.itemDescription.SetTypeface(MainActivity.typeface, TypefaceStyle.Normal);
            ovh.itemName.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
            ovh.itemPrice.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
            ovh.itemRatings.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
        }

        public void OnClick(View itemView, int position, bool isLongClick, CardView categoryCardview, TextView categoryTitle)
        {
            menuItemClicked.Invoke(mainActivity, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.menu_items, parent, false);
            return new MenuItemsViewHolder(itemView);
        }
    }
}