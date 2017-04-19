using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using log4net;

namespace ComPortPackages.Core.RS232
{
    public abstract class RS232Base
    {
        public const int Unavailable = -2;
        public const int Timeout = -1;

        private readonly ILog _log;

        protected RS232Base(string devicePort, BaudRates deviceBaudRate, Parity parity, int bits, StopBits stopBits)
        {
            SerialPort = new SerialPort(devicePort, (int)deviceBaudRate, parity, bits, stopBits);
            _log = LogManager.GetLogger(GetType());
        }

        public SerialPort SerialPort { get; private set; }

        private string NormalizePort(int devicePort)
        {
            return String.Format("COM{0}", devicePort);
        }

        protected ILog Log
        {
            get { return _log; }
        }

        public void Open()
        {
            if (SerialPort.IsOpen == false)
            {
                SerialPort.Open();
            }
        }

        public void Close()
        {
            if (SerialPort.IsOpen)
            {
                SerialPort.Close();
                SerialPort.Dispose();
            }
        }

        public abstract int Send(byte ch);
        public abstract byte[] Send(byte[] data, int timeout = 0);
    }
}
