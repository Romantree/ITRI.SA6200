using GIGA.ITRI.SA6200.UI.Models.Alarm;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Alarm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Alarm
{
    public class AlarmHistoryViewModel : IAlarmViewModel
    {
        private readonly ContentControl _view = new AlarmHistoryView();

        public override int No => 1;

        public override string Name => "History";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_calendar_year", AP.ICON_RESOURCE);

        public DateTime StartTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public DateTime EndTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public ObservableCollection<AlarmHistoryModel> AlarmList { get; set; } = new ObservableCollection<AlarmHistoryModel>();

        public override void Init()
        {
            try
            {
                base.Init();

                this.StartTime = DateTime.Now.Date;
                this.EndTime = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnNotifyCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "SEARCH":
                        {
                            var start = this.StartTime;
                            var end = this.EndTime.AddDays(1).AddSeconds(-1);

                            this.UpdateAlarmHistory(start, end);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateAlarmHistory(DateTime start, DateTime end)
        {
            try
            {
                this.AlarmList.Clear();

                var list = AP.Alarm.GetAlarmHistoryList(start, end);

                foreach (var item in list)
                {
                    if (item == null) continue;

                    this.AlarmList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
