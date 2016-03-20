namespace Andrule.Accelerometer
{
    public class Accelerometer
    {
        public Accelerometer(int rotation, int throttle, int zAxis)
        {
            Rotation = rotation;
            Throttle = throttle;
            ZAxis = zAxis;
        }

        public int Rotation { get; set; }

        public int Throttle { get; set; }

        public int ZAxis { get; set; }
    }
}