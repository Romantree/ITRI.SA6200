namespace GIGA.ITRI.SA6200.UI.Managers
{
    public partial class InOutManager
    {
        public const int OA = 0x040;

        [IOSetting(OUT, OA + 0x000, "TOWER LAMP RED")]
        public bool TOWER_LAMP_RED { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x001, "TOWER LAMP YELLOW")]
        public bool TOWER_LAMP_YELLOW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x002, "TOWER LAMP GREEN")]
        public bool TOWER_LAMP_GREEN { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x003, "RESET S/W")]
        public bool RESET_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x004, "LIFT PIN UP S/W")]
        public bool LIFT_PIN_UP_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x005, "LIFT PIN DOWN S/W")]
        public bool LIFT_PIN_DOWN_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x006, "FILM CLAMP UP S/W")]
        public bool FILM_CLAMP_UP_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x007, "FILM CLAMP DOWN S/W")]
        public bool FILM_CLAMP_DOWN_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x008, "ROLL CLAMP UP S/W")]
        public bool ROLL_CLAMP_UP_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x009, "ROLL CLAMP DOWN S/W")]
        public bool ROLL_CLAMP_DOWN_SW { get => this.ReadY(); set => this.WriteY(value); }

        //[IOSetting(OUT, OA + 0x00A, "ROLL GAP UP S/W")]
        //public bool ROLL_GAP_UP_SW { get => this.ReadY(); set => this.WriteY(value); }

        //[IOSetting(OUT, OA + 0x00B, "ROLL GAP DOWN S/W")]
        //public bool ROLL_GAP_DOWN_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x00A, "VACUUM 06 S/W")]
        public bool VACUUM_INCH_06_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x00B, "VACUUM 08 S/W")]
        public bool VACUUM_INCH_08_SW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x00C, "EQUIPMENT LAMP ON")]
        public bool EQ_LAMP_ON { get => this.ReadY(); set => this.WriteY(value); }

        //[IOSetting(OUT, OA + 0x00D, "전장박스 LAMP ON")]
        //public bool ELEC_BOX_LAMP_ON { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x00E, "BUZZER #1")]
        public bool BUZZER_01 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x00F, "BUZZER #2")]
        public bool BUZZER_02 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x010, "FRONT DOOR LOCK #1")]
        public bool DOOR_LOCK_01 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x011, "RIGHT DOOR LOCK #2")]
        public bool DOOR_LOCK_02 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x012, "RIGHT DOOR LOCK #3")]
        public bool DOOR_LOCK_03 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x013, "BACK DOOR LOCK #4")]
        public bool DOOR_LOCK_04 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x014, "BACK DOOR LOCK #5")]
        public bool DOOR_LOCK_05 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x015, "LEFT DOOR LOCK #6")]
        public bool DOOR_LOCK_06 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x016, "LEFT DOOR LOCK #7")]
        public bool DOOR_LOCK_07 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x017, "MC ON")]
        public bool MC_ON { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x018, "ION BLOWER RUN")]
        public bool ION_BLOWER_RUN { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x019, "ION BLOWER TIP CLENING")]
        public bool ION_BLOWER_TIP_CLENING { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x01A, "UV LAMP RUN")]
        public bool UV_LAMP_RUN { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x01B, "LOADCELL ZERO")]
        public bool LOADCELL_ZERO { get => this.ReadY(); set => this.WriteY(value); }

        //[IOSetting(OUT, OA + 0x01C, "HEATER ON")]
        //public bool HEATER_ON { get => this.ReadY(); set => this.WriteY(value); }
    }

    public partial class InOutManager
    {

        [IOSetting(OUT, OA + 0x020, "ROLL GAP RIGHT UP")]
        public bool ROLL_GAP_RIGHT_UP { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x021, "ROLL GAP RIGHT DOWN")]
        public bool ROLL_GAP_RIGHT_DOWN { get => this.ReadY(); set => this.WriteY(value); }
        [IOSetting(OUT, OA + 0x022, "ROLL GAP LEFT UP")]
        public bool ROLL_GAP_LEFT_UP { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x023, "ROLL GAP LEFT DOWN")]
        public bool ROLL_GAP_LEFT_DOWN { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x024, "LIFT PIN UP")]
        public bool LIFT_PIN_UP { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x025, "LIFT PIN DOWN")]
        public bool LIFT_PIN_DOWN { get => this.ReadY(); set => this.WriteY(value); }
        
        [IOSetting(OUT, OA + 0x026, "UV CYLINDERE UP")]
        public bool UV_CYLINDER_UP { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x027, "UV CYLINDERE DOWN")]
        public bool UV_CYLINDER_DOWN { get => this.ReadY(); set => this.WriteY(value); }
        
        [IOSetting(OUT, OA + 0x028, "FILM CLAMP UP")]
        public bool FILM_CLAMP_UP { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x029, "FILM CLAMP DOWN")]
        public bool FILM_CLAMP_DOWN { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x02A, "ROLL CLAMP UP")]
        public bool ROLL_CLAMP_UP { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x02B, "ROLL CLAMP DOWN")]
        public bool ROLL_CLAMP_DOWN { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x02C, "VACUUM 06 INCH ON")]
        public bool VACUUM_ON_06 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x02D, "VACUUM 06 INCH OFF")]
        public bool VACUUM_OFF_06 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x02E, "VACUUM 08 INCH ON")]
        public bool VACUUM_ON_08 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x02F, "VACUUM 08 INCH OFF")]
        public bool VACUUM_OFF_08 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x030, "UV LAMP COLLING")]
        public bool UV_LAMP_COLLING { get => this.ReadY(); set => this.WriteY(value); }
    }
}
