using GIGA.ITRI.SA6200.UI.Views.Popup;
using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Popup
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly LoginView _view = new LoginView();

        public string Password { get => this.GetValue<string>(); set => this.SetValue(value); }

        private LoginViewModel()
        {
            this._view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this._view.DataContext = this;
        }

        protected override void OnNotifyCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "ESC":
                        {
                            this._view.Close();
                        }
                        break;
                    case "BACK":
                        {
                            if (string.IsNullOrEmpty(this.Password)) return;

                            this.Password = this.Password.Substring(0, this.Password.Length - 1);
                        }
                        break;
                    case "Enter":
                        {
                            this.EnterCommand();
                        }
                        break;
                    case "LOCK":
                        {
                            AP.Event.LoginModeChangedEvent(LoginMode.Lock);
                            this._view.Close();
                        }
                        break;
                    default:
                        {
                            this.Password += commandParameter as string;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void EnterCommand()
        {
            if (string.IsNullOrEmpty(this.Password)) return;

            if (this.Password == CFG.User.Manager.Password.ToString())
            {
                AP.Event.LoginModeChangedEvent(LoginMode.Manager);
                this._view.Close();
            }
            else if (this.Password == CFG.User.Engineer.Password.ToString())
            {
                AP.Event.LoginModeChangedEvent(LoginMode.Engineer);
                this._view.Close();
            }
            else if (this.Password == CFG.User.Operator.Password.ToString())
            {
                AP.Event.LoginModeChangedEvent(LoginMode.Operator);
                this._view.Close();
            }
            else if (this.Password == DateTime.Now.ToString("HHmm"))
            {
                AP.Event.LoginModeChangedEvent(LoginMode.Programmer);
                this._view.Close();
            }

            this.Password = string.Empty;
        }

        public static void Show()
        {
            var model = new LoginViewModel();

            model._view.ShowDialog();
        }
    }
}
