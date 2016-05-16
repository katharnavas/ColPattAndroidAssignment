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
using Android.Graphics;

namespace ColPattAndroidAssignment
{
    public class Shape
    {
        public int radius;
        public int centerX;
        public int centerY;
        public bool isCircle = true;
        private int redVal = 255;
        private int greenVal = 255;
        private int blueVal = 255;
        private string imageUrl = "";
        private bool isWebServiceCallSucess = false;
        private Bitmap sqBmp = null;
     

        public Shape(int centerX, int centerY, int radius)
        {
            this.radius = radius;
            this.centerX = centerX;
            this.centerY = centerY;
        }

        public Shape(int centerX, int centerY, int radius, bool isCircleFlag)
        {
            this.radius = radius;
            this.centerX = centerX;
            this.centerY = centerY;
            this.isCircle = isCircleFlag;
        }

        public string toString()
        {
            return "Circle[" + centerX + ", " + centerY + ", " + radius + "]";
        }

        public void setColors(int red, int green, int blue)
        {
            this.redVal = red;
            this.greenVal = green;
            this.blueVal = blue;
        }

        public int[] getColorsValueAsArray()
        {
            int[] colorArray = new int[] { this.redVal, this.greenVal, this.blueVal};
            return colorArray;
        }

        public void setImageUrl(string imgUrl)
        {
            this.imageUrl = imgUrl;
        }

        public string getSquareImageUrl()
        {
            return this.imageUrl;
        }

        public void setSquareBitmap(Bitmap bmp)
        {
            this.sqBmp = bmp;
        }

        public Bitmap getSquareBitmap()
        {
            return this.sqBmp;
        }

        public void setWebServiceCallSuccess(bool isSucessFlag)
        {
            this.isWebServiceCallSucess = isSucessFlag;
        }

        public bool getIsWebServiceCallSuccess()
        {
            return this.isWebServiceCallSucess;
        }

    }//End of class Shape
}