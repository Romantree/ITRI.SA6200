using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Recipe
{
    [DataContract]
    public class RcpMotionDataModel : DataModelBase
    {
        [DataMember]
        public double Position { get => this.GetValue<double>(); set => this.SetValue(value); }

        [DataMember]
        public double Speed { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
