using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class MotScaleDataModel : DataModelBase
    {
        public double RollGap { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Demold { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
