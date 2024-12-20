using GIGA.ITRI.SA6200.UI.Managers.Net;
using System;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Models.Service
{
    public class LoadcellModel : ISerialPortModel
    {
        private new NetLoadcell _Client => base._Client as NetLoadcell;

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public double Data { get => this.GetValue<double>(); set => this.SetValue(value); }

        public LoadcellModel(bool isLeft) : base(isLeft ? NetworkUnit.StageLeftLD : NetworkUnit.StageRightLD)
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
    }
}
