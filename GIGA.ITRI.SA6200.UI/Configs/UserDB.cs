using TS.FW.Dac.Cfg;

namespace GIGA.ITRI.SA6200.UI.Configs
{
    public class UserDB : IConfigDb
    {
        public UserData Operator { get => this.GetValueClass<UserData>(); set => this.SetValue(value); }

        public UserData Engineer { get => this.GetValueClass<UserData>(); set => this.SetValue(value); }

        public UserData Manager { get => this.GetValueClass<UserData>(); set => this.SetValue(value); }

        public UserDB()
        {
            this.Operator = this.Operator ?? new UserData();
            this.Engineer = this.Engineer ?? new UserData();
            this.Manager = this.Manager ?? new UserData();
        }
    }

    public class UserCfg
    {
        public UserData Operator { get; set; }

        public UserData Engineer { get; set; }

        public UserData Manager { get; set; }

        public UserData Programmer { get; set; } = new UserData();

        public UserData Lock { get; set; } = new UserData() { Recipe = false, Service = false, Config = false, Utilty = false, Setup = false, Alarm = false };

        public void Update(UserDB db)
        {
            this.Operator = db.Operator;
            this.Engineer = db.Engineer;
            this.Manager = db.Manager;
        }
    }

    public class UserData
    {
        public int Password { get; set; }

        public bool Recipe { get; set; } = true;

        public bool Service { get; set; } = true;

        public bool Config { get; set; } = true;

        public bool Utilty { get; set; } = true;

        public bool Setup { get; set; } = true;

        public bool Alarm { get; set; } = true;
    }
}
