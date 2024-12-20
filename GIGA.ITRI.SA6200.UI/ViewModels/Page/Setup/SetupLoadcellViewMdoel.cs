using GIGA.ITRI.SA6200.UI.Models.Service;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Setup;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Setup
{
    public class SetupLoadcellViewMdoel : ISetupViewModel
    {
        private readonly ContentControl _view = new SetupLoadcellView();

        public override int No => 2;

        public override string Name => "Loadcell";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_scale_unbalanced", AP.ICON_RESOURCE);

        public LoadcellModel Left { get; set; } = new LoadcellModel(true);

        public LoadcellModel Right { get; set; } = new LoadcellModel(false);

        public override void Init()
        {
            try
            {
                base.Init();

                this.Left.Init();
                this.Right.Init();
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

                this.Left.Update();
                this.Right.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
