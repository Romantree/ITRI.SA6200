using TS.FW.Dac.Cfg;

namespace GIGA.ITRI.SA6200.UI.Configs
{
    public class TimeoutDB
    {
        public readonly CylinderTimeout Cylinder = new CylinderTimeout();
        public readonly MotionTimeout Motion = new MotionTimeout();
        public readonly TimeoutVacDB Vac = new TimeoutVacDB();
    }

    public class CylinderTimeout: IConfigDb
    {
        public double LiftPin { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double RollGap { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double FilmClamp { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double RollClamp { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double UvCylinder { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double ProcBuzzerOffTime { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public class MotionTimeout : IConfigDb
    {
        public double StageX { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double RollGap { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public class TimeoutVacDB : IConfigDb
    {
        public double Timeout { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double VentOffDelay { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double VentDelay { get => this.GetValueDouble(); set => this.SetValue(value); }
    }
}
