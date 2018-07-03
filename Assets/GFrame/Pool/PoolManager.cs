namespace GFrame
{
    using System.Collections.Generic;

    [MonoSingletonPath("Tools/PoolManager")]
    public class PoolManager : MonoSingleton<PoolManager>
    {
        Dictionary<string, ISpawnPool> m_Pools = new Dictionary<string, ISpawnPool>();
        public ISpawnPool Pool<T>()
            where T : SpawnPool<T>, new()
        {
            string key = SimpleName.Instance.GetName<T>();

            if(m_Pools.ContainsKey(key))
            {
                return m_Pools[key];
            }

            ISpawnPool pool = new T();
            m_Pools.Add(key, pool);
            return pool;
        }
    }
}
