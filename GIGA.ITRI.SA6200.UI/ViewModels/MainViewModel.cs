using GIGA.ITRI.SA6200.UI.Models;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.ViewModels.Popup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using TS.FW;
using TS.FW.Diagnostics;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Converters;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BackgroundTimer _trUpdate = new BackgroundTimer(ApartmentState.MTA);

        public double Memory { get => this.GetValue<double>(); set => this.SetValue(value); }

        public bool Alarm { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Buzzer { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string Version { get => this.GetValue<string>(); set => this.SetValue(value); }

        public LoginMode Login { get => this.GetValue<LoginMode>(); set => this.SetValue(value); }

        public bool IsEnable { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public ObservableCollection<IPageViewModel> MenuList { get; set; } = new ObservableCollection<IPageViewModel>();

        public IPageViewModel SelectedMenu { get => this.GetValue<IPageViewModel>(); set => this.SetValue(value); }

        public TowerLamp TowerLamp { get; set; } = new TowerLamp();

        public NetworkState NetworkState { get; set; } = new NetworkState();

        public MainViewModel()
        {
            MainViewModel.SourceLevels = System.Diagnostics.SourceLevels.Critical;

            Logger.RootPath = AP.LOG_FILE;
            Logger.LogLevel = AP.LOG_LEVEL;
            Logger.FileDeleteDay = 30;

            CommandLog.IsTracking = false;
            NumberPad.Scale = 1;
            Keyboard.Scale = 1;

            OnOffColorConverter.InitColorList(this.ToColorList().ToArray());

            AP.Event.OnInterlockMsgEvent += Event_OnInterlockMsgEvent;
            AP.Event.OnInterlockCheckEvent += Event_OnInterlockCheckEvent;

            if (this.IsDesignMode) return;

            this.MainWindow.Loaded += MainWindow_Loaded;

            this._trUpdate.SleepTimeMsc = 100;
            this._trUpdate.DoWork += _trUpdate_DoWork;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Version = string.Format("v {0}", Assembly.GetExecutingAssembly().GetName().Version);

                Logger.Write(this, $"========== Program Start {this.Version} ============");

                PuLodingView.ShowLoding("SA 6200 Starting...", Loading, LoadingCmp);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Loading(object sender, DoWorkEventArgs e)
        {
            try
            {
                var model = sender as PuLodingViewModel;

                model.SetCaption("Initializing the database...", 0);
                DB.LoadDatabase();
                AP.Alarm.LoadDataBase();
                AP.Rcp.InitPath();

                model.SetCaption("Initializing the device...", 10);
                var open = AP.Device.Open();
                if (open == false) throw new Exception(open.Comment);

                model.SetCaption("Initializing In/Out...", 20);
                AP.IO.LoadDataBase();
                AP.IO.Start();

                model.SetCaption("Initializing Motion...", 30);
                var iniAxis = AP.Device.Axis.InitAxis(typeof(eAxis));
                if (iniAxis == false) throw new Exception(iniAxis.Comment);

                AP.Device.InitScale();

                model.SetCaption("Connecting to communication...", 30);
                AP.Net.Start();

                model.SetCaption("Starting memory collection...", 40);
                if (AP.IsSim == false) AP.Perf.Start();

                model.SetCaption("Configuring menu...", 99);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                AP.Event.InterlockMsgEvent("Program initialization failed. \r\n"
                    + "Please refer to the log file for details..\r\n" + $@"Path:{Logger.RootPath}\Year\Month\Day");
            }
        }

        private void LoadingCmp(Object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                AP.Event.OnLoginModeChangedEvent += Event_OnLoginModeChangedEvent;

                this.InitMenu();

                AP.Event.ModeChangedEvent(false);
                AP.Event.LoginModeChangedEvent(AP.IsSim ? LoginMode.Programmer : LoginMode.Lock);

                this._trUpdate.Start();

                AP.Proc.Saferty.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Event_OnLoginModeChangedEvent(object sender, LoginMode e)
        {
            try
            {
                this.Login = e;

                if (this.Login == LoginMode.Lock)
                {
                    this.IsEnable = false;

                    foreach (var item in this.MenuList)
                    {
                        item.SetUserMenu(this.Login, CFG.User.Lock);
                    }

                    this.SelectedMenu = this.MenuList.First();

                    return;
                }

                this.IsEnable = true;

                switch (this.Login)
                {
                    case LoginMode.Operator:
                        {
                            foreach (var item in this.MenuList)
                            {
                                item.SetUserMenu(this.Login, CFG.User.Operator);
                            }
                        }
                        break;
                    case LoginMode.Engineer:
                        {
                            foreach (var item in this.MenuList)
                            {
                                item.SetUserMenu(this.Login, CFG.User.Engineer);
                            }
                        }
                        break;
                    case LoginMode.Manager:
                        {
                            foreach (var item in this.MenuList)
                            {
                                item.SetUserMenu(this.Login, CFG.User.Manager);
                            }
                        }
                        break;
                    case LoginMode.Programmer:
                        {
                            foreach (var item in this.MenuList)
                            {
                                item.SetUserMenu(this.Login, CFG.User.Programmer);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Event_OnInterlockMsgEvent(object sender, string e)
        {
            try
            {
                Logger.CustomWrite("Interlock", sender, e, Logger.LogEventLevel.Information);

                this.Dispatcher.Invoke(() => MsgBox.ShowMsg(e));
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private bool? Event_OnInterlockCheckEvent(string e)
        {
            try
            {
                Logger.CustomWrite("Interlock", this, e, Logger.LogEventLevel.Information);

                return this.Dispatcher.Invoke(() => MsgBox.ShowMsg(e, true));
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);

                return false;
            }
        }

        private void _trUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Memory = AP.Perf.Memory;
                this.Alarm = AP.Alarm.IsAlarm;
                this.Buzzer = AP.IO.BUZZER_01 || AP.IO.BUZZER_02;

                this.TowerLamp.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                if (this.SelectedMenu != null) this.SelectedMenu.Update();
            }
        }

        protected override void OnNotifyCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "Alarm":
                        {
                            this.SelectedMenu = this.MenuList.FirstOrDefault(t => t.Name == "Alarm");
                        }
                        break;
                    case "Buzzer":
                        {
                            AP.IO.BUZZER_01 = false;
                            AP.IO.BUZZER_02 = false;
                        }
                        break;
                    case "Login":
                        {
                            LoginViewModel.Show();
                        }
                        break;

                    case "EXIT":
                        {
                            Logger.Write(this, $"========== Program Stop {this.Version} ============");

                            BackgroundTimer.AllStop();
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowMsg(ex.Message);
                Logger.Write(this, ex);
            }
        }

        private void InitMenu()
        {
            foreach (var item in IMenuViewModel.ToMenuViewModelList<IPageViewModel>())
            {
                item.Init();
                this.MenuList.Add(item);
            }

            this.SelectedMenu = this.MenuList.FirstOrDefault();
        }

        private IEnumerable<OnOffColor> ToColorList()
        {
            yield return OnOffColor.ToSolidColor("DOOR", Colors.BlueViolet, Colors.Red);
            yield return OnOffColor.ToRadialColor("EMO", Colors.White, Colors.Red, Colors.White, Colors.DimGray);

            yield return OnOffColor.ToSolidColor("NET_BACK", Colors.WhiteSmoke, Colors.IndianRed);
            yield return OnOffColor.ToSolidColor("NET_FONT", Colors.Black, Colors.WhiteSmoke);

            yield return OnOffColor.ToSolidColor("Red", Colors.Red, Colors.DimGray);
            yield return OnOffColor.ToSolidColor("Yellow", Colors.Yellow, Colors.DimGray);
            yield return OnOffColor.ToSolidColor("Green", Colors.Green, Colors.DimGray);

            yield return OnOffColor.ToSolidColor("INTERLOCK", Colors.Green, Colors.Red);
        }
    }
}
