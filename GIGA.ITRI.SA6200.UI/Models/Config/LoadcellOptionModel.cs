using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class LoadcellOptionModel : DataModelBase
    {
        public double ZeroSetTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Alarm { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Warning { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Error { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double DetectTime { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
 