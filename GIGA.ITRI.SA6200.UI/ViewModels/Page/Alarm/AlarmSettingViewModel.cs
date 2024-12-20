using GIGA.ITRI.SA6200.UI.Models.Alarm;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Utils;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Alarm

{
    public class AlarmSettingViewModel : IAlarmViewModel
    {
        private readonly ContentControl _view = new AlarmSettingView();

        public override int No => 2;

        public override string Name => "Setting";

        public override ContentControl View => _view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_cogs", AP.ICON_RESOURCE);

        public List<AlarmSettingModel> AlarmList { get => this.GetValue<List<AlarmSettingModel>>(); set => this.SetValue(value); }

        public int CurPage { get => this.GetValue<int>(); set => this.SetValue(value); }

        private List<List<AlarmSettingModel>> _Page = new List<List<AlarmSettingModel>>();

        public int MaxPage { get => this.GetValue<int>(); set => this.SetValue(value); }

        public bool IsPrev { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsNext { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string SearchText { get => this.GetValue<string>(); set => this.SetValue(value); }

        public override void Init()
        {
            try
            {
                base.Init();

                this.LoadAlarmSetting();
                this.UpdateAlarmSetting();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void LoadAlarmSetting()
        {
            try
            {
                var list = this.ToAlarmSettingModel();

                this._Page = list.ToPageList(17).Where(t => t != null && t.Count() > 0).Select(t => t.ToList()).ToList();

                this.CurPage = 1;
                this.MaxPage = this._Page.Count;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateAlarmSetting()
        {
            try
            {
                if (this._Page.Count == 0) return;

                this.AlarmList = this._Page[this.CurPage - 1];
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private IEnumerable<AlarmSettingModel> ToAlarmSettingModel()
        {
            var list = AP.Alarm.GetAlarmList();

            foreach (var item in list.OrderBy(t => (int)t.Alarm))
            {
                yield return item;
            }
        }

        public override void Update()
        {
            try
            {
                this.IsPrev = this.CurPage != 1;
                this.IsNext = this.CurPage != this.MaxPage;

                if (this.AlarmList != null)
                {
                    foreach (var item in this.AlarmList.ToList())
                    {
                        item.Check();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                base.Update();
            }
        }

        protected override void OnNotifyCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "Prev":
                        {
                            if (this.CurPage == 1) return;
                            this.CurPage--;

                            this.UpdateAlarmSetting();
                        }
                        break;
                    case "Next":
                        {
                            if (this.CurPage == this.MaxPage) return;
                            this.CurPage++;

                            this.UpdateAlarmSetting();
                        }
                        break;
                    case "SearchText":
                        {
                            this.SearchText = Keyboard.Show();
                        }
                        break;
                    case "Search":
                        {
                            if (string.IsNullOrWhiteSpace(this.SearchText))
                            {
                                this.UpdateAlarmSetting();
                                return;
                            }

                            var text = this.SearchText.ToUpper();

                            this.AlarmList = this._Page.SelectMany(t => t).Where(t => t.Alarm.ToString().ToUpper().Contains(text)).ToList();
                        }
                        break;
                    case "Refresh":
                        {
                            this.LoadAlarmSetting();
                            this.UpdateAlarmSetting();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
