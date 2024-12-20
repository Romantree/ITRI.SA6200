using System;
using System.Runtime.Serialization;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Product
{
    [DataContract]
    public class ProductDataModel : ModelBase
    {
        [DataMember]
        public DateTime Time { get; set; } = DateTime.Now;

        [DataMember]
        public double StageX { get; set; }

        [DataMember]
        public double LoadcellLeft { get; set; }

        [DataMember]
        public double LoadcellRight { get; set; }

        [DataMember]
        public double UvTemp { get; set; }

        [DataMember]
        public double UvLux { get; set; }
    }
}
