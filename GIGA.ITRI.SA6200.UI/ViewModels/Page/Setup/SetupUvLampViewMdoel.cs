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
    public class SetupUvLampViewMdoel : ISetupViewModel
    {
        private readonly ContentControl _view = new SetupUvLampView();

        public override int No => 3;

        public override string Name => "Uv Lamp";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_thermometer_digital", AP.ICON_RESOURCE);

        public UvLampModel Stage { get; set; } = new UvLampModel();

        public override void Init()
        {
            try
            {
                base.Init();
                this.Stage.Init();
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
                this.Stage.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
