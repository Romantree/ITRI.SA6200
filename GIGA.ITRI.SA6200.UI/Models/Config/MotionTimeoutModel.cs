using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class MotionTimeoutModel : DataModelBase
    {
        public double StageX { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double RollGap { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
