using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using TS.FW;
using TS.FW.Diagnostics;
using TS.FW.Utils;

namespace GIGA.ITRI.SA6200.UI.Managers.Net
{
    public class NetUvLamp : INetSerialPort
    {
        private const int WAIT_TIME = 1000;

        private const ushort DE = 0xFA;
        private const ushort STX = 0xFABC;
        private const ushort ETX = 0xFADE;
        private const byte ID = 0xA0;

        private readonly EventWaitHandle wait = new EventWaitHandle(true, EventResetMode.ManualReset);
        private readonly BackgroundTimer trUpdate = new BackgroundTimer(ApartmentState.MTA);
        private bool isInit = false;

        private DateTime? _dtTime = null;

        private bool IsUvLampIO => DB.Network.UvLampOption.IO;

        private int UvLampTempLimit => DB.Network.UvLampOption.TempLimit;

        public int Retry { get; set; } = 0;

        public UvLampData Data { get; private set; } = new UvLampData();

        public bool LedOn => IsUvLampIO ? AP.IO.UV_LAMP_RUN : Data.LedOn;

        public bool Cooling => AP.IO.UV_LAMP_COLLING;

        public NetUvLamp()
        {
            this.baudRate = 38400;
            this.dataBit = 8;
            this.stopBits = StopBits.One;
            this.parity = Parity.None;
            this.readBuffer = 50;
            this.IsRecvEvent = true;

            this.trUpdate.SleepTimeMsc = 100;
            this.trUpdate.DoWork += TrUpdate_DoWork;
        }

        public override void Init()
        {
            try
            {
                this.trUpdate.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void LedOnOff(bool onoff)
        {
            try
            {
                if (AP.IS_UV_LAMP == false) return;

                if (AP.IsSim)
                {
                    AP.IO.UV_LAMP_RUN = onoff;
                    this.Data.LedOn = onoff;

                    return;
                }

                if (this.IsOpen == false) return;

                if (this.IsUvLampIO)
                {
                    AP.IO.UV_LAMP_RUN = onoff;
                }
                else
                {
                    this.wait.WaitOne(WAIT_TIME);
                    this.wait.Reset();

                    var buffer = this.ToCmd(onoff ? UvCmd.LED_ON : UvCmd.LED_OFF);
                    this.Send(buffer);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void SetPower(int power)
        {
            try
            {
                if (AP.IS_UV_LAMP == false) return;

                if (AP.IsSim)
                {
                    this.Data.Power = power;
                    return;
                }

                if (this.IsOpen == false) return;

                this.wait.WaitOne(WAIT_TIME);
                this.wait.Reset();

                var buffer = this.ToCmd(UvCmd.SET_POWER, (byte)power);
                this.Send(buffer);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void SetPowerLimit(int power)
        {
            try
            {
                if (AP.IS_UV_LAMP == false) return;

                if (this.IsOpen == false) return;

                this.wait.WaitOne(WAIT_TIME);
                this.wait.Reset();

                var buffer = this.ToCmd(UvCmd.SET_LIMIT_POWER, (byte)power);
                this.Send(buffer);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void SetTempLimit(int temp)
        {
            try
            {
                if (AP.IS_UV_LAMP == false) return;

                if (this.IsOpen == false) return;

                this.wait.WaitOne(WAIT_TIME);
                this.wait.Reset();

                var buffer = this.ToCmd(UvCmd.SET_LIMIT_TEMP, (byte)temp);
                this.Send(buffer);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public bool PowerCheck(int power, int gap = 1) => Math.Sqrt(Math.Pow(Data.Power - power, 2)) <= gap;

        private void UpdateStatus(UvStatus status)
        {
            try
            {
                if (this.IsOpen == false) return;

                this.wait.WaitOne(WAIT_TIME);
                this.wait.Reset();

                var buffer = this.ToCmd(UvCmd.STATUS, status);
                this.Send(buffer);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void TrUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (this.IsOpen == false) return;

                if (this.isInit == false)
                {
                    this.UpdateStatus(UvStatus.STATUS_1);
                    this.UpdateStatus(UvStatus.STATUS_2);

                    this.isInit = true;
                }

                this.UpdateStatus(UvStatus.STATUS_3);
                this.UpdateStatus(UvStatus.STATUS_5);

                if (this.UvLampTempLimit <= 0)
                {
                    this.SetUvLedColling(this.Data.LedOn);
                    _dtTime = null;
                }
                else if (this.Data.Temp >= this.UvLampTempLimit)
                {
                    if(_dtTime == null)
                    {
                        _dtTime = DateTime.Now;
                    }
                    else if((DateTime.Now - _dtTime.Value).TotalSeconds >= 3)
                    {
                        this.SetUvLedColling(true);
                    }
                }
                else
                {
                    this.SetUvLedColling(false);
                    _dtTime = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void DataReceived(SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof) return;
            if (this._client.BytesToRead <= 0) return;

            try
            {
                this.wait.Set();

                var buffer = new byte[this._client.BytesToRead];
                var len = this._client.Read(buffer, 0, buffer.Length);

                if (buffer.Length != len) return;

                this.SetData(buffer);
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

        private void SetUvLedColling(bool onoff)
        {
            try
            {
                AP.IO.UV_LAMP_COLLING = onoff;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SetData(byte[] buffer)
        {
            try
            {
                if (buffer.Length < 10) return;

                var temp = this.LastSkipWhile(buffer);
                if (temp.Length < 6) return;

                var id = temp[0];
                var cmd = (UvResult)temp[1];

                switch (cmd)
                {
                    case UvResult.STATUS_0:
                        {
                            this.Data.Temp = temp[2];
                            this.Data.Power = temp[4];
                        }
                        break;
                    case UvResult.STATUS_1:
                        {
                            this.Data.PowerLimit = temp[2];
                            this.Data.TempLimit = temp[4];
                        }
                        break;
                    case UvResult.STATUS_2:
                        {
                            this.Data.LifeTime = (temp[2] * 100) + temp[3] + (temp[4] * 15000);
                        }
                        break;
                    case UvResult.STATUS_3:
                        {
                            this.Data.Ready = temp[2] == 1;
                            this.Data.LedOn = temp[3] == 1;
                            this.Data.Error = temp[4] == 1;
                            this.Data.ErrorType = temp[5];
                        }
                        break;
                    case UvResult.STATUS_5:
                        {
                            var data = temp.ToUInt16(2, BitHandler.ByteOrder.BigEndian);

                            this.Data.Power = temp[4];
                            this.Data.Temp = temp[5];

                            this.Data.Illuminance = data;
                        }
                        break;
                    case UvResult.LED_ON:
                        {
                            this.Data.LedOn = true;
                        }
                        break;
                    case UvResult.LED_OFF:
                        {
                            this.Data.LedOn = false;
                        }
                        break;
                    case UvResult.SET_POWER:
                        {
                            this.Data.Power = temp[3];
                        }
                        break;
                    case UvResult.SET_LIMIT_POWER:
                        {
                            this.Data.PowerLimit = temp[3];
                        }
                        break;
                    case UvResult.SET_LIMIT_TEMP:
                        {
                            this.Data.TempLimit = temp[3];
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private byte[] ToCmd(UvCmd cmd) => this.ToCmd(cmd, UvStatus.STATUS_0, 0x00).ToArray();

        private byte[] ToCmd(UvCmd cmd, UvStatus status) => this.ToCmd(cmd, status, 0x00).ToArray();

        private byte[] ToCmd(UvCmd cmd, byte data) => this.ToCmd(cmd, UvStatus.STATUS_0, data).ToArray();

        private IEnumerable<byte> ToCmd(UvCmd cmd, UvStatus status, byte data)
        {
            yield return (byte)((STX & 0xFF00) >> 8);
            yield return (byte)(STX & 0x00FF);

            yield return ID;

            yield return (byte)cmd;

            yield return (byte)status;
            yield return data;
            yield return 0x00;
            yield return 0x00;

            yield return (byte)((ETX & 0xFF00) >> 8);
            yield return (byte)(ETX & 0x00FF);
        }

        private byte[] LastSkipWhile(byte[] buffer)
        {
            return buffer.ToInt16List(0, buffer.Length / 2, BitHandler.ByteOrder.BigEndian)
                .Select(t => (ushort)t)
                .Reverse().SkipWhile(t => t != ETX)
                .Skip(1) // ETX 삭제
                .TakeWhile(t => t != STX).Reverse()
                .SelectMany(t => t.ToByte(BitHandler.ByteOrder.BigEndian))
                .ToArray();
        }
    }

    public enum UvCmd : byte
    {
        STATUS = 0x01,
        LED_ON = 0x02,
        LED_OFF = 0x03,

        SET_POWER = 0x05,
        SET_LIMIT_POWER = 0x07,
        SET_LIMIT_TEMP = 0x08,
    }

    public enum UvStatus : byte
    {
        STATUS_0 = 0x00,
        STATUS_1 = 0x01,
        STATUS_2 = 0x02,
        STATUS_3 = 0x03,
        STATUS_5 = 0x05,
    }

    public enum UvResult : byte
    {
        STATUS_0 = 0x00,
        STATUS_1 = 0x01,
        STATUS_2 = 0x02,
        STATUS_3 = 0x03,
        STATUS_5 = 0x05,

        LED_ON = 0xC2,
        LED_OFF = 0xC3,

        SET_POWER = 0xC5,
        SET_LIMIT_POWER = 0xC7,
        SET_LIMIT_TEMP = 0xC8,
    }

    public class UvLampData
    {
        public int Temp { get; set; }

        public int Power { get; set; }

        public int PowerLimit { get; set; }

        public int TempLimit { get; set; }

        public int LifeTime { get; set; }

        public bool Ready { get; set; }

        public bool LedOn { get; set; }

        public bool Error { get; set; }

        public int ErrorType { get; set; }

        public double Illuminance { get; set; }
    }
}
