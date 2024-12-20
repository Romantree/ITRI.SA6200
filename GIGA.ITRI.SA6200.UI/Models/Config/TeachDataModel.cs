using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Config
{
    public class TeachDataModel : DataModelBase
    {
        public string Name { get; private set; }

        public double Position { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Speed { get => this.GetValue<double>(); set => this.SetValue(value); }

        public TeachDataModel(string name) => this.Name = name;
    }
}
