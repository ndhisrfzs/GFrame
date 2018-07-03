using GFrame;
using UnityEngine;
using UnityEngine.UI;

public class UICreateDialogComponents : IUIComponents
{
	public void InitUIComponents()
	{
		Name_InputField = UIManager.Instance.Get<UICreateDialog>("Name").GetComponent<InputField>();
		RandName_Button = UIManager.Instance.Get<UICreateDialog>("RandName").GetComponent<Button>();
		Start_Button = UIManager.Instance.Get<UICreateDialog>("Start").GetComponent<Button>();
		Sex_Transform = UIManager.Instance.Get<UICreateDialog>("Sex").GetComponent<Transform>();
	}

	public void Clear()
	{
		Name_InputField = null;
		RandName_Button = null;
		Start_Button = null;
		Sex_Transform = null;
	}

	public InputField Name_InputField;
	public Button RandName_Button;
	public Button Start_Button;
	public Transform Sex_Transform;
}
