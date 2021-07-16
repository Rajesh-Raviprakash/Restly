using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Restly
{
    public class MenuCategoryViewHolder : RecyclerView.ViewHolder , View.IOnClickListener
    {
        public TextView categoryTitle;
        public ImageView categoryIcon;
        public CardView categoryCardview;
        private IItemClickListener itemClickListener;

        public MenuCategoryViewHolder(View itemView) : base(itemView)
        {
            categoryTitle = itemView.FindViewById<TextView>(Resource.Id.category_title);
            categoryIcon = itemView.FindViewById<ImageView>(Resource.Id.category_icon);
            categoryCardview = itemView.FindViewById<CardView>(Resource.Id.category_card);
            categoryCardview.SetOnClickListener(this);

            categoryCardview.UseCompatPadding = true;
            categoryCardview.Radius = 5;
            categoryCardview.Elevation = 5;
        }

        public void SetItemClickListener(IItemClickListener itemClickListener)
        {
            this.itemClickListener = itemClickListener;
        }
        public void OnClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, false, categoryCardview, categoryTitle);
        }
    }


        public class MenuCategoryListAdapter : RecyclerView.Adapter , IItemClickListener
        {
            private MainActivity mainActivity;
            private MenuCategory[] menuCategories;
            private Bitmap[] catIcons;
            public EventHandler<int> categorySelected;



        public MenuCategoryListAdapter(MainActivity mainActivity, MenuCategory[] menuCategories, Bitmap[] catIcons)
        {
            this.mainActivity = mainActivity;
            this.menuCategories = menuCategories;
            this.catIcons = catIcons;
        }

        public override int ItemCount
            {
                get
                {
                    return menuCategories.Length;
                }
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MenuCategoryViewHolder ovh = holder as MenuCategoryViewHolder;
                ovh.categoryTitle.Text = menuCategories[position].Title;
                ovh.categoryIcon.SetImageBitmap(Bitmap.CreateScaledBitmap(catIcons[position], 120, 120, false));
                ovh.SetItemClickListener(this);

                if (MainActivity.categoryTabSelected == position)
                {
                    ovh.categoryCardview.SetCardBackgroundColor(Color.Rgb(44, 165, 111));
                    ovh.categoryTitle.SetTextColor(Color.White);
                }
                else
                {
                    ovh.categoryTitle.SetTextColor(Color.Black);
                    ovh.categoryCardview.SetCardBackgroundColor(Color.White);
                }

            ovh.categoryTitle.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);

        }
            public void OnClick(View itemView, int position, bool isLongClick, CardView categoryCardview, TextView categoryTitle)
            {
                MainActivity.categoryTabSelected = position;
                categorySelected.Invoke(mainActivity, position);
            }

        public override RecyclerView.ViewHolder OnCreateViewHolder(Android.Views.ViewGroup parent, int viewType)
            {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.category_items, parent, false);
            return new MenuCategoryViewHolder(itemView);
        }
        }

}