using GIGA.ITRI.SA6200.UI.Models.Recipe;
using GIGA.ITRI.SA6200.UI.Views.Popup;
using System;
using System.Collections.ObjectModel;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Popup
{
    public class RcpSeletcViewModel : ViewModelBase
    {
        private readonly RcpSeletcView _view = new RcpSeletcView();

        public RecipeType Title { get => this.GetValue<RecipeType>(); set => this.SetValue(value); }

        public ObservableCollection<IRecipeModel> RcpList { get; set; } = new ObservableCollection<IRecipeModel>();

        public IRecipeModel RcpSelected { get => this.GetValue<IRecipeModel>(); set => this.SetValue(value); }

        public NormalCommand OnRcpSelectedCmd => new NormalCommand(RcpSelectedCmd);

        public RcpSeletcViewModel(RecipeType title)
        {
            this.Title = title;

            this._view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this._view.DataContext = this;
        }

        public bool? Show()
        {
            this.LoadRecipe(this.Title);

            return this._view.ShowDialog();
        }

        private void LoadRecipe(RecipeType unit)
        {
            try
            {
                this.RcpList.Clear();

                foreach (var item in AP.Rcp.ToRecipeList(unit))
                {
                    this.RcpList.Add(item);
                }

                this.RcpSelected = null;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void RcpSelectedCmd(object param)
        {
            try
            {
                var item = param as IRecipeModel;
                if (item == null) return;

                this.RcpSelected = item;
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
                    case "OK":
                        {
                            this._view.DialogResult = true;
                            this._view.Close();
                        }
                        break;
                    case "CANCEL":
                        {
                            this._view.DialogResult = false;
                            this._view.Close();
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
