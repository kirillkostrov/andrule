using System;
using Android.Content;
using Android.Hardware;
using Android.Runtime;

namespace Andrule
{
    //class SimulationListener : View, ISensorEventListener
    //{
    //    private SensorManager _sensorManager;
    //    private Sensor _sensor;

    //    public int Rotation { get; private set; }

    //    public SimulationListener(Context context, SensorManager sensorManager)
    //        : base(context)
    //    {
    //        _sensorManager = sensorManager;
    //        _sensor = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
    //    }

    //    public void StartSimulation()
    //    {
    //        _sensorManager.RegisterListener(this, _sensor, SensorDelay.Normal);
    //    }

    //    public void StopSimulation()
    //    {
    //        _sensorManager.UnregisterListener(this);
    //    }

    //    public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
    //    {

    //    }

    //    public void OnSensorChanged(SensorEvent e)
    //    {
    //        if (e.Sensor.Type != _sensor.Type)
    //        {
    //            return;
    //        }

    //        Rotation = (int)(e.Values[1] * 100);
    //    }
    //}
}