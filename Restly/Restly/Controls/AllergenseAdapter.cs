using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Restly.Activities;
using System.Collections.Generic;

namespace Restly
{
    public class AllergenseViewHolder : RecyclerView.ViewHolder
    {
        public TextView itemName;


        public AllergenseViewHolder(View itemView) : base(itemView)
        {
            itemName = itemView.FindViewById<TextView>(Resource.Id.allergenceTitle);
        }
    }


    internal class AllergenseAdapter : RecyclerView.Adapter
    {
        private Android.Content.Context _context;
        private IList<string> allergens;

        public AllergenseAdapter(Android.Content.Context _context, IList<string> allergens)
        {
            this._context = _context;
            this.allergens = allergens;
        }

        public override int ItemCount
        {
            get
            {
                return allergens.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AllergenseViewHolder ovh = holder as AllergenseViewHolder;
            ovh.itemName.Text = allergens[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.allergence_items, parent, false);
            return new AllergenseViewHolder(itemView);
        }
    }
}