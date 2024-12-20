using GIGA.ITRI.SA6200.UI.Managers.Net;
using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Service
{
    public class RegulatorModel : ISerialPortModel
    {
        private new NetRequlator _Client => base._Client as NetRequlator;

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public double SetData { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Data { get => this.GetValue<double>(); set => this.SetValue(value); }

        public NormalCommand OnSetDataCmd => new NormalCommand(SetDataCmd);

        public RegulatorModel(bool isLeft) : base(isLeft ? NetworkUnit.StageLeftRG : NetworkUnit.StageRightRG)
        {
            this.Name = isLeft ? "Left" : "Right";
        }

        public override void Update()
        {
            try
            {
                base.Update();
                this.Data = _Client.Data;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SetDataCmd(object param)
        {
            try
            {
                _Client.Set(this.SetData);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
