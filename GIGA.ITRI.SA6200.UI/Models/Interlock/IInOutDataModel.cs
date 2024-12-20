using GIGA.ITRI.SA6200.UI.Managers;
using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Interlock
{
    public abstract class IInOutDataModel : ModelBase
    {
        protected readonly string _key;
        private readonly IOData _data;

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool On { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public IInOutDataModel(string key, eIOType type)
        {
            _key = key;
            _data = type == eIOType.IN ? AP.IO.inList[key] : AP.IO.outList[key];

            this.Name = _data.Name;
        }

        public void Update()
        {
            try
            {
                this.On = this._data.OnOff;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override string ToString() => Name;
    }

    public class InDataModel : IInOutDataModel
    {
        public InDataModel(string key) : base(key, eIOType.IN) { }
    }

    public class OutDataModel : IInOutDataModel
    {
        public NormalCommand OnSignalOnCmd => new NormalCommand(t => AP.IO.WriteY(true, _key));

        public NormalCommand OnSignalOffCmd => new NormalCommand(t => AP.IO.WriteY(false, _key));

        public OutDataModel(string key) : base(key, eIOType.OUT) { }
    }
}
