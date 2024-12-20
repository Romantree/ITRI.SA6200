using GIGA.ITRI.SA6200.UI.Subscribe;
using GIGA.ITRI.SA6200.UI.Views.Page.Utilty;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.Helper;

namespace GIGA.ITRI.SA6200.UI.ViewModels.Page.Utilty
{
    public class UtLogFileViewModel : IUtilViewModel
    {
        private readonly ContentControl _view = new UtLogFileView();

        public override int No => 1;

        public override string Name => "Logger";

        public override ContentControl View => this._view;

        public override Visual Visual => ResourceHelper.ToVisual("appbar_calendar_year", AP.ICON_RESOURCE);

        public DateTime StartTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public DateTime EndTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public ObservableCollection<LogFileModel> LogList { get; set; } = new ObservableCollection<LogFileModel>();

        public string LogMsg { get => this.GetValue<string>(); set => this.SetValue(value); }

        public NormalCommand OnLogSelectedCmd => new NormalCommand(LogSelectedCmd);

        public override void Init()
        {
            try
            {
                base.Init();

                this.StartTime = DateTime.Now.Date;
                this.EndTime = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void LogSelectedCmd(object param)
        {
            try
            {
                var model = param as LogFileModel;
                if (model == null) return;

                if (File.Exists(model.FilePath) == false) return;

                this.LogMsg = File.ReadAllText(model.FilePath, Encoding.Default);
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
                    case "SEARCH":
                        {
                            this.UpdateLog();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateLog()
        {
            try
            {
                this.LogList.Clear();
                var total = (int)Math.Ceiling((this.EndTime - this.StartTime).TotalDays + 1);

                for (int i = 0; i < total; i++)
                {
                    var time = this.StartTime.AddDays(i);
                    var path = Path.Combine(Logger.RootPath, time.ToString("yyyy"), time.ToString("MM"), time.ToString("dd"));
                    if (Directory.Exists(path) == false) continue;

                    foreach (var file in Directory.EnumerateFiles(path, "*.log"))
                    {
                        if (File.Exists(file) == false) continue;

                        var name = Path.GetFileNameWithoutExtension(file);
                        var tiem = time.Date.ToString("yyyy-MM-dd");

                        this.LogList.Add(new LogFileModel()
                        {
                            FilePath = file,
                            Name = name,
                            Time = tiem,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }

    public class LogFileModel : ModelBase
    {
        public string FilePath { get; set; }

        public string Name { get; set; }

        public string Time { get; set; }
    }
}
