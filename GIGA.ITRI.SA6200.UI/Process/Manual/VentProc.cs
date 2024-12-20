using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI.Process.Manual
{
    public class VentProc : IUnitProcess<VentStep>
    {
        public override string Name => "VentProc";

        protected override void Recovery(AlarmData<eAlarm> model) => base.Resume();

        public VacuumUnit Unit { get; set; } = VacuumUnit.Inch06;

        public VentProc() : base(true)
        {
        }

        public StepResult VENT_ENTER()
        {
            return VacuumEnter(Unit, false);
        }

        public StepResult VENT_POLLING()
        {
            return VacuumPolling(Unit, false);
        }

        public StepResult END()
        {
            return StepResult.Finish;
        }
    }

    public enum VentStep
    {
        VENT_ENTER,
        VENT_POLLING,

        END,
    }
}
