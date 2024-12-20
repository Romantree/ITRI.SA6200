using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class VacTimeoutModel : DataModelBase
    {
        public double Timeout { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double VentOffDelay { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double VentDelay { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
