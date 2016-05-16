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
using System.Net;
using Android.Util;
using System.Xml.Linq;

namespace ColPattAndroidAssignment
{
    public static class Network
    {
        public static byte[] getDataAsByteArrayFromURL(String url)
        {
            byte[] imageBytes = null;
            try
            {
                using (var webClient = new WebClient())
                {
                    imageBytes = webClient.DownloadData(url);

                }
            }
            catch (Exception asd)
	        {
                Log.Warn(Util.TAG, "Error in downloading Bitmap : " + asd.StackTrace);
            }
            return imageBytes;
        }//fn getDataAsByteArrayFromURL


        public static XDocument getXDocumentFromUrl(string url)
        {
            XDocument xDoc = null;
            try
            {
                WebClient wclient = new WebClient();
                wclient.Headers.Add("Accept-Language", " en-US");
                wclient.Headers.Add("Accept", "application/xml");
                wclient.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                string xmlStr = wclient.DownloadString(url);

                if(xmlStr != null && xmlStr != "")
                {
                    xDoc = XDocument.Parse(xmlStr);
                    // Console.WriteLine("XMl Value :"+xDoc);
                }
            }
            catch(Exception asd)
            {
                Log.Warn(Util.TAG, "Error in xdocument creation: " + asd.StackTrace);
            }
            return xDoc;
        }

    }//End of class Network
}