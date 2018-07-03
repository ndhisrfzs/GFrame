using GFrame;
using UnityEngine;

public class UIMainPanel : UIBehaviour
{
	protected override void InitUI()
	{
		mUIComponents = m_IComponents as UIMainPanelComponents;
        //var user = UserData.Instance.user;
        //mUIComponents.Name_Text.text = user.name;
        //mUIComponents.Lv_Text.text = user.user_level.ToString();
        //mUIComponents.Gold_Text.text = user.gold.ToString();

        //var go = PoolManager.Instance.Pool<CommonPool>().Spawn<Loading>();
        //go.transform.SetParent(mUIComponents.Arena_Button.transform);
        //var uiGoRectTrans = go.GetComponent<RectTransform>();
        //uiGoRectTrans.offsetMin = Vector2.zero;
        //uiGoRectTrans.offsetMax = Vector2.zero;
        //uiGoRectTrans.anchoredPosition3D = Vector3.zero;
        //uiGoRectTrans.anchorMin = Vector2.zero;
        //uiGoRectTrans.anchorMax = Vector2.one;

        //go.transform.localScale = Vector3.one;
    }

	protected override void RegisterUIEvent()
	{
        mUIComponents.Arena_Button.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenUI<UIReadyDialog>(UILevel.Common);
            this.Hide();
        });
	}

	protected override void OnShow()
	{
		base.OnShow();
	}

	protected override void OnHide()
	{
		base.OnHide();
	}

	protected override void OnClose()
	{
		base.OnClose();
	}

	protected override void DestoryUI()
	{
		base.DestoryUI();
	}

	void ShowLog(string content)
	{
		UnityEngine.Debug.Log("[UILoginBg:]" + content);
	}

	UIMainPanelComponents mUIComponents = null;
}
