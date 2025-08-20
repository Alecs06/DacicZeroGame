using UnityEngine;
namespace Spawning
{
    public class MonoBehaviourSpawner<Id, T> : MonoBehaviour, ISpawner<Id, T> where T : MonoBehaviour, ISpawnable<Id>
    {
        public virtual T Create(ObjectData<Id> data)
        {
            var d = data as MonoBehaviourData<Id, T>;
            if (d == null)
            {
                Debug.Log($"{this} received invalid object data!");
                return null;
            }
            var t = Instantiate(d.Prefab);
            t.Init(d);
            return t;
        }
    }
}
