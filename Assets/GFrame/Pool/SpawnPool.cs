namespace GFrame
{
    using System.Collections.Generic;
    using UnityEngine;
    public abstract class SpawnPool<T> : ISpawnPool
    {
        protected Dictionary<string, IPool> m_Pools = new Dictionary<string, IPool>();
        private IPool Pool<P>()
            where P : IUIComponents, new()
        {
            string key = SimpleName.Instance.GetName<P>();

            if (m_Pools.ContainsKey(key))
            {
                return m_Pools[key];
            }

            throw new System.Exception(string.Format("SpawnPool Don't have {0} Pool", key));
        }

        public GameObject Prefab<P>()
           where P : IUIComponents, new()
        {
            IPool pool = Pool<P>();
            return pool.Prefab();
        }

        public void Despawn<P>(GameObject go)
            where P : IUIComponents, new()
        {
            IPool pool = Pool<P>();
            pool.Despawn(go);
        }

        public GameObject Spawn<P>()
            where P : IUIComponents, new()
        {
            IPool pool = Pool<P>();
            return pool.Spawn();
        }
    }
}
