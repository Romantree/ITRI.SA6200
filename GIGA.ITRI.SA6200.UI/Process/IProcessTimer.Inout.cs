using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GIGA.ITRI.SA6200.UI.Process
{
    public abstract partial class IProcessTimer
    {
        protected StepResult VacuumEnter(VacuumUnit unit, bool onoff)
        {
            if (IO.VacuumCheck(unit, onoff ? VacuumSetting.Vacuum : VacuumSetting.Off)) return StepResult.Jump;

            SetProcMsg($"{unit} {(onoff ? "Vacuum" : "Vent")} Enter");

            IO.SetVacuum(unit, onoff ? VacuumSetting.Vacuum : VacuumSetting.Vent);

            TimeStart();

            return StepResult.Next;
        }

        protected StepResult VacuumPolling(VacuumUnit unit, bool onoff)
        {
            var db = DB.Timeout.Vac;

            if (IO.VacuumCheck(unit, onoff ? VacuumSetting.Vacuum : VacuumSetting.Vent))
            {
                TimeStop();

                SetProcMsg($"{unit} {(onoff ? "Vacuum" : "Vent")}");

                if (onoff == false)
                {
                    this.Sleep(db.VentOffDelay);

                    IO.SetVacuum(unit, VacuumSetting.Off);

                    this.Sleep(db.VentDelay);
                }

                return StepResult.Next;
            }
            else if (Timeout(db.Timeout))
            {
                TimeStop();

                IO.SetVacuum(unit, VacuumSetting.Off);

                SetProcMsg($"{unit} {(onoff ? "Vacuum" : "Vent")} Timeout");

                return AlarmPort(onoff ? eAlarm.VACUUM_TIMEOUT : eAlarm.VENT_TIMEOUT);
            }

            return StepResult.Pending;
        }

        protected StepResult LiftPinEnter(CylinderAction action, VacuumUnit? vac = null)
        {
            if (vac != null && action == CylinderAction.DOWN) IO.SetVacuum(vac.Value, VacuumSetting.Vacuum);

            return this.CylinderEnter(CylinderUnit.LIFT_PIN, action);
        }

        protected StepResult LiftPinPolling(CylinderAction action, object item = null)
        {
            var time = DB.Timeout.Cylinder.LiftPin;

            return this.CylinderPolling(CylinderUnit.LIFT_PIN, action, time, eAlarm.LIFT_PIN_UP_DOWN_TIMEOUT, item);
        }

        protected StepResult RollGapEnter(CylinderAction action) => this.CylinderEnter(CylinderUnit.ROLL_GAP, action);

        protected StepResult RollGapPolling(CylinderAction action, object item = null)
        {
            var time = DB.Timeout.Cylinder.RollGap;

            return this.CylinderPolling(CylinderUnit.ROLL_GAP, action, time, eAlarm.ROLL_GAP_UP_DOWN_TIMEOUT, item);
        }

        protected StepResult FilmClampEnter(CylinderAction action) => this.CylinderEnter(CylinderUnit.FILM_CLAMP, action);

        protected StepResult FilmClampPolling(CylinderAction action, object item = null)
        {
            var time = DB.Timeout.Cylinder.FilmClamp;

            return this.CylinderPolling(CylinderUnit.FILM_CLAMP, action, time, eAlarm.FILM_CLAMP_UP_DOWN_TIMEOUT, item);
        }

        protected StepResult RollClampEnter(CylinderAction action) => this.CylinderEnter(CylinderUnit.ROLL_CLAMP, action);

        protected StepResult RollClampPolling(CylinderAction action, object item = null)
        {
            var time = DB.Timeout.Cylinder.RollClamp;

            return this.CylinderPolling(CylinderUnit.ROLL_CLAMP, action, time, eAlarm.ROLL_CLAMP_UP_DOWN_TIMEOUT, item);
        }

        protected StepResult UvCylinderEnter(CylinderAction action) => this.CylinderEnter(CylinderUnit.UV_CYLINDER, action);

        protected StepResult UvCylinderPolling(CylinderAction action, object item = null)
        {
            var time = DB.Timeout.Cylinder.UvCylinder;

            return this.CylinderPolling(CylinderUnit.UV_CYLINDER, action, time, eAlarm.UV_CYLINDER_UP_DOWN_TIMEOUT, item);
        }

        private StepResult CylinderEnter(CylinderUnit unit, CylinderAction action)
        {
            this.StepMsg = $"{unit} {action}".Replace("_", " ");

            if (IO.GetCylinder(unit, action)) return StepResult.Jump;

            this.SetProcMsg($"{unit} {action} Enter".Replace("_", " "));

            IO.SetCylinder(unit, action, false);

            this.TimeStart();

            return StepResult.Next;
        }

        //여기
        private StepResult CylinderPolling(CylinderUnit unit, CylinderAction action, double time, eAlarm alram, object item = null)
        {
            if (IO.GetCylinder(unit, action))
            {
                this.TimeStop();

                this.SetProcMsg($"{unit} {action}".Replace("_", " "));

                if (unit == CylinderUnit.ROLL_GAP && action == CylinderAction.DOWN)
                {
                    this.Sleep(2D);
                }

                return StepResult.Next;
            }
            else if (this.Timeout(time))
            {
                this.TimeStop();

                this.SetProcMsg($"{unit} {action} Timeout".Replace("_", " "));

                return this.AlarmPort(alram, item);
            }

            return StepResult.Pending;
        }
    }
}
