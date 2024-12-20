using GIGA.ITRI.SA6200.UI.Models.Recipe;
using GIGA.ITRI.SA6200.UI.Process;
using GIGA.ITRI.SA6200.UI.Process.FilmLoading;
using GIGA.ITRI.SA6200.UI.Process.Init;
using GIGA.ITRI.SA6200.UI.Process.Manual;
using GIGA.ITRI.SA6200.UI.Process.Saferty;
using GIGA.ITRI.SA6200.UI.Process.Work;
using System;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Managers
{
    public class ProcManager
    {
        public readonly InitProc Init = new InitProc();
        public readonly SafertyProc Saferty = new SafertyProc();
        public readonly ImprintProcess Imprint = new ImprintProcess();
        public readonly VentProc Vac = new VentProc();
        private readonly FilmLoadProc FilmLoading = new FilmLoadProc();

        public bool IsAuto { get; set; } = false;

        public bool IsInit => Init.Init;

        public bool IsBusy => Init.IsBusy || Imprint.IsBusy | FilmLoading.IsBusy | Vac.IsBusy;

        public void InitReset() => Init.Init = false;

        public void StartAuto(MainRecipeModel rcp)
        {
            try
            {
                if (ProcessCheck(rcp)) return;

                var ready = DB.MotParam.StageXReady;

                if (AP.Device[eAxis.StageX].ActPosition > ready.Position + 1)
                {
                    AP.Event.InterlockMsgEvent("Cannot start from the current position.\\r\\nPlease move to the Ready position and try again");
                    return;
                }

                if (this.IsAuto == false)
                {
                    AP.Event.InterlockCheckEvent(" Please switch to Auto mode and try again.");
                    return;
                }

                this.Imprint.SetWorkItem(new WorkItem(rcp));
                this.Imprint.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void StartManual(MainRecipeModel rcp, bool isImprint)
        {
            this.StartManual(rcp, isImprint ? WorkMode.IMPRINT : WorkMode.DEMOLD);
        }


        public void StartManual(MainRecipeModel rcp, WorkMode mode)
        {
            try
            {
                if (ProcessCheck(rcp)) return;

                var ready = DB.MotParam.StageXReady;

                switch (mode)
                {
                    case WorkMode.AUTO:
                    case WorkMode.IMPRINT:
                        {
                            if (AP.Device[eAxis.StageX].ActPosition > ready.Position + 1)
                            {
                                AP.Event.InterlockMsgEvent("Cannot start from the current position.\\r\\nPlease move to the Ready position and try again");
                                return;
                            }
                        }
                        break;
                    case WorkMode.DEMOLD:
                        {
                            if (AP.Device[eAxis.StageX].ActPosition < ready.Position - 1)
                            {
                                AP.Event.InterlockMsgEvent("Cannot start from the current position.\\r\\nPlease move to the Ready position and try again.");
                                return;
                            }
                        }
                        break;
                }

                if (this.IsAuto == true)
                {
                    AP.Event.InterlockCheckEvent("Please change to Manual mode and try again.");
                    return;
                }

                this.Imprint.SetWorkItem(new WorkItem(rcp, mode));
                this.Imprint.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }


        public void StartFilmLoading(MainRecipeModel rcp, FilmLoadingMdoe mode)
        {
            try
            {
                if (ProcessCheck(rcp)) return;

                this.FilmLoading.Mode = mode;
                this.FilmLoading.Rcp = rcp;
                
                this.FilmLoading.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void StartVent(VacuumUnit unit)
        {
            try
            {
                if (Vac.IsBusy) return;

                Vac.Unit = unit;
                Vac.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private bool ProcessCheck(MainRecipeModel rcp)
        {
            if (AP.Alarm.IsAlarmNotLight == true)
            {
                AP.Event.InterlockMsgEvent("Please disable the alarm and try again.");
                return true;
            }

            if (this.IsInit == false)
            {
                AP.Event.InterlockMsgEvent("Please initialize the process and try again.");
                return true;
            }

            if (this.IsBusy == true)
            {
                AP.Event.InterlockMsgEvent("Please stop the process and try again.");
                return true;
            }

            if (rcp == null)
            {
                AP.Event.InterlockMsgEvent("The recipe information does not exist.");
                return true;
            }

            return false;
        }

        public void Abort()
        {
            IProcessTimer.Abort();
        }
    }
}
