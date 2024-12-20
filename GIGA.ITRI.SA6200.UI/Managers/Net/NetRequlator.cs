using System;
using System.IO.Ports;
using TS.FW;
using TS.FW.Device.Dto.Analog;
using TS.FW.Helper;
using TS.FW.Utils;

namespace GIGA.ITRI.SA6200.UI.Managers.Net
{
    public class NetRequlator : INetSerialPort
    {
        private const int MIN_VOLTAGE = 0;
        private const int MAX_VOLTAGE = 1023;
        private const int MIN_DIGIT = 0;
        private const int MAX_DIGIT = 100;

        public double Data { get; private set; }

        public NetRequlator()
        {
            this.baudRate = 9600;
            this.dataBit = 8;
            this.stopBits = StopBits.One;
            this.parity = Parity.None;
            this.readBuffer = 10;
            this.IsRecvEvent = true;
        }

        public override void Init()
        {
            this.Read();
        }

        public void Set(double data)
        {
            var value = Convert.ToInt32(data);

            this.Set(value);
        }

        public void Set(int data)
        {
            try
            {
                if (AP.IsSim)
                {
                    this.Data = data;
                    return;
                }

                if (this.IsOpen == false) return;

                var value = (int)AnalogRange.ToDigit(data, MIN_DIGIT, MAX_DIGIT, MAX_VOLTAGE);

                if (value >= 1023) value = 1023;

                this.Send($"SET {value}\r\n");
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Read()
        {
            try
            {
                if (this.IsOpen == false) return;

                this.Send("MON\r\n");
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public bool CheckData(double value, double gap = 1) => this.Data.CheckPosition(value, gap);

        protected override void DataReceived(SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof) return;
            if (this._client.BytesToRead <= 0) return;

            try
            {
                var buffer = new byte[this._client.BytesToRead];
                var len = this._client.Read(buffer, 0, buffer.Length);

                if (buffer.Length != len) return;

                var cmd = this.encoding.GetString(buffer);

                if (string.Equals(cmd, "OUT OF RANGE\r\n", StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Write(this, "OUT OF RANGE", Logger.LogEventLevel.Error);
                    return;
                }
                else if (string.Equals(cmd, "UNKNOWN COMMAND\r\n", StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Write(this, "UNKNOWN COMMAND", Logger.LogEventLevel.Error);
                    return;
                }

                if (int.TryParse(cmd, out int value) == false || value > 1023)
                {
                    Logger.Write(this, $"Data Error : {cmd} [{buffer.ToHex()}]", Logger.LogEventLevel.Error);
                    return;
                }

                this.Data = Math.Round(Math.Ceiling(AnalogRange.ToDigit(value, MIN_VOLTAGE, MAX_VOLTAGE, MAX_DIGIT)), 3);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                this._client.DiscardInBuffer();
            }
        }

        public static implicit operator double(NetRequlator item) => item.Data;
    }
}
