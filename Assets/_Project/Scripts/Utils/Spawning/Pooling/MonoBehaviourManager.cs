using Spawning;

namespace Pooling
{
    public class MonoBehaviourManager<Id> : ObjectManager<Id>
    {
        protected override void Awake()
        {
            base.Awake();
            spawner = GetComponent<MonoBehaviourSpawner<Id, Poolable<Id>>>();
        }
    }
}