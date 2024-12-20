using GIGA.ITRI.SA6200.UI.Managers.Net;
using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Service
{
    public class UvLampModel : ISerialPortModel
    {
        private new NetUvLamp _Client => base._Client as NetUvLamp;

        public bool Cooling { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool On { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public int Power { get => this.GetValue<int>(); set => this.SetValue(value); }

        public int Temp { get => this.GetValue<int>(); set => this.SetValue(value); }

        public bool Error { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public double Illuminance { get => this.GetValue<double>(); set => this.SetValue(value); }

        public int SetPower { get => this.GetValue<int>(); set => this.SetValue(value); }

        public NormalCommand OnLedOnCmd => new NormalCommand(t => this._Client.LedOnOff(true));

        public NormalCommand OnLedOffCmd => new NormalCommand(t => this._Client.LedOnOff(false));

        public NormalCommand OnPowerCmd => new NormalCommand(PowerCmd);

        public UvLampModel() : base(NetworkUnit.StageUv)
        {

        }

        public override void Update()
        {
            try
            {
                base.Update();

                this.Cooling = this._Client.Cooling;

                this.On = this._Client.LedOn;
                this.Power = this._Client.Data.Power;
                this.Temp = this._Client.Data.Temp;
                this.Error = this._Client.Data.Error;
                this.Illuminance = this._Client.Data.Illuminance;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void PowerCmd(object param)
        {
            try
            {
                this.SetPower = TS.FW.Wpf.Controls.Pu.NumberPad.Show(0);
                _Client.SetPower(this.SetPower);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
