# Device Motion Plugin details

Cross platform plugin to detect motion sensors value changes for: Accelerometer, Gyroscope, Magnetometer, Compass. Across iOS, Android and Windows Phone.

### Features:
* Start listening sensor value changes
* Stop listening sensor value changes
* Access sensor value changes
* Check if is listening sensor


### Example
```
CrossDeviceMotion.Current.Start (MotionSensorType.Accelerometer);
CrossDeviceMotion.Current.SensorValueChanged+=(s, a)=>{

				switch(a.SensorType)
				{
				   
				   case MotionSensorType.Accelerometer:
					Debug.WriteLine("A: {0},{1},{2}",((MotionVector)a.Value).X,((MotionVector)a.Value).Y,((MotionVector)a.Value).Z);
				
					break;

			   }
}
```

Works completely from a shared code or PCL project.