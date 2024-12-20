using System.Runtime.Serialization;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Recipe
{
    [DataContract]
    public class StageDataModel : DataModelBase
    {
        [DataMember]
        public RcpMotionDataModel Stage { get => this.GetValue<RcpMotionDataModel>(); set => this.SetValue(value); }

        [DataMember]
        public RcpMotionDataModel Demold { get => this.GetValue<RcpMotionDataModel>(); set => this.SetValue(value); }

        [DataMember]
        public double GapLeft { get => this.GetValue<double>(); set => this.SetValue(value); }

        [DataMember]
        public double GapRight { get => this.GetValue<double>(); set => this.SetValue(value); }

        [DataMember]
        public int RegulatorLeft { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public int RegulatorRight { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public bool UvLampUsed { get => this.GetValue<bool>(); set => this.SetValue(value); }

        [DataMember]
        public int UvLampPower { get => this.GetValue<int>(); set => this.SetValue(value); }

        public StageDataModel()
        {
            this.Stage = new RcpMotionDataModel();
            this.Demold = new RcpMotionDataModel();
        }
    }
}
