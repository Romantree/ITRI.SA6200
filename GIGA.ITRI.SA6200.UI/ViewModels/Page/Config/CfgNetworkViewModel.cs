using GIGA.ITRI.SA6200.UI.Models.Config;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Config;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Config
{
    public class CfgNetworkViewModel : IConfigViewModel
    {
        private readonly ContentControl _view = new CfgNetworkView();

        public override int No => 2;

        public override string Name => "Network";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_network_server_connecting", AP.ICON_RESOURCE);

        public UvLampOptionModel UvLampOption { get; set; } = new UvLampOptionModel();

        public LoadcellOptionModel LoadcellOption { get; set; } = new LoadcellOptionModel();

        public RegulatorOptionModel RegulatorOption { get; set; } = new RegulatorOptionModel();

        public override void Show()
        {
            try
            {
                base.Show();

                this.Refresh();
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
                    case "SAVE":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to save the data?") == false) return;

                            this.Refresh(true);
                        }
                        break;
                    case "Refresh":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to initialize the data?") == false) return;

                            this.Refresh();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                AP.Event.InterlockMsgEvent(ex.Message);
            }
        }

        private void Refresh(bool isSave = false)
        {
            try
            {
                if (isSave)
                {
                    DB.DataCopy(DB.Network.UvLampOption, this.UvLampOption);
                    DB.DataCopy(DB.Network.LoadcellOption, this.LoadcellOption);
                    DB.DataCopy(DB.Network.RegulatorOption, this.RegulatorOption);
                }

                DB.DataCopyEx(DB.Network.UvLampOption, this.UvLampOption);
                DB.DataCopyEx(DB.Network.LoadcellOption, this.LoadcellOption);
                DB.DataCopyEx(DB.Network.RegulatorOption, this.RegulatorOption);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
