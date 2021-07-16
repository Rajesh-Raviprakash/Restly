using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restly.Activities;
using Restly.Controls;
using Restly.Model;
using Restly.Resources;
using Restly.WebService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Restly
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        //Android.Widget.Toolbar _toolbar;

        public static MenuModel menuData;
        public static MenuItemsModel menuItemsData;
        public static HttpClient _client = new HttpClient();

        MenuItemsListAdapter menuAdapter;
        MenuCategoryListAdapter categoryAdapter;

        public RecyclerView categoryRecyclerView, menuRecyclerView;

        public static Bitmap[] catIcons,menuIcons;
        public static int categoryTabSelected = 0;

        public EditText searchText;
        public Button clear;
        public TextView menu, location;

        // Unique ID for our notification: 
        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";

        public int menuPageShown = 0;
        public int itemsPerPage = Convert.ToInt32(AppResource.ItemsPerPage);
        public int restaurantId = Convert.ToInt32(AppResource.RestaurantId);

        public List<int> categorylist = new List<int>();

        int count = 1;
        private LinearLayoutManager menu_LayoutManager;
        private bool isLoadingData = false;
        private XamarinRecyclerViewOnScrollListener onScrollListener;
        public static ProductModel productData;
        private FloatingActionButton fab;
        public static Typeface typeface;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            //removing default title
            Window.RequestFeature(WindowFeatures.NoTitle);
            //set contentview
            SetContentView(Resource.Layout.activity_main);

            //initialize components
            clear = FindViewById<Button>(Resource.Id.clear);
            categoryRecyclerView = FindViewById<RecyclerView>(Resource.Id.categoryRecyclerView);
            menuRecyclerView = FindViewById<RecyclerView>(Resource.Id.menuRecyclerView);
            searchText = FindViewById<EditText>(Resource.Id.menu_searchbar);
            menu = FindViewById<TextView>(Resource.Id.titleText);
            location = FindViewById<TextView>(Resource.Id.location);

            //category layout manager
            var category_LayoutManager = new LinearLayoutManager(this);
            category_LayoutManager.Orientation = LinearLayoutManager.Horizontal;
            categoryRecyclerView.SetLayoutManager(category_LayoutManager);

            //menu layout manager
            menu_LayoutManager = new LinearLayoutManager(this);
            menuRecyclerView.SetLayoutManager(menu_LayoutManager);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //floating action button 
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabButton);



            typeface = Typeface.CreateFromAsset(Assets, "montserrat_regular.ttf");
            menu.SetTypeface(typeface,TypefaceStyle.Bold);
            location.SetTypeface(typeface, TypefaceStyle.Normal);

            searchText.SetTypeface(typeface, TypefaceStyle.Normal);
            clear.SetTypeface(typeface, TypefaceStyle.Normal);
            
            CreateNotificationChannel();

            Attachments();
            await GetDataandSetAdapterAsync();
        }

        /// <summary>
        /// all click events will be defined
        /// </summary>
        private void Attachments()
        {
            fab.Click += FabOnClick;
            clear.Click += Clear_Click;
            searchText.EditorAction += SearchText_EditorAction;
        }

        /// <summary>
        /// Matches the search string with catergory names, if match found then set adapter for new category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SearchText_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Search)
            {
                SearchStringWithcategoryItems();
                e.Handled = true;
            }
        }

        private void SearchStringWithcategoryItems()
        {
            GetMenuItemsForSearchString(searchText.Text);
        }

        private void GetMenuItemsForSearchString(string searchText)
        {
            if(!String.IsNullOrEmpty(searchText))
            {
                closeKeyboard();
                for (int i = 0; i < menuData.Data.MenuCategories.Length; i++)
                {
                    if (searchText.Contains(menuData.Data.MenuCategories[i].Title))
                    {
                        GetItemsWithCategoryIdAsync((int)menuData.Data.MenuCategories[i].Id);
                        categoryTabSelected = i;
                        SetCategoryAdapter(categoryAdapter);
                    }
                }

            }
        }

        /// <summary>
        /// closes soft keyboard
        /// </summary>
        private void closeKeyboard()
        {
            View view = this.CurrentFocus;
            if (view != null)
            {
                InputMethodManager manager = (InputMethodManager)GetSystemService(Context.InputMethodService);
                manager.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }

        /// <summary>
        /// clears the text from search editText
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, EventArgs e)
        {
            searchText.Text = "";
        }

        /// <summary>
        /// Creating local notification channel
        /// </summary>

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var name = AppResource.AppName;
            var description = "";
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        /// <summary>
        /// Floating Action Button clicked : publishing local notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FabOnClick(object sender, EventArgs e)
        {
            try
            {
                var valuesForActivity = new Bundle();
                valuesForActivity.PutInt(COUNT_KEY, count);

                var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                              .SetAutoCancel(true)
                              .SetContentTitle(AppResource.HelloText)
                              .SetNumber(count)
                              .SetSmallIcon(Resource.Drawable.search)
                              .SetContentText(AppResource.NotificationDescription);

                var notificationManager = NotificationManagerCompat.From(this);
                notificationManager.Notify(NOTIFICATION_ID, builder.Build());

                count++;
            }
            catch(Exception ee)
            {
                // handling exceptions
            }
        }

        /// <summary>
        /// Get the data from web service and sets to respective Recyclerview adapter
        /// </summary>
        /// <returns></returns>
        private async Task GetDataandSetAdapterAsync()
        {
            try
            {
                ProgressIndicator.StartAnimation(this);

                menuData =  await HttpWebService.GetMenuDataAsync();

                GetCategorIcons();

                menuItemsData =  await HttpWebService.GetMenuItems(menuPageShown, itemsPerPage, restaurantId, categorylist);

                GetMenuIcons();

                SetCategoryAdapter(categoryAdapter);

                SetMenuListAdapter(menuAdapter);
                
                //On reaching last item in the menu list updating with new items
                onScrollListener = new XamarinRecyclerViewOnScrollListener(menu_LayoutManager);
                onScrollListener.LoadMoreEvent += async (object sender, EventArgs e) =>
                {
                    if (!isLoadingData)
                    {
                        menuPageShown += 1;
                        if (menuPageShown < menuItemsData.data.totalPages)
                        {
                            isLoadingData = true;

                            ProgressIndicator.StartAnimation(this);

                            //web service call
                            menuItemsData = await HttpWebService.GetMenuItems(menuPageShown, itemsPerPage, restaurantId, categorylist);

                            GetMenuIcons();

                            menuAdapter = new MenuItemsListAdapter(this, menuItemsData.data, menuIcons);
                            menuRecyclerView.SetAdapter(menuAdapter);
                            menuAdapter.menuItemClicked += async (sender, index) =>
                            {
                                ProgressIndicator.StartAnimation(this);

                                productData = await HttpWebService.GetItemById(GetProductIdbyIndex(index));
                                ProgressIndicator.StopAnimation();

                                if (productData.Success && productData != null)
                                {
                                    Intent i = new Intent(this, typeof(ProductActivity));
                                    i.PutExtra("productID", GetProductIdbyIndex(index).ToString());
                                    StartActivity(i);
                                }
                            };
                            isLoadingData = false;
                            ProgressIndicator.StopAnimation();
                        }
                    }
                };

                menuRecyclerView.AddOnScrollListener(onScrollListener);
                ProgressIndicator.StopAnimation();
            }
            catch (Exception exc)
            {
                //
                ProgressIndicator.StopAnimation();
            }
        }
        /// <summary>
        /// set data to the menu adapter to show
        /// </summary>
        /// <param name="menuAdapter"></param>
        private void SetMenuListAdapter(MenuItemsListAdapter menuAdapter)
        {
            menuAdapter = new MenuItemsListAdapter(this, menuItemsData.data, menuIcons);
            menuRecyclerView.SetAdapter(menuAdapter);
            menuAdapter.menuItemClicked += async (sender, index) =>
            {
                ProgressIndicator.StartAnimation(this);
                productData = await HttpWebService.GetItemById(GetProductIdbyIndex(index));

                ProgressIndicator.StopAnimation();
                Intent i = new Intent(this, typeof(ProductActivity));
                StartActivity(i);
            };
        }
        /// <summary>
        /// set data to the category adapter to show
        /// </summary>
        /// <param name="categoryAdapter"></param>
        private void SetCategoryAdapter(MenuCategoryListAdapter categoryAdapter)
        {
            categoryAdapter = new MenuCategoryListAdapter(this, menuData.Data.MenuCategories, catIcons);
            categoryRecyclerView.SetAdapter(categoryAdapter);

            //click handler
            categoryAdapter.categorySelected += (sender, e) =>
            {
                if (e > 3)
                {
                    categoryRecyclerView.GetLayoutManager().ScrollToPosition(e);
                }
                categoryRecyclerView.SetAdapter(categoryAdapter);

                GetItemsWithCategoryIdAsync(GetCategoryIdbyIndex(e));
            };

        }

        /// <summary>
        /// Get all the menu icons from server
        /// </summary>
        private void GetMenuIcons()
        {
            if (menuItemsData.success && menuItemsData.data.data.Count > 0)
            {
                menuIcons = new Bitmap[menuItemsData.data.data.Count];
                menuIcons = HttpWebService.GetImages(menuItemsData.data.data);
            }
        }

        /// <summary>
        /// get all the category icons from server
        /// </summary>
        private void GetCategorIcons()
        {
            if (menuData.Success && menuData.Data.MenuCategories.Length > 0)
            {
                catIcons = new Bitmap[menuData.Data.MenuCategories.Length];
                catIcons = HttpWebService.GetImages(menuData.Data.MenuCategories);
            }
        }


        /// <summary>
        /// get product ID by its index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>productID</returns>
        private int GetProductIdbyIndex(int index)
        {
            for(int i=0; i < menuItemsData.data.data.Count; i++)
            {
                if(i == index)
                {
                    return menuItemsData.data.data[i].id;
                }
            }
            return -1;
        }

        /// <summary>
        /// gets items for respective categoryID passed
        /// </summary>
        /// <param name="categoryID"></param>
        private async void GetItemsWithCategoryIdAsync(int categoryID)
        {
            menuPageShown = 0;
            categorylist.Clear();
            categorylist.Add(categoryID);
            isLoadingData = true;

            ProgressIndicator.StartAnimation(this);

            //web service call

            menuItemsData = await HttpWebService.GetMenuItems(menuPageShown, itemsPerPage, restaurantId, categorylist);
            GetMenuIcons();

            SetMenuListAdapter(menuAdapter);
            ProgressIndicator.StopAnimation();
        }

        /// <summary>
        /// returns the categoryID by its index
        /// </summary>
        /// <param name="posOfCategory"></param>
        /// <returns></returns>
        private int GetCategoryIdbyIndex(int posOfCategory)
        {
            for(int i=0; i< menuData.Data.MenuCategories.Length; i++)
            {
                if(i == posOfCategory)
                {
                    return Convert.ToInt32(menuData.Data.MenuCategories[i].Id);
                }
            }
            return -1;
        }

        /// <summary>
        /// handles the permission required
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_menu:
                    return true;
                case Resource.Id.navigation_order:
                    return true;
                case Resource.Id.navigation_tab:
                    return true;
                case Resource.Id.navigation_account:
                    return true;
            }
            return false;
        }
    }
}

