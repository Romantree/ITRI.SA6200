using GIGA.ITRI.SA6200.UI.Configs;
using GIGA.ITRI.SA6200.UI.Models.Recipe;
using System;
using System.Collections.Generic;
using TS.FW;
using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI.Process.Work
{
    public class ImprintProcess : IUnitProcess<ImprintStep>
    {
        private readonly object _locker = new object();

        private DateTime _tackTime = DateTime.Now;

        private double _stageXTimeout;
        private double _gapRollTimeout;

        public override string Name => "ImprintProcess";

        public WorkItem Item { get; private set; }

        public MainRecipeModel Rcp => Item.Rcp;

        public MotParamDB Mot => DB.MotParam;

        public double TackTime => (DateTime.Now - _tackTime).TotalSeconds;


        public ImprintProcess() : base(true) { }

        public void SetWorkItem(WorkItem item)
        {
            try
            {
                lock (this._locker)
                {
                    this.Item = item;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void ClearWorkItem() => this.SetWorkItem(null);

        protected override void Recovery(AlarmData<eAlarm> model) => base.Resume();

        protected override void Finish()
        {
            try
            {
                this.ClearWorkItem();
                base.Finish();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void DoWorkCallback()
        {
            try
            {
                lock (this._locker)
                {
                    if (this.Item == null) return;

                    base.DoWorkCallback();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public StepResult START()
        {
            this.SetProcMsg("================= Start Process =================");
            this.SetProcMsg($"Mode : {Item.Mode}");

            this._tackTime = DateTime.Now;

            IO.LOADCELL_ZERO = false;

            this._stageXTimeout = DB.Timeout.Motion.StageX <= 0 ? 60 : DB.Timeout.Motion.StageX;
            this._gapRollTimeout = DB.Timeout.Motion.RollGap <= 0 ? 60 : DB.Timeout.Motion.RollGap;

            return StepResult.Next;
        }

        public StepResult MOT_GANTRY_ENABLE_ENTER() => this.GantryEnalbeEnter(eAxis.StageX, eAxis.StageXSlave);

        public StepResult MOT_GANTRY_ENABLE_POLLING() => this.GantryEnalbePolling(eAxis.StageX, Item);

        public StepResult UV_LAMP_OFF_ENTER() => this.UvLampOnOffEnter(false);

        public StepResult UV_LAMP_OFF_POLLING() => this.UvLampOnOffPolling(false, Item);

        public StepResult FILM_CLAMP_DOWN_ENTER() => this.FilmClampEnter(CylinderAction.DOWN);

        public StepResult FILM_CLAMP_DOWN_POLLING() => this.FilmClampPolling(CylinderAction.DOWN, Item);

        public StepResult ROLL_CLAMP_DOWN_ENTER() => this.RollClampEnter(CylinderAction.DOWN);

        public StepResult ROLL_CLAMP_DOWN_POLLING() => this.RollClampPolling(CylinderAction.DOWN, Item);

        public StepResult LIFT_PIN_DOWN_ENTER() => this.LiftPinEnter(CylinderAction.DOWN, Rcp.VacuumUnit);

        public StepResult LIFT_PIN_DOWN_POLLING() => this.LiftPinPolling(CylinderAction.DOWN, Item);

        public StepResult VACUUN_ON_ENTER() => this.VacuumEnter(Rcp.VacuumUnit, true);

        public StepResult VACUUN_ON_POLLING() => this.VacuumPolling(Rcp.VacuumUnit, true);

        public StepResult MODE_CHECK()
        {
            switch (Item.Mode)
            {
                case WorkMode.IMPRINT:
                    {
                        this.Step.Change(ImprintStep.IMP_START);
                        return StepResult.Change;
                    }
                case WorkMode.DEMOLD:
                    {
                        this.Step.Change(ImprintStep.DE_START);
                        return StepResult.Change;
                    }
            }

            return StepResult.Next;
        }

        public StepResult IMP_START() => StepResult.Next;

        public StepResult IMP_REG_SETTING_ENTER() => this.RegulatorEnter(Rcp.Imprint.RegulatorLeft, Rcp.Imprint.RegulatorRight);

        public StepResult IMP_REG_SETTING_POLLING() => this.RegulatorPolling(Rcp.Imprint.RegulatorLeft, Rcp.Imprint.RegulatorRight, Item);

        public StepResult IMP_UV_LAMP_POWER_ENTER()
        {
            if (Rcp.Imprint.UvLampUsed == false) return StepResult.Jump;

            return this.UvLampPowerEnter(Rcp.Imprint.UvLampPower);
        }

        public StepResult IMP_UV_LAMP_POWER_POLLING() => this.UvLampPowerPolling(Rcp.Imprint.UvLampPower, Item);

        public StepResult IMP_MOT_STAGE_READY_ENTER()
        {
            var x = Mot.StageXReady;

            if (Device[eAxis.StageX].ActPosition >= x.Position) return StepResult.Jump;

            this.StepMsg = "Stage : [Ready] Position Move";
            this.SetProcMsg(this.StepMsg);

            this.MotionSet(eAxis.StageX, x.Position, x.Speed);
            this.MotionSet(eAxis.Demold, 0, x.Speed);

            return this.MotionEnter(eAxis.StageX, eAxis.Demold);
        }

        public StepResult IMP_MOT_STAGE_READY_POLLING() => this.MotionPolling(Item, _stageXTimeout, eAxis.StageX, eAxis.Demold);

        public StepResult IMP_UV_DOWN_ENTER()
        {
            if (Rcp.Imprint.UvLampUsed == false && Rcp.Demold.UvLampUsed == false) return StepResult.Jump;

            return this.UvCylinderEnter(CylinderAction.DOWN);
        }

        public StepResult IMP_UV_DOWN_POLLING() => this.UvCylinderPolling(CylinderAction.DOWN, Item);

        public StepResult IMP_GAP_PRESS_DOWN_ENTER() => this.RollGapEnter(CylinderAction.DOWN);

        public StepResult IMP_GAP_PRESS_DOWN_POLLING() => this.RollGapPolling(CylinderAction.DOWN, Item);

        public StepResult IMP_MOT_GAP_PRESS_ENTER()
        {
            this.StepMsg = "Roll Gap : [Imrpint] Position Move";
            this.SetProcMsg(this.StepMsg);

            var data = Rcp.Imprint;
            var speed = Mot.HomeSpeed;

            this.MotionSet(eAxis.RollGapRight, -data.GapLeft, speed.GapLeft);
            this.MotionSet(eAxis.RollGapLeft, -data.GapRight, speed.GapRight);

            return this.MotionEnter(eAxis.RollGapRight, eAxis.RollGapLeft);
        }

        public StepResult IMP_MOT_GAP_PRESS_POLLING() => this.MotionPolling(Item, _gapRollTimeout, eAxis.RollGapRight, eAxis.RollGapLeft);

        public StepResult IMP_MOT_STAGE_SIZE_ENTER()
        {
            this.StepMsg = $"StageX : [{Rcp.VacuumUnit}] Position Move";
            this.SetProcMsg(this.StepMsg);

            var data = Rcp.VacuumUnit == VacuumUnit.Inch06 ? (ITeachData)DB.MotParam.WaferSize06 : (ITeachData)DB.MotParam.WaferSize08;
            var readyPos = DB.MotParam.StageXReady.Position;

            var pos = data.Position < readyPos ? readyPos : data.Position;
            var speed = data.Speed;

            var demold = pos - readyPos;

            this.MotionSet(eAxis.StageX, pos, speed);
            this.MotionSet(eAxis.Demold, demold, speed);

            return this.MotionEnter(eAxis.StageX, eAxis.Demold);
        }

        public StepResult IMP_MOT_STAGE_SIZE_POLLING() => this.MotionPolling(Item, _stageXTimeout, eAxis.StageX, eAxis.Demold);

        public StepResult IMP_UV_LAMP_ON_ENTER()
        {
            if (Rcp.Imprint.UvLampUsed == false) return StepResult.Jump;

            return this.UvLampOnOffEnter(true);
        }

        public StepResult IMP_UV_LAMP_ON_POLLING() => this.UvLampOnOffPolling(true, Item);

        public StepResult IMP_MOT_STAGE_ENTER()
        {
            this.StepMsg = "StageX : [Imrpint] Position Move";
            this.SetProcMsg(this.StepMsg);

            var data = Rcp.Imprint.Stage;
            var readyPos = DB.MotParam.StageXReady.Position;

            var pos = data.Position < readyPos ? readyPos : data.Position;
            var speed = data.Speed;

            var demold = pos - readyPos;

            this.MotionSet(eAxis.StageX, pos, speed);
            this.MotionSet(eAxis.Demold, demold, speed);

            var result = this.MotionEnter(eAxis.StageX, eAxis.Demold);

            if (result == StepResult.Next) AP.Product.Start(Rcp.Name);

            return result;
        }

        public StepResult IMP_MOT_STAGE_POLLING()
        {
            var result = this.MotionPolling(Item, _stageXTimeout, eAxis.StageX, eAxis.Demold);

            if (result == StepResult.Pending) AP.Product.Collect();
            else if (result == StepResult.Next) AP.Product.Stop();

            return result;
        }

        public StepResult IMP_UV_LAMP_OFF_ENTER() => this.UvLampOnOffEnter(false);

        public StepResult IMP_UV_LAMP_OFF_POLLING() => this.UvLampOnOffPolling(false, Item);

        public StepResult IMP_END()
        {
            if (Item.Mode != WorkMode.AUTO)
            {
                this.Step.Change(ImprintStep.END);
                return StepResult.Change;
            }

            return StepResult.Next;
        }

        public StepResult DE_START() => StepResult.Next;

        public StepResult DE_REG_SETTING_ENTER() => this.RegulatorEnter(Rcp.Demold.RegulatorLeft, Rcp.Demold.RegulatorRight);

        public StepResult DE_REG_SETTING_POLLING() => this.RegulatorPolling(Rcp.Demold.RegulatorLeft, Rcp.Demold.RegulatorRight, Item);

        public StepResult DE_UV_LAMP_POWER_ENTER()
        {
            if (Rcp.Demold.UvLampUsed == false) return StepResult.Jump;

            return this.UvLampPowerEnter(Rcp.Demold.UvLampPower);
        }

        public StepResult DE_UV_LAMP_POWER_POLLING() => this.UvLampPowerPolling(Rcp.Demold.UvLampPower, Item);

        public StepResult DE_GAP_PRESS_DOWN_ENTER()
        {
            return this.RollGapEnter(CylinderAction.DOWN);
        }

        public StepResult DE_GAP_PRESS_DOWN_POLLING() => this.RollGapPolling(CylinderAction.DOWN, Item);

        public StepResult DE_MOT_GAP_PRESS_ENTER()
        {
            this.StepMsg = "Roll Gap : [Demold] Position Move";
            this.SetProcMsg(this.StepMsg);

            var data = Rcp.Demold;
            var speed = Mot.HomeSpeed;

            this.MotionSet(eAxis.RollGapRight, -data.GapLeft, speed.GapLeft);
            this.MotionSet(eAxis.RollGapLeft, -data.GapRight, speed.GapRight);

            return this.MotionEnter(eAxis.RollGapRight, eAxis.RollGapLeft);
        }

        public StepResult DE_MOT_GAP_PRESS_POLLING() => this.MotionPolling(Item, _gapRollTimeout, eAxis.RollGapRight, eAxis.RollGapLeft);

        public StepResult DE_UV_LAMP_ON_ENTER()
        {
            if (Rcp.Demold.UvLampUsed == false) return StepResult.Jump;

            return this.UvLampOnOffEnter(true);
        }

        public StepResult DE_UV_LAMP_ON_POLLING() => this.UvLampOnOffPolling(true, Item);

        public StepResult DE_MOT_STAGE_ENTER()
        {
            var data = Rcp.Demold.Stage;

            var readyPos = DB.MotParam.StageXReady.Position;

            var pos = data.Position < readyPos ? readyPos : data.Position;
            var speed = data.Speed;

            if (Device[eAxis.StageX].ActPosition <= pos) return StepResult.Jump;

            this.StepMsg = "StageX : [Demold] Position Move";
            this.SetProcMsg(this.StepMsg);

            var demold = pos - readyPos;

            this.MotionSet(eAxis.StageX, pos, speed);
            this.MotionSet(eAxis.Demold, demold, speed);

            return this.MotionEnter(eAxis.StageX, eAxis.Demold);
        }

        public StepResult DE_MOT_STAGE_POLLING() => this.MotionPolling(Item, _stageXTimeout, eAxis.StageX, eAxis.Demold);

        public StepResult DE_UV_LAMP_OFF_ENTER() => this.UvLampOnOffEnter(false);

        public StepResult DE_UV_LAMP_OFF_POLLING() => this.UvLampOnOffPolling(false, Item);

        public StepResult DE_MOT_STAGE_READY_ENTER()
        {
            var x = Mot.StageXReady;

            if (Device[eAxis.StageX].ActPosition <= x.Position) return StepResult.Jump;

            this.StepMsg = "Stage : [Ready] Position Move";
            this.SetProcMsg(this.StepMsg);

            this.MotionSet(eAxis.StageX, x.Position, x.Speed);
            this.MotionSet(eAxis.Demold, 0, x.Speed);

            return this.MotionEnter(eAxis.StageX, eAxis.Demold);
        }

        public StepResult DE_MOT_STAGE_READY_POLLING() => this.MotionPolling(Item, _stageXTimeout, eAxis.StageX, eAxis.Demold);

        //public StepResult DE_MOT_GAP_HOME_ENTER()
        //{
        //    this.StepMsg = "Roll Gap : [Home] Position Move";
        //    this.SetProcMsg(this.StepMsg);

        //    var data = DB.MotParam.MotEtc.RollGapHomePos;
        //    var speed = Mot.HomeSpeed;

        //    this.MotionSet(eAxis.RollGapRight, -data, speed.GapLeft);
        //    this.MotionSet(eAxis.RollGapLeft, -data, speed.GapRight);

        //    return this.MotionEnter(eAxis.RollGapRight, eAxis.RollGapLeft);
        //}

        //public StepResult DE_MOT_GAP_HOME_POLLING() => this.MotionPolling(Item, _gapRollTimeout, eAxis.RollGapRight, eAxis.RollGapLeft);

        public StepResult DE_UV_UP_ENTER() => this.UvCylinderEnter(CylinderAction.UP);

        public StepResult DE_UV_UP_POLLING() => this.UvCylinderPolling(CylinderAction.UP, Item);

        public StepResult DE_END()
        {
            return StepResult.Next;
        }

        public StepResult END()
        {
            this.StepMsg = string.Empty;

            IO.BUZZER_02 = true;

            this.SetProcMsg("================= End Process =================");

            return StepResult.Finish;
        }

    }
}
