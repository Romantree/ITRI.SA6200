using GIGA.ITRI.SA6200.UI.Managers.Net;
using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models
{
    public abstract class ISerialPortModel : DataModelBase
    {
        private readonly NetworkUnit _unit;

        protected INetSerialPort _Client => AP.Net[_unit];

        public bool IsOpen { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string Port { get => this.GetValue<string>(); set => this.SetValue(value); }

        public NormalCommand OnStartCmd => new NormalCommand(StartCmd);

        public NormalCommand OnStopCmd => new NormalCommand(t => AP.Net[_unit].Stop());

        public ISerialPortModel(NetworkUnit unit)
        {
            this._unit = unit;
            this.Init();
        }

        public void Init()
        {
            try
            {
                this.Port = DB.Config[_unit];
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public virtual void Update()
        {
            try
            {
                this.IsOpen = _Client.IsOpen;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void StartCmd(object param)
        {
            try
            {
                DB.Config[_unit] = this.Port;

                var res = AP.Net.Start(_Client, _unit);
                if (res == true) return;

                AP.Event.InterlockMsgEvent(res.Comment);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
