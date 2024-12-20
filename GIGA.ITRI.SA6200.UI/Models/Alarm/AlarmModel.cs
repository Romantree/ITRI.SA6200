using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI.Models.Alarm
{
    public class AlarmModel
    {
        public string Time { get; set; }

        public int ID { get; set; }

        public eAlarm Alarm { get; set; }

        public AlarmLevel Level { get; set; }

        public bool IsRecovery { get; set; }

        public static implicit operator AlarmModel(AlarmData<eAlarm> item)
        {
            if (item == null) return null;

            return new AlarmModel()
            {
                Time = item.Time.ToString("HH:mm:ss"),
                ID = (int)item.Alarm,
                Alarm = item.Alarm,
                Level = item.Level,
                IsRecovery = item.IsRecovery,
            };
        }
    }
}
