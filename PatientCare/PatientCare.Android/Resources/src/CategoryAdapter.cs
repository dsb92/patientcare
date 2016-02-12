using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using Java.Lang;
using PatientCare.Shared.Model;
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace PatientCare.Android.Resources.src
{
    public class CategoryAdapter : BaseAdapter
    {
        private Context mContext;
        private CategoryEntity[] Categories;

        // Constructor
        public CategoryAdapter(Context c, CategoryEntity[] categories)
        {
            mContext = c;
            Categories = categories;
        }

        public override Object GetItem(int position)
        {
            return Categories[position].Name;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView = new ImageView(mContext);
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            imageView.LayoutParameters = (new GridView.LayoutParams(250, 250));

            if (Categories[position].Picture != null)
            {
                var webClient = new WebClient();
                webClient.DownloadDataCompleted += (s, e) =>
                {
                    try
                    {
                        var bytes = e.Result; // get the downloaded data
                        
                        imageView.SetImageBitmap(ImageHandler.BytesToImage(bytes));  // convert the data to an actual image
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Something went wrong loading image for cell..." + ex.Message);
                    }

                };
                webClient.DownloadDataAsync(new Uri(Categories[position].Picture));
            }

            return imageView;
        }

        public override int Count
        {
            get { return Categories.Length; }
        }
    }
}