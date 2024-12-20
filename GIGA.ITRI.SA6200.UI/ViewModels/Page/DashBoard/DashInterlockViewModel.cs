using GIGA.ITRI.SA6200.UI.Models.Interlock;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.DashBoard;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.DashBoard
{
    public class DashInterlockViewModel : IDashViewModel
    {
        private readonly ContentControl _view = new DashInterlockView();

        public override int No => 1;

        public override string Name => "Interlock";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_calendar_tomorrow", AP.ICON_RESOURCE);

        public InterlockModel Interlock { get; set; } = new InterlockModel();

        public override void Init()
        {
            try
            {
                base.Init();

                Interlock.InitControl();
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

                Interlock.Update();
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
                    case "ALL ON":
                        {
                            Interlock.DOOR_LOCK_01.OnSignalOnCmd.Execute(null);
                            Interlock.DOOR_LOCK_02.OnSignalOnCmd.Execute(null);
                            Interlock.DOOR_LOCK_03.OnSignalOnCmd.Execute(null);
                            Interlock.DOOR_LOCK_04.OnSignalOnCmd.Execute(null);
                            Interlock.DOOR_LOCK_05.OnSignalOnCmd.Execute(null);
                            Interlock.DOOR_LOCK_06.OnSignalOnCmd.Execute(null);
                            Interlock.DOOR_LOCK_07.OnSignalOnCmd.Execute(null);
                        }
                        break;
                    case "ALL OFF":
                        {
                            Interlock.DOOR_LOCK_01.OnSignalOffCmd.Execute(null);
                            Interlock.DOOR_LOCK_02.OnSignalOffCmd.Execute(null);
                            Interlock.DOOR_LOCK_03.OnSignalOffCmd.Execute(null);
                            Interlock.DOOR_LOCK_04.OnSignalOffCmd.Execute(null);
                            Interlock.DOOR_LOCK_05.OnSignalOffCmd.Execute(null);
                            Interlock.DOOR_LOCK_06.OnSignalOffCmd.Execute(null);
                            Interlock.DOOR_LOCK_07.OnSignalOffCmd.Execute(null);
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
