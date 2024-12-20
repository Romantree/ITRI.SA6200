using System;
using TS.FW;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Service
{
    public class CylinderModel : ModelBase
    {
        private readonly CylinderUnit _unit;

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string Status { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool Up { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Down { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string UpText { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string DownText { get => this.GetValue<string>(); set => this.SetValue(value); }

        public NormalCommand OnUpCmd => new NormalCommand((t) => AP.IO.SetCylinder(_unit, CylinderAction.UP));

        public NormalCommand OnDownCmd => new NormalCommand((t) => AP.IO.SetCylinder(_unit, CylinderAction.DOWN));

        public CylinderModel(CylinderUnit unit)
        {
            this._unit = unit;
            this.Init();
        }

        private void Init()
        {
            try
            {
                this.Name = $"{_unit}".Replace("_", " ");

                if (_unit == CylinderUnit.FILM_CLAMP || _unit == CylinderUnit.ROLL_CLAMP)
                {
                    UpText = "UNCLAMP";
                    DownText = "CLAMP";
                }
                else
                {
                    UpText = "UP";
                    DownText = "DOWN";
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Update()
        {
            try
            {
                this.Up = AP.IO.GetCylinder(_unit, CylinderAction.UP);
                this.Down = AP.IO.GetCylinder(_unit, CylinderAction.DOWN);

                if(this.Up == true && this.Down == false)
                {
                    this.Status = this.UpText;
                }
                else if (this.Up == false && this.Down == true)
                {
                    this.Status = this.DownText;
                }
                else if (this.Up == false && this.Down == false)
                {
                    this.Status = "NONE";
                }
                else
                {
                    this.Status = "ERROR";
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
