using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models
{
    public class TowerLamp : ModelBase
    {
        public bool Red { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Yellow { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Green { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public void Update()
        {
            this.Red = AP.IO.TOWER_LAMP_RED;
            this.Yellow = AP.IO.TOWER_LAMP_YELLOW;
            this.Green = AP.IO.TOWER_LAMP_GREEN;
        }
    }
}
