using UnityEngine;

namespace GFrame
{
    public interface IPool
    {
        GameObject Spawn();
        void Despawn(GameObject go);
        GameObject Prefab();
    }
}
