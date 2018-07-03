namespace GFrame
{
    using UnityEngine;
    public interface ISpawnPool
    {
        GameObject Prefab<P>() where P : IUIComponents, new();
        void Despawn<P>(GameObject go) where P : IUIComponents, new();
        GameObject Spawn<P>() where P : IUIComponents, new();
    }
}
