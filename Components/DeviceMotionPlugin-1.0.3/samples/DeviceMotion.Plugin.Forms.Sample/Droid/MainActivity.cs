using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DeviceMotion.Plugin.Forms.Sample.Droid
{
	[Activity (Label = "DeviceMotion.Plugin.Forms.Sample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());

			CrossDeviceMotion.Current.SensorValueChanged+=(s,a)=>{
				System.Diagnostics.Debug.WriteLine("From ANDROID");
			};
		}
	}
}

