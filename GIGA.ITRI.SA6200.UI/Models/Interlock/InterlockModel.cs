using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Models.Interlock
{
    public class InterlockModel
    {
        private static readonly List<PropertyInfo> _info = new List<PropertyInfo>();

        static InterlockModel() => _info.AddRange(typeof(InterlockModel).GetProperties());

        public InDataModel X_MAIN_CDA_RESSURE_01 { get; set; }

        public InDataModel X_MAIN_CDA_RESSURE_02 { get; set; }

        public InDataModel X_EMO_01 { get; set; }

        public InDataModel X_EMO_02 { get; set; }

        public InDataModel X_EMO_03 { get; set; }

        public InDataModel X_EMO_04 { get; set; }

        public InDataModel X_ION_BLOWER_ALARM { get; set; }

        public InDataModel X_UV_LAMP_ERROR { get; set; }

        public InDataModel X_DOOR_LOCK_01 { get; set; }

        public InDataModel X_DOOR_LOCK_02 { get; set; }

        public InDataModel X_DOOR_LOCK_03 { get; set; }

        public InDataModel X_DOOR_LOCK_04 { get; set; }

        public InDataModel X_DOOR_LOCK_05 { get; set; }

        public InDataModel X_DOOR_LOCK_06 { get; set; }

        public InDataModel X_DOOR_LOCK_07 { get; set; }

        public OutDataModel MC_ON { get; set; }

        public OutDataModel EQ_LAMP_ON { get; set; }

        //public OutDataModel ELEC_BOX_LAMP_ON { get; set; }

        public OutDataModel ION_BLOWER_RUN { get; set; }

        public OutDataModel ION_BLOWER_TIP_CLENING { get; set; }

        public OutDataModel LOADCELL_ZERO { get; set; }

        public OutDataModel DOOR_LOCK_01 { get; set; }

        public OutDataModel DOOR_LOCK_02 { get; set; }

        public OutDataModel DOOR_LOCK_03 { get; set; }

        public OutDataModel DOOR_LOCK_04 { get; set; }

        public OutDataModel DOOR_LOCK_05 { get; set; }

        public OutDataModel DOOR_LOCK_06 { get; set; }

        public OutDataModel DOOR_LOCK_07 { get; set; }

        public void InitControl()
        {
            try
            {
                foreach (var info in _info)
                {
                    if (info.PropertyType == typeof(InDataModel))
                    {
                        info.SetValue(this, new InDataModel(info.Name));
                    }
                    else if (info.PropertyType == typeof(OutDataModel))
                    {
                        info.SetValue(this, new OutDataModel(info.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Update()
        {
            try
            {
                foreach (var item in _info.Select(t=> t.GetValue(this) as IInOutDataModel))
                {
                    if (item == null) continue;

                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }    
}
