using System;
using System.IO.Ports;

namespace ComPortPackages.Core.RS232
{
    public class Rs232Impl : RS232Base
    {
        /// <summary> Время передачи одного байта </summary>
        private const int DefaultTimeout = 50;

        public Rs232Impl(string devicePort, BaudRates deviceBaudRate, Parity parity, int bits, StopBits stopBits) : base(devicePort, deviceBaudRate, parity, bits, stopBits)
        {
        }

        public override int Send(byte ch)
        {
            SerialPort.DiscardOutBuffer(); // cбросить выходной буфер
            SerialPort.ReadTimeout = DefaultTimeout;

            try
            {
                SerialPort.Write(new[] { ch }, 0, 1);
            }
            catch (InvalidOperationException e0)
            {
                Log.Error(e0);
                return Unavailable;
            }
            catch (TimeoutException e1)
            {
                Log.Error(e1);
                return Timeout;
            }
            return 0;
        }

        public override byte[] Send(byte[] data, int timeout = 0)
        {
            try
            {
                SerialPort.DiscardOutBuffer();
                SerialPort.DiscardInBuffer();
                //SerialPort.ReadTimeout = DefaultTimeout;

                SerialPort.Write(data, 0, data.Length);
            }
            catch (InvalidOperationException e0)
            {
                Log.Error(e0);
            }
            catch (TimeoutException e1)
            {
                Log.Error(e1);
            }
            return null;
        }
    }
}
