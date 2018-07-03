namespace GFrame
{
    using UnityEngine;
    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }
        protected virtual void OnShow() { }

        public void Hide()
        {
            OnHide();
            gameObject.SetActive(false);
        }
        protected virtual void OnHide() { }

        protected virtual void OnDestroy()
        {
            OnBeforeDestory();
        }

        protected virtual void OnBeforeDestory() { }
    }
}
