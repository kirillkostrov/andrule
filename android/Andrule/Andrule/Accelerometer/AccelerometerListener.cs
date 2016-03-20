using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Android.Views;

namespace Andrule.Accelerometer
{
    class AccelerometerListener : View, ISensorEventListener
    {
        private readonly SensorManager _sensorManager;
        private readonly Sensor _sensor;
        private bool _needUpdate;

        private const float SmoothingFactor = 0.4f;

        private float _previousSteeringReading;
        private float _previousThrottleReading;

        private const int QuarterMaxValue = 16383;

        public delegate bool UpdateAccelerometer(Accelerometer accelerometer);

        public event UpdateAccelerometer OnUpdateAccelerometer;

        public AccelerometerListener(Context context, SensorManager sensorManager, UpdateAccelerometer onUpdateAccelerometer)
            : base(context)
        {
            _sensorManager = sensorManager;
            _sensor = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            OnUpdateAccelerometer = onUpdateAccelerometer;
        }

        public void Start()
        {
            _sensorManager.RegisterListener(this, _sensor, SensorDelay.Ui);
            _needUpdate = true;
        }

        public void Stop()
        {
            _sensorManager.UnregisterListener(this);
        }

        private float LowPassFilter(float newReading, ref float oldReading)
        {
            var filteredValue = 0.0f;

            filteredValue = SmoothingFactor * newReading + (1 - SmoothingFactor) * oldReading;
            oldReading = filteredValue;

            return filteredValue;
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (!_needUpdate)
            {
                return;
            }

            var rotation = (int)(LowPassFilter(e.Values[1], ref _previousSteeringReading) * 1638 + QuarterMaxValue);
            var throttle = (int)(QuarterMaxValue - LowPassFilter(e.Values[2], ref _previousThrottleReading) * 1638 * 2);
            var zAxis = QuarterMaxValue;
            var accelerometerData = new Accelerometer(rotation, throttle, zAxis );

            _needUpdate = OnUpdateAccelerometer == null || OnUpdateAccelerometer(accelerometerData);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) { }
    }
}