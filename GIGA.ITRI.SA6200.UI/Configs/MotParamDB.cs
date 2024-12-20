using TS.FW.Dac.Cfg;

namespace GIGA.ITRI.SA6200.UI.Configs
{
    public class MotParamDB : IConfigDb
    {
        public readonly StageXLimit StageXLimit = new StageXLimit();
        public readonly DemoldLimit DemoldLimit = new DemoldLimit();
        public readonly GapPressLimit GapPressLimit = new GapPressLimit();

        public readonly StageXReady StageXReady = new StageXReady();
        public readonly StageXFilmLoading StageXFilmLoading = new StageXFilmLoading();
        public readonly WaferSize06 WaferSize06 = new WaferSize06();
        public readonly WaferSize08 WaferSize08 = new WaferSize08();

        public readonly HomeSpeedData HomeSpeed = new HomeSpeedData();
        public readonly MotScaleData MotScale = new MotScaleData();
        public readonly MotEtcData MotEtc = new MotEtcData();
    }

    public abstract class ILimitData : IConfigDb
    {
        public double Speed { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Plus { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Minus { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public abstract class ITeachData : IConfigDb
    {
        public double Position { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Speed { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public class HomeSpeedData : IConfigDb
    {
        public double StageX { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Demold { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double GapLeft { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double GapRight { get => this.GetValueDouble(); set => this.SetValue(value); }

        public bool Interlock { get => this.GetValueBool(); set => this.SetValue(value); }
    }

    public class MotScaleData : IConfigDb
    {
        public double RollGap { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Demold { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public class MotEtcData : IConfigDb
    {
        public double DemoldDelay { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double RollGapHomePos { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public class StageXReady : ITeachData { }

    public class StageXFilmLoading : ITeachData { }

    public class WaferSize06 : ITeachData { }

    public class WaferSize08 : ITeachData { }

    public class StageXLimit : ILimitData { }

    public class DemoldLimit : ILimitData { }

    public class GapPressLimit : ILimitData { }
}
