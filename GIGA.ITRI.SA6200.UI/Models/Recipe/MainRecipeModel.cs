using GIGA.ITRI.SA6200.UI.Configs;
using System;
using System.Runtime.Serialization;
using TS.FW;
using TS.FW.Wpf.Controls.Pu;
using TS.FW.Wpf.Core;

namespace GIGA.ITRI.SA6200.UI.Models.Recipe
{
    [DataContract]
    public class MainRecipeModel : IRecipeModel
    {
        [DataMember]
        public VacuumUnit VacuumUnit { get => this.GetValue<VacuumUnit>(); set => this.SetValue(value); }

        [DataMember]
        public StageDataModel Imprint { get => this.GetValue<StageDataModel>(); set => this.SetValue(value); }

        [DataMember]
        public StageDataModel Demold { get => this.GetValue<StageDataModel>(); set => this.SetValue(value); }

        public NormalCommand OnSetValueCmd => new NormalCommand(SetValueCmd);

        public MainRecipeModel() : base(RecipeType.MAIN)
        {
            this.Imprint = new StageDataModel();
            this.Demold = new StageDataModel();
        }

        private void SetValueCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "IMP_X_P":
                        {
                            this.CehckPos(this.Imprint.Stage, DB.MotParam.StageXLimit);
                        }
                        break;
                    case "IMP_X_S":
                        {
                            this.CehckSpeed(this.Imprint.Stage, DB.MotParam.StageXLimit);
                        }
                        break;
                    case "IMP_D_P":
                        {
                            this.CehckPos(this.Imprint.Demold, DB.MotParam.DemoldLimit);
                        }
                        break;
                    case "IMP_D_S":
                        {
                            this.CehckSpeed(this.Imprint.Demold, DB.MotParam.DemoldLimit);
                        }
                        break;
                    case "IMP_L_P":
                        {
                            this.Imprint.GapLeft = this.CehckPos(this.Imprint.GapLeft, DB.MotParam.GapPressLimit);
                        }
                        break;
                    case "IMP_R_P":
                        {
                            this.Imprint.GapRight = this.CehckPos(this.Imprint.GapRight, DB.MotParam.GapPressLimit);
                        }
                        break;
                    case "IMP_RG_L":
                        {
                            var old = this.Imprint.RegulatorLeft;

                            var min = DB.Network.RegulatorOption.Min;
                            var max = DB.Network.RegulatorOption.Max;

                            this.Imprint.RegulatorLeft = (int)GetValue(old, min, max);
                        }
                        break;
                    case "IMP_RG_R":
                        {
                            var old = this.Imprint.RegulatorRight;

                            var min = DB.Network.RegulatorOption.Min;
                            var max = DB.Network.RegulatorOption.Max;

                            this.Imprint.RegulatorRight = (int)GetValue(old, min, max);
                        }
                        break;
                    case "IMP_UV_POWER":
                        {
                            var old = this.Imprint.UvLampPower;

                            var min = 0;
                            var max = 100;

                            this.Imprint.UvLampPower = (int)GetValue(old, min, max);
                        }
                        break;

                    case "DM_X_P":
                        {
                            this.CehckPos(this.Demold.Stage, DB.MotParam.StageXLimit);
                        }
                        break;
                    case "DM_X_S":
                        {
                            this.CehckSpeed(this.Demold.Stage, DB.MotParam.StageXLimit);
                        }
                        break;
                    case "DM_D_P":
                        {
                            this.CehckPos(this.Demold.Demold, DB.MotParam.DemoldLimit);
                        }
                        break;
                    case "DM_D_S":
                        {
                            this.CehckSpeed(this.Demold.Demold, DB.MotParam.DemoldLimit);
                        }
                        break;
                    case "DM_L_P":
                        {
                            this.Demold.GapLeft = this.CehckPos(this.Demold.GapLeft, DB.MotParam.GapPressLimit);
                        }
                        break;
                    case "DM_R_P":
                        {
                            this.Demold.GapRight = this.CehckPos(this.Demold.GapRight, DB.MotParam.GapPressLimit);
                        }
                        break;
                    case "DM_RG_L":
                        {
                            var old = this.Demold.RegulatorLeft;

                            var min = DB.Network.RegulatorOption.Min;
                            var max = DB.Network.RegulatorOption.Max;

                            this.Demold.RegulatorLeft = (int)GetValue(old, min, max);
                        }
                        break;
                    case "DM_RG_R":
                        {
                            var old = this.Demold.RegulatorRight;

                            var min = DB.Network.RegulatorOption.Min;
                            var max = DB.Network.RegulatorOption.Max;

                            this.Demold.RegulatorRight = (int)GetValue(old, min, max);
                        }
                        break;
                    case "DM_UV_POWER":
                        {
                            var old = this.Demold.UvLampPower;

                            var min = 0;
                            var max = 100;

                            this.Demold.UvLampPower = (int)GetValue(old, min, max);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void CehckPos(RcpMotionDataModel model, ILimitData limit)
        {
            var old = model.Position;

            model.Position = CehckPos(old, limit);
        }

        private double CehckPos(double old, ILimitData limit)
        {
            var min = limit.Minus;
            var max = limit.Plus;

            return GetValue(old, min, max);
        }

        private void CehckSpeed(RcpMotionDataModel model, ILimitData limit)
        {
            var old = model.Speed;
            var max = limit.Speed;

            model.Speed = GetValue(old, max);
        }

        private double GetValue(double old, double min, double max)
        {
            var value = NumberPad.Show(old);
            if (value == old) return old;

            if (this.Cheack(value, min, max))
            {
                AP.Event.InterlockMsgEvent("The Setting exceeds the allowed range. MIN:{0} MAX:{1}", min, max);
                return old;
            }

            return value;
        }

        private double GetValue(double old, double max)
        {
            var value = NumberPad.Show(old);
            if (value == old) return old;

            if (this.Cheack(value, max))
            {
                AP.Event.InterlockMsgEvent("The Setting exceeds the allowed range.. MAX:{0}", max);
                return old;
            }

            return value;
        }

        private bool Cheack(double value, double min, double max)
        {
            if (value < min) return true;
            else if (value > max) return true;

            return false;
        }

        private bool Cheack(double value, double max)
        {
            if (value > max) return true;

            return false;
        }
    }
}
