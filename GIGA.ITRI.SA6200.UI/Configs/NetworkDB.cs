using TS.FW.Dac.Cfg;

namespace GIGA.ITRI.SA6200.UI.Configs
{
    public class NetworkDB
    {
        public readonly UvLampOption UvLampOption = new UvLampOption();
        public readonly LoadcellOption LoadcellOption = new LoadcellOption();
        public readonly RegulatorOption RegulatorOption = new RegulatorOption();
        public readonly HeaterOption HeaterOption = new HeaterOption();
    }

    public abstract class IUvLampOption : IConfigDb
    {
        public bool IO { get => this.GetValueBool(); set => this.SetValue(value); }

        public double OnOffTimeout { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double PowerSetTimeout { get => this.GetValueDouble(); set => this.SetValue(value); }

        public int TempLimit { get => this.GetValueInt(); set => this.SetValue(value); }

        public int RetryCount { get => this.GetValueInt(); set => this.SetValue(value); }
    }

    public abstract class ILoadcellOption : IConfigDb
    {
        public double ZeroSetTimeout { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Alarm { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Warning { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Error { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double DetectTime { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public abstract class IRegulatorOption : IConfigDb
    {
        public int Default { get => this.GetValueInt(); set => this.SetValue(value); }

        public int Min { get => this.GetValueInt(); set => this.SetValue(value); }

        public int Max { get => this.GetValueInt(); set => this.SetValue(value); }

        public double SetTimeout { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public abstract class IHeaterOption : IConfigDb
    {
        public double Temp { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Alarm { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double Error { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double SetTimeout { get => this.GetValueDouble(); set => this.SetValue(value); }

        public double CheckTimeout { get => this.GetValueDouble(); set => this.SetValue(value); }
    }

    public class UvLampOption : IUvLampOption { }

    public class LoadcellOption : ILoadcellOption { }

    public class RegulatorOption : IRegulatorOption { }

    public class HeaterOption : IHeaterOption { }
}