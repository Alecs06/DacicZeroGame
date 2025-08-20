using Spawning;
using UnityEngine;
namespace Pooling
{
    /// <summary>
    /// Handles object creation and pooling.
    /// </summary>
    /// <typeparam name="Id">The pooling id of the objects.</typeparam>
    public abstract class ObjectManager<Id> : MonoBehaviour
    {
        /// <summary>
        /// Holds inactive objects in categories.
        /// </summary>
        protected MultiPool<Id, Poolable<Id>> multiPool = new();
        /// <summary>
        /// Used to create new objects.
        /// </summary>
        protected ISpawner<Id, Poolable<Id>> spawner;
        /// <summary>
        /// Manager instance. Should only be one per scene.
        /// </summary>
        public static ObjectManager<Id> Instance { get; protected set; }
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        /// <summary>
        /// Get an object from the pool or create a new one if necessary.
        /// </summary>
        /// <param name="data">The data that will be used to retrieve an object or create a new object.</param>
        /// <returns>An instance of the requested object.</returns>
        public Poolable<Id> Get(ObjectData<Id> data)
        {
            Poolable<Id> t;
            if (multiPool.TryGet(data.ID, out t))
            {
                t.ResetObject();
                return t;
            }
            return spawner.Create(data);
        }
        /// <summary>
        /// Release the object into the pool.
        /// </summary>
        /// <param name="object">The object to be released.</param>
        public void Release(Poolable<Id> @object)
        {
            multiPool.Release(@object);
        }
    }
}