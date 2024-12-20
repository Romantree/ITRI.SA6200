using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class LimitDataModel : DataModelBase
    {
        public string Name { get; private set; }

        public double Speed { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Plus { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Minus { get => this.GetValue<double>(); set => this.SetValue(value); }

        public LimitDataModel(string name) => this.Name = name;
    }
}