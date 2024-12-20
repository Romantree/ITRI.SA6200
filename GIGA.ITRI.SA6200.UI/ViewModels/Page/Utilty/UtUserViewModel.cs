using GIGA.ITRI.SA6200.UI.Configs;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Utilty;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Utilty
{
    public class UtUserViewModel : IUtilViewModel
    {
        private readonly ContentControl _view = new UtUserView();

        public override int No => 3;

        public override string Name => "User";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_user_tie", AP.ICON_RESOURCE);

        public UserModel Operator { get => this.GetValue<UserModel>(); set => this.SetValue(value); }

        public UserModel Engineer { get => this.GetValue<UserModel>(); set => this.SetValue(value); }

        public UserModel Manager { get => this.GetValue<UserModel>(); set => this.SetValue(value); }

        public override void Init()
        {
            try
            {
                base.Init();

                this.Operator = new UserModel();
                this.Engineer = new UserModel();
                this.Manager = new UserModel();
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
                this.Operator = DB.User.Operator;
                this.Engineer = DB.User.Engineer;
                this.Manager = DB.User.Manager;

                base.Show();
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
                            if (MsgBox.ShowMsg("Would you like to save the data?", true) == false) return;

                            DB.User.Operator = this.Operator;
                            DB.User.Engineer = this.Engineer;
                            DB.User.Manager = this.Manager;

                            CFG.User.Update(DB.User);
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

    public class UserModel : DataModelBase
    {
        public int Password { get => this.GetValue<int>(); set => this.SetValue(value); }

        public bool Recipe { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Service { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Config { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Utilty { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Setup { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Alarm { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public static implicit operator UserModel(UserData data)
        {
            if (data == null) return new UserModel();

            return new UserModel()
            {
                Password = data.Password,
                Recipe = data.Recipe,
                Service = data.Service,
                Config = data.Config,
                Utilty = data.Utilty,
                Setup = data.Setup,
                Alarm = data.Alarm,
            };
        }

        public static implicit operator UserData(UserModel data)
        {
            if (data == null) return new UserData();

            return new UserData()
            {
                Password = data.Password,
                Recipe = data.Recipe,
                Service = data.Service,
                Config = data.Config,
                Utilty = data.Utilty,
                Setup = data.Setup,
                Alarm = data.Alarm,
            };
        }
    }
}
