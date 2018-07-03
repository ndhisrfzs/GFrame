using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMovie : MonoBehaviour {
    public Transform p1;
    public Transform p2;
    public Toggle Male;
    public Toggle Female;
    private bool isMale = true;
    private bool isMovie = false;
	void Start () {
        SetListener();
    }
	
    void SetListener()
    {
        if(isMale)
        {
            Female.onValueChanged.AddListener(ValueChange);
            Male.onValueChanged.RemoveAllListeners();
        }
        else
        {
            Male.onValueChanged.AddListener(ValueChange);
            Female.onValueChanged.RemoveAllListeners();
        }
    }
    public void ValueChange(bool isOn)
    {
        isMale = !isMale;
        StartSwitch();
        SetListener();
    }

    public void StartSwitch()
    {
        if (isMovie)
            return;
        isMovie = true;
        Male.enabled = false;
        Female.enabled = false;

        var p1_pos = p1.position;
        var p2_pos = p2.position;
        if (!isMale)
        {
            p2.DOMoveX(-2, 0.2f).SetRelative();
            p1.DOScale(0.6f, 0.4f);
            p2.DOScale(1f, 0.4f);
            p1.DOMoveX(2, 0.2f).SetRelative().OnComplete(() =>
            {
                p1.SetAsFirstSibling();
                p1.DOMove(p1_pos, 0.2f);
                p2.DOMove(p2_pos, 0.2f).OnComplete(() =>
                {
                    isMovie = false;
                    Male.enabled = true;
                    Female.enabled = true;
                });
            });
        }
        else
        {
            p1.DOMoveX(-2, 0.2f).SetRelative();
            p2.DOScale(0.6f, 0.4f);
            p1.DOScale(1f, 0.4f);
            p2.DOMoveX(2, 0.2f).SetRelative().OnComplete(() => {
                p2.SetAsFirstSibling();
                p2.DOMove(p2_pos, 0.2f);
                p1.DOMove(p1_pos, 0.2f).OnComplete(() =>
                {
                    isMovie = false;
                    Male.enabled = true;
                    Female.enabled = true;
                });
            });
        }
    }
}
