using System;

namespace AndruleServer
{
    public static class DataProcessor
    {
        public static PhoneData Process(float x, float y, float z, int b1, int b2, int b3, int b4)
        {
            return new PhoneData
            {
                AxisX = (int)Math.Abs(x),
                AxisY = (int)Math.Abs(y),
                AxisZ = (int)Math.Abs(z),
                Button1 = b1,
                Button2 = b2,
                Button3 = b3,
                Button4 = b4
            };
        }
    }
}