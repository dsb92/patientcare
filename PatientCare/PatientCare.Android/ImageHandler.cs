using System;
using Android.Graphics;

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