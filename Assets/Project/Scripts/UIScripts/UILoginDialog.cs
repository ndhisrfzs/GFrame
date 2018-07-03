using GFrame;
using HHW.Service;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UILoginDialog : UIBehaviour
{
	protected override void InitUI()
	{
		mUIComponents = m_IComponents as UILoginDialogComponents;
    }

    protected override void RegisterUIEvent()
	{
        mUIComponents.Start_Button.onClick.AddListener(OnClickLoginBtn);
    }

    private async void OnClickLoginBtn()
    {
        var account = mUIComponents.Account_InputField.GetComponent<InputField>().text;
        var password = mUIComponents.Password_InputField.GetComponent<InputField>().text;

        var session = Game.Scene.GetComponent<NetOuterComponent>().Create(NetworkHelper.ToIPEndPoint("127.0.0.1", 7777));
        var result = (Login.Response)await session.Call(new Login.Request() { Account = account, Password = password });

        if (result.IsLogin)
        {
            UIManager.Instance.CloseUI<UILoginDialog>();
            UIManager.Instance.OpenUI<UICreateDialog>(UILevel.Common);
        }
    }

	protected override void OnShow()
	{
		base.OnShow();
	}

	protected override void OnHide()
	{
		base.OnHide();
	}

    protected override void DestoryUI()
    {
        base.DestoryUI();
    }

    void ShowLog(string content)
	{
		UnityEngine.Debug.Log("[UILoginDialog:]" + content);
	}

    //[MethodType(MethodTypeEnum.InvokeWhenServerResponse)]
    //[CommandID((int)LoginCommand.Login)]
    //public void LoginResult(LoginResult result)
    //{
    //    if (!result.is_success)
    //        return;

    //    if (result.servers.Count <= 0)
    //        return;

    //    NetworkManager.Instance.Get<GameClient>().IP = result.servers[0].ip;
    //    NetworkManager.Instance.Get<GameClient>().Port = (ushort)result.servers[0].port;
    //    NetworkManager.Instance.Get<LoginClient>().Client.BeginInvokeServiceService((int)LoginCommand.UserToken, new object[] { result.servers[0].id });
    //}


    //[MethodType(MethodTypeEnum.InvokeWhenServerResponse)]
    //[CommandID((int)LoginCommand.UserToken)]
    //public void UserToken(TokenResult result)
    //{
    //    if (!result.is_success)
    //        return;

    //    GameLib.BetterSerialize.BytesWriter bw = new GameLib.BetterSerialize.BytesWriter(48);
    //    bw.WriteRaw(result.token, 32);
    //    bw.Write(result.uid);
    //    bw.Write(DateTime.Now);
    //    NetworkManager.Instance.Get<GameClient>().Token = bw.Bytes;
    //    NetworkManager.Instance.Get<GameClient>().ConnectToServer();

    //    NetworkManager.Instance.Get<GameClient>().Client.RegisterInvokeInstance(null, this, "GameServerLogin", typeof(User));
    //    NetworkManager.Instance.Get<GameClient>().Client.BeginInvokeServiceService((int)ServerCommand.Login, new object[] { });

    //    NetworkManager.Instance.Close<LoginClient>();
    //}

    //[MethodType(MethodTypeEnum.InvokeWhenServerResponse)]
    //[CommandID((int)ServerCommand.Login)]
    //public void GameServerLogin(User user)
    //{
    //    if (user == null)
    //    {
    //        UIManager.Instance.CloseUI<UILoginDialog>();
    //        UIManager.Instance.OpenUI<UICreateDialog>(UILevel.Common);
    //    }
    //    else
    //    {
    //        UserData.Instance.user = user;
    //        UIManager.Instance.CloseUI<UILoginDialog>();
    //        UIManager.Instance.CloseUI<UILoginBg>();
    //        UIManager.Instance.OpenUI<UIMainBg>(UILevel.Bg);
    //        UIManager.Instance.OpenUI<UIMainPanel>(UILevel.Common);
    //    }
    //}

    UILoginDialogComponents mUIComponents = null;
}
