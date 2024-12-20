using GIGA.ITRI.SA6200.UI.Managers;
using GIGA.ITRI.SA6200.UI.Models.Service;
using GIGA.ITRI.SA6200.UI.Models.Setup;
using System;
using TS.FW;
using TS.FW.Device.Dto.Analog;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models
{
    public class StageModel : ModelBase
    {
        public AxisModel StageX { get; set; } = new AxisModel(eAxis.StageX, eAxis.StageXSlave);

        public AxisModel GapLeft { get; set; } = new AxisModel(eAxis.RollGapLeft);

        public AxisModel GapRight { get; set; } = new AxisModel(eAxis.RollGapRight);

        public AxisModel Demold { get; set; } = new AxisModel(eAxis.Demold);

        public UvLampModel UvLamp { get; set; } = new UvLampModel();

        public CylinderModel LiftPin { get; set; } = new CylinderModel(CylinderUnit.LIFT_PIN);

        public CylinderModel FilmClamp { get; set; } = new CylinderModel(CylinderUnit.FILM_CLAMP);

        public CylinderModel RollClamp { get; set; } = new CylinderModel(CylinderUnit.ROLL_CLAMP);

        public CylinderModel GapLeftLD { get; set; } = new CylinderModel(CylinderUnit.ROLL_GAP_LEFT);

        public CylinderModel GapRightLD { get; set; } = new CylinderModel(CylinderUnit.ROLL_GAP_RIGHT);

        public CylinderModel GapLD { get; set; } = new CylinderModel(CylinderUnit.ROLL_GAP);

        public CylinderModel UvCylinder { get; set; } = new CylinderModel(CylinderUnit.UV_CYLINDER);

        public RegulatorModel GapLeftRG { get; set; } = new RegulatorModel(true);

        public RegulatorModel GapRightRG { get; set; } = new RegulatorModel(false);

        public StageUIModel UI { get; set; } = new StageUIModel();

        public VacuumModel Vac06 { get; set; } = new VacuumModel(VacuumUnit.Inch06);

        public VacuumModel Vac08 { get; set; } = new VacuumModel(VacuumUnit.Inch08);

        public double LoadcellLeft { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double LoadcellRight { get => this.GetValue<double>(); set => this.SetValue(value); }

        public StageModel() { }

        public void Update()
        {
            try
            {
                this.StageX.Update();
                this.GapLeft.Update();
                this.GapRight.Update();
                this.Demold.Update();

                this.UvLamp.Update();
                this.LiftPin.Update();
                this.FilmClamp.Update();
                this.RollClamp.Update();
                this.GapLeftLD.Update();
                this.GapRightLD.Update();
                this.GapLD.Update();
                this.UvCylinder.Update();
                this.GapLeftRG.Update();
                this.GapRightRG.Update();

                this.Vac06.Update();
                this.Vac08.Update();

                this.LoadcellLeft = AP.Net.StageLeftLD.Data;
                this.LoadcellRight = AP.Net.StageRightLD.Data;

                this.UI.Update(this.StageX.ActPosition, this.Demold.ActPosition);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }

    public class StageUIModel : ModelBase
    {
        private const double UI_STAGE_X = 200;
        private const double UI_DMOLD = 72;
        private const double UI_FILM = -71;

        public double StageX { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Demold { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Film { get => this.GetValue<double>(); set => this.SetValue(value); }

        public StageUIModel()
        {
            Film = UI_FILM;
        }

        public void Update(double x, double d)
        {
            try
            {
                this.StageX = -AnalogRange.ToDigit(x, 0, DeviceManager.MAX_STAGE_X, UI_STAGE_X);
                this.Demold = AnalogRange.ToDigit(d, 0, DeviceManager.MAX_DEMOLD, UI_DMOLD);

                this.Film = UI_FILM + this.Demold;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
