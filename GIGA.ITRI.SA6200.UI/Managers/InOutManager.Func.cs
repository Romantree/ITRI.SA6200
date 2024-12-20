using System;
using System.Collections.Generic;
using TS.FW;

namespace GIGA.SA6200.UI.Managers
{
    public partial class InOutManager
    {
        private void BitChanged(KeyValuePair<string, IOData> data)
        {
            try
            {
                return;

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
                            this.SetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.UP);
                        }
                        break;
                    case nameof(X_FILM_CLAMP_DOWN_SW):
                        {
                            this.SetCylinder(CylinderUnit.FILM_CLAMP, CylinderAction.DOWN);
                        }
                        break;
                    case nameof(X_ROLL_CLAMP_UP_SW):
                        {
                            this.SetCylinder(CylinderUnit.ROLL_CLAMP, CylinderAction.UP);
                        }
                        break;
                    case nameof(X_ROLL_CLAMP_DOWN_SW):
                        {
                            this.SetCylinder(CylinderUnit.ROLL_CLAMP, CylinderAction.DOWN);
                        }
                        break;
                    case nameof(X_ROLL_GAP_UP_SW):
                        {
                            this.SetCylinder(CylinderUnit.ROLL_GAP, CylinderAction.UP);
                        }
                        break;
                    case nameof(X_ROLL_GAP_DOWN_SW):
                        {
                            this.SetCylinder(CylinderUnit.ROLL_GAP, CylinderAction.DOWN);
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

        public bool GetHeaterRun() => this.X_HEATER_ON;

        public void SetHeaterRun(bool onoff)
        {
            this.HEATER_ON = onoff;

            if (AP.IsSim) this.X_HEATER_ON = onoff;
        }

        public bool GetCylinder(CylinderUnit unit, CylinderAction action)
        {
            try
            {
                if (unit == CylinderUnit.ROLL_GAP)
                {
                    return this.GetCylinder(CylinderUnit.ROLL_GAP_LEFT, action) && this.GetCylinder(CylinderUnit.ROLL_GAP_RIGHT, action);
                }
                else if (unit == CylinderUnit.ROLL_GAP_LEFT)
                {
                    return this.X_ROLL_GAP_LEFT_UP == (action == CylinderAction.UP);
                }
                else if (unit == CylinderUnit.ROLL_GAP_RIGHT)
                {
                    return this.X_ROLL_GAP_RIGHT_UP == (action == CylinderAction.UP);
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

        public void SetCylinder(CylinderUnit unit, CylinderAction action)
        {
            try
            {
                if (unit == CylinderUnit.ROLL_GAP)
                {
                    this.SetCylinder(CylinderUnit.ROLL_GAP_LEFT, action);
                    this.SetCylinder(CylinderUnit.ROLL_GAP_RIGHT, action);
                }
                else
                {
                    var on = $"{unit}_{action}";
                    var off = $"{unit}_{action.Reverse()}";

                    this.WriteY(false, off);
                    this.WriteY(true, on);

                    if (AP.IsSim == false) return;

                    if (unit == CylinderUnit.ROLL_GAP_LEFT)
                    {
                        this.X_ROLL_GAP_LEFT_UP = (action == CylinderAction.UP);
                    }
                    else if (unit == CylinderUnit.ROLL_GAP_RIGHT)
                    {
                        this.X_ROLL_GAP_RIGHT_UP = (action == CylinderAction.UP);
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
    }
}
