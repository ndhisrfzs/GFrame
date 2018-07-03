using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ArrowMovie : MonoBehaviour {
    bool isOn = true;
    public RectTransform bar;
    public RectTransform arrow;
    public Toggle toggle;
    // Use this for initialization
    void Start () {
        var tweener = bar.DOLocalMoveY(-bar.anchoredPosition.y * 2, 0.2f);
        tweener.SetAutoKill(false);
        tweener.Pause();

        toggle.onValueChanged.AddListener((x) => {
            if (isOn)
            {
                arrow.DORotate(new Vector3(0, 0, 180), 0.1f);
                bar.DOPlayForward();
            }
            else
            {
                arrow.DORotate(new Vector3(0, 0, 0), 0.1f);
                bar.DOPlayBackwards();
            }
            isOn = !isOn;
        });
	}
}
