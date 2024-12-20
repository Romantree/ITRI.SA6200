using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.ViewModels.Page.Alarm;
using GIGA.ITRI.SA6200.UI.Views.Page;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page
{
    public class PgAlarmViewModel : IPageViewModel
    {
        private readonly ContentControl _view = new PageView();

        public override int No => 99;

        public override string Name => "Alarm";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_alert", AP.ICON_RESOURCE);

        public MenuSelectModel<IAlarmViewModel> Menu { get; set; } = new MenuSelectModel<IAlarmViewModel>();

        public PgAlarmViewModel()
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        public override void Init()
        {
            try
            {
                base.Init();
                this.Menu.Init();
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
                base.Show();
                this.Menu.Show();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Update()
        {
            try
            {
                base.Update();
                this.Menu.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void SetUserMenu(LoginMode mode, Configs.UserData data)
        {
            try
            {
                foreach (var item in this.Menu.MenuList)
                {
                    if (item is AlarmListViewModel) continue;

                    item.IsEnabled = data.Alarm;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
