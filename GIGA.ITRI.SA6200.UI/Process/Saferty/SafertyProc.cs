using System;
using System.Collections.Generic;
using TS.FW;
using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI.Process.Saferty
{
    public class SafertyProc : IUnitProcess<SafertyStep>
    {
        private int _blank = 0;

        public override string Name => "SafertyProc";

        public SafertyProc() : base(false) { }

        protected override void Recovery(AlarmData<eAlarm> model) => base.Resume();

        public StepResult CHECK()
        {
            SW();
            TOWER();

            EMO();
            DOOR();
            SIGNAL();
            Buzzer();

            return StepResult.Pending;
        }

        private void SW()
        {
            try
            {
                IO.LIFT_PIN_UP_SW = IO.GetCylinder(CylinderUnit.LIFT_PIN, CylinderAction.UP);
                IO.LIFT_PIN_DOWN_SW = IO.GetCylinder(CylinderUnit.LIFT_PIN, CylinderAction.DOWN);

                IO.FILM_CLAMP_DOWN_SW = IO.GetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.UP);
                IO.FILM_CLAMP_UP_SW = IO.GetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.DOWN);

                IO.ROLL_CLAMP_DOWN_SW = IO.GetCylinder(CylinderUnit.ROLL_CLAMP, CylinderAction.UP);
                IO.ROLL_CLAMP_UP_SW = IO.GetCylinder(CylinderUnit.ROLL_CLAMP, CylinderAction.DOWN);

                IO.VACUUM_INCH_06_SW = IO.VacuumCheck(VacuumUnit.Inch06, VacuumSetting.Vacuum);
                IO.VACUUM_INCH_08_SW = IO.VacuumCheck(VacuumUnit.Inch08, VacuumSetting.Vacuum);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void TOWER()
        {
            try
            {
                if(AP.Alarm.IsAlarm)
                {
                    IO.TOWER_LAMP_RED = true;
                    IO.TOWER_LAMP_YELLOW = false;
                    IO.TOWER_LAMP_GREEN = false;
                }
                else if(AP.Proc.IsBusy)
                {
                    IO.TOWER_LAMP_RED = false;
                    IO.TOWER_LAMP_YELLOW = false;
                    IO.TOWER_LAMP_GREEN = true;
                }
                else if(AP.Proc.IsInit == false)
                {
                    IO.TOWER_LAMP_RED = false;
                    _blank++; if (_blank >= 10) { IO.TOWER_LAMP_YELLOW = !IO.TOWER_LAMP_YELLOW; _blank = 0; }
                    IO.TOWER_LAMP_GREEN = false;
                }
                else
                {
                    IO.TOWER_LAMP_RED = false;
                    IO.TOWER_LAMP_YELLOW = true;
                    IO.TOWER_LAMP_GREEN = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void EMO()
        {
            try
            {
                if (IO.X_EMO_01) this.AlarmPort(eAlarm.EMO_01);
                if (IO.X_EMO_02) this.AlarmPort(eAlarm.EMO_02);
                if (IO.X_EMO_03) this.AlarmPort(eAlarm.EMO_03);
                if (IO.X_EMO_04) this.AlarmPort(eAlarm.EMO_04);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void DOOR()
        {
            try
            {
                if (IO.X_DOOR_LOCK_01) this.AlarmPort(eAlarm.DOOR_OPEN_01);
                if (IO.X_DOOR_LOCK_02) this.AlarmPort(eAlarm.DOOR_OPEN_02);
                if (IO.X_DOOR_LOCK_03) this.AlarmPort(eAlarm.DOOR_OPEN_03);
                if (IO.X_DOOR_LOCK_04) this.AlarmPort(eAlarm.DOOR_OPEN_04);
                if (IO.X_DOOR_LOCK_05) this.AlarmPort(eAlarm.DOOR_OPEN_05);
                if (IO.X_DOOR_LOCK_06) this.AlarmPort(eAlarm.DOOR_OPEN_06);
                if (IO.X_DOOR_LOCK_07) this.AlarmPort(eAlarm.DOOR_OPEN_07);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SIGNAL()
        {        
            try
            {
                if (IO.X_MC_ON == false) this.AlarmPort(eAlarm.MC_OFF);
                if (IO.X_MAIN_CDA_RESSURE_01) this.AlarmPort(eAlarm.MAIN_CDA_PRESSURE_01);
                if (IO.X_MAIN_CDA_RESSURE_02) this.AlarmPort(eAlarm.MAIN_CDA_PRESSURE_02);

                if (IO.X_UV_LAMP_READY) this.AlarmPort(eAlarm.UV_LAMP_NOT_READY);
                if (IO.X_UV_LAMP_ERROR) this.AlarmPort(eAlarm.UV_LAMP_ERROR);

                //if (IO.X_HEATER_ALARM) this.AlarmPort(eAlarm.HEATER_OVERLOAD_ALARM);

                if (IO.X_ION_BLOWER_ALARM) this.AlarmPort(eAlarm.ION_BLOWER_ALARM);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Buzzer()
        {
            try
            {
                DelayTimeHelper.Timer("Buzzer", DB.Timeout.Cylinder.ProcBuzzerOffTime, IsBuzzer, BuzzerOff);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private bool IsBuzzer() => IO.BUZZER_02;

        private void BuzzerOff() => IO.BUZZER_02 = false;
    }
    
    public static class DelayTimeHelper
    {
        private static Dictionary<string, DateTime?> _time = new Dictionary<string, DateTime?>();

        public static void Timer(this string key, double time, Func<bool> func, Action action)
        {
            if (time <= 0 || func() == false)
            {
                SetTime(key, null);
            }
            else if (GetTime(key).HasValue == false)
            {
                SetTime(key, DateTime.Now);
            }
            else if ((DateTime.Now - GetTime(key).Value).TotalSeconds >= time)
            {
                action();
            }
        }

        private static DateTime? GetTime(string key)
        {
            if (_time.ContainsKey(key) == false) _time.Add(key, null);

            return _time[key];
        }

        private static void SetTime(string key, DateTime? time)
        {
            if (_time.ContainsKey(key) == false) _time.Add(key, time);
            else _time[key] = time;
        }
    }

    public enum SafertyStep
    {
        CHECK,
    }
}
