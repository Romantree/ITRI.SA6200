using GIGA.ITRI.SA6200.UI.Managers;
using GIGA.ITRI.SA6200.UI.Models.Setup;
using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Utils;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Setup
{
    public class SetupInOutViewMdoel : ISetupViewModel
    {
        private readonly ContentControl _view = new SetupInOutView();
        private Dictionary<int, InOutModel[]> inList = new Dictionary<int, InOutModel[]>();
        private Dictionary<int, InOutModel[]> outList = new Dictionary<int, InOutModel[]>();
        private int inPage = 0;
        private int outPage = 0;

        private int inMaxPage = 0;
        private int outMaxPage = 0;

        public override int No => 1;

        public override string Name => "I/O";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_source_fork", AP.ICON_RESOURCE);

        public InOutModel[] In { get => this.GetValue<InOutModel[]>(); set => this.SetValue(value); }

        public InOutModel[] Out { get => this.GetValue<InOutModel[]>(); set => this.SetValue(value); }

        public NormalCommand OnTypeChangedCmd => new NormalCommand(OnTypeChanged);

        public NormalCommand OnDataOnOffCmd => new NormalCommand(OnDataOnOff);

        public override void Init()
        {
            try
            {
                base.Init();

                this.InitPage();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Update()
        {
            try
            {
                base.Update();

                this.UpdateIn();
                this.UpdateOut();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnNotifyCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "IN_P":
                        {
                            if (this.inPage == 0) return;

                            this.inPage--;
                            this.UpdateInPage();
                        }
                        break;
                    case "IN_N":
                        {
                            if (this.inPage == this.inMaxPage) return;

                            this.inPage++;
                            this.UpdateInPage();
                        }
                        break;
                    case "OUT_P":
                        {
                            if (this.outPage == 0) return;

                            this.outPage--;
                            this.UpdateOutPage();
                        }
                        break;
                    case "OUT_N":
                        {
                            if (this.outPage == this.outMaxPage) return;

                            this.outPage++;
                            this.UpdateOutPage();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void InitPage()
        {
            try
            {
                foreach (var item in this.ToList(AP.IO.CreateIOList(eIOType.IN, 0, 2), AP.IO.inList).ToPageList(32))
                {
                    this.inList.Add(inPage++, item.ToArray());
                }
                this.inMaxPage = inPage - 2;
                this.inPage = 0;
                this.UpdateInPage();

                foreach (var item in this.ToList(AP.IO.CreateIOList(eIOType.OUT, 3, 2), AP.IO.outList).ToPageList(32))
                {
                    this.outList.Add(outPage++, item.ToArray());
                }

                this.outMaxPage = outPage - 2;
                this.outPage = 0;
                this.UpdateOutPage();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateInPage()
        {
            try
            {
                lock (this.inList)
                {
                    this.In = this.inList[inPage];
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateOutPage()
        {
            try
            {
                lock (this.outList)
                {
                    this.Out = this.outList[outPage];
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateIn()
        {
            try
            {
                lock (this.inList)
                {
                    foreach (var item in this.In)
                    {
                        item.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateOut()
        {
            try
            {
                lock (this.outList)
                {
                    foreach (var item in this.Out)
                    {
                        item.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void OnTypeChanged(object param)
        {
            try
            {
                var model = param as InOutModel;
                if (model == null || string.IsNullOrEmpty(model.Name)) return;

                model.data.SignalType = model.IsAType ? eSignalType.B : eSignalType.A;

                AP.IO.CreateFile();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void OnDataOnOff(object param)
        {
            try
            {
                var model = param as InOutModel;
                if (model == null || string.IsNullOrEmpty(model.Name)) return;

                AP.IO.WriteY(!model.OnOff, model.Key);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public IEnumerable<InOutModel> ToList(List<IOData> aList, Dictionary<string, IOData> bList)
        {
            foreach (var item in aList.GroupJoin(bList, a => a.Address, b => b.Value.Address, (a, b) => new { a, b }))
            {
                var b = item.b.FirstOrDefault();
                if (b.Value != null) yield return new InOutModel(b.Key, b.Value);
                else yield return item.a;
            }
        }
    }
}
