using GFrame;
using UnityEngine;
using UnityEngine.UI;

public class UIReadyDialogComponents : IUIComponents
{
	public void InitUIComponents()
	{
		Grid_Transform = UIManager.Instance.Get<UIReadyDialog>("Right/ScrollView/Viewport/Grid").GetComponent<Transform>();
		Lv_Text = UIManager.Instance.Get<UIReadyDialog>("Left/Header/Lv/Lv").GetComponent<Text>();
		Name_Text = UIManager.Instance.Get<UIReadyDialog>("Left/Header/Name").GetComponent<Text>();
		Rank_Text = UIManager.Instance.Get<UIReadyDialog>("Left/Header/Rank/Rank").GetComponent<Text>();
		StartFight_Button = UIManager.Instance.Get<UIReadyDialog>("Left/Bottom/StartFight").GetComponent<Button>();
		Close_Button = UIManager.Instance.Get<UIReadyDialog>("Close").GetComponent<Button>();
	}

	public void Clear()
	{
		Grid_Transform = null;
		Lv_Text = null;
		Name_Text = null;
		Rank_Text = null;
		StartFight_Button = null;
		Close_Button = null;
	}

	public Transform Grid_Transform;
	public Text Lv_Text;
	public Text Name_Text;
	public Text Rank_Text;
	public Button StartFight_Button;
	public Button Close_Button;
}
