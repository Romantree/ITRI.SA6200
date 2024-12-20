using TS.FW.Dac.Cfg;

namespace GIGA.ITRI.SA6200.UI.Configs
{
    public class NetConfigDB : IConfigDb
    {
        public string this[NetworkUnit unit] { get => this.GetValue(unit); set => this.SetValue(unit, value); }

        private string GetValue(NetworkUnit unit) => this.GetValue(unit.ToString());

        private void SetValue(NetworkUnit unit, string value) => this.SetValue(value, unit.ToString());
    }
}
