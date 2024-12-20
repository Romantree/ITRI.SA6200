namespace GIGA.ITRI.SA6200.UI.Process
{
    public abstract partial class IProcessTimer
    {
        protected StepResult LoadcellZeroEnter()
        {
            this.StepMsg = $"Loadcell Zero Setting";

            if (Net.LoadcellCheck(0, 0)) return StepResult.Jump;

            this.SetProcMsg($"Loadcell Zero Setting Enter");

            IO.LOADCELL_ZERO = true;

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult LoadcellZeroPolling()
        {
            if (Net.LoadcellCheck(0, 0))
            {
                this.TimeStop();

                IO.LOADCELL_ZERO = false;

                this.SetProcMsg($"Loadcell Zero Setting");

                return StepResult.Next;
            }
            else if (this.Timeout(3D))
            {
                this.TimeStop();

                IO.LOADCELL_ZERO = false;

                this.SetProcMsg($"Loadcell Zero Setting Timeout");

                return StepResult.Next;
            }

            return StepResult.Pending;
        }

        protected StepResult RegulatorEnter(double left, double right)
        {
            this.StepMsg = $"Regulator Setting";

            if (Net.RequlatorCheck(left, right)) return StepResult.Jump;

            this.SetProcMsg($"Regulator [L:{left}%] [R:{right}%] Enter");

            Net.StageLeftRG.Set(left);
            Net.StageRightRG.Set(right);

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult RegulatorPolling(double left, double right, object item = null)
        {
            var time = DB.Network.RegulatorOption.SetTimeout;

            if (Net.RequlatorCheck(left, right))
            {
                this.TimeStop();

                this.SetProcMsg($"Regulator [L:{left}%] [R:{right}%]");

                return StepResult.Next;
            }
            else if (this.Timeout(time))
            {
                this.TimeStop();

                this.SetProcMsg($"Regulator [L:{left}%] [R:{right}%] Timeout");

                return this.AlarmPort(eAlarm.REGULATOR_SETTING_TIMEOUT, item);
            }

            return StepResult.Pending;
        }

        protected StepResult UvLampOnOffEnter(bool onoff)
        {
            if (AP.IS_UV_LAMP == false) return StepResult.Jump;

            this.StepMsg = $"UvLamp {(onoff ? "ON" : "OFF")}";

            if (Net.StageUv.LedOn == onoff) return StepResult.Jump;

            this.SetProcMsg($"UvLamp {(onoff ? "ON" : "OFF")} Enter");

            Net.StageUv.LedOnOff(onoff);

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult UvLampOnOffPolling(bool onoff, object item = null)
        {
            var time = DB.Network.UvLampOption.OnOffTimeout;

            if (Net.StageUv.LedOn == onoff)
            {
                this.TimeStop();

                this.SetProcMsg($"UvLamp {(onoff ? "ON" : "OFF")}");

                return StepResult.Next;
            }
            else if (this.Timeout(time))
            {
                this.TimeStop();

                this.SetProcMsg($"UvLamp {(onoff ? "ON" : "OFF")} Timeout");

                return this.AlarmPort(eAlarm.UV_LED_ON_OFF_TIMEOUT, item);
            }

            return StepResult.Pending;
        }

        protected StepResult UvLampPowerEnter(int power)
        {
            if (AP.IS_UV_LAMP == false) return StepResult.Jump;

            this.StepMsg = $"UvLamp Power Setting";

            if (Net.StageUv.Data.Power == power) return StepResult.Jump;

            this.SetProcMsg($"UvLamp Power : {power}% Enter");

            Net.StageUv.Retry = 0;
            Net.StageUv.SetPower(power);

            this.TimeStart();

            return StepResult.Next;
        }

        protected StepResult UvLampPowerPolling(int power, object item = null)
        {
            var time = DB.Network.UvLampOption.PowerSetTimeout;
            var retry = DB.Network.UvLampOption.RetryCount;

            if (Net.StageUv.PowerCheck(power))
            {
                this.TimeStop();

                this.SetProcMsg($"UvLamp Power : {power}%");

                return StepResult.Next;
            }
            else if (this.Timeout(time))
            {
                this.TimeStop();

                Net.StageUv.Retry++;

                if (Net.StageUv.Retry < retry)
                {
                    this.Sleep(1000);

                    this.SetProcMsg($"UvLamp Power : {power}% Retry {Net.StageUv.Retry}");

                    Net.StageUv.SetPower(power);

                    this.TimeStart();

                    return StepResult.Pending;
                }

                this.SetProcMsg($"UvLamp Power : {power}% Timeout");

                return this.AlarmPort(eAlarm.UV_LED_SET_POWER_TIMEOUT, item);
            }

            return StepResult.Pending;
        }
    }
}