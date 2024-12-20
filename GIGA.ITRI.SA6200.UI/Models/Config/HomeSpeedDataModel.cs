using System;
using TS.FW;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class HomeSpeedDataModel : DataModelBase
    {
        public double StageX { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Demold { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double GapLeft { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double GapRight { get => this.GetValue<double>(); set => this.SetValue(value); }

        public bool Interlock { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public NormalCommand OnInterlockCmd => new NormalCommand(InterlockCmd);

        private void InterlockCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "ON":
                        {
                            this.Interlock = true;
                        }
                        break;
                    case "OFF":
                        {
                            if (this.Interlock == false) return;
                            if (MsgBox.ShowMsg("Do you want to release the motion interlock?", true) == false) return;

                            this.Interlock = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
