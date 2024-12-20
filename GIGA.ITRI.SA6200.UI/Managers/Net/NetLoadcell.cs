using System;
using System.IO.Ports;
using System.Linq;
using TS.FW;
using TS.FW.Helper;
using TS.FW.Utils;

namespace GIGA.ITRI.SA6200.UI.Managers.Net
{
    public class NetLoadcell : INetSerialPort
    {
        private const byte STX = 0x02;
        private const byte ETX = 0x03;

        private DateTime _recvTime = DateTime.Now;

        public double Data { get; set; }

        public bool IsConnection => (DateTime.Now - _recvTime).TotalSeconds <= 5;

        public NetLoadcell()
        {
            this.baudRate = 9600;
            this.dataBit = 8;
            this.stopBits = StopBits.One;
            this.parity = Parity.None;
            this.readBuffer = 50;
            this.IsRecvEvent = true;
        }

        public bool CheckData(double data = 0, double gap = 0.1) => this.Data.CheckPosition(data, gap);

        protected override void DataReceived(SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof) return;
            if (this._client.BytesToRead <= 0) return;

            try
            {
                this._recvTime = DateTime.Now;

                System.Threading.Thread.Sleep(500);

                var buffer = new byte[this._client.BytesToRead];
                var len = this._client.Read(buffer, 0, buffer.Length);

                if (buffer.Length != len) return;

                this.Data = this.ToData(buffer);
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

        private double ToData(byte[] buffer)
        {
            if (buffer.Length < 10) return this.Data;

            var temp = this.LastSkipWhile(buffer);
            var data = this.encoding.GetString(temp.Where(t => t != 63).ToArray()).Replace(" ", "");

            if (double.TryParse(data, out double value) == false)
            {
                Logger.Write(this, $"Data Error : {data} [{temp.ToHex()}]", Logger.LogEventLevel.Error);
                return this.Data;
            }

            return value;
        }

        private byte[] LastSkipWhile(byte[] buffer)
        {
            return buffer
                .Reverse().SkipWhile(t => t != ETX)
                .Skip(1) // ETX 삭제
                .TakeWhile(t => t != STX).Reverse()
                .Skip(1) // ID 삭제
                .ToArray();
        }

        public static implicit operator double(NetLoadcell item) => item.Data;
    }
}
