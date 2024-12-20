using GIGA.ITRI.SA6200.UI.Models.Alarm;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Alarm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Dac.Alarm;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Alarm
{
    public class AlarmListViewModel : IAlarmViewModel
    {
        private readonly ContentControl _view = new AlarmListView();
        private readonly object _locker = new object();

        public override int No => 0;

        public override string Name => "List";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_alert", AP.ICON_RESOURCE);

        public ObservableCollection<AlarmModel> AlarmList { get; set; } = new ObservableCollection<AlarmModel>();

        public override void Init()
        {
            try
            {
                base.Init();

                AP.Alarm.OnAlarmPostEvent += OnAlarmPostEvent;
                AP.Alarm.OnAlarmClearEvent += OnAlarmClearEvent;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Show()
        {
            try
            {
                this.UpdateAlarm();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                base.Show();
            }
        }

        public override void Hide()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                base.Hide();
            }
        }

        protected override void OnNotifyCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "ALL_CLEAR":
                        {
                            this.AllAlarmClear();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void OnAlarmPostEvent(object sender, AlarmData<eAlarm> e)
        {
            try
            {
                this.Dispatcher.Invoke(() => this.InsertAlarm(e));
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void OnAlarmClearEvent(object sender, eAlarm e)
        {
            try
            {
                this.Dispatcher.Invoke(() => this.DeleteAlarm(e));
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateAlarm()
        {
            try
            {
                this.AlarmList.Clear();

                var list = AP.Alarm.GetPostAlarm();

                foreach (var item in list)
                {
                    if (item == null) continue;

                    this.InsertAlarm(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void AllAlarmClear()
        {
            var list = this.AlarmList.ToList();

            if (list.Any(t => t.Level == AlarmLevel.Heavy) && MsgBox.ShowMsg("Would you like to resume the operation?", true) == false)
            {
                AP.ProcessStop();
            }

            try
            {
                this.AlarmList.Clear();

                foreach (var item in list)
                {
                    AP.Alarm.AlarmRecovery(item.Alarm);
                }
            }
            catch (Exception ex)
            { 
                Logger.Write(this, ex);
            }
        }

        private void InsertAlarm(AlarmModel item)
        {
            try
            {
                lock (this._locker)
                {
                    var temp = this.AlarmList.FirstOrDefault(t => t.Alarm == item.Alarm);
                    if (temp != null) return;

                    this.AlarmList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void DeleteAlarm(eAlarm item)
        {
            try
            {
                lock (this._locker)
                {
                    var temp = this.AlarmList.FirstOrDefault(t => t.Alarm == item);
                    if (temp == null) return;

                    this.AlarmList.Remove(temp);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
