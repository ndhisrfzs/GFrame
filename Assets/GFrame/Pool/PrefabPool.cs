namespace GFrame
{
    using System.Collections.Generic;
    using UnityEngine;
    public abstract class PrefabPool<T> : MonoBehaviour, IPool
        where T : UIComponents 
    {
        protected int m_Capacity = 0;
        Queue<GameObject> m_DormantObjects = new Queue<GameObject>();
        public GameObject prefab;

        public void Despawn(GameObject go)
        {
            if (m_DormantObjects.Count >= m_Capacity)
            {
                GameObject temp = m_DormantObjects.Dequeue();
                Object.Destroy(temp);
            }
            go.transform.parent = null;
            go.SetActive(false);
            m_DormantObjects.Enqueue(go);
        }

        public GameObject Prefab()
        {
            if (prefab == null)
            {
                string path = PrefabPath();
                prefab =  ResManager.Instance.LoadSync(path) as GameObject;
                PoolMark mark = prefab.GetComponent<PoolMark>();
                if(mark != null)
                {
                    m_Capacity = mark.Capacity;
                }
            }

            return prefab;
        }

        public abstract string PrefabPath();

        public GameObject Spawn()
        {
            GameObject go = null;
            if (m_DormantObjects.Count > 0)
            {
                go = m_DormantObjects.Dequeue();
            }
            else
            {
                go = GameObject.Instantiate(Prefab());
            }

            UIComponents ui = go.AddComponent<T>();
            ui.InitUIComponents();

            return go;
        }
    }
}
