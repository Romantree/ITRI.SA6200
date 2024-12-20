using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI.Process.Init
{
    public class InitProc : IUnitProcess<InitStep>
    {
        private int _reg;

        public override string Name => "InitProc";

        public bool Init { get; set; } = false;
        public InitProc() : base(true) { }

        protected override void Recovery(AlarmData<eAlarm> model) => base.Resume();

        public StepResult START()
        {
            this.SetProcMsg("================= Start Process =================");

            this.Init = false;

            IO.MC_ON = true;

            AP.Device.Stop(_moveAxis);

            _reg = DB.Network.RegulatorOption.Default;
            if (_reg <= 0) _reg = 30;

            return StepResult.Next;
        }

        public StepResult UV_LAMP_OFF_ENTER() => this.UvLampOnOffEnter(false);

        public StepResult UV_LAMP_OFF_POLLING() => this.UvLampOnOffPolling(false);

        public StepResult REG_SETTING_ENTER() => this.RegulatorEnter(_reg, _reg);

        public StepResult REG_SETTING_POLLING() => this.RegulatorPolling(_reg, _reg);

        public StepResult GAP_PRESS_UP_ENTER() => this.RollGapEnter(CylinderAction.UP);

        public StepResult GAP_PRESS_UP_POLLING() => this.RollGapPolling(CylinderAction.UP);

        public StepResult UV_CYLINDER_UP_ENTER() => this.UvCylinderEnter(CylinderAction.UP);

        public StepResult UV_CYLINDER_UP_POLLING() => this.UvCylinderPolling(CylinderAction.UP);

        public StepResult LIFT_PIN_DOWN_ENTER() => this.LiftPinEnter(CylinderAction.DOWN);

        public StepResult LIFT_PIN_DOWN_POLLING() => this.LiftPinPolling(CylinderAction.DOWN);

        public StepResult FILM_CLAMP_DOWN_ENTER() => this.FilmClampEnter(CylinderAction.DOWN);

        public StepResult FILM_CLAMP_DOWN_POLLING() => this.FilmClampPolling(CylinderAction.DOWN);

        public StepResult ROLL_CLAMP_DOWN_ENTER() => this.RollClampEnter(CylinderAction.DOWN);

        public StepResult ROLL_CLAMP_DOWN_POLLING() => this.RollClampPolling(CylinderAction.DOWN);

        public StepResult MOT_SERVO_ON_ENTER() => this.ServoOnEnter(_totalAxis);

        public StepResult MOT_SERVO_ON_POLLING() => this.ServoOnPolling(null, _totalAxis);

        public StepResult MOT_ALARM_RESET_ENTER() => this.ServoAlarmResetEnter(_totalAxis);

        public StepResult MOT_ALARM_RESET_POLLING() => this.ServoAlarmResetPolling(_totalAxis);

        public StepResult MOT_GANTRY_ENABLE_ENTER() => this.GantryEnalbeEnter(eAxis.StageX, eAxis.StageXSlave);

        public StepResult MOT_GANTRY_ENABLE_POLLING() => this.GantryEnalbePolling(eAxis.StageX);

        public StepResult MOT_HOME_ENTER() => this.HomeEnter(_moveAxis);

        public StepResult MOT_HOME_POLLING() => this.HomePolling(null, _moveAxis);

        public StepResult MOT_SET_LIMIT()
        {
            this.SetProcMsg("Motion Software Limit Enter");

            if (AP.IsSim) return StepResult.Next;

            foreach (var axis in _moveAxis)
            {
                AP.Device.ToAjin(axis).SetSoftwareLimit();
            }

            return StepResult.Next;
        }

        public StepResult LOADCELL_ZERO_ENTER() => this.LoadcellZeroEnter();

        public StepResult LOADCELL_ZERO_POLLING() => this.LoadcellZeroPolling();

        public StepResult END()
        {
            this.SetProcMsg("================= End Process =================");

            this.Init = true;

            return StepResult.Finish;
        }
    }
}
