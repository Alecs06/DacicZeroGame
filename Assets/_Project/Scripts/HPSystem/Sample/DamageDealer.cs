using EventBus;
using HP;
using UnityEngine;
public class DamageDealer : MonoBehaviour
{
    public bool b;
    public Transform target;
    public int damage;
    void Update()
    {
        if (b)
        {
            EventBus<TakeDamage>.Raise(target.GetInstanceID(), new TakeDamage(damage, transform, null));
            b = false;
        }
    }
}
