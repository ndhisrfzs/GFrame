using GFrame;
using UnityEngine;
using UnityEngine.UI;

public class UIReadyDialog : UIBehaviour
{
	protected override void InitUI()
	{
		mUIComponents = m_IComponents as UIReadyDialogComponents;
        mBars = new GameObject[9];
        for(int i = 0; i < 9; i++)
        {
            var bar = PoolManager.Instance.Pool<CommonPool>().Spawn<Bar>();
            bar.transform.SetParent(mUIComponents.Grid_Transform);
            bar.GetComponent<Toggle>().group = mUIComponents.Grid_Transform.GetComponent<ToggleGroup>();
            bar.GetComponent<Toggle>().onValueChanged.AddListener(ToggleValueChanged);
            bar.name = i.ToString();
            Reset(bar);
            mBars[i] = bar;
            SetHero(bar);
        }
	}

    private void SetHero(GameObject bar)
    {
        var bar_component = bar.GetComponent<Bar>();
        Transform[] heroes = new Transform[] { bar_component.Hero0_Image.transform, bar_component.Hero1_Image.transform, bar_component.Hero2_Image.transform,
        bar_component.Hero3_Image.transform, bar_component.Hero4_Image.transform};
        for (int i = 0; i < 5; i++)
        {
            var hero = PoolManager.Instance.Pool<CommonPool>().Spawn<Hero>();
            hero.transform.SetParent(heroes[i]);
            Reset(hero);
        }
    }

    private void ToggleValueChanged(bool b)
    {
        for(int i = 0; i < mBars.Length; i++)
        {
            if (mBars[i].GetComponent<Toggle>().isOn)
            {
                curSelected = mBars[i].name;
                ShowLog(curSelected + "selected");
                break;
            }
        }
    }

	protected override void RegisterUIEvent()
	{
        mUIComponents.Close_Button.onClick.AddListener(() => {
            this.Hide();
            UIManager.Instance.OpenUI<UIMainPanel>(UILevel.Common);
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

    private void Reset(GameObject go)
    {
        var uiGoRectTrans = go.GetComponent<RectTransform>();
        uiGoRectTrans.offsetMin = Vector2.zero;
        uiGoRectTrans.offsetMax = Vector2.zero;
        uiGoRectTrans.anchoredPosition3D = Vector3.zero;
        uiGoRectTrans.anchorMin = Vector2.zero;
        uiGoRectTrans.anchorMax = Vector2.one;

        go.transform.localScale = Vector3.one;
    }

    void ShowLog(string content)
	{
		UnityEngine.Debug.Log("[UIReadyDialog:]" + content);
	}

	UIReadyDialogComponents mUIComponents = null;
    GameObject[] mBars = null;
    string curSelected;
}
