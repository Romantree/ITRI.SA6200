using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models
{
    public class NetworkState : ModelBase
    {
        public bool LeftLD { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool RightLD { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Uv { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool LeftRG { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool RightRG { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public void Update()
        {
            this.LeftLD = AP.Net.StageLeftLD.IsOpen;
            this.RightLD = AP.Net.StageRightLD.IsOpen;
            this.Uv = AP.Net.StageUv.IsOpen;
            this.LeftRG = AP.Net.StageLeftRG.IsOpen;
            this.RightRG = AP.Net.StageRightRG.IsOpen;
        }
    }
}
