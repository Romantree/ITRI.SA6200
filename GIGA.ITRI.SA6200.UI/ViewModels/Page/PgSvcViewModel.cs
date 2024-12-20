using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page
{
    public class PgSvcViewModel : IPageViewModel
    {
        private readonly ContentControl _view = new PageView();

        public override int No => 2;

        public override string Name => "Service";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_stock", AP.ICON_RESOURCE);

        public MenuSelectModel<ISvcViewModel> Menu { get; set; } = new MenuSelectModel<ISvcViewModel>();

        public override void Init()
        {
            try
            {
                AP.Event.OnModeChangedEvent += Event_OnModeChangedEvent;

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

        private void Event_OnModeChangedEvent(object sender, bool e)
        {
            try
            {
                this.IsEnabled = !e;
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
                this.IsEnabled = AP.Proc.IsAuto ? false : data.Service;
                this.Visibility = data.Service ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
