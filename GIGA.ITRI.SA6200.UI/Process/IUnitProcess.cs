using System;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Process
{
    public abstract class IUnitProcess<T> : IProcessTimer where T : struct, IConvertible
    {
        public ProcessStep<T> Step { get; protected set; } = new ProcessStep<T>();

        public IUnitProcess(bool isManagement) : base(isManagement) { }

        public override void Start()
        {

            if (this.IsBusy == true) return;

            this.Step.Init();
            base.Start();
        }

        public override void Resume()
        {
            try
            {
                if (this.Step.Contains("POLLING")) this.Step.Prev();

                base.Resume();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void Finish() => this.Stop();

        protected override void DoWorkCallback()
        {
            try
            {
                var res = this.Step.ExcuteProcess(this, null);

                switch (res)
                {
                    case StepResult.Next:
                        {
                            this.Step.Next();
                        }
                        break;
                    case StepResult.Alarm:
                        {
                            this.SetProcMsg($"Pause : {this.Step.CurrentStep}");
                            this.Pause();
                        }
                        break;
                    case StepResult.Not_Found_Step:
                    case StepResult.Error:
                    case StepResult.Stop:
                        {
                            this.SetProcMsg($"{res} : {this.Step.CurrentStep}");
                            this.Stop();
                        }
                        break;
                    case StepResult.Finish:
                        {
                            this.Stop();
                        }
                        break;
                    case StepResult.Jump:
                        {
                            this.Step.Next(2);
                        }
                        break;
                }

                if (AP.IsSim) System.Threading.Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
