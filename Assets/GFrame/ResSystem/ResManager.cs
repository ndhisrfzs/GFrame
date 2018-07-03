namespace GFrame
{
    [MonoSingletonPath("Tools/ResManager")]
    public class ResManager : MonoSingleton<ResManager>
    {
        public UnityEngine.Object LoadSync(string name)
        {
            return UnityEngine.Resources.Load(name);
        }
    }
}
