﻿namespace GIGA.ITRI.SA6200.UI
{
    public enum eAlarm
    {
        EMO_01,
        EMO_02,
        EMO_03,
        EMO_04,

        DOOR_OPEN_01,
        DOOR_OPEN_02,
        DOOR_OPEN_03,
        DOOR_OPEN_04,
        DOOR_OPEN_05,
        DOOR_OPEN_06,
        DOOR_OPEN_07,

        MAIN_CDA_PRESSURE_01,
        MAIN_CDA_PRESSURE_02,

        ION_BLOWER_ALARM,

        UV_LAMP_NOT_READY,
        UV_LAMP_ERROR,
        UV_LED_ON_OFF_TIMEOUT,
        UV_LED_SET_POWER_TIMEOUT,        

        //HEATER_OVERLOAD_ALARM,
        //HEATER_TEMP_ALARM,
        //HEATER_TEMP_ERROR,
        //HEATER_SETTING_TIMEOUT,
        //HEATER_ON_OFF_TIMEOUT,
        //HEATER_TEMP_CHECK_TIMEOUT,

        REGULATOR_SETTING_TIMEOUT,

        MOTION_STAGE_X_SERVO_OFF,
        MOTION_STAGE_X_SLAVE_SERVO_OFF,
        MOTION_GAP_PRESS_LEFT_SERVO_OFF,
        MOTION_GAP_PRESS_RIGHT_SERVO_OFF,
        MOTION_STAGE_Z_SERVO_OFF,

        MOTION_STAGE_X_ALARM,
        MOTION_STAGE_X_SLAVE_ALARM,
        MOTION_GAP_PRESS_LEFT_ALARM,
        MOTION_GAP_PRESS_RIGHT_ALARM,
        MOTION_STAGE_Z_ALARM,

        MOTION_SERVO_ON_TIMEOUT,
        MOTION_ALARM_RESET_TIMEOUT,
        MOTION_HOME_TIMEOUT,
        MOTION_HOME_FAIL,
        MOTION_GANTRY_TIMEOUT,

        MOTION_MOVE_TIMEOUT,
        MOTION_STAGE_X_MOVE_TIMEOUT,
        MOTION_GAP_PRESS_LEFT_MOVE_TIMEOUT,
        MOTION_GAP_PRESS_RIGHT_MOVE_TIMEOUT,
        MOTION_DEMOLD_MOVE_TIMEOUT,               

        LIFT_PIN_UP_DOWN_TIMEOUT,
        ROLL_GAP_UP_DOWN_TIMEOUT,
        FILM_CLAMP_UP_DOWN_TIMEOUT,
        ROLL_CLAMP_UP_DOWN_TIMEOUT,
        UV_CYLINDER_UP_DOWN_TIMEOUT,

        VACUUM_TIMEOUT,
        VENT_TIMEOUT,

        MC_OFF,
    }
}