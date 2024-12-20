using GIGA.ITRI.SA6200.UI.Models;
using GIGA.ITRI.SA6200.UI.Models.Recipe;
using GIGA.ITRI.SA6200.UI.Process.FilmLoading;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.ViewModels.Popup;
using GIGA.ITRI.SA6200.UI.Views.Page.DashBoard;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.DashBoard
{
    public class DashMainViewModel : IDashViewModel
    {
        private readonly ContentControl _view = new DashMainView();

        public override int No => 0;

        public override string Name => "Main";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_clipboard", AP.ICON_RESOURCE);

        public StageModel Stage { get; set; } = new StageModel();

        public bool IsInit { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsAuto { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsBusy { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool isInitStart { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsAutoStart { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsManualStart { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public MainRecipeModel Recipe { get => this.GetValue<MainRecipeModel>(); set => this.SetValue(value); }

        public string Step { get => this.GetValue<string>(); set => this.SetValue(value); }

        public double TackTime { get => this.GetValue<double>(); set => this.SetValue(value); }

        public string LogMsg { get => this.GetValue<string>(); set => this.SetValue(value); }

        public override void Init()
        {
            try
            {
                base.Init();

                AP.Event.OnProcessMsgEvent += Event_OnProcessMsgEvent;
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

                if (this.Recipe != null)
                {
                    this.Recipe = AP.Rcp.Reload<MainRecipeModel>(this.Recipe);
                }
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

                this.IsInit = AP.Proc.IsInit;
                this.IsAuto = AP.Proc.IsAuto;
                this.IsBusy = AP.Proc.IsBusy;

                this.Step = AP.Proc.Imprint.StepMsg;

                this.isInitStart = IsInit && !IsBusy;
                this.IsAutoStart = IsInit && IsAuto && !IsBusy;
                this.IsManualStart = IsInit && !IsAuto && !IsBusy;

                this.UpdateTime();

                this.Stage.Update();
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
                    case "Auto":
                        {
                            if (AP.Proc.IsAuto == true) return;

                            AP.Proc.InitReset();
                            AP.Proc.IsAuto = true;

                            AP.Event.ModeChangedEvent(true);
                        }
                        break;
                    case "Manual":
                        {
                            if (AP.Proc.IsAuto == false) return;

                            AP.Proc.IsAuto = false;

                            AP.Event.ModeChangedEvent(false);
                        }
                        break;
                    case "Init":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to perform the initialization operation?") == false) return;

                            this.LogMsg = string.Empty;

                            AP.Proc.Init.Start();
                        }
                        break;
                    case "Start":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to proceed with the process operation?") == false) return;

                            this.LogMsg = string.Empty;

                            AP.Proc.StartAuto(this.Recipe);
                        }
                        break;
                    case "Home":
                        {
                            var curPos = AP.Device[eAxis.StageX].ActPosition;
                            var readyPos = DB.MotParam.StageXReady.Position;

                            if(AP.Device.CheckPos(eAxis.StageX, readyPos) == false)
                            {
                                if (AP.Event.InterlockCheckEvent($"The stage in not the ready position.\r\n Would you like to perform the home operation?") == false) return;
                            }
                            else
                            {
                                if (AP.Event.InterlockCheckEvent($"Would you like to perform the home operation?") == false) return;
                            }

                            this.LogMsg = string.Empty;

                            AP.Proc.StartFilmLoading(this.Recipe, FilmLoadingMdoe.Home);
                        }
                        break;
                    case "Ready":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to perform the ready operation?") == false) return;

                            this.LogMsg = string.Empty;

                            AP.Proc.StartFilmLoading(this.Recipe, FilmLoadingMdoe.Ready);
                        }
                        break;
                    case "Film Loading":
                        {
                            var curPos = AP.Device[eAxis.StageX].ActPosition;
                            var readyPos = DB.MotParam.StageXReady.Position;

                            if (AP.Device.CheckPos(eAxis.StageX, readyPos) == false)
                            {
                                if (AP.Event.InterlockCheckEvent($" The Stage is not in the ready position.\r\n Would you like to perform the film loading operation ?") == false) return;
                            }
                            else
                            {
                                if (AP.Event.InterlockCheckEvent($"Would you like to perform the film loading operation ?") == false) return;
                            }

                            this.LogMsg = string.Empty;

                            AP.Proc.StartFilmLoading(this.Recipe, FilmLoadingMdoe.Loading);
                        }
                        break;
                    case "Imprint":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to perform the Imprint operation?") == false) return;

                            this.LogMsg = string.Empty;

                            AP.Proc.StartManual(this.Recipe, true);
                        }
                        break;
                    case "Demold":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to perform the demold operation?") == false) return;

                            this.LogMsg = string.Empty;

                            AP.Proc.StartManual(this.Recipe, false);
                        }
                        break;
                    case "Abort":
                        {
                            AP.ProcessStop();
                        }
                        break;
                    case "Recipe":
                        {
                            var view = new RcpSeletcViewModel(RecipeType.MAIN);
                            if (view.Show() == false) return;

                            this.Recipe = view.RcpSelected as MainRecipeModel;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateTime()
        {
            try
            {
                if (AP.Proc.Imprint.IsBusy == false) return;
                this.TackTime = AP.Proc.Imprint.TackTime;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Event_OnProcessMsgEvent(object sender, string e)
        {
            try
            {
                if (this.View.IsVisible == false) return;

                this.SetLogMsg(e);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SetLogMsg(string msg)
        {
            try
            {
                this.LogMsg = string.Format("{2}[{0}] {1}\r\n", DateTime.Now.ToString("HH:mm:ss.fff"), msg, LogMsg);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
