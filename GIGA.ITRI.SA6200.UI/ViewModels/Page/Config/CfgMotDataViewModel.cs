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
    public class CfgMotDataViewModel : IConfigViewModel
    {
        private readonly ContentControl _view = new CfgMotDataView();

        public override int No => 0;

        public override string Name => "Motion";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_server", AP.ICON_RESOURCE);

        public LimitDataModel StageXLimit { get; set; } = new LimitDataModel("Stage ");

        public LimitDataModel GapPressLimit { get; set; } = new LimitDataModel("Gap Press");

        public LimitDataModel DemoldLimit { get; set; } = new LimitDataModel("Demold");

        public TeachDataModel StageXReady { get; set; } = new TeachDataModel("Stage X Ready");

        public TeachDataModel StageXFilmLoading { get; set; } = new TeachDataModel("Stage X Film Loading");

        public TeachDataModel WaferSize06 { get; set; } = new TeachDataModel("Wafer 6 Inch");

        public TeachDataModel WaferSize08 { get; set; } = new TeachDataModel("Wafer 8 Inch");

        public HomeSpeedDataModel HomeSpeed { get; set; } = new HomeSpeedDataModel();

        public MotScaleDataModel MotScale { get; set; } = new MotScaleDataModel();

        public MotEtcModel MotEtc { get; set; } = new MotEtcModel();

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
                    DB.DataCopy(DB.MotParam.StageXLimit, this.StageXLimit);
                    DB.DataCopy(DB.MotParam.GapPressLimit, this.GapPressLimit);
                    DB.DataCopy(DB.MotParam.DemoldLimit, this.DemoldLimit);

                    DB.DataCopy(DB.MotParam.StageXReady, this.StageXReady);
                    DB.DataCopy(DB.MotParam.StageXFilmLoading, this.StageXFilmLoading);
                    DB.DataCopy(DB.MotParam.WaferSize06, this.WaferSize06);
                    DB.DataCopy(DB.MotParam.WaferSize08, this.WaferSize08);

                    DB.DataCopy(DB.MotParam.HomeSpeed, this.HomeSpeed);
                    DB.DataCopy(DB.MotParam.MotScale, this.MotScale);
                    DB.DataCopy(DB.MotParam.MotEtc, this.MotEtc);

                    AP.Device.SetRollPressScale(DB.MotParam.MotScale.RollGap);
                    AP.Device.SetDemoldScale(DB.MotParam.MotScale.Demold);
                }

                DB.DataCopyEx(DB.MotParam.StageXLimit, this.StageXLimit);
                DB.DataCopyEx(DB.MotParam.GapPressLimit, this.GapPressLimit);
                DB.DataCopyEx(DB.MotParam.DemoldLimit, this.DemoldLimit);

                DB.DataCopyEx(DB.MotParam.StageXReady, this.StageXReady);
                DB.DataCopyEx(DB.MotParam.StageXFilmLoading, this.StageXFilmLoading);
                DB.DataCopyEx(DB.MotParam.WaferSize06, this.WaferSize06);
                DB.DataCopyEx(DB.MotParam.WaferSize08, this.WaferSize08);

                DB.DataCopyEx(DB.MotParam.HomeSpeed, this.HomeSpeed);
                DB.DataCopyEx(DB.MotParam.MotScale, this.MotScale);
                DB.DataCopyEx(DB.MotParam.MotEtc, this.MotEtc);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
