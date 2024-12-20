using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class HeaterOptionModel : DataModelBase
    {
        public double Temp { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Alarm { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Error { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double SetTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double CheckTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
