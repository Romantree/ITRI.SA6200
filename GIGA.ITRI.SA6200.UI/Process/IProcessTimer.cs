using GIGA.ITRI.SA6200.UI.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using TS.FW;
using TS.FW.Dac.Alarm;
using TS.FW.Diagnostics;
using TS.FW.Helper;

namespace GIGA.ITRI.SA6200.UI.Process
{
    public abstract partial class IProcessTimer : BackgroundTimer
    {
        private const int SLEEP_TIME = 50;
        private static readonly List<IProcessTimer> _procList = new List<IProcessTimer>();

        protected readonly eAxis[] _totalAxis = new eAxis[] { eAxis.StageX, eAxis.StageXSlave, eAxis.RollGapRight, eAxis.RollGapLeft, eAxis.Demold };
        protected readonly eAxis[] _moveAxis = new eAxis[] { eAxis.StageX, eAxis.RollGapRight, eAxis.RollGapLeft, eAxis.Demold };

        protected static DeviceManager Device => AP.Device;
        protected static InOutManager IO => AP.IO;        
        protected static NetManager Net => AP.Net;

        private readonly Stopwatch _watch = new Stopwatch();
        protected string _procMsg = string.Empty;

        public string StepMsg { get; protected set; }

        public double WatchTime => this._watch.Elapsed.TotalSeconds;

        public abstract string Name { get; }

        public IProcessTimer(bool isManagement)
        {
            if (isManagement && _procList.Contains(this) == false) _procList.Add(this);

            this.SleepTimeMsc = SLEEP_TIME;
            this.DoWork += IProcessTimer_DoWork;
        }

        public override void Stop()
        {
            this.TimeStop();
            this._procMsg = string.Empty;
            base.Stop();
        }

        private void IProcessTimer_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (this.IsPause) return;

                this.DoWorkCallback();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected abstract void DoWorkCallback();

        protected void SetProcMsg(string format, params object[] arg) => this.SetProcMsg(string.Format(format, arg));

        protected void SetProcMsg(string msg)
        {
            if (this._procMsg == msg) return;

            try
            {
                Logger.CustomWrite(this.Name, this, msg, Logger.LogEventLevel.Information);

                AP.Event.ProcessMsgEvent(this.Name, msg);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                this._procMsg = msg;
            }
        }

        protected void TimeStart() => this._watch.Restart();

        protected void TimeStop() => this._watch.Stop();

        protected bool Timeout(int time)
        {
            if (time <= 0) time = 30000;

            return this._watch.TimeOutCheck(time);
        }

        protected bool Timeout(double time) => this.Timeout(Convert.ToInt32(time * 1000));

        protected void Sleep(int time)
        {
            if (time <= 0) return;

            this.SetProcMsg("Delay Time {0} msec", time);

            System.Threading.Thread.Sleep(time);
        }

        protected void Sleep(double time)
        {
            this.Sleep(Convert.ToInt32(time * 1000));
        }

        public StepResult AlarmPort(eAlarm alarm, object item = null)
        {
            try
            {
                //this.SetProcMsg($"Alarm : {alarm}");

                var level = AP.Alarm.AlarmPost(alarm, this.Recovery, item);
                if (level == AlarmLevel.Light || level == AlarmLevel.CycleStop) return StepResult.Next;
                if (level == AlarmLevel.Heavy) return StepResult.Alarm;

                return StepResult.Stop;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);

                return StepResult.Error;
            }
        }

        protected abstract void Recovery(AlarmData<eAlarm> model);

        public static void Abort()
        {
            foreach (var item in _procList)
            {
                item.Stop();
            }
        }

        public static void PauseAll()
        {
            foreach (var item in _procList)
            {
                item.Pause();
            }
        }

        public static void ResumeAll()
        {
            foreach (var item in _procList)
            {
                item.Resume();
            }
        }
    }
}