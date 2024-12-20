using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI.Models.Alarm
{
    public class AlarmHistoryModel
    {
        public string PostTime { get; set; }

        public string ClearTime { get; set; }

        public eAlarm Alarm { get; set; }

        public AlarmLevel Level { get; set; }

        public static implicit operator AlarmHistoryModel(AlarmHistoryEntity item)
        {
            if (item == null) return null;

            return new AlarmHistoryModel()
            {
                Alarm = item.ToType<eAlarm>(),
                Level = (AlarmLevel)item.AlarmLevel,

                PostTime = item.PostTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ClearTime = item.ClearTime.HasValue ? item.ClearTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
            };
        }
    }
}
