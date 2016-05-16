using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using System.Net;
using System.IO;
using Android.Graphics;

namespace ColPattAndroidAssignment 
{
    public static class Util
    {
        public const string TAG = "ColPattAndroidAssignment";
        public const string url_color = "http://www.colourlovers.com/api/colors/random";
        public const string url_pattern = "http://www.colourlovers.com/api/patterns/random";
       
        public const int CIRCLE_RADIUS = 100;
        public const int SHAPES_LIMIT = 20;
        public const int RECT_SIZE = 150;

        private static Bitmap squareBmp = null;
        private static Random mRandGenerator = new Random();

        public static Bitmap getBitmapFromUrl(string bmpurl)
        {
            try
            {
                if(bmpurl != null && bmpurl != "")
                {
                    var imageBytes = Network.getDataAsByteArrayFromURL(bmpurl);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        squareBmp = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch(Exception asd)
            {
                Log.Warn(TAG, "Error in Bitmap Creation : " + asd.StackTrace);
            }
            
            return squareBmp;
        }//fn getBitmapFromUrl

        public static int getRandomIntValue(int start, int end)
        {
            int randval = 0;
            try
            {
                randval = mRandGenerator.Next(start, end);
            }
            catch (Exception asd){
                Log.Warn(TAG, "Error in Random Number Creation : " + asd.StackTrace);
            }
            return randval;
        }//fn getRandomIntValue


        public static int[] getRandomColorsValueAsArray()
        {
            int[] colorArray = new int[3];
            try
            {
                colorArray[0] = mRandGenerator.Next(0, 255);
                colorArray[1] = mRandGenerator.Next(0, 255);
                colorArray[2] = mRandGenerator.Next(0, 255);
            }
            catch (Exception asd)
            {
                Log.Warn(TAG, "Error in Random Color Creation : " + asd.StackTrace);
            }
            return colorArray;
        }

        public static void displayToastMessage(Context ctx, string message)
        {
            Toast.MakeText(ctx, message, ToastLength.Long).Show();
        }//fn displayToastMessage

    }//End of class Util
}