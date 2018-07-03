using GFrame;

public class UICreateDialog : UIBehaviour
{
	protected override void InitUI()
	{
		mUIComponents = m_IComponents as UICreateDialogComponents;
	}

	protected override void RegisterUIEvent()
	{
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
		UnityEngine.Debug.Log("[UILoginBg:]" + content);
	}

	UICreateDialogComponents mUIComponents = null;
}
