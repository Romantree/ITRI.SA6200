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
    public class CfgTimeoutViewModel : IConfigViewModel
    {
        private readonly ContentControl _view = new CfgTimeoutView();

        public override int No => 1;

        public override string Name => "Time";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_timer_pause", AP.ICON_RESOURCE);

        public CylinderTimeoutModel Cylinder { get; set; } = new CylinderTimeoutModel();

        public MotionTimeoutModel Motion { get; set; } = new MotionTimeoutModel();

        public VacTimeoutModel Vac { get; set; } = new VacTimeoutModel();

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
                    DB.DataCopy(DB.Timeout.Cylinder, this.Cylinder);
                    DB.DataCopy(DB.Timeout.Motion, this.Motion);
                    DB.DataCopy(DB.Timeout.Vac, this.Vac);
                }

                DB.DataCopyEx(DB.Timeout.Cylinder, this.Cylinder);
                DB.DataCopyEx(DB.Timeout.Motion, this.Motion);
                DB.DataCopyEx(DB.Timeout.Vac, this.Vac);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
