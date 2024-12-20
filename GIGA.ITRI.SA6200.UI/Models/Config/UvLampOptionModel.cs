using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class UvLampOptionModel : DataModelBase
    {
        public bool IO { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public double OnOffTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double PowerSetTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }

        public int TempLimit { get => this.GetValue<int>(); set => this.SetValue(value); }

        public int RetryCount { get => this.GetValue<int>(); set => this.SetValue(value); }
    }
}
