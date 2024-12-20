using System;
using System.Linq;
using System.Windows.Forms;
using TS.FW;
using TS.FW.Device;
using TS.FW.Device.Ajin;
using TS.FW.Device.Dto.Analog;
using TS.FW.Device.Simulation;
using TS.FW.Helper;

namespace GIGA.ITRI.SA6200.UI.Managers
{
    public partial class DeviceManager
    {
        public IAxis this[eAxis axis] => this.Axis[axis];

        public IDevice Ins { get; private set; }

        public IAxisModule Axis => this.Ins as IAxisModule;

        public IDInOut IO => (this.Ins as IDInOutModule).IO;

        public IAnalog Analog => (this.Ins as IAnalogModule).Al;

        public DeviceManager()
        {
            if (AP.IsSim)
            {
                this.Ins = new SimulationDevice();
            }
            else
            {
                this.Ins = new AjinDevice();
            }
        }

        public Response Open()
        {
            try
            {
                var res = this.Ins.Open();

                if (res == true && this.Ins is AjinDevice)
                {
                    var load = (this.Ins as AjinDevice).LoadMotionFile(AP.MOT_FILE);
                    if (load == false) return load;
                }

                return res;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }

    public partial class DeviceManager
    {
        public const double MAX_STAGE_X = 700;
        public const double MIN_DEMOLD = 0;
        public const double MAX_DEMOLD = 360;

        private HomeAsyncResult StageX;
        private HomeAsyncResult RollGap_02;
        private HomeAsyncResult RollGap_01;
        private HomeAsyncResult Demold;

        public bool IsServoOnAll => this.Axis.All(t => t.IsServoOn == true);

        public bool IsAlarmAll => this.Axis.Any(t => t.IsAlarm == true);

        public bool IsBusyAll => this.Axis.Any(t => t.IsBusy == true);

        public void InitScale()
        {
            if (AP.IsSim) return;

            this.ToAjin(eAxis.StageX).Setting.SCALE = 0.001;
            this.ToAjin(eAxis.StageXSlave).Setting.SCALE = 0.001;

            SetRollPressScale(DB.MotParam.MotScale.RollGap);
            SetDemoldScale(DB.MotParam.MotScale.Demold);
        }

        public void SetRollPressScale(double scale)
        {
            var data = scale <= 0 ? 0.001 : Math.Round(0.001D / scale, 10);

            Logger.Write(this, $"Roll Gap Press Scale : {scale} = {data}");

            if (AP.IsSim) return;

            this.ToAjin(eAxis.RollGapLeft).Setting.SCALE = data;
            this.ToAjin(eAxis.RollGapRight).Setting.SCALE = data;
        }

        public void SetDemoldScale(double scale)
        {
            var data = scale <= 0 ? 0.001 : Math.Round(0.001D / scale, 10);

            Logger.Write(this, $"Demold Scale : {scale} = {data}");

            if (AP.IsSim) return;

            this.ToAjin(eAxis.Demold).Setting.SCALE = data;
        }

        public AjinAxis ToAjin(eAxis axis) => this[axis] as AjinAxis;

        public void ServoOn()
        {
            this.ServoOn(EnumHelper.Axis.ToArray());
        }

        public void ServoOn(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsServoOn == true) continue;

                    item.IsServoOn = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public bool IsServoOn(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsServoOn == false) return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return false;
            }
        }

        public void ResetAlarm()
        {
            this.ResetAlarm(EnumHelper.Axis.ToArray());
        }

        public void ResetAlarm(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsAlarm == false) continue;

                    item.ResetAlarm();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public bool IsAlarm(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsAlarm == true) return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return true;
            }
        }

        public void Stop()
        {
            this.Stop(EnumHelper.Axis.ToArray());
        }

        public void Stop(params eAxis[] list)
        {
            try
            {
                foreach (var item in list)
                {
                    this[item].Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Home(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return;

            foreach (var item in axis)
            {
                switch (item)
                {
                    case eAxis.StageX:
                        this[item].HomeAsync(out StageX);
                        break;
                    case eAxis.RollGapLeft:
                        this[item].HomeAsync(out RollGap_02);
                        break;
                    case eAxis.RollGapRight:
                        this[item].HomeAsync(out RollGap_01);
                        break;
                    case eAxis.Demold:
                        this[item].HomeAsync(out Demold);
                        break;
                }
            }
        }

        public void HomeStop(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return;

            foreach (var item in axis)
            {
                var home = this.ToHomeResult(item);
                home.IsStop = true;
            }
        }

        public bool HomeSuccess(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return false;

            foreach (var item in axis)
            {
                var home = this.ToHomeResult(item);

                if (home.Complete == false || home.Success == false) return false;
            }

            return true;
        }

        public bool HomeFail(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return false;

            foreach (var item in axis)
            {
                var home = this.ToHomeResult(item);

                if (home.Complete == true && home.Success == false) return true;
            }

            return false;
        }

        private HomeAsyncResult ToHomeResult(eAxis axis)
        {
            switch (axis)
            {
                case eAxis.StageX: return StageX;
                case eAxis.RollGapLeft: return RollGap_02;
                case eAxis.RollGapRight: return RollGap_01;
                case eAxis.Demold: return Demold;
            }

            return null;
        }

        public string HomeComment(eAxis axis) => this.ToHomeResult(axis).Comment;

        public bool Gantry(eAxis axis, bool gantry)
        {
            if (AP.IsSim) return gantry;

            return this.ToAjin(axis).Gantry == gantry;
        }

        public bool IsBusy(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return false;

            foreach (var type in axis)
            {
                if (this[type].IsBusy == false) return false;
            }

            return true;
        }

        public bool CheckPos(eAxis axis, double pos, double gap = 0.01)
        {
            var item = this[axis];
            if (item.IsBusy == true) return false;

            var curPos = AP.IsSim ? item.ActPosition : item.ComPosition;

            return item.IsBusy == false && curPos.CheckPosition(pos, gap);
        }

        public Response GantryEnable(eAxis master, eAxis slave)
        {
            try
            {
                if (AP.IsSim) return new Response();

                var masterAxis = this.ToAjin(master);

                if (masterAxis.Gantry == true) return new Response();

                var res = masterAxis.GantryEnable(this.ToAjin(slave));
                if (res == false) throw new Exception(res.Comment);

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public Response GantryDisable(eAxis master, eAxis slave)
        {
            try
            {
                if (AP.IsSim) return new Response();

                var masterAxis = this.ToAjin(master);

                if (masterAxis.Gantry == false) return new Response();

                var res = masterAxis.GantryDisable(this.ToAjin(slave));
                if (res == false) throw new Exception(res.Comment);

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }
    }

    public partial class DeviceManager
    {
        public double ToDemoldPosition(double stageX)
        {
            return AnalogRange.ToDigit(stageX, 0, MAX_STAGE_X, MAX_DEMOLD);
        }

        public double ToDemoldSpped(double demold, double stageX, double spd)
        {
            var dX = ToDis(eAxis.StageX, stageX);
            var dd = ToDis(eAxis.Demold, demold);

            var sec = dX / spd;

            return dd / sec;
        }

        public double ToDis(eAxis axis, double target)
        {
            var cur = this[axis].ActPosition;

            return Math.Sqrt(Math.Pow(cur - target, 2));
        }
    }
}
