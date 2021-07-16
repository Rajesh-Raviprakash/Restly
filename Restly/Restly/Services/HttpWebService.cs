using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restly.Model;
using Restly.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Restly.WebService
{
    class HttpWebService
    {
        public static MenuModel menuData;
        public static HttpClient _client = new HttpClient();

        /// <summary>
        /// Web service call for menu data for initial screen
        /// </summary>
        /// <returns> Menu data (categories and menu items)</returns>

        public static async Task<MenuModel> GetMenuDataAsync()
        {
            var menuUrl = AppResource.BaseUrl + AppResource.InitUrl + AppResource.RestaurantId;
            var menuResponse = await _client.GetStringAsync(menuUrl);

            return JsonConvert.DeserializeObject<MenuModel>(menuResponse);  
        }

        public static Bitmap[] GetImages(MenuCategory[] menuCategories)
        {
            var tempIcons = new Bitmap[menuCategories.Length];
            for (int i = 0; i < menuCategories.Length; i++)
            {
                Bitmap imageBitmap = GetImageBitmapFromUrl(menuCategories[i].ImageUrl.ToString());
                tempIcons[i] = imageBitmap;
            }
            return tempIcons;
        }

        public static Bitmap GetImageBitmapFromUrl(string imageUrl)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(imageUrl);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }

        /// <summary>
        /// web service call to get menu items of specified category by its ID
        /// </summary>
        /// <param name="menuPageShown"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="restaurantId"></param>
        /// <param name="categorylist"></param>
        /// <returns> menu itsms</returns>
        internal static async Task<MenuItemsModel> GetMenuItems(int menuPageShown, int itemsPerPage, int restaurantId, List<int> categorylist)
        {
            //get Only menu items data
            var menuItemsUrl = AppResource.BaseUrl + AppResource.MenuItemsUrl;

            var obj = new JObject();
            obj["Page"] = menuPageShown;
            obj["PageSize"] = itemsPerPage;
            obj["RestaurantId"] = restaurantId;
            obj["Term"] = "";
            obj["ProductCategoryIdList"] = (JToken)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(categorylist));

            var serialized = JsonConvert.SerializeObject(obj);

            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(menuItemsUrl, content);

            // on error throw a exception  
            result.EnsureSuccessStatusCode();

            // handling the answer  
            var resultString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MenuItemsModel>(resultString);
        }

        /// <summary>
        /// Get all the icons 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Array of Bitmap</returns>
        public static Bitmap[] GetImages(IList<Datas> data)
        {
            var tempIcons = new Bitmap[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                Bitmap imageBitmap = GetImageBitmapFromUrl(data[i].imageUrl.ToString());
                tempIcons[i] = imageBitmap;
            }
            return tempIcons;
        }

        /// <summary>
        /// web service call to get details of the menu item by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>product details</returns>
        internal static async Task<ProductModel> GetItemById(int id)
        {
            var productUrl = AppResource.BaseUrl + AppResource.ProductByIdUrl + id;
            var productResponse = await _client.GetStringAsync(productUrl);

            return JsonConvert.DeserializeObject<ProductModel>(productResponse);
        }
    }
}