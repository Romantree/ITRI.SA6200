using GIGA.ITRI.SA6200.UI.Managers;
using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Setup
{
    public class InOutModel : ModelBase
    {
        public readonly IOData data;
        public readonly string Key;

        public bool IsAType { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string Address { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool OnOff { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public InOutModel(string key, IOData data) : this(data)
        {
            this.Key = key;
        }

        public InOutModel(IOData data)
        {
            this.data = data;
            this.IsAType = data.IsAType;
            this.Address = data.Address;
            this.Name = data.Name;
            this.OnOff = data.OnOff;
        }

        public void Update()
        {
            try
            {
                this.IsAType = this.data.IsAType;
                this.OnOff = this.data.OnOff;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public string ToCSV() => $"{this.Address},{this.Name}";

        public static implicit operator InOutModel(IOData data)
        {
            return new InOutModel(data);
        }
    }
}
