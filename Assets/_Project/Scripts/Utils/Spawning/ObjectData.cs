using UnityEngine;
namespace Spawning
{
    public class ObjectData<Id> : ScriptableObject
    {
        /// <summary>
        /// The id by which the object will be categorized.
        /// </summary>
        [field: SerializeField] public Id ID { get; protected set; }
    }
    public class MonoBehaviourData<Id, T> : ObjectData<Id> where T : ISpawnable<Id>
    {
        /// <summary>
        /// The prefab of this MonoBehaviour.
        /// </summary>
        [field: SerializeField] public T Prefab { get; protected set; }
    }
}