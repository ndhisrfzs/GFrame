using System.Collections.Generic;
using System.Threading;

namespace GFrame
{
    [MonoSingletonPath("Tools/NetworkManager")]
    public class NetworkManager : MgrBehaviour, ISingleton
    {
        Dictionary<string, INetwork> m_Clients = new Dictionary<string, INetwork>();
        List<INetwork> m_Updates = null;
        object locker = new object();
        public static NetworkManager Instance
        {
            get
            {
                return MonoSingletonProperty<NetworkManager>.Instance;
            }
        }

        public T Get<T>()
            where T : class, INetwork, new()
        {
            var key = typeof(T).ToString();
            if (m_Clients.ContainsKey(key))
            {
                return m_Clients[key] as T;
            }
            else
            {
                var client = new T();
                Add(client);
                return client;
            }
        }

        public bool Add<T>(T client)
            where T : class, INetwork
        {
            var key = typeof(T).ToString();
            if (!m_Clients.ContainsKey(key))
            {
                lock (locker)
                {
                    m_Clients[key] = client;
                    m_Updates = new List<INetwork>(m_Clients.Values);
                }
                return true;
            }
            return false;
        }

        public bool Close<T>()
            where T: class, INetwork
        {
            var key = typeof(T).ToString();
            if (m_Clients.ContainsKey(key))
            {
                var client = m_Clients[key];
                client.Close();
                lock (locker)
                {
                    m_Clients.Remove(key);
                    m_Updates = new List<INetwork>(m_Clients.Values);
                }
                return true;
            }

            return false;
        }

        void Update()
        {
            if(Monitor.TryEnter(locker))
            {
                for(int i = 0; i < m_Updates.Count; i++)
                {
                    m_Updates[i].Update();
                }
                Monitor.Exit(locker);
            }
        }

        public void OnSingletonInit()
        {
        }

        public void Dispose()
        {
            foreach (var client in m_Clients)
            {
                client.Value.Close();
            }
            m_Clients.Clear();
        }

        void OnApplicationQuit()
        {
            Dispose();
        }
    }
}