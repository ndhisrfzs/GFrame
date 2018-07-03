using System.Collections.Generic;

namespace GFrame
{
    public class SimpleName : Singleton<SimpleName>
    {
        private Dictionary<string, string> m_FullName2SimpleName = new Dictionary<string, string>();
        private SimpleName() { }
        public string GetName<T>()
        {
            string fullName = typeof(T).ToString();

            if (m_FullName2SimpleName.ContainsKey(fullName))
            {
                return m_FullName2SimpleName[fullName];
            }
            else
            {
                string[] nameSplites = fullName.Split(new char[] { '.' });
                var ret = nameSplites[nameSplites.Length - 1];
                m_FullName2SimpleName.Add(fullName, ret);
                return ret;
            }
        }
    }
}
