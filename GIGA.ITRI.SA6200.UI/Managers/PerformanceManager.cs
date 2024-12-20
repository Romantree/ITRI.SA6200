using System;
using System.Collections.Generic;
using System.Linq;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Managers
{
    public class PerformanceManager
    {
        private const double MB = 1024 * 1024;
        private const double UNIT = 25;

        private readonly TS.FW.Diagnostics.BackgroundTimer _trUpdate = new TS.FW.Diagnostics.BackgroundTimer();
        private readonly List<double> _mList = new List<double>();

        private System.Diagnostics.PerformanceCounter _mem = null;
        private double _maxMemory = 0;

        public double Memory { get; private set; }

        public PerformanceManager()
        {
            this._trUpdate.SleepTimeMsc = 100;
            this._trUpdate.DoWork += _trUpdate_DoWork;
        }

        public void Start()
        {
            try
            {
                var name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                this._mem = new System.Diagnostics.PerformanceCounter("Process", "Working Set - Private", name);

                this._trUpdate.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void _trUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                this._mList.Add(this._mem.NextValue());

                if (this._mList.Count < 10) return;

                this.Memory = this._mList.Average() / MB;
                this._mList.Clear();

                var value = this.ToMemory(this.Memory, UNIT);
                if (value <= this._maxMemory) return;

                this._maxMemory = value;

                Logger.Write(this, string.Format("메모리 {0:f3} MB", this.Memory), Logger.LogEventLevel.Error);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public double ToMemory(double memory, double unit)
        {
            return memory - (memory % unit);
        }
    }
}
