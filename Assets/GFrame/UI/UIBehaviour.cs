using System;

namespace GFrame
{
    public abstract class UIBehaviour : BaseMonoBehaviour, IUI
    {
        protected IUIComponents m_IComponents = null;
        public void Init()
        {
            InnerInit();
            RegisterUIEvent();
        }

        void InnerInit()
        {
            m_IComponents = UIFactory.Instance.CreateUIComponents(name);
            m_IComponents.InitUIComponents();
            InitUI();
        }

        protected abstract void InitUI();
        protected abstract void RegisterUIEvent();

        public void Close(bool destroy = true)
        {
            OnClose();

            if (destroy)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnClose()
        {
        }

        protected override void OnBeforeDestory()
        {
            DestoryUI();

            if(m_IComponents != null)
            {
                m_IComponents.Clear();
            }
        }
        protected virtual void DestoryUI() { }
    }
}
