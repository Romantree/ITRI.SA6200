using GIGA.ITRI.SA6200.UI.Managers;
using System;
using System.Collections.Generic;
using TS.FW.Device;
using TS.FW.Device.Ajin.Lib;

namespace GIGA.ITRI.SA6200.UI.Process
{
    public abstract partial class IProcessTimer
    {
        private static readonly Dictionary<eAxis, MotionMove> _axisList = new Dictionary<eAxis, MotionMove>();

        static IProcessTimer()
        {
            foreach (var item in EnumHelper.Axis) _axisList.Add(item, new MotionMove());
        }

        protected StepResult ServoOnEnter(params eAxis[] list)
        {
            if (Device.IsServoOn(list)) return StepResult.Jump;

            this.SetProcMsg($"Motion Servo On Enter ({string.Join(", ", list)})");

            Device.ServoOn(list);

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult ServoOnPolling(object item = null, params eAxis[] list)
        {
            if (Device.IsServoOn(list))
            {
                this.TimeStop();

                this.SetProcMsg($"Motion Servo On ({string.Join(", ", list)})");

                return StepResult.Next;
            }
            else if (this.Timeout(1000))
            {
                this.TimeStop();

                this.SetProcMsg($"Motion Servo On Timeout ({string.Join(", ", list)})");

                return this.AlarmPort(eAlarm.MOTION_SERVO_ON_TIMEOUT, item);
            }

            return StepResult.Pending;
        }

        protected StepResult ServoAlarmResetEnter(params eAxis[] list)
        {
            if (Device.IsAlarm(list) == false) return StepResult.Jump;

            this.SetProcMsg($"Motion Alarm Reset Enter ({string.Join(", ", list)})");

            Device.ResetAlarm(list);

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult ServoAlarmResetPolling(object item = null, params eAxis[] list)
        {
            if (Device.IsAlarm(list) == false)
            {
                this.TimeStop();

                this.SetProcMsg($"Motion Alarm Reset ({string.Join(", ", list)})");

                return StepResult.Next;
            }
            else if (this.Timeout(3000))
            {
                this.TimeStop();

                this.SetProcMsg($"Motion Alarm Reset Timeout ({string.Join(", ", list)})");

                return this.AlarmPort(eAlarm.MOTION_ALARM_RESET_TIMEOUT, item);
            }

            return StepResult.Pending;
        }

        protected StepResult GantryEnalbeEnter(eAxis master, eAxis slave)
        {
            if (Device.Gantry(master, true)) return StepResult.Jump;

            this.SetProcMsg($"Motion {master}, {slave} Gantry Enable Enter");

            Device.GantryEnable(master, slave);

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult GantryEnalbePolling(eAxis master, object item = null)
        {
            if (Device.Gantry(master, true))
            {
                this.TimeStop();

                this.SetProcMsg($"Motion {master} Gantry Enable");

                return StepResult.Next;
            }
            else if (this.Timeout(5D))
            {
                this.TimeStop();

                this.SetProcMsg($"Motion {master} Gantry Enable Timeout");

                return this.AlarmPort(eAlarm.MOTION_GANTRY_TIMEOUT, item);
            }

            return StepResult.Pending;
        }

        protected StepResult HomeEnter(params eAxis[] list)
        {
            this.SetProcMsg($"Motion Home Enter ({string.Join(", ", list)})");

            Device.Home(list);

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult HomePolling(object item = null, params eAxis[] list)
        {
            if (Device.HomeSuccess(list))
            {
                this.TimeStop();

                this.SetProcMsg($"Motion Home ({string.Join(", ", list)})");

                return StepResult.Next;
            }
            else if (Device.HomeFail(list))
            {
                this.TimeStop();

                Device.Stop(list);
                Device.HomeStop(list);

                this.SetProcMsg($"Motion Home Fail ({string.Join(", ", list)})");

                return this.AlarmPort(eAlarm.MOTION_HOME_FAIL, item);
            }
            else if (this.Timeout(120 * 1000))
            {
                this.TimeStop();

                Device.Stop(list);
                Device.HomeStop(list);

                this.SetProcMsg($"Motion Home Timeout ({string.Join(", ", list)})");

                return this.AlarmPort(eAlarm.MOTION_HOME_TIMEOUT, item);
            }

            return StepResult.Pending;
        }

        protected void MotionSet(eAxis axis, double pos, double speed)
        {
            if (speed <= 0) speed = 1;

            if(axis == eAxis.Demold)
            {
                if(pos < DeviceManager.MIN_DEMOLD)
                {
                    pos = DeviceManager.MIN_DEMOLD;
                }
                else if(pos > DeviceManager.MAX_DEMOLD)
                {
                    pos = DeviceManager.MAX_DEMOLD;
                }
            }

            var item = _axisList[axis];

            item.Position = Math.Round(pos, 3);
            item.Speed = speed;
            item.Acc = speed * 4;
        }

        protected StepResult MotionEnter(params eAxis[] list)
        {
            foreach (var axis in list)
            {
                var data = _axisList[axis];
                var mot = AP.Device[axis];

                var curPos = AP.IsSim ? mot.ActPosition : mot.ComPosition;
                var isBusy = mot.IsBusy;

                if (isBusy == false && AP.Device.CheckPos(axis, data.Position))
                {
                    data.Bypass = true;
                    data.IsComplete = true;
                    this.SetProcMsg("{0} Axis Movement : Cur:{1:f3}mm Move:{2:f3}mm Bypass", axis, curPos, data.Position);
                }
                else
                {
                    data.Bypass = false;
                    this.SetProcMsg("{0} Axis Movement : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);
                }
            }

            var demoldDelay = DB.MotParam.MotEtc.DemoldDelay;

            foreach (var axis in list)
            {
                var data = _axisList[axis];
                if (data.Bypass) continue;

                if (axis == eAxis.Demold) this.Sleep(demoldDelay);

                var mot = Device[axis];
                mot.MoveABS(data.Position, data.Speed, data.Acc, data.Acc);

                data.BusyTime = null;
                data.IsComplete = false;
            }

            this.TimeStart();

            return StepResult.Next;
        }
        protected StepResult MotionPolling(double timeout, params eAxis[] list) => this.MotionPolling(null, timeout, list);

        protected StepResult MotionPolling(object item, double timeout, params eAxis[] list)
        {
            foreach (var axis in list)
            {
                var data = _axisList[axis];
                var mot = Device[axis];

                if (data.IsComplete || data.Bypass) continue;

                if (mot.IsBusy == false && Device.CheckPos(axis, data.Position))
                {
                    var curPos = AP.IsSim ? mot.ActPosition : mot.ComPosition;
                    this.SetProcMsg("{0} Axis Movement Complete : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);

                    data.IsComplete = true;
                }
                else if (this.Timeout(timeout))
                {
                    mot.Stop();

                    var res = this.MotionTimeout(axis, mot, data, item);
                    if (res != StepResult.Next) return res;
                }
                else if (mot.IsBusy == false)
                {
                    if (data.BusyTime == null)
                    {
                        data.BusyTime = DateTime.Now;
                    }
                    else if ((DateTime.Now - data.BusyTime.Value).TotalSeconds >= 1000)
                    {
                        var curPos = AP.IsSim ? mot.ActPosition : mot.ComPosition;
                        this.SetProcMsg("{0} Axis Movement Retry : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);

                        mot.MoveABS(data.Position, data.Speed, data.Acc, data.Acc);

                        data.BusyTime = null;
                    }
                }
            }

            if (this.IsComplete(list))
            {
                this.TimeStop();

                return StepResult.Next;
            }

            return StepResult.Pending;
        }

        private bool IsComplete(params eAxis[] list)
        {
            foreach (var axis in list)
            {
                if (_axisList[axis].IsComplete == false) return false;
            }

            return true;
        }

        private StepResult MotionTimeout(eAxis axis, IAxis mot, MotionMove data, object item)
        {
            mot.Stop();

            var curPos = AP.IsSim ? mot.ActPosition : mot.ComPosition;
            this.SetProcMsg("{0} Axis Movement Timeout : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);

            var res = this.AlarmPort(this.ToAlarm(axis), item);
            if (res != StepResult.Next) return res;

            data.IsComplete = true;

            return StepResult.Next;
        }

        private eAlarm ToAlarm(eAxis axis)
        {
            switch (axis)
            {
                case eAxis.StageX: return eAlarm.MOTION_STAGE_X_MOVE_TIMEOUT;
                case eAxis.RollGapLeft: return eAlarm.MOTION_GAP_PRESS_LEFT_MOVE_TIMEOUT;
                case eAxis.RollGapRight: return eAlarm.MOTION_GAP_PRESS_RIGHT_MOVE_TIMEOUT;
                case eAxis.Demold: return eAlarm.MOTION_DEMOLD_MOVE_TIMEOUT;
            }

            return eAlarm.MOTION_MOVE_TIMEOUT;
        }
    }

    public class MotionMove
    {
        public double Position { get; set; }

        public double Speed { get; set; }

        public double Acc { get; set; }

        public DateTime? BusyTime { get; set; } = null;

        public bool Bypass { get; set; } = false;

        public bool IsComplete { get; set; } = false;
    }
}