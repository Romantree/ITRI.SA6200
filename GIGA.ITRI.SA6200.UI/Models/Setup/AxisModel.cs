using System;
using TS.FW;
using TS.FW.Device;
using TS.FW.Device.Ajin;
using TS.FW.Device.Ajin.Lib;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Setup
{
    public class AxisModel : DataModelBase
    {
        protected readonly IAxis axis;
        protected readonly IAxis axisSlave;

        public eAxis No => (eAxis)axis.No;

        public eAxis NoSlave => (eAxis)axisSlave.No;

        public bool IsSeleted { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool IsServoOn { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsAlarm { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsBusy { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool HomeSensor { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool LimitPlus { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool LimitMinus { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public double ActPosition { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double ComPosition { get => this.GetValue<double>(); set => this.SetValue(value); }

        public bool IsGantry { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public double Speed { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double AbsPos { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double RelPos { get => this.GetValue<double>(); set => this.SetValue(value); }

        public bool GantryAxis { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool SoftLimitEnable { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool SoftLimitStopMode { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool SoftLimitType { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public double SoftLimitPlus { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double SoftLimitMinus { get => this.GetValue<double>(); set => this.SetValue(value); }

        public AxisModel(eAxis axis)
        {
            this.axis = AP.Device[axis];
            this.Name = this.ToName(axis);


            this.Speed = 1;
            this.AbsPos = 0;
            this.RelPos = 1;
            this.GantryAxis = false;

            this.UpdateSoftLimit();
        }

        public AxisModel(eAxis axis, eAxis axisSlave) : this(axis)
        {
            this.axisSlave = AP.Device[axisSlave];
            this.GantryAxis = true;
        }

        public void Update()
        {
            try
            {
                this.IsServoOn = this.axis.IsServoOn;

                if(this.GantryAxis)
                {
                    this.IsAlarm = this.axis.IsAlarm || this.axisSlave.IsAlarm;
                }
                else
                {
                    this.IsAlarm = this.axis.IsAlarm;
                }

                this.IsBusy = this.axis.IsBusy;
                this.HomeSensor = this.axis.HomeSensor;
                this.LimitPlus = this.axis.LimitPlus;
                this.LimitMinus = this.axis.LimitMinus;

                this.ActPosition = this.axis.ActPosition;
                this.ComPosition = this.axis.ComPosition;

                this.IsGantry = AP.Device.Gantry(this.No, true);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateSoftLimit()
        {
            try
            {
                if (AP.IsSim) return;

                var item = this.axis as AjinAxis;

                this.SoftLimitEnable = item.Setting.SoftwareLimit.Enable;
                this.SoftLimitStopMode = item.Setting.SoftwareLimit.StopMode == AXT_MOTION_STOPMODE.SLOWDOWN_STOP;
                this.SoftLimitType = item.Setting.SoftwareLimit.Selection == AXT_MOTION_SELECTION.ACTUAL;
                this.SoftLimitPlus = item.Setting.SoftwareLimit.PositivePos * item.Setting.SCALE;
                this.SoftLimitMinus = item.Setting.SoftwareLimit.NegativePos * item.Setting.SCALE;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SetSoftLimit()
        {
            try
            {
                if (AP.IsSim) return;

                var item = this.axis as AjinAxis;

                var enable = this.SoftLimitEnable;
                var stopMode = this.SoftLimitStopMode ? AXT_MOTION_STOPMODE.SLOWDOWN_STOP : AXT_MOTION_STOPMODE.EMERGENCY_STOP;
                var type = this.SoftLimitType ? AXT_MOTION_SELECTION.ACTUAL : AXT_MOTION_SELECTION.COMMAND;
                var plus = this.SoftLimitPlus;
                var minus = this.SoftLimitMinus;

                item.SetSoftwareLimit(enable, stopMode, type, plus, minus);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private bool CheckAction()
        {
            if (this.IsBusy) return true;

            if (this.GantryAxis == true && this.IsGantry == false)
            {
                AP.Event.InterlockMsgEvent("The Gantry is not in the correct state.");
                return true;
            }
            
            if (DB.MotParam.HomeSpeed.Interlock == false) return false;

            if (this.No == eAxis.StageX)
            {
                if (AP.IO.GetCylinder(CylinderUnit.LIFT_PIN, CylinderAction.DOWN) == false)
                {
                    AP.Event.InterlockMsgEvent("The Lift Pin is not in the Down position.");
                    return true;
                }
                else if (AP.IO.GetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.DOWN) == false)
                {
                    AP.Event.InterlockMsgEvent("The Film Clamp is not in the Down position.");
                    return true;
                }
            }

            return false;
        }

        private bool CheckJogP()
        {
            if (DB.MotParam.HomeSpeed.Interlock == false) return false;

            return false;
        }

        private bool CheckJogM()
        {
            if (DB.MotParam.HomeSpeed.Interlock == false) return false;

            return false;
        }

        private bool CheckJogABS()
        {
            var pos = this.AbsPos - this.ActPosition;

            if (pos >= 0)
            {
                return this.CheckJogP();
            }
            else
            {
                return this.CheckJogM();
            }
        }

        public NormalCommand OnServoCmd => new NormalCommand(ServoCmd);

        public NormalCommand OnAlarmCmd => new NormalCommand(AlarmCmd);

        public NormalCommand OnResetPosCmd => new NormalCommand(ResetPosCmd);

        public NormalCommand OnHomeCmd => new NormalCommand(HomeCmd);

        public NormalCommand OnStopCmd => new NormalCommand(StopCmd);

        public NormalCommand OnJogMoveCmd => new NormalCommand(JogMoveCmd);

        public NormalCommand OnAbsMoveCmd => new NormalCommand(AbsMoveCmd);

        public NormalCommand OnRelMoveCmd => new NormalCommand(RelMoveCmd);

        public NormalCommand OnGantryCmd => new NormalCommand(GantryCmd);

        public NormalCommand OnSoftwareLimitCmd => new NormalCommand(SoftwareLimitCmd);

        protected virtual void ServoCmd(object param)
        {
            try
            {
                axis.IsServoOn = !this.IsServoOn;
                if (this.GantryAxis) this.axisSlave.IsServoOn = axis.IsServoOn;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void AlarmCmd(object param)
        {
            try
            {
                this.axis.ResetAlarm();

                if(this.GantryAxis)
                {
                    this.axisSlave.ResetAlarm();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void ResetPosCmd(object param)
        {
            try
            {
                this.axis.ResetPosition();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void HomeCmd(object param)
        {
            try
            {
                if (this.CheckAction()) return;

                AP.Device.Home(this.No);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void StopCmd(object param)
        {
            try
            {
                this.axis.Stop();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void JogMoveCmd(object param)
        {
            try
            {
                if (this.CheckAction()) return;

                switch (param as string)
                {
                    case "+":
                        {
                            if (this.CheckJogP()) return;

                            this.axis.MoveVEL(eDirection.Plus, this.Speed, this.Speed * 4, this.Speed * 4);
                        }
                        break;
                    case "-":
                        {
                            if (this.CheckJogM()) return;

                            this.axis.MoveVEL(eDirection.Minus, this.Speed, this.Speed * 4, this.Speed * 4);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void AbsMoveCmd(object param)
        {
            try
            {
                if (this.CheckAction()) return;
                if (this.CheckJogABS()) return;

                this.axis.MoveABS(this.AbsPos, this.Speed, this.Speed * 4, this.Speed * 4);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void RelMoveCmd(object param)
        {
            try
            {
                if (this.CheckAction()) return;

                switch (param as string)
                {
                    case "+":
                        {
                            if (this.CheckJogP()) return;

                            this.axis.MoveREL(this.RelPos, this.Speed, this.Speed * 4, this.Speed * 4);
                        }
                        break;
                    case "-":
                        {
                            if (this.CheckJogM()) return;

                            this.axis.MoveREL(-this.RelPos, this.Speed, this.Speed * 4, this.Speed * 4);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void GantryCmd(object param)
        {
            try
            {
                if (this.IsBusy) return;

                switch (param as string)
                {
                    case "ON":
                        {
                            AP.Device.GantryEnable(this.No, this.NoSlave);
                        }
                        break;
                    case "OFF":
                        {
                            AP.Device.GantryDisable(this.No, this.NoSlave);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void SoftwareLimitCmd(object param)
        {
            try
            {
                if (this.CheckAction()) return;

                switch (param as string)
                {
                    case "ON":
                        {
                            this.SetSoftLimit();
                        }
                        break;
                    case "OFF":
                        {
                            this.UpdateSoftLimit();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private string ToName(eAxis axis)
        {
            switch (axis)
            {
                case eAxis.StageX:
                case eAxis.StageXSlave: return "Stage X";

                case eAxis.RollGapLeft: return "Gap Left";
                case eAxis.RollGapRight: return "Gap Right";

                case eAxis.Demold: return "Demold";
            }

            return "None";
        }
    }
}
