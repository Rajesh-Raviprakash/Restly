using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Restly.Controls;
using Restly.Model;
using Restly.Resources;
using Restly.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Restly.Activities
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductActivity : Activity
    {
        RecyclerView productAllergenseRecyclerView;
        TextView productTitle, productPrice, productDescription,suggestionTitle, preferenceTitle, quantity;
        ProductModel productData;
        ImageView productImage;
        AllergenseAdapter productAllergenseAlergense;
        ImageButton plus, minus;
        Button addToCart;
        EditText preferenceEditText;

        RecyclerView optionsRecyclerView,suggestionRecyclerView;
        public static int[] selectedPos;
        public static bool[] checkedOptions;

        private Bitmap[] suggestedIcons;
        int itemQuantity = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.product_activity);
            
            Initialization();

            productData = MainActivity.productData;

            SetAdapterAsync();

            Attachments();

            ChangeButtonStates();
            preferenceTitle.Text = AppResource.SpecialPreferenceText;

            preferenceTitle.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
            suggestionTitle.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
        }

        private void Attachments()
        {
            plus.Click += Plus_Click;
            minus.Click += Minus_Click;
            addToCart.Click += AddToCart_Click;
        }

        private void AddToCart_Click(object sender, EventArgs e)
        {
             Dialog dialog =new Dialog(this);
            dialog.SetTitle("yeaa !");
            dialog.SetContentView(Resource.Layout.dailog_layout);
            int width = (int)(this.Resources.DisplayMetrics.WidthPixels * 0.90);
            int height = (int)(this.Resources.DisplayMetrics.HeightPixels * 0.40);

            dialog.Window.SetLayout(width, height);
            dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnimation;
            Button btnDismiss = (Button)dialog.Window.FindViewById<Button>(Resource.Id.dismiss);

            btnDismiss.Click += (sender, e) =>
            {
                dialog.Dismiss();
            };

            dialog.Show();
        }

        private void Minus_Click(object sender, EventArgs e)
        {
            if(itemQuantity > 1)
            {
                itemQuantity -= 1;
                quantity.Text = itemQuantity.ToString();
            }
            ChangeButtonStates();
        }

        private void ChangeButtonStates()
        {
            if(itemQuantity > 1)
            {
                minus.Enabled = true;
                minus.Alpha =1;
            }
            else
            {
                minus.Enabled = false;
                minus.Alpha = (float)0.5;
            }
        }

        private void Plus_Click(object sender, EventArgs e)
        {
            itemQuantity += 1;
            quantity.Text = itemQuantity.ToString();
            ChangeButtonStates();
        }

        private void Initialization()
        {
            productAllergenseRecyclerView = FindViewById<RecyclerView>(Resource.Id.product_allergens_recyclerview);
            suggestionRecyclerView = FindViewById<RecyclerView>(Resource.Id.suggestionRecyclerView);

            productTitle = FindViewById<TextView>(Resource.Id.product_title);
            productPrice = FindViewById<TextView>(Resource.Id.product_price);
            productDescription = FindViewById<TextView>(Resource.Id.product_discription);

            suggestionTitle = FindViewById<TextView>(Resource.Id.suggestion_title_text);
            preferenceTitle = FindViewById<TextView>(Resource.Id.preference_title_text);

            optionsRecyclerView = FindViewById<RecyclerView>(Resource.Id.options_RecyclerView);
            productImage = FindViewById<ImageView>(Resource.Id.product_image);

            var product_LayoutManager = new LinearLayoutManager(this);
            product_LayoutManager.Orientation = LinearLayoutManager.Horizontal;
            productAllergenseRecyclerView.SetLayoutManager(product_LayoutManager);

            var optionsLinearlayout = new LinearLayoutManager(this);
            optionsRecyclerView.SetLayoutManager(optionsLinearlayout);

            var suggestionLinearlayout = new LinearLayoutManager(this);
            suggestionLinearlayout.Orientation = LinearLayoutManager.Horizontal;
            suggestionRecyclerView.SetLayoutManager(suggestionLinearlayout);

            plus = FindViewById<ImageButton>(Resource.Id.quantity_plus);
            minus = FindViewById<ImageButton>(Resource.Id.quantity_minus);

            quantity = FindViewById<TextView>(Resource.Id.quantity);
            addToCart = FindViewById<Button>(Resource.Id.addToCart);
            preferenceEditText = FindViewById<EditText>(Resource.Id.preference_edittext);


            quantity.Text = itemQuantity.ToString();

            quantity.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
            addToCart.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
            preferenceEditText.SetTypeface(MainActivity.typeface, TypefaceStyle.Normal);

            
        }

        private async void SetAdapterAsync()
        {
            try
            {
                ProgressIndicator.StartAnimation(this);
                if (productData.Success)
                {
                    productTitle.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
                    productPrice.SetTypeface(MainActivity.typeface, TypefaceStyle.Bold);
                    productDescription.SetTypeface(MainActivity.typeface, TypefaceStyle.Normal);
                    var productImageBitmap = HttpWebService.GetImageBitmapFromUrl(productData.Data.ImageUrl.ToString());
                    productTitle.Text = productData.Data.Name;
                    productPrice.Text = StringOperations.AddCurrencyText(productData.Data.Price.ToString());
                    productDescription.Text = productData.Data.Description;
                    productImage.SetImageBitmap(productImageBitmap);
                    productAllergenseAlergense = new AllergenseAdapter(this, productData.Data.Allergens);
                    productAllergenseRecyclerView.SetAdapter(productAllergenseAlergense);

                    if (productData.Data.Options != null && productData.Data.Options.Length > 0)
                    {
                        selectedPos = new int[productData.Data.Options.Length];
                        checkedOptions = new bool[productData.Data.Options.Length];
                        for(int i=0; i<selectedPos.Length;i++)
                        {
                            selectedPos[i] = 0;
                            checkedOptions[i] = false;
                        }
                        var _adapter = new OptionsScreenAdapter(this, productData.Data.Options);
                        optionsRecyclerView.SetAdapter(_adapter);

                        _adapter.single_Selection += (sender, pos) =>
                        {
                            optionsRecyclerView.SetAdapter(_adapter);
                        };
                    }

                    //get menu images
                    if (productData.Success && productData != null && productData.Data.FrequentlyBoughtProducts.Length > 0)
                    {
                        suggestionTitle.Text = AppResource.YouMayAlsoLikeText;
                        suggestedIcons = new Bitmap[productData.Data.FrequentlyBoughtProducts.Length];
                        //get all category icons 
                        for (int i = 0; i < productData.Data.FrequentlyBoughtProducts.Length; i++)
                        {
                            Bitmap suggestedBitmap = HttpWebService.GetImageBitmapFromUrl(productData.Data.FrequentlyBoughtProducts[i].ImageUrl.ToString());
                            suggestedIcons[i] = suggestedBitmap;
                        }
                    }
                    else
                    {
                        suggestionTitle.Visibility = ViewStates.Gone;
                    }

                    var suggestedAdapter = new SuggestedListAdapter(this, productData.Data.FrequentlyBoughtProducts,suggestedIcons);
                    suggestionRecyclerView.SetAdapter(suggestedAdapter);
                }
                ProgressIndicator.StopAnimation();
            }
            catch(Exception pex)
            {
                ProgressIndicator.StopAnimation();
            }
        }
    }
}