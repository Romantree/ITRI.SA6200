using GIGA.ITRI.SA6200.UI.Managers.Net;
using System;
using System.Linq;
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Managers
{
    public class NetManager
    {
        public readonly NetLoadcell StageLeftLD = new NetLoadcell();
        public readonly NetLoadcell StageRightLD = new NetLoadcell();
        public readonly NetUvLamp StageUv = new NetUvLamp();
        public readonly NetRequlator StageLeftRG = new NetRequlator();
        public readonly NetRequlator StageRightRG = new NetRequlator();

        public INetSerialPort this[NetworkUnit unit]
        {
            get
            {
                switch (unit)
                {
                    case NetworkUnit.StageLeftLD: return this.StageLeftLD;
                    case NetworkUnit.StageRightLD: return this.StageRightLD;
                    case NetworkUnit.StageUv: return this.StageUv;
                    case NetworkUnit.StageLeftRG: return this.StageLeftRG;
                    case NetworkUnit.StageRightRG: return this.StageRightRG;
                }

                return null;
            }
        }

        public void Start()
        {
            if (AP.IsSim) return;

            var flag = false;

            if (this.Start(this.StageLeftLD, NetworkUnit.StageLeftLD) == false) flag = true;
            if (this.Start(this.StageRightLD, NetworkUnit.StageRightLD) == false) flag = true;
            if (AP.IS_UV_LAMP) if (this.Start(this.StageUv, NetworkUnit.StageUv) == false) flag = true;
            if (this.Start(this.StageLeftRG, NetworkUnit.StageLeftRG) == false) flag = true;
            if (this.Start(this.StageRightRG, NetworkUnit.StageRightRG) == false) flag = true;

            if (flag == true) AP.Event.InterlockMsgEvent("Failed to connect to the communication.");
        }

        public void Abort()
        {
            try
            {
                this.StageUv.LedOnOff(false);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public Response Start(INetSerialPort net, NetworkUnit unit)
        {
            try
            {
                var port = DB.Config[unit];

                if (string.IsNullOrWhiteSpace(port)) throw new Exception($"The port is null.[{net.GetType()}]");
                if (EnumHelper.SerialPort.Any(t => t == port) == false) throw new Exception($"There is no port. [{net.GetType()}]");

                net.Start(port);
                net.Init();

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public bool LoadcellCheck(double left, double right, double gap = 0.1) => StageLeftLD.CheckData(left, gap) && StageRightLD.CheckData(right, gap);

        public bool RequlatorCheck(double left, double right, double gap = 1) => this.StageLeftRG.CheckData(left, gap) && this.StageRightRG.CheckData(right, gap);
    }
}
