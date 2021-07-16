using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Restly.Activities;
using Restly.Model;
using Restly.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restly.Controls
{

    public class OptionsScreenAdapterHolder : RecyclerView.ViewHolder
    {

        public RecyclerView optionScreenRecyclerView;
        public TextView title,optionalText;

        public OptionsScreenAdapterHolder(View itemView) : base(itemView)
        {
            optionScreenRecyclerView = itemView.FindViewById<RecyclerView>(Resource.Id.product_options_recyclerview);
            title = ItemView.FindViewById<TextView>(Resource.Id.product_options_title);
            optionalText = ItemView.FindViewById<TextView>(Resource.Id.product_options_optional);
        }
    }
    class OptionsScreenAdapter : RecyclerView.Adapter
    {
        private ProductActivity productActivity;
        private DataOption[] options;

        public EventHandler<int> single_Selection;
        public EventHandler<int> multiple_Selection;


        public OptionsScreenAdapter(ProductActivity productActivity, DataOption[] options)
        {
            this.productActivity = productActivity;
            this.options = options;
            
        }

        public override int ItemCount
        {
            get
            {
                return options.Length;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OptionsScreenAdapterHolder adapter_holder = holder as OptionsScreenAdapterHolder;

            var LM = new LinearLayoutManager(productActivity);
            adapter_holder.optionScreenRecyclerView.SetLayoutManager(LM);

            adapter_holder.title.Text = options[position].Name;

            if(options[position].IsRequired)
            {
                adapter_holder.optionalText.Visibility = ViewStates.Gone;
            }
            else
            {
                adapter_holder.optionalText.Visibility = ViewStates.Visible;
                adapter_holder.optionalText.Text = AppResource.OptionalText;
            }

            if (options[position].Type == 0)
            {
                var optionsAdapter = new SingleSelectionAdapter(productActivity, options[position],ProductActivity.selectedPos[position]);
                adapter_holder.optionScreenRecyclerView.SetAdapter(optionsAdapter);

                optionsAdapter.sidesOptionSelected += (sender, pos) =>
                {
                    ProductActivity.selectedPos[position] = pos;
                    single_Selection.Invoke(sender, pos);
                };
            }
            else
            {
                var optionsAdapter = new OptionsDataAdapter(productActivity, options[position],ProductActivity.checkedOptions);
                adapter_holder.optionScreenRecyclerView.SetAdapter(optionsAdapter);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.options_screen_activity, parent, false);
            return new OptionsScreenAdapterHolder(itemView);
        }
    }
}