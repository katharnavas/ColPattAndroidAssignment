using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Util;

namespace ColPattAndroidAssignment
{
    [Activity(Label = "ColPattAndroidAssignment", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ISensorEventListener //, View.IOnTouchListener
    {
        private LinearLayout myLinearLayout = null;
        private CustomShapeView customView = null;

        private SensorManager mSensorManager;
        private float mAccel; // acceleration apart from gravity
        private float mAccelCurrent; // current acceleration including gravity
        private float mAccelLast; // last acceleration including gravity


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            myLinearLayout = FindViewById<LinearLayout>(Resource.Id.MylinearLayout);
            customView = FindViewById<CustomShapeView>(Resource.Id.shapesview_main);
            try
            {
                mSensorManager = (SensorManager)GetSystemService(Context.SensorService);
                mSensorManager.RegisterListener(this, mSensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);
                mAccel = 0.00f;
                mAccelCurrent = SensorManager.GravityEarth;
                mAccelLast = SensorManager.GravityEarth;
            }
            catch(Exception asd)
            {
                Log.Warn(Util.TAG, "Error in Main Activity Accelerometer Init: " + asd.StackTrace);
            }
        }//fn OnCreate

        protected override void OnResume()
        {
            base.OnResume();
            if(mSensorManager != null)
                 mSensorManager.RegisterListener(this, mSensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);

            mAccel = 0.00f;
            mAccelCurrent = SensorManager.GravityEarth;
            mAccelLast = SensorManager.GravityEarth;
        }

        protected override void OnPause()
        {
            if(mSensorManager != null)
                 mSensorManager.UnregisterListener(this);

            base.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }


        void ISensorEventListener.OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //Nothing to perform here
        }

        void ISensorEventListener.OnSensorChanged(SensorEvent se)
        {
            float x = se.Values[0];
            float y = se.Values[1];
            float z = se.Values[2];
            mAccelLast = mAccelCurrent;
            mAccelCurrent = (float)Math.Sqrt((double)(x * x + y * y + z * z));
            float delta = mAccelCurrent - mAccelLast;
            mAccel = mAccel * 0.9f + delta; // perform low-cut filter
            //Log.Warn(Util.TAG, "Acceleration Sensor Values : "+x+ " : "+y+" : "+z+" : "+ mAccelLast+" : "+ mAccel);
            //Check mAccel value to denote the amount of shake

            
            if (mAccel > 6)
            {
                try
                {
                    Log.Warn(Util.TAG, "Acceleration Value > one : " + mAccel);
                    if (customView != null)
                    {
                        customView.clearAllShapes();
                        customView.Invalidate();
                    }
                }
                catch (Exception asd)
                {
                    Log.Warn(Util.TAG, "Error in Main Activity clearing view: " + asd.StackTrace);
                }
            }
        }

    }//End of class Main Activity
}
