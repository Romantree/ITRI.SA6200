using GIGA.ITRI.SA6200.UI.Models.Recipe;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Recipe
{
    public class RcpMainViewModel : IRcpViewModel
    {
        private readonly ContentControl view = new RcpMainView();

        public override int No => 0;

        public override string Name => "Main";

        public override ContentControl View => view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_clipboard", AP.ICON_RESOURCE);

        public UcRecipeModel<MainRecipeModel> UC { get; set; } = new UcRecipeModel<MainRecipeModel>(RecipeType.MAIN);

        public override void Show()
        {
            try
            {
                base.Show();

                UC.LoadRecipe();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
