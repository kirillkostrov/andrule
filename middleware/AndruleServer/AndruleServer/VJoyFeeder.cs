using System;
using vJoyInterfaceWrap;

namespace AndruleServer
{
    public class VJoyFeeder
    {
        private readonly vJoy _joystick;
        private vJoy.JoystickState _joystickState;

        private int _axisX;
        private int _axisY;
        private int _axisZ;

        private readonly uint _id;

        public VJoyFeeder()
        {
            _joystick = new vJoy();
            _joystickState = new vJoy.JoystickState();
            _id = 1;
            _joystick.ResetVJD(_id);

            if (!_joystick.vJoyEnabled())
            {
                Console.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                return;
            }
            Console.WriteLine("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", _joystick.GetvJoyManufacturerString(), _joystick.GetvJoyProductString(), _joystick.GetvJoySerialNumberString());

            // Get the state of the requested device
            VjdStat status = _joystick.GetVJDStatus(_id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    Console.WriteLine("vJoy Device {0} is already owned by this feeder\n", _id);
                    break;
                case VjdStat.VJD_STAT_FREE:
                    Console.WriteLine("vJoy Device {0} is free\n", _id);
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    Console.WriteLine("vJoy Device {0} is already owned by another feeder\nCannot continue\n", _id);
                    return;
                case VjdStat.VJD_STAT_MISS:
                    Console.WriteLine("vJoy Device {0} is not installed or disabled\nCannot continue\n", _id);
                    return;
                default:
                    Console.WriteLine("vJoy Device {0} general error\nCannot continue\n", _id);
                    return;
            };

            // Test if DLL matches the driver
            UInt32 dllVer = 0, drvVer = 0;
            bool match = _joystick.DriverMatch(ref dllVer, ref drvVer);
            if (match)
                Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", dllVer);
            else
                Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", drvVer, dllVer);

            _joystick.AcquireVJD(_id);

        }

        public void Feed(PhoneData data)
        {
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            int nButtons = _joystick.GetVJDButtonNumber(_id);
            int contPovNumber = _joystick.GetVJDContPovNumber(_id);
            int discPovNumber = _joystick.GetVJDDiscPovNumber(_id);



            long maxval = 0;
            _joystick.GetVJDAxisMax(_id, HID_USAGES.HID_USAGE_X, ref maxval);

            _axisX = data.AxisX; if (_axisX > maxval) _axisX = (int)maxval; if (_axisX < 0) _axisX = 0;
            _axisY = data.AxisY; if (_axisY > maxval) _axisY = (int)maxval; if (_axisY < 0) _axisY = 0;
            _axisZ = data.AxisZ; if (_axisZ > maxval) _axisZ = (int)maxval; if (_axisZ < 0) _axisZ = 0;

            uint count = 0;

            byte[] pov = new byte[4];

            _joystickState.bDevice = (byte)_id;
            _joystickState.AxisX = _axisX;
            _joystickState.AxisY = _axisY;
            _joystickState.AxisZ = _axisZ;

            var btn1 = data.Button1 == 0 ? 0 : 1;
            var btn2 = data.Button2 == 0 ? 0 : 2;
            var btn3 = data.Button3 == 0 ? 0 : 4;
            var btn4 = data.Button4 == 0 ? 0 : 8;

            _joystickState.Buttons = (uint)(btn1 | btn2 | btn3 | btn4);

            if (contPovNumber > 0)
            {
                    _joystickState.bHats = 0xFFFFFFFF; // Neutral state
                    _joystickState.bHatsEx1 = 0xFFFFFFFF; // Neutral state
                    _joystickState.bHatsEx2 = 0xFFFFFFFF; // Neutral state
                    _joystickState.bHatsEx3 = 0xFFFFFFFF; // Neutral state
            }
            else
            {
                    _joystickState.bHats = 0xFFFFFFFF; // Neutral state
            };

            /*** Feed the driver with the position packet - is fails then wait for input then try to re-acquire device ***/

            if (!_joystick.UpdateVJD(_id, ref _joystickState))
            {
                Console.WriteLine("Feeding vJoy device number {0} failed - try to enable device then press enter\n", _id);
                Console.ReadKey(true);
                _joystick.AcquireVJD(_id);
            }

            System.Threading.Thread.Sleep(20);
            count++;

            if (count > 640) { count = 0; }

        }
    }
}