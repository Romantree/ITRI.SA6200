using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class RegulatorOptionModel : DataModelBase
    {
        public int Default { get => this.GetValue<int>(); set => this.SetValue(value); }

        public int Min { get => this.GetValue<int>(); set => this.SetValue(value); }

        public int Max { get => this.GetValue<int>(); set => this.SetValue(value); }

        public double SetTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
