using System;

using Xamarin.Forms;
using System.Diagnostics;
using DeviceMotion.Plugin.Abstractions;

namespace DeviceMotion.Plugin.Forms.Sample
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							XAlign = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
			CrossDeviceMotion.Current.Start (MotionSensorType.Accelerometer);
			CrossDeviceMotion.Current.Start (MotionSensorType.Gyroscope);
			CrossDeviceMotion.Current.Start (MotionSensorType.Magnetometer);
			CrossDeviceMotion.Current.Start (MotionSensorType.Compass);

			CrossDeviceMotion.Current.SensorValueChanged+=(s, a)=>{

				switch(a.SensorType){
				case MotionSensorType.Accelerometer:
					Debug.WriteLine("A: {0},{1},{2}",((MotionVector)a.Value).X,((MotionVector)a.Value).Y,((MotionVector)a.Value).Z);

					break;
				case MotionSensorType.Gyroscope:
					Debug.WriteLine("G:  {0},{1},{2}",((MotionVector)a.Value).X,((MotionVector)a.Value).Y,((MotionVector)a.Value).Z);

					break;
				case MotionSensorType.Magnetometer:
					Debug.WriteLine("M:  {0},{1},{2}",((MotionVector)a.Value).X,((MotionVector)a.Value).Y,((MotionVector)a.Value).Z);
					break;
				case MotionSensorType.Compass:
					Debug.WriteLine("H: {0}",a.Value);
					break;
				}
			};
		}

		protected override void OnSleep ()
		{

			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

