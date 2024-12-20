using System;
using System.Collections.ObjectModel;
using System.Linq;
using TS.FW;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Recipe
{
    public class UcRecipeModel<T> : ViewModelBase where T : IRecipeModel
    {
        public RecipeType Type { get; private set; }

        public ObservableCollection<T> RcpList { get; set; } = new ObservableCollection<T>();

        public T RcpSelected { get => this.GetValue<T>(); set => this.SetValue(value); }

        public NormalCommand OnRcpSelectedCmd => new NormalCommand(RcpSelectedCmd);

        public UcRecipeModel(RecipeType type)
        {
            this.Type = type;
        }

        private void RcpSelectedCmd(object param)
        {
            try
            {
                var item = param as T;
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
            switch (commandParameter as string)
            {
                case "NAME":
                    {
                        if (this.RcpSelected == null)
                        {
                            MsgBox.ShowMsg("No recipe has been selected.");
                            return;
                        }

                        var name = Keyboard.Show();
                        if (string.IsNullOrWhiteSpace(name)) return;

                        if (this.RcpList.Any(t => t.Name == name))
                        {
                            MsgBox.ShowMsg("A recipe with the same name already exists.");
                            return;
                        }

                        this.RcpSelected.Name = name;
                    }
                    break;
                case "RELOAD":
                    {
                        this.LoadRecipe();
                    }
                    break;
                case "NEW":
                    {
                        var name = Keyboard.Show();
                        if (string.IsNullOrWhiteSpace(name)) return;

                        if (this.RcpList.Any(t => t.Name == name))
                        {
                            MsgBox.ShowMsg("A recipe with the same name already exists.");
                            return;
                        }

                        var res = AP.Rcp.NewRecipe(Type, name);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            MsgBox.ShowMsg("Failed to create the recipe.");
                            return;
                        }

                        this.LoadRecipe();

                        MsgBox.ShowMsg("The recipe has been newly created.");
                    }
                    break;
                case "DELTE":
                    {
                        if (this.RcpSelected == null)
                        {
                            MsgBox.ShowMsg("No recipe has been selected.");
                            return;
                        }

                        if (MsgBox.ShowMsg("Do you want to delete it?", true) == false) return;

                        var res = AP.Rcp.Delete(this.RcpSelected);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            MsgBox.ShowMsg("Failed to delete the recipe.");
                            return;
                        }

                        this.LoadRecipe();

                        MsgBox.ShowMsg("The recipe has been deleted.");
                    }
                    break;

                case "SAVE":
                    
                    {
                        if (this.RcpSelected == null)
                        {
                            MsgBox.ShowMsg("No recipe has been selected.");
                            return;
                        }

                        var res = AP.Rcp.Save(this.RcpSelected);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            MsgBox.ShowMsg("Failed to delete the recipe.");
                            return;
                        }

                        this.LoadRecipe();

                        MsgBox.ShowMsg("The recipe has been saved.");
                    }
                    break;
                case "SAVE AS":
                    {
                        if (this.RcpSelected == null)
                        {
                            MsgBox.ShowMsg("No recipe has been seleted.");
                            return;
                        }

                        var name = Keyboard.Show();
                        if (string.IsNullOrWhiteSpace(name)) return;

                        if (this.RcpList.Any(t => t.Name == name))
                        {
                            MsgBox.ShowMsg("A recipe with the same name already exists.");
                            return;
                        }

                        var res = AP.Rcp.SaveAs(name, this.RcpSelected);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            MsgBox.ShowMsg("Failed to create the recipe.");
                            return;
                        }

                        this.LoadRecipe();

                        MsgBox.ShowMsg("The recipe has been copied.");
                    }
                    break;
            }
        }

        public void ChangedType(RecipeType type)
        {
            try
            {
                if (this.Type == type) return;

                this.Type = type;
                this.LoadRecipe();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void LoadRecipe()
        {
            try
            {
                var no = this.RcpSelected != null ? this.RcpSelected.No : 0;

                this.RcpList.Clear();

                foreach (T item in AP.Rcp.ToRecipeList(Type))
                {
                    if (item == null) continue;

                    this.RcpList.Add(item);
                }

                this.RcpSelected = this.RcpList.FirstOrDefault(t => t.No == no);

                if (this.RcpSelected != null) this.RcpSelected.IsSelcted = true;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
