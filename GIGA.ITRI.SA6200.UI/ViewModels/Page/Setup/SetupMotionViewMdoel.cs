using GIGA.ITRI.SA6200.UI.Models.Setup;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Setup;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Setup
{
    public class SetupMotionViewMdoel : ISetupViewModel
    {
        private readonly ContentControl _view = new SetupMotionView();

        public override int No => 0;

        public override string Name => "Motion";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_server", AP.ICON_RESOURCE);

        public ObservableCollection<AxisModel> AxisList { get; set; } = new ObservableCollection<AxisModel>();

        public AxisModel SelectedAxis { get => this.GetValue<AxisModel>(); set => this.SetValue(value); }

        public NormalCommand OnSelectedCmd => new NormalCommand(SelectedCmd);

        public override void Init()
        {
            try
            {
                base.Init();

                this.AxisList.Add(new AxisModel(eAxis.StageX, eAxis.StageXSlave));
                this.AxisList.Add(new AxisModel(eAxis.RollGapRight));
                this.AxisList.Add(new AxisModel(eAxis.RollGapLeft));
                this.AxisList.Add(new AxisModel(eAxis.Demold));

                this.SelectedCmd(this.AxisList.First());
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

                foreach (var item in this.AxisList)
                {
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
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
    }
}
