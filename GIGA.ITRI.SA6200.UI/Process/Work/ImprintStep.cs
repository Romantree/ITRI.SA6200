﻿namespace GIGA.ITRI.SA6200.UI.Process.Work
{
    public enum ImprintStep
    {
        START,

        MOT_GANTRY_ENABLE_ENTER,
        MOT_GANTRY_ENABLE_POLLING,

        UV_LAMP_OFF_ENTER,
        UV_LAMP_OFF_POLLING,

        FILM_CLAMP_DOWN_ENTER,
        FILM_CLAMP_DOWN_POLLING,

        ROLL_CLAMP_DOWN_ENTER,
        ROLL_CLAMP_DOWN_POLLING,

        LIFT_PIN_DOWN_ENTER,
        LIFT_PIN_DOWN_POLLING,

        VACUUN_ON_ENTER,
        VACUUN_ON_POLLING,

        MODE_CHECK,

        IMP_START,

        IMP_REG_SETTING_ENTER,
        IMP_REG_SETTING_POLLING,

        IMP_UV_LAMP_POWER_ENTER,
        IMP_UV_LAMP_POWER_POLLING,

        IMP_MOT_STAGE_READY_ENTER,
        IMP_MOT_STAGE_READY_POLLING,

        IMP_UV_DOWN_ENTER,
        IMP_UV_DOWN_POLLING,

        IMP_GAP_PRESS_DOWN_ENTER,
        IMP_GAP_PRESS_DOWN_POLLING,

        IMP_MOT_GAP_PRESS_ENTER,
        IMP_MOT_GAP_PRESS_POLLING,

        IMP_MOT_STAGE_SIZE_ENTER,
        IMP_MOT_STAGE_SIZE_POLLING,

        IMP_UV_LAMP_ON_ENTER,
        IMP_UV_LAMP_ON_POLLING,

        IMP_MOT_STAGE_ENTER,
        IMP_MOT_STAGE_POLLING,

        IMP_UV_LAMP_OFF_ENTER,
        IMP_UV_LAMP_OFF_POLLING,

        IMP_END,

        DE_START,

        DE_REG_SETTING_ENTER,
        DE_REG_SETTING_POLLING,

        DE_UV_LAMP_POWER_ENTER,
        DE_UV_LAMP_POWER_POLLING,

        DE_GAP_PRESS_DOWN_ENTER,
        DE_GAP_PRESS_DOWN_POLLING,

        DE_MOT_GAP_PRESS_ENTER,
        DE_MOT_GAP_PRESS_POLLING,

        DE_UV_LAMP_ON_ENTER,
        DE_UV_LAMP_ON_POLLING,
        
        DE_MOT_STAGE_ENTER,
        DE_MOT_STAGE_POLLING,
        
        DE_UV_LAMP_OFF_ENTER,
        DE_UV_LAMP_OFF_POLLING,

        DE_MOT_STAGE_READY_ENTER,
        DE_MOT_STAGE_READY_POLLING,

        //DE_MOT_GAP_HOME_ENTER,
        //DE_MOT_GAP_HOME_POLLING,

        DE_UV_UP_ENTER,
        DE_UV_UP_POLLING,

        DE_END,

        END,
    }
}