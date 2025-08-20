using Pooling;

public class SphereManager : ObjectManager<float>
{
    protected override void Awake()
    {
        base.Awake();
        spawner = GetComponent<SphereSpawner>();
    }
}
