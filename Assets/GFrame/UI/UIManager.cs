namespace GFrame
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    public enum UILevel
    {
        Bg,
        Common
    }

    public class UIManager : MgrBehaviour, ISingleton
    {
        [SerializeField]
        Dictionary<string, UIBehaviour> m_AllUI = new Dictionary<string, UIBehaviour>();

        [SerializeField] Camera m_UICamera;
        [SerializeField] Canvas m_Canvas;
        [SerializeField] CanvasScaler m_CanvasScaler;
        [SerializeField] Transform m_BgTrans;
        [SerializeField] Transform m_CommonTrans;
        [SerializeField] string m_PrefabPath = "UIPrefab";

        static GameObject m_Go;

        private UIManager() { }
        public static UIManager Instance
        {
            get
            {
                if (!m_Go)
                {
                    m_Go = GameObject.Find("UIManager");
                    if (!m_Go)
                    {
                        m_Go = Instantiate(Resources.Load("UIManager")) as GameObject;
                    }
                    m_Go.name = "UIManager";
                }

                return MonoSingletonProperty<UIManager>.Instance;
            }
        }
        public void Dispose()
        {
            SingletonProperty<UIManager>.Instance.Dispose();
        }

        public void OnSingletonInit() { }

        void Awake()
        {
            DontDestroyOnLoad(this);    
        }

        public void SetResolution(int width, int height)
        {
            m_CanvasScaler.referenceResolution = new Vector2(width, height);
        }

        public void SetMatchOnWidthOrHeight(float heightPercent)
        {
            m_CanvasScaler.matchWidthOrHeight = heightPercent;
        }

        public T OpenUI<T>(UILevel level) 
            where T : UIBehaviour
        {
            string behaviourName = SimpleName.Instance.GetName<T>();

            UIBehaviour ui;
            if(!m_AllUI.TryGetValue(behaviourName, out ui))
            {
                ui = CreateUI<T>(level);
            }
            ui.Show();
            return ui as T; 
        }

        public T CreateUI<T>(UILevel level)
            where T : UIBehaviour
        {
            string behavioutName = SimpleName.Instance.GetName<T>();
            UIBehaviour ui;
            if(m_AllUI.TryGetValue(behavioutName, out ui))
            {
                return ui as T;
            }

            GameObject prefab =  ResManager.Instance.LoadSync(m_PrefabPath + "/" + behavioutName) as GameObject;
            GameObject uiGo = Instantiate(prefab);
            switch(level)
            {
                case UILevel.Bg:
                    uiGo.transform.SetParent(m_BgTrans);
                    break;
                case UILevel.Common:
                    uiGo.transform.SetParent(m_CommonTrans);
                    break;
            }

            var uiGoRectTrans = uiGo.GetComponent<RectTransform>();
            uiGoRectTrans.offsetMin = Vector2.zero;
            uiGoRectTrans.offsetMax = Vector2.zero;
            uiGoRectTrans.anchoredPosition3D = Vector3.zero;
            uiGoRectTrans.anchorMin = Vector2.zero;
            uiGoRectTrans.anchorMax = Vector2.one;

            uiGo.transform.localScale = Vector3.one;
            uiGo.gameObject.name = behavioutName;

            ui = uiGo.AddComponent<T>();
            m_AllUI.Add(behavioutName, ui);
            ui.Init();

            return ui as T;
        }

        public Transform Get<T>(string strUIName)
        {
            string strDlg = SimpleName.Instance.GetName<T>();
            if(m_AllUI.ContainsKey(strDlg))
            {
                return m_AllUI[strDlg].transform.Find(strUIName);
            }
            else
            {
                Debug.LogError(string.Format("panel={0},ui={1} not exist!", strDlg, strUIName));
                return null;
            }
        }

        public void CloseUI<T>() {
            string behaviourName = SimpleName.Instance.GetName<T>();
            CloseUI(behaviourName);
        }

        public void CloseUI(string behaviourName)
        {
            UIBehaviour behaviour = null;

            m_AllUI.TryGetValue(behaviourName, out behaviour);
            if(behaviour != null)
            {
                behaviour.Close();
                m_AllUI.Remove(behaviourName);
            }
        }
    }
}
