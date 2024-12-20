using GIGA.ITRI.SA6200.UI.Configs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Subscribe
{
    public abstract class IPageViewModel : IMenuViewModel
    {
        public abstract void SetUserMenu(LoginMode mode, UserData data);
    }

    public class MenuSelectModel<T> : DataModelBase where T : IMenuViewModel
    {
        public ObservableCollection<T> MenuList { get; set; } = new ObservableCollection<T>();

        public T SelectedMenu { get => this.GetValue<T>(); set => this.SetValue(value); }

        public void Init()
        {
            foreach (var item in IMenuViewModel.ToMenuViewModelList<T>())
            {
                item.Init();
                this.MenuList.Add(item);
            }

            this.SelectedMenu = this.MenuList.FirstOrDefault();
        }

        public void Show()
        {
            try
            {
                if (this.SelectedMenu == null) return;

                var menu = this.SelectedMenu;
                this.SelectedMenu = null;
                this.SelectedMenu = menu;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Update()
        {
            try
            {
                if (this.SelectedMenu == null) return;

                this.SelectedMenu.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }

    public abstract class IDashViewModel : IMenuViewModel { }

    public abstract class IRcpViewModel : IMenuViewModel { }

    public abstract class ISvcViewModel : IMenuViewModel { }

    public abstract class IConfigViewModel : IMenuViewModel { }

    public abstract class IUtilViewModel : IMenuViewModel { }

    public abstract class ISetupViewModel : IMenuViewModel { }

    public abstract class IAlarmViewModel : IMenuViewModel { }
}
