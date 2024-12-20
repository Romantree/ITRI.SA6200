using System;
using System.Collections.Generic;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Managers
{
    public partial class InOutManager
    {
        private void BitChanged(KeyValuePair<string, IOData> data)
        {
            try
            {
                if (AP.Proc.IsBusy) return;

                if (data.Value.OnOff == true)
                {
                    this.BitChangedOn(data.Key);
                }
                else
                {
                    this.BitChangedOff(data.Key);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void BitChangedOn(string name)
        {
            try
            {
                switch (name)
                {
                    case nameof(X_RESET_SW):
                        {

                        }
                        break;
                    case nameof(X_LIFT_PIN_UP_SW):
                        {
                            this.SetCylinder(CylinderUnit.LIFT_PIN, CylinderAction.UP);
                        }
                        break;
                    case nameof(X_LIFT_PIN_DOWN_SW):
                        {
                            this.SetCylinder(CylinderUnit.LIFT_PIN, CylinderAction.DOWN);
                        }
                        break;
                    case nameof(X_FILM_CLAMP_UP_SW):
                        {
                            this.SetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.DOWN);
                        }
                        break;
                    case nameof(X_FILM_CLAMP_DOWN_SW):
                        {
                            this.SetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.UP);
                        }
                        break;
                    case nameof(X_ROLL_CLAMP_UP_SW):
                        {
                            this.SetCylinder(CylinderUnit.ROLL_CLAMP, CylinderAction.DOWN);
                        }
                        break;
                    case nameof(X_ROLL_CLAMP_DOWN_SW):
                        {
                            this.SetCylinder(CylinderUnit.ROLL_CLAMP, CylinderAction.UP);
                        }
                        break;
                    case nameof(X_VACUUM_INCH_06_SW):
                        {
                            if (AP.Proc.Vac.IsBusy) return;

                            if(this.VacuumCheck(VacuumUnit.Inch06, VacuumSetting.Off))
                            {
                                this.SetVacuum(VacuumUnit.Inch06, VacuumSetting.Vacuum);
                            }
                            else
                            {
                                AP.Proc.StartVent(VacuumUnit.Inch06);
                            }
                        }
                        break;
                    case nameof(X_VACUUM_INCH_08_SW):
                        {
                            if (AP.Proc.Vac.IsBusy) return;

                            if (this.VacuumCheck(VacuumUnit.Inch08, VacuumSetting.Off))
                            {
                                this.SetVacuum(VacuumUnit.Inch08, VacuumSetting.Vacuum);
                            }
                            else
                            {
                                AP.Proc.StartVent(VacuumUnit.Inch08);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void BitChangedOff(string name)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public bool GetCylinder(CylinderUnit unit, CylinderAction action)
        {
            try
            {
                if (unit == CylinderUnit.ROLL_GAP)
                {
                    return this.GetCylinder(CylinderUnit.ROLL_GAP_LEFT, action) && this.GetCylinder(CylinderUnit.ROLL_GAP_RIGHT, action);
                }
                else if(unit == CylinderUnit.ROLL_GAP_LEFT)
                {
                    return action == CylinderAction.UP ? X_ROLL_GAP_LEFT_UP : X_ROLL_GAP_LEFT_UP == false;
                }
                else if(unit == CylinderUnit.ROLL_GAP_RIGHT)
                {
                    return action == CylinderAction.UP ? X_ROLL_GAP_RIGHT_UP : X_ROLL_GAP_RIGHT_UP == false;
                }
                else
                {
                    var on = $"X_{unit}_{action}";
                    var off = $"X_{unit}_{action.Reverse()}";

                    return this.ReadX(on) == true && this.ReadX(off) == false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return false;
            }
        }

        public void SetCylinder(CylinderUnit unit, CylinderAction action, bool isInterlock = true)
        {
            try
            {
                if (isInterlock == true && this.InterlockCheck(unit, action)) return;

                if (unit == CylinderUnit.ROLL_GAP)
                {
                    this.SetCylinder(CylinderUnit.ROLL_GAP_LEFT, action, isInterlock);
                    this.SetCylinder(CylinderUnit.ROLL_GAP_RIGHT, action, isInterlock);
                }
                else
                {
                    var on = $"{unit}_{action}";
                    var off = $"{unit}_{action.Reverse()}";

                    this.WriteY(false, off);
                    this.WriteY(true, on);

                    if (AP.IsSim == false) return;

                    if(unit == CylinderUnit.ROLL_GAP_LEFT)
                    {
                        X_ROLL_GAP_LEFT_UP = action == CylinderAction.UP;
                    }
                    else if(unit == CylinderUnit.ROLL_GAP_RIGHT)
                    {
                        X_ROLL_GAP_RIGHT_UP = action == CylinderAction.UP;
                    }
                    else
                    {
                        this.WriteX(false, $"X_{off}");
                        this.WriteX(true, $"X_{on}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private bool InterlockCheck(CylinderUnit unit, CylinderAction action)
        {
            if (DB.MotParam.HomeSpeed.Interlock == false) return false;

            if(unit == CylinderUnit.UV_CYLINDER)
            {
                if(action == CylinderAction.DOWN)
                {
                    if(this.GetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.DOWN) == false)
                    {
                        AP.Event.InterlockMsgEvent($"The Film Clamp is not in the Down position.");
                        return true;
                    }
                    else if (this.GetCylinder(CylinderUnit.LIFT_PIN, CylinderAction.DOWN) == false)
                    {
                        AP.Event.InterlockMsgEvent("The Lift Pin is not in the Down position.");
                        return true;
                    }
                }
            }
            else if(unit == CylinderUnit.FILM_CLAMP)
            {
                if(action == CylinderAction.UP)
                {
                    if(this.GetCylinder(CylinderUnit.UV_CYLINDER, CylinderAction.UP) == false)
                    {
                        AP.Event.InterlockMsgEvent($"The UV Lamp is not the Up position.");
                        return true;
                    }
                }
            }
            else if(unit == CylinderUnit.LIFT_PIN)
            {
                if (action == CylinderAction.UP)
                {
                    if (this.VacuumCheck(VacuumUnit.Inch08, VacuumSetting.Off) == false)
                    {
                        AP.Event.InterlockMsgEvent($"\"The Vacuum is not in the ATM state.");
                        return true;
                    }
                }
            }

            return false;
        }

        public bool VacuumCheck(VacuumUnit unit, VacuumSetting set)
        {
            GetVacuum(out VacuumStatus inch06, out VacuumStatus inch08);

            switch (unit)
            {
                case VacuumUnit.Inch06: return IsVacuumStatus(inch06, set);
                case VacuumUnit.Inch08: return IsVacuumStatus(inch06, set) && IsVacuumStatus(inch08, set);
            }

            return false;
        }

        public VacuumStatus GetVacuum(VacuumUnit unit)
        {
            switch (unit)
            {
                case VacuumUnit.Inch06: return GetVacuum06();
                case VacuumUnit.Inch08: return GetVacuum08();
            }

            throw new TypeAccessException($"{unit}");
        }

        public void SetVacuum(VacuumUnit unit, VacuumSetting set)
        {
            switch (unit)
            {
                case VacuumUnit.Inch06:
                    {
                        SetVacuum06(set);
                    }
                    break;
                case VacuumUnit.Inch08:
                    {
                        SetVacuum06(set);
                        SetVacuum08(set);
                    }
                    break;
            }
        }

        private VacuumStatus GetVacuum06()
        {
            if (X_VACUUM_06 == true) return VacuumStatus.Vacuum;
            if (VACUUM_OFF_06 == true && X_VACUUM_06 == false) return VacuumStatus.Vent;

            if (VACUUM_ON_06 == true) return VacuumStatus.VacuumProcess;
            if (VACUUM_OFF_06 == true) return VacuumStatus.VentProcess;

            return X_VACUUM_06 == false ? VacuumStatus.ATM : VacuumStatus.Error;
        }

        private VacuumStatus GetVacuum08()
        {
            if (X_VACUUM_08 == true) return VacuumStatus.Vacuum;
            if (VACUUM_OFF_08 == true && X_VACUUM_08 == false) return VacuumStatus.Vent;

            if (VACUUM_ON_08 == true) return VacuumStatus.VacuumProcess;
            if (VACUUM_OFF_08 == true) return VacuumStatus.VentProcess;

            return X_VACUUM_08 == false ? VacuumStatus.ATM : VacuumStatus.Error;
        }

        private void SetVacuum06(VacuumSetting set)
        {
            switch (set)
            {
                case VacuumSetting.Vacuum:
                    {
                        VACUUM_OFF_06 = false;
                        VACUUM_ON_06 = true;

                        if (AP.IsSim)
                        {
                            X_VACUUM_06 = true;
                        }
                    }
                    break;
                case VacuumSetting.Vent:
                    {
                        VACUUM_ON_06 = false;
                        VACUUM_OFF_06 = true;

                        if (AP.IsSim)
                        {
                            X_VACUUM_06 = false;
                        }
                    }
                    break;
                case VacuumSetting.Off:
                    {
                        VACUUM_ON_06 = false;
                        VACUUM_OFF_06 = false;

                        if (AP.IsSim)
                        {
                            X_VACUUM_06 = false;
                        }
                    }
                    break;
            }
        }

        private void SetVacuum08(VacuumSetting set)
        {
            switch (set)
            {
                case VacuumSetting.Vacuum:
                    {
                        VACUUM_OFF_08 = false;
                        VACUUM_ON_08 = true;

                        if (AP.IsSim)
                        {
                            X_VACUUM_08 = true;
                        }
                    }
                    break;
                case VacuumSetting.Vent:
                    {
                        VACUUM_ON_08 = false;
                        VACUUM_OFF_08 = true;

                        if (AP.IsSim)
                        {
                            X_VACUUM_08 = false;
                        }
                    }
                    break;
                case VacuumSetting.Off:
                    {
                        VACUUM_ON_08 = false;
                        VACUUM_OFF_08 = false;

                        if (AP.IsSim)
                        {
                            X_VACUUM_08 = false;
                        }
                    }
                    break;
            }
        }

        private void GetVacuum(out VacuumStatus inch06, out VacuumStatus inch08)
        {
            inch06 = GetVacuum06();
            inch08 = GetVacuum08();
        }

        private bool IsVacuumStatus(VacuumStatus state, VacuumSetting set)
        {
            switch (set)
            {
                case VacuumSetting.Vacuum: return state == VacuumStatus.Vacuum;
                case VacuumSetting.Vent: return state == VacuumStatus.Vent;
                case VacuumSetting.Off: return state == VacuumStatus.ATM;
            }

            return false;
        }
    }
}
