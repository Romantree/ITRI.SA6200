using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page
{
    public class PgRcpViewModel : IPageViewModel
    {
        private readonly ContentControl _view = new PageView();

        public override int No => 1;

        public override string Name => "Recipe";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_page_edit", AP.ICON_RESOURCE);

        public MenuSelectModel<IRcpViewModel> Menu { get; set; } = new MenuSelectModel<IRcpViewModel>();

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
                this.IsEnabled = data.Recipe;
                this.Visibility = this.IsEnabled ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
