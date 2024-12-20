namespace GIGA.ITRI.SA6200.UI.Managers
{
    public partial class InOutManager
    {
        [IOSetting(IN, 0x000, "MAIN CDA PRESSURE #1")]
        public bool X_MAIN_CDA_RESSURE_01 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x001, "MAIN CDA PRESSURE #2")]
        public bool X_MAIN_CDA_RESSURE_02 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x003, "RESET S/W")]
        public bool X_RESET_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x004, "LIFT PIN UP S/W")]
        public bool X_LIFT_PIN_UP_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x005, "LIFT PIN DOWN S/W")]
        public bool X_LIFT_PIN_DOWN_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x006, "FILM CLAMP UP S/W")]
        public bool X_FILM_CLAMP_UP_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x007, "FILM CLAMP DOWN S/W")]
        public bool X_FILM_CLAMP_DOWN_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x008, "ROLL CLAMP UP S/W")]
        public bool X_ROLL_CLAMP_UP_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x009, "ROLL CLAMP DOWN S/W")]
        public bool X_ROLL_CLAMP_DOWN_SW { get => this.ReadX(); set => this.WriteX(value); }

        //[IOSetting(IN, 0x00A, "ROLL GAP UP S/W")]
        //public bool X_ROLL_GAP_UP_SW { get => this.ReadX(); set => this.WriteX(value); }

        //[IOSetting(IN, 0x00B, "ROLL GAP DOWN S/W")]
        //public bool X_ROLL_GAP_DOWN_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x00A, "VACUUM 06 INCH S/W")]
        public bool X_VACUUM_INCH_06_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x00B, "VACUUM 08 INCH S/W")]
        public bool X_VACUUM_INCH_08_SW { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x00C, "EMO #1")]
        public bool X_EMO_01 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x00D, "EMO #2")]
        public bool X_EMO_02 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x00E, "EMO #3")]
        public bool X_EMO_03 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x00F, "EMO #4")]
        public bool X_EMO_04 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x010, "FRONT DOOR LOCK #1")]
        public bool X_DOOR_LOCK_01 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x011, "RIGHT DOOR LOCK #2")]
        public bool X_DOOR_LOCK_02 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x012, "RIGHT DOOR LOCK #3")]
        public bool X_DOOR_LOCK_03 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x013, "BACK DOOR LOCK #4")]
        public bool X_DOOR_LOCK_04 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x014, "BACK DOOR LOCK #5")]
        public bool X_DOOR_LOCK_05 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x015, "LEFT DOOR LOCK #6")]
        public bool X_DOOR_LOCK_06 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x016, "LEFT DOOR LOCK #7")]
        public bool X_DOOR_LOCK_07 { get => this.ReadX(); set => this.WriteX(value); }       
        
        [IOSetting(IN, 0x017, "MC ON")]
        public bool X_MC_ON { get => this.ReadX(); set => this.WriteX(value); }

        //[IOSetting(IN, 0x017, "DOOR LOCK ALL")]
        //public bool X_DOOR_LOCK_ALL { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x018, "ION BLOWER RUN")]
        public bool X_ION_BLOWER_RUN { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x019, "ION BLOWER ALARM")]
        public bool X_ION_BLOWER_ALARM { get => this.ReadX(); set => this.WriteX(value); }

        //[IOSetting(IN, 0x01A, "HEATER 과부하 ALARM")]
        //public bool X_HEATER_ALARM { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x01B, "UV LAMP READY")]
        public bool X_UV_LAMP_READY { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x01C, "UV LAMP ON")]
        public bool X_UV_LAMP_ON { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x01D, "UV LAMP ERROR")]
        public bool X_UV_LAMP_ERROR { get => this.ReadX(); set => this.WriteX(value); }

        //[IOSetting(IN, 0x01E, "HEATER ON")]
        //public bool X_HEATER_ON { get => this.ReadX(); set => this.WriteX(value); }
    }

    public partial class InOutManager
    {
        [IOSetting(IN, 0x020, "ROLL GAP LEFT UP")]
        public bool X_ROLL_GAP_LEFT_UP { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x021, "ROLL GAP RIGHT UP")]
        public bool X_ROLL_GAP_RIGHT_UP { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x024, "LIFT PIN UP")]
        public bool X_LIFT_PIN_UP { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x025, "LIFT PIN DOWN")]
        public bool X_LIFT_PIN_DOWN { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x026, "UV CYLINDERE UP")]
        public bool X_UV_CYLINDER_UP { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x027, "UV CYLINDERE DOWN")]
        public bool X_UV_CYLINDER_DOWN { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x028, "FILM CLAMP UP")]
        public bool X_FILM_CLAMP_UP { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x029, "FILM CLAMP DOWN")]
        public bool X_FILM_CLAMP_DOWN { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x02A, "ROLL CLAMP UP")]
        public bool X_ROLL_CLAMP_UP { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x02B, "ROLL CLAMP DOWN")]
        public bool X_ROLL_CLAMP_DOWN { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x02C, "VACUUM 06 INCH")]
        public bool X_VACUUM_06 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x02D, "VACUUM 08 INCH")]
        public bool X_VACUUM_08 { get => this.ReadX(); set => this.WriteX(value); }
    }
}
