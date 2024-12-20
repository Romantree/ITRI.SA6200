using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class CylinderTimeoutModel : DataModelBase
    {
        public double LiftPin { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double RollGap { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double FilmClamp { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double RollClamp { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double UvCylinder { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double ProcBuzzerOffTime { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
