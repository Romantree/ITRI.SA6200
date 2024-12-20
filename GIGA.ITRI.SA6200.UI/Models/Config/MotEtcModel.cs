using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class MotEtcModel : DataModelBase
    {
        public double DemoldDelay { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double RollGapHomePos { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
