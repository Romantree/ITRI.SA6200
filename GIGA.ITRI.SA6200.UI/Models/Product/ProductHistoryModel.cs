using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Product
{
    [DataContract]
    public class ProductHistoryModel : ModelBase
    {
        private readonly List<double> _list = new List<double>();

        [DataMember]
        public string RecipeName { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public List<ProductDataModel> List { get; set; } = new List<ProductDataModel>();

        public void Collect()
        {
            try
            {
                var stageX = AP.Device[eAxis.StageX].ActPosition;
                if (_list.Contains(stageX)) return;

                _list.Add(stageX);

                this.List.Add(new ProductDataModel()
                {
                    StageX = stageX,
                    LoadcellLeft = AP.Net.StageLeftLD.Data,
                    LoadcellRight = AP.Net.StageRightLD.Data,
                    UvTemp = AP.Net.StageUv.Data.Temp,
                    UvLux = AP.Net.StageUv.Data.Illuminance,
                });
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
