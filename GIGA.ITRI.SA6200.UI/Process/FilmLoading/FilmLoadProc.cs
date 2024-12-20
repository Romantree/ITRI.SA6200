using GIGA.ITRI.SA6200.UI.Configs;
using GIGA.ITRI.SA6200.UI.Models.Recipe;
using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI.Process.FilmLoading
{
    public class FilmLoadProc : IUnitProcess<FilmLoadStep>
    {
        private int _reg;

        public FilmLoadingMdoe Mode { get; set; }

        public MainRecipeModel Rcp { get; set; }

        public FilmLoadProc() : base(true) { }

        public override string Name => "FilmLoadingProc";

        protected override void Recovery(AlarmData<eAlarm> model) => base.Resume();

        public StepResult START()
        {
            this.SetProcMsg("================= Start Process =================");

            AP.Device.Stop(_moveAxis);

            return StepResult.Next;
        }

        public StepResult MOT_GANTRY_ENABLE_ENTER() => this.GantryEnalbeEnter(eAxis.StageX, eAxis.StageXSlave);

        public StepResult MOT_GANTRY_ENABLE_POLLING() => this.GantryEnalbePolling(eAxis.StageX);

        public StepResult UV_LAMP_OFF_ENTER() => this.UvLampOnOffEnter(false);

        public StepResult UV_LAMP_OFF_POLLING() => this.UvLampOnOffPolling(false);

        public StepResult REG_SETTING_ENTER() => this.RegulatorEnter(Rcp.Demold.RegulatorLeft, Rcp.Demold.RegulatorRight);

        public StepResult REG_SETTING_POLLING() => this.RegulatorPolling(Rcp.Demold.RegulatorLeft, Rcp.Demold.RegulatorRight);

        public StepResult GAP_PRESS_UP_ENTER() => this.RollGapEnter(CylinderAction.UP);

        public StepResult GAP_PRESS_UP_POLLING() => this.RollGapPolling(CylinderAction.UP);

        public StepResult UV_CYLINDER_UP_ENTER() => this.UvCylinderEnter(CylinderAction.UP);

        public StepResult UV_CYLINDER_UP_POLLING() => this.UvCylinderPolling(CylinderAction.UP);

        public StepResult LIFT_PIN_DOWN_ENTER() => this.LiftPinEnter(CylinderAction.DOWN);

        public StepResult LIFT_PIN_DOWN_POLLING() => this.LiftPinPolling(CylinderAction.DOWN);

        public StepResult FILM_CLAMP_DOWN_ENTER() => this.FilmClampEnter(CylinderAction.DOWN);

        public StepResult FILM_CLAMP_DOWN_POLLING() => this.FilmClampPolling(CylinderAction.DOWN);

        public StepResult MOT_GAP_HOME_MOVE_ENTER()
        {
            var data = DB.MotParam.MotEtc.RollGapHomePos;
            var speed = DB.MotParam.HomeSpeed;

            this.MotionSet(eAxis.RollGapRight, -data, speed.GapLeft);
            this.MotionSet(eAxis.RollGapLeft, -data, speed.GapRight);

            return this.MotionEnter(eAxis.RollGapRight, eAxis.RollGapLeft);
        }

        public StepResult MOT_GAP_HOME_MOVE_POLLING() => this.MotionPolling(60, eAxis.RollGapRight, eAxis.RollGapLeft);

        public StepResult MOT_READY_MOVE_ENTER()
        {
            var stageX = DB.MotParam.StageXReady;

            if(Mode == FilmLoadingMdoe.Home)
            {
                if (Device[eAxis.StageX].ActPosition <= stageX.Position) return StepResult.Jump;
            }
            else if(Mode == FilmLoadingMdoe.Loading)
            {
                if (Device[eAxis.StageX].ActPosition >= stageX.Position) return StepResult.Jump;
            }

            this.SetProcMsg("Ready Pos Move Enter");            

            MotionSet(eAxis.StageX, stageX.Position, stageX.Speed);
            MotionSet(eAxis.Demold, 0, stageX.Speed);

            return MotionEnter(eAxis.StageX, eAxis.Demold);
        }

        public StepResult MOT_READY_MOVE_POLLING()
        {
            return MotionPolling(60, eAxis.StageX, eAxis.Demold);
        }

        public StepResult MOT_HOME_MOVE_ENTER()
        {
            if (Mode != FilmLoadingMdoe.Home) return StepResult.Jump;

            this.SetProcMsg("Home Pos Move Enter");

            var stageX = DB.MotParam.HomeSpeed.StageX;

            MotionSet(eAxis.StageX, 0, stageX);
            MotionSet(eAxis.Demold, 0, stageX);

            return MotionEnter(eAxis.StageX, eAxis.Demold);
        }

        public StepResult MOT_HOME_MOVE_POLLING()
        {
            return MotionPolling(60, eAxis.StageX, eAxis.Demold);
        }

        public StepResult MOT_FILM_LOADING_MOVE_ENTER()
        {
            if (Mode != FilmLoadingMdoe.Loading) return StepResult.Jump;

            this.SetProcMsg("Film Loading Pos Move Enter");

            var ready = DB.MotParam.StageXReady;

            var stageX = DB.MotParam.StageXFilmLoading;
            var demold = DB.MotParam.StageXFilmLoading.Position - ready.Position;

            MotionSet(eAxis.StageX, stageX.Position, stageX.Speed);
            MotionSet(eAxis.Demold, demold, stageX.Speed);

            return MotionEnter(eAxis.StageX, eAxis.Demold);
        }

        public StepResult MOT_FILM_LOADING_MOVE_POLLING()
        {
            return MotionPolling(60, eAxis.StageX, eAxis.Demold);
        }

        public StepResult GAP_PRESS_DOWN_ENTER()
        {
            if (Mode == FilmLoadingMdoe.Home) return StepResult.Jump;

             return this.RollGapEnter(CylinderAction.DOWN);
        }

        public StepResult GAP_PRESS_DOWN_POLLING()
        {
            return this.RollGapPolling(CylinderAction.DOWN);
        }

        public StepResult MOT_GAP_PRESS_MOVE_ENTER()
        {
            if (Mode == FilmLoadingMdoe.Home) return StepResult.Jump;

            var data = Rcp.Demold;
            var speed = DB.MotParam.HomeSpeed;

            this.MotionSet(eAxis.RollGapRight, -data.GapLeft, speed.GapLeft);
            this.MotionSet(eAxis.RollGapLeft, -data.GapRight, speed.GapRight);

            return this.MotionEnter(eAxis.RollGapRight, eAxis.RollGapLeft);
        }

        public StepResult MOT_GAP_PRESS_MOVE_POLLING()
        {
            return MotionPolling(60, eAxis.RollGapRight, eAxis.RollGapLeft);
        }

        public StepResult END()
        {
            this.SetProcMsg("================= End Process =================");

            return StepResult.Finish;
        }
    }

    public enum FilmLoadingMdoe
    {
        Home,
        Ready,
        Loading,
    }
}
