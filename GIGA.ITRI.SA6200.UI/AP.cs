using GIGA.ITRI.SA6200.UI.Configs;
using GIGA.ITRI.SA6200.UI.Managers;
using GIGA.ITRI.SA6200.UI.Process;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI
{
    public static class AP
    {
        public const string ICON_RESOURCE = "Resources/Icons.xaml";

        public const bool IS_UV_LAMP = true;

        public static bool IsSim => ConfigurationManager.AppSettings["Simulation"] == "1";

        public static Logger.LogEventLevel LOG_LEVEL => (Logger.LogEventLevel)Enum.Parse(typeof(Logger.LogEventLevel), ConfigurationManager.AppSettings["LOG_LEVEL"]);

        public static string LOG_FILE => ConfigurationManager.AppSettings["LOG_FILE"];

        public static string INOUT_FILE => ConfigurationManager.AppSettings["INOUT_FILE"];

        public static string MOT_FILE => ConfigurationManager.AppSettings["MOT_FILE"];

        public readonly static AlarmManager Alarm = new AlarmManager();
        public readonly static EventManager Event = new EventManager();
        public readonly static PerformanceManager Perf = new PerformanceManager();

        public readonly static DeviceManager Device = new DeviceManager();
        public readonly static InOutManager IO = new InOutManager();
        public readonly static NetManager Net = new NetManager();

        public readonly static RecipeManager Rcp = new RecipeManager();
        public readonly static ProcManager Proc = new ProcManager();
        public readonly static ProductManager Product = new ProductManager();

        public static void ProcessStop()
        {
            try
            {
                AP.Device.Stop();
                AP.IO.Abort();
                AP.Net.Abort();
                AP.Proc.Abort();
            }
            catch (Exception ex)
            {
                Logger.Write(typeof(AP), ex);
            }
        }
    }

    public static class DB
    {
        public readonly static NetConfigDB Config = new NetConfigDB();
        public readonly static UserDB User = new UserDB();        

        public readonly static MotParamDB MotParam = new MotParamDB();
        public readonly static NetworkDB Network = new NetworkDB();
        public readonly static TimeoutDB Timeout = new TimeoutDB();

        public static void LoadDatabase()
        {
            CFG.User.Update(User);
        }

        public static void DataCopy(object a, object b)
        {
            try
            {
                var aList = a.GetType().GetProperties().Where(t => t.CanRead && t.CanWrite);
                var bList = b.GetType().GetProperties().Where(t => t.CanRead && t.CanWrite);

                foreach (var aInfo in aList)
                {
                    var bInfo = bList.FirstOrDefault(t => t.Name == aInfo.Name);
                    if (bInfo == null) continue;

                    var value = bInfo.GetValue(b);
                    aInfo.SetValue(a, value);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(typeof(DB), ex);
            }
        }

        public static void DataCopyEx(object a, object b) => DataCopy(b, a);
    }

    public static class CFG
    {
        public readonly static UserCfg User = new UserCfg();
    }
}
