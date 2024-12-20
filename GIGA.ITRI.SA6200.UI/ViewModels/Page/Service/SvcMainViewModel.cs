
using GIGA.ITRI.SA6200.UI.Models;
using GIGA.ITRI.SA6200.UI.Models.Config;
using GIGA.ITRI.SA6200.UI.Models.Recipe;
using GIGA.ITRI.SA6200.UI.Models.Setup;
using GIGA.ITRI.SA6200.UI.Process.FilmLoading;
using GIGA.ITRI.SA6200.UI.Process.Work;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.ViewModels.Popup;
using GIGA.ITRI.SA6200.UI.Views.Page.DashBoard;
using GIGA.ITRI.SA6200.UI.Views.Page.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Service
{
    public class SvcMainViewModel : ISvcViewModel
    {
        private readonly ContentControl _view = new SvcMainView();

        public override int No => 0;

        public override string Name => "Main";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_clipboard", AP.ICON_RESOURCE);

        public bool IsBusy { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsStart { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public ObservableCollection<AxisModel> AxisList { get; set; } = new ObservableCollection<AxisModel>();

        public AxisModel SelectedAxis { get => this.GetValue<AxisModel>(); set => this.SetValue(value); }

        public StageModel Stage { get; set; } = new StageModel();

        public TeachDataModel StageXReady { get; set; } = new TeachDataModel("Stage X Ready");

        public TeachDataModel StageXFilmLoading { get; set; } = new TeachDataModel("Stage X Film Loading");

        public UcRecipeModel<MainRecipeModel> UC { get; set; } = new UcRecipeModel<MainRecipeModel>(RecipeType.MAIN);

        public bool isInitStart { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public NormalCommand OnSelectedCmd => new NormalCommand(SelectedCmd);

        public override void Init()
        {
            try
            {
                base.Init();

                this.AxisList.Add(Stage.StageX);
                this.AxisList.Add(Stage.GapLeft);
                this.AxisList.Add(Stage.GapRight);
                this.AxisList.Add(Stage.Demold);

                this.SelectedCmd(this.AxisList.First());
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

                this.Refresh();

                UC.LoadRecipe();
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

                this.IsBusy = AP.Proc.IsBusy;
                
                this.IsStart = !AP.Proc.IsAuto && AP.Proc.IsInit && !IsBusy;
                this.isInitStart = AP.Proc.IsInit && !IsBusy;

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
                    case "RCP":
                        {
                            var view = new RcpSeletcViewModel(RecipeType.MAIN);
                            if (view.Show() == false) return;

                            this.UC.RcpSelected = view.RcpSelected as MainRecipeModel;
                        }
                        break;
                    case "Start":
                        {
                            if (this.UC.RcpSelected == null)
                            {
                                AP.Event.InterlockMsgEvent("The selected recipe does not exist.");
                                return;
                            }

                            if (MsgBox.ShowMsg("Would you like to start the program?", true) == false) return;

                            AP.Proc.StartManual(this.UC.RcpSelected, WorkMode.AUTO);
                        }
                        break;
                    case "Abort":
                        {
                            AP.ProcessStop();
                        }
                        break;
                    case "Home":
                        {
                            var curPos = AP.Device[eAxis.StageX].ActPosition;
                            var readyPos = DB.MotParam.StageXReady.Position;

                            if (AP.Device.CheckPos(eAxis.StageX, readyPos) == false)
                            {
                                if (AP.Event.InterlockCheckEvent($"The stage is not in the readay position.\r\n Would you like to perform the home operation?") == false) return;
                            }
                            else
                            {
                                if (AP.Event.InterlockCheckEvent($"Would you like to perform the home operation?") == false) return;
                            }

                            AP.Proc.StartFilmLoading(this.UC.RcpSelected, FilmLoadingMdoe.Home);
                        }
                        break;
                    case "Ready":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to perform the ready operation?") == false) return;

                            AP.Proc.StartFilmLoading(this.UC.RcpSelected, FilmLoadingMdoe.Ready);
                        }
                        break;
                    case "Film Loading":
                        {
                            var curPos = AP.Device[eAxis.StageX].ActPosition;
                            var readyPos = DB.MotParam.StageXReady.Position;

                            if (AP.Device.CheckPos(eAxis.StageX, readyPos) == false)
                            {
                                if (AP.Event.InterlockCheckEvent($"The stage in not in the ready position.\r\nWould you like to perform the film loading operation ?") == false) return;
                            }
                            else
                            {
                                if (AP.Event.InterlockCheckEvent($"Would you like to perform the film loading operation?") == false) return;
                            }

                            AP.Proc.StartFilmLoading(this.UC.RcpSelected, FilmLoadingMdoe.Loading);
                        }
                        break;
                    case "IMP_MOVE":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to perform the imprint operation?") == false) return;

                            AP.Proc.StartManual(this.UC.RcpSelected, true);
                        }
                        break;
                    case "IMP_P_MOVE":
                        {
                            if (this.UC.RcpSelected == null)
                            {
                                MsgBox.ShowMsg("No recipe selected.");
                                return;
                            }

                            var axis = this.Stage;
                            var data = this.UC.RcpSelected.Imprint;
                            var speed = DB.MotParam.HomeSpeed;

                            this.MotionMove(axis.GapLeft, data.GapLeft, speed.GapLeft, -1);
                            this.MotionMove(axis.GapRight, data.GapRight, speed.GapRight, -1);
                        }
                        break;
                    case "DM_MOVE":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to perform the demold operation ?") == false) return;

                            AP.Proc.StartManual(this.UC.RcpSelected, false);
                        }
                        break;
                    case "DM_P_MOVE":
                        {
                            if (this.UC.RcpSelected == null)
                            {
                                MsgBox.ShowMsg("No recipe selected.");
                                return;
                            }

                            var axis = this.Stage;
                            var data = this.UC.RcpSelected.Demold;
                            var speed = DB.MotParam.HomeSpeed;

                            this.MotionMove(axis.GapLeft, data.GapLeft, speed.GapLeft, -1);
                            this.MotionMove(axis.GapRight, data.GapRight, speed.GapRight, -1);
                        }
                        break;

                    case "SAVE":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to initialize the data?") == false) return;

                            this.Refresh(true);
                        }
                        break;
                    case "Refresh":
                        {
                            if (AP.Event.InterlockCheckEvent("Would you like to initialize the data?") == false) return;

                            this.Refresh();
                        }
                        break;
                    case "R_STAGE_X":
                        {
                            this.StageXReady.Position = AP.Device[eAxis.StageX].ActPosition;
                        }
                        break;
                    case "M_STAGE_X":
                        {
                            var axis = this.Stage.StageX;
                            var pos = this.StageXReady.Position;
                            var speed = this.StageXReady.Speed;

                            this.MotionMove(axis, pos, speed);
                        }
                        break;
                    case "R_FILM_X":
                        {
                            this.StageXFilmLoading.Position = AP.Device[eAxis.StageX].ActPosition;
                        }
                        break;
                    case "M_FILM_X":
                        {
                            var axis = this.Stage.StageX;
                            var pos = this.StageXFilmLoading.Position;
                            var speed = this.StageXFilmLoading.Speed;

                            this.MotionMove(axis, pos, speed);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void MotionMove(AxisModel axis, RcpMotionDataModel data, double offset = 1) => MotionMove(axis, data.Position, data.Speed, offset);

        private void MotionMove(AxisModel axis, double pos, double speed, double offset = 1)
        {
            axis.AbsPos = pos * offset;
            axis.Speed = speed;

            axis.OnAbsMoveCmd.Execute(null);
        }

        private void SelectedCmd(object param)
        {
            try
            {
                var axis = param as AxisModel;
                if (axis == null) return;

                if (this.SelectedAxis != null) this.SelectedAxis.IsSeleted = false;

                this.SelectedAxis = axis;
                this.SelectedAxis.IsSeleted = true;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Refresh(bool isSave = false)
        {
            try
            {
                if (isSave)
                {
                    DB.DataCopy(DB.MotParam.StageXReady, this.StageXReady);
                    DB.DataCopy(DB.MotParam.StageXFilmLoading, this.StageXFilmLoading);
                }

                DB.DataCopyEx(DB.MotParam.StageXReady, this.StageXReady);
                DB.DataCopyEx(DB.MotParam.StageXFilmLoading, this.StageXFilmLoading);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
