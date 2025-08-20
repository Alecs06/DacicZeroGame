using Detection;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    public bool b;
    [SerializeField] protected float intensity;
    private void Update()
    {
        if (b)
        {
            b = false;
            EventBus.EventBus<SoundEvent>.Raise(0, new SoundEvent(intensity, transform.position, gameObject.layer));
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, intensity);
    }
}
