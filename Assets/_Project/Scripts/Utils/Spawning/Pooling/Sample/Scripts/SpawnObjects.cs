using Spawning;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public bool Cube, Sphere;
    [SerializeField] MonoBehaviourData<int, Poolable<int>> data;
    [SerializeField] MonoBehaviourData<float, Poolable<float>> data1;
    void Update()
    {
        var instance = CubeManager.Instance;
        if (Cube)
        {
            Cube = false;
            var a = CubeManager.Instance.Get(data);
            a.gameObject.
            SetActive(true);
        }
        if (Sphere)
        {
            Sphere = false;
            SphereManager.Instance.Get(data1).gameObject.SetActive(true);
        }
    }
}
