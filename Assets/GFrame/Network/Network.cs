using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GFrame
{
    public abstract class Network : INetwork
    {
        internal void OnReportInfo(bool isException, string info)
        {
            if (isException)
            {
                UnityEngine.Debug.LogError(info);
            }
            else
            {
                UnityEngine.Debug.Log(info);
            }
        }

        //internal void ConnectStateChange(ClientStatus oldStatus, ClientStatus newStatue)
        //{
        //    //U3DAction.Instance.QueueOnU3DThread((obj) =>
        //    //{
        //    switch (newStatue)
        //    {
        //        case ClientStatus.NotConnected:
        //            //没有和游戏服进行连接
        //            break;
        //        case ClientStatus.Connecting:
        //            //正在进行连接
        //            break;
        //        case ClientStatus.WaitingResponse:
        //            break;
        //        case ClientStatus.ConnectionFault:
        //            if (oldStatus != newStatue)
        //            {
        //                //V_Message.Instance.ShowMessage("无法连接服务器，请检查网络！");
        //                //V_Busying.Instance.Close();
        //            }
        //            //断线，超时，数据包错误
        //            break;
        //        case ClientStatus.WrongVerifyInfo:
        //            if (oldStatus != newStatue)
        //            {
        //                //V_Message.Instance.ShowMessage("无法连接服务器！");
        //                //V_Busying.Instance.Close();
        //            }
        //            //登录验证信息错
        //            break;
        //        case ClientStatus.Connected:
        //            //处于正常连接状态
        //            break;
        //    }
        //    //}, null);
        //}
        public abstract void Close();
        public abstract void ConnectToServer();
        public abstract void Update();
    }
}
