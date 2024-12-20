using System;
using System.Collections.Generic;
using TS.FW;
using TS.FW.Dac.Alarm;

namespace GIGA.ITRI.SA6200.UI
{
    public static class EnumHelper
    {
        public static List<eAxis> Axis { get; set; } = new List<eAxis>();

        public static List<AlarmLevel> AlarmLevel { get; set; } = new List<AlarmLevel>();

        public static List<string> SerialPort { get; set; } = new List<string>();

        public static List<VacuumUnit> VacuumUnit { get; set; } = new List<VacuumUnit>();

        static EnumHelper()
        {
            foreach (var item in System.IO.Ports.SerialPort.GetPortNames())
            {
                SerialPort.Add(item);
            }

            InitEnum(AlarmLevel);
            InitEnum(Axis);
            InitEnum(VacuumUnit);
        }

        private static void InitEnum<T>(List<T> list)
        {
            try
            {
                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(typeof(EnumHelper), ex);
            }
        }

        public static string ToUpDown(this bool updown) => updown ? "Up" : "Down";

        public static CylinderAction Reverse(this CylinderAction action) 
            => action == CylinderAction.UP ? CylinderAction.DOWN : CylinderAction.UP;
    }

    public enum eAxis
    {
        StageX,
        StageXSlave,
        RollGapLeft,
        RollGapRight,
        Demold,
    }

    public enum NetworkUnit
    {
        StageLeftLD,
        StageRightLD,
        StageUv,
        StageLeftRG,
        StageRightRG,
    }

    public enum CylinderUnit
    {
        LIFT_PIN,
        ROLL_GAP,
        ROLL_GAP_LEFT,
        ROLL_GAP_RIGHT,
        FILM_CLAMP,
        ROLL_CLAMP,
        UV_CYLINDER,
    }

    public enum CylinderAction
    {
        UP,
        DOWN,
    }

    public enum LoginMode
    {
        Lock,
        Operator,
        Engineer,
        Manager,
        Programmer,
    }

    public enum eIOType
    {
        IN,
        OUT,
    }

    public enum eSignalType
    {
        A,
        B,
    }

    public enum VacuumUnit
    {
        Inch06,
        Inch08,
    }

    public enum VacuumSetting
    {
        Vacuum,
        Vent,
        Off,
    }

    public enum VacuumStatus
    {
        ATM,

        Vacuum,
        Vent,

        VacuumProcess,
        VentProcess,

        Error,
    }
}
