using GFrame;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPanelComponents : IUIComponents
{
	public void InitUIComponents()
	{
		Arena_Button = UIManager.Instance.Get<UIMainPanel>("Arena").GetComponent<Button>();
		Group_Button = UIManager.Instance.Get<UIMainPanel>("Group").GetComponent<Button>();
		Shop_Button = UIManager.Instance.Get<UIMainPanel>("Shop").GetComponent<Button>();
		Rank_Button = UIManager.Instance.Get<UIMainPanel>("Rank").GetComponent<Button>();
		Pub_Button = UIManager.Instance.Get<UIMainPanel>("Pub").GetComponent<Button>();
		Smithy_Button = UIManager.Instance.Get<UIMainPanel>("Smithy").GetComponent<Button>();
		Copy_Button = UIManager.Instance.Get<UIMainPanel>("Copy").GetComponent<Button>();
		War_Button = UIManager.Instance.Get<UIMainPanel>("War").GetComponent<Button>();
		Exper_Button = UIManager.Instance.Get<UIMainPanel>("Exper").GetComponent<Button>();
		Expedition_Button = UIManager.Instance.Get<UIMainPanel>("Expedition").GetComponent<Button>();
		Name_Text = UIManager.Instance.Get<UIMainPanel>("UserInfo/Line1/Name").GetComponent<Text>();
		Lv_Text = UIManager.Instance.Get<UIMainPanel>("UserInfo/Line2/Lv/Lv").GetComponent<Text>();
		Gold_Text = UIManager.Instance.Get<UIMainPanel>("UserInfo/Line2/Gold/Gold").GetComponent<Text>();
		User_Button = UIManager.Instance.Get<UIMainPanel>("Sidebar/Sidebar/Bar/User").GetComponent<Button>();
		Stone_Button = UIManager.Instance.Get<UIMainPanel>("Sidebar/Sidebar/Bar/Stone").GetComponent<Button>();
		Generals_Button = UIManager.Instance.Get<UIMainPanel>("Sidebar/Sidebar/Bar/Generals").GetComponent<Button>();
		Office_Button = UIManager.Instance.Get<UIMainPanel>("Sidebar/Sidebar/Bar/Office").GetComponent<Button>();
		Email_Button = UIManager.Instance.Get<UIMainPanel>("Sidebar/Sidebar/Bar/Email").GetComponent<Button>();
		Settings_Button = UIManager.Instance.Get<UIMainPanel>("Sidebar/Sidebar/Bar/Settings").GetComponent<Button>();
	}

	public void Clear()
	{
		Arena_Button = null;
		Group_Button = null;
		Shop_Button = null;
		Rank_Button = null;
		Pub_Button = null;
		Smithy_Button = null;
		Copy_Button = null;
		War_Button = null;
		Exper_Button = null;
		Expedition_Button = null;
		Name_Text = null;
		Lv_Text = null;
		Gold_Text = null;
		User_Button = null;
		Stone_Button = null;
		Generals_Button = null;
		Office_Button = null;
		Email_Button = null;
		Settings_Button = null;
	}

	public Button Arena_Button;
	public Button Group_Button;
	public Button Shop_Button;
	public Button Rank_Button;
	public Button Pub_Button;
	public Button Smithy_Button;
	public Button Copy_Button;
	public Button War_Button;
	public Button Exper_Button;
	public Button Expedition_Button;
	public Text Name_Text;
	public Text Lv_Text;
	public Text Gold_Text;
	public Button User_Button;
	public Button Stone_Button;
	public Button Generals_Button;
	public Button Office_Button;
	public Button Email_Button;
	public Button Settings_Button;
}
