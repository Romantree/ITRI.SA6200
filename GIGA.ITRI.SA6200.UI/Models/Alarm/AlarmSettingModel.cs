using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TS.FW;
using TS.FW.Dac.Alarm;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Alarm
{
    public class AlarmSettingModel : ModelBase
    {
        private readonly Dictionary<string, object> _firstValue = new Dictionary<string, object>();

        public int ID { get; set; }

        public eAlarm Alarm { get; set; }

        public AlarmLevel Level { get => this.GetValue<AlarmLevel>(); set => this.SetValue(value); }

        public string Contetns { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string Action { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool Enable { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsSave { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public NormalCommand OnSaveCmd => new NormalCommand(SaveCmd);

        public NormalCommand OnEnableChangeCmd => new NormalCommand(EnableChangeCmd);

        public void Check()
        {
            try
            {
                var type = this.GetType();

                foreach (var item in this._firstValue)
                {
                    var info = type.GetProperty(item.Key);
                    var value = info.GetValue(this);

                    if (value.ToString() != item.Value.ToString())
                    {
                        this.IsSave = true;
                        return;
                    }
                }

                this.IsSave = false;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void EnableChangeCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "ON":
                        {
                            this.Enable = true;
                        }
                        break;
                    case "OFF":
                        {
                            this.Enable = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SaveCmd(object param)
        {
            try
            {
                var res = AP.Alarm.UpdateAlarm(this);
                if (res == false)
                {
                    Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);

                    MsgBox.ShowMsg("Failed to save to the database.");
                }
                else
                {
                    this.UpdateFirstValue();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateFirstValue()
        {
            try
            {
                var type = this.GetType();

                foreach (var item in this._firstValue.Keys.ToList())
                {
                    var info = type.GetProperty(item);
                    var value = info.GetValue(this);

                    this._firstValue[info.Name] = value;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void SetValue(object value, [CallerMemberName] string propertyName = null)
        {
            try
            {
                base.SetValue(value, propertyName);
            }
            finally
            {
                if (string.Equals(propertyName, nameof(this.IsSave)) == false)
                {
                    if (this._firstValue.ContainsKey(propertyName) == false)
                    {
                        this._firstValue.Add(propertyName, value);
                    }
                }
            }
        }

        public static implicit operator AlarmSettingModel(AlarmData<eAlarm> item)
        {
            if (item == null) return null;

            return new AlarmSettingModel()
            {
                ID = (int)item.Alarm,
                Alarm = item.Alarm,
                Level = item.Level,
                Contetns = item.Contetns,
                Action = item.Action,
                Enable = item.Enable,
            };
        }

        public static implicit operator AlarmData<eAlarm>(AlarmSettingModel item)
        {
            if (item == null) return null;

            return new AlarmData<eAlarm>()
            {
                Alarm = item.Alarm,
                Level = item.Level,
                Contetns = item.Contetns,
                Action = item.Action,
                Enable = item.Enable,
            };
        }
    }
}
