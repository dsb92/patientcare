using System;
using Foundation;
using UIKit;

namespace PatientCare.iOS
{
    public static class ImageHandler
    {
        public static UIImage BytesToImage(byte[] imageBytes)
        {
            try
            {

                UIImage image = new UIImage(NSData.FromArray(imageBytes));
                return image;

            }
            catch (Exception e)
            {

                Console.WriteLine("Error converting imageBytes to UIImage: " + e.Message);
                return null;
            }
        }
    }
}
