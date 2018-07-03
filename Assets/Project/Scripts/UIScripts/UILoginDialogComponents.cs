using GFrame;
using UnityEngine;
using UnityEngine.UI;

public class UILoginDialogComponents : IUIComponents
{
	public void InitUIComponents()
	{
		Account_InputField = UIManager.Instance.Get<UILoginDialog>("Account").GetComponent<InputField>();
		Password_InputField = UIManager.Instance.Get<UILoginDialog>("Password").GetComponent<InputField>();
		Start_Button = UIManager.Instance.Get<UILoginDialog>("Start").GetComponent<Button>();
	}

	public void Clear()
	{
		Account_InputField = null;
		Password_InputField = null;
		Start_Button = null;
	}

	public InputField Account_InputField;
	public InputField Password_InputField;
	public Button Start_Button;
}
