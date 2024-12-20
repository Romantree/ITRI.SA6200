using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Service
{
    public class VacuumModel : ModelBase
    {
        private readonly VacuumUnit _unit;

        public VacuumModel(VacuumUnit unit) => _unit = unit;

        public VacuumStatus Status { get => this.GetValue<VacuumStatus>(); set => this.SetValue(value); }

        public bool IsProcess { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public NormalCommand OnVacuumCmd => new NormalCommand(VacuumCmd);

        public void Update()
        {
            try
            {
                this.Status = AP.IO.GetVacuum(_unit);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void VacuumCmd(object param)
        {
            try
            {
                this.IsProcess = AP.Proc.Vac.IsBusy;

                switch (param as string)
                {
                    case "VAC":
                        {
                            if (this.IsProcess) return;

                            AP.IO.SetVacuum(_unit, VacuumSetting.Vacuum);
                        }
                        break;
                    case "VNT":
                        {
                            if (this.IsProcess) return;

                            AP.Proc.StartVent(_unit);
                        }
                        break;
                    case "OFF":
                        {
                            AP.IO.SetVacuum(_unit, VacuumSetting.Off);
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
