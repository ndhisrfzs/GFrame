namespace GFrame
{
    using UnityEngine;
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton
        where T : MonoSingleton<T>
    {
        protected static T m_Instance = null;
        public static T Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = SingletonCreator.CreateMonoSingleton<T>();
                }

                return m_Instance;
            }
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            m_Instance = null;
        }

        public void OnSingletonInit()
        {
        }
    }
}
