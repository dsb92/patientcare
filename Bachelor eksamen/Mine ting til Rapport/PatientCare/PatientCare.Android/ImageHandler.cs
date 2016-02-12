using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PatientCare.Android
{
    public static class ImageHandler
    {
        public static Bitmap BytesToImage(byte[] imageBytes)
        {
            try
            {
                return BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error converting imageBytes to Bitmap: " + e.Message);
                return null;
            }
            
        }
    }
}