using Detection;
using EventBus;
using HP;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using Weapons;

public class Projectile : MonoBehaviour
{
    public float velocity = 0;
    public int damage = 0;
    [SerializeField] Rigidbody ProjectileBody;
    [SerializeField] protected LayerMask obstructionMask = 1 << 0;

    Transform owner;
    LayerMask targetMask;
    public Transform Owner
    {
        set
        {
            owner = value;
            if (owner != null)
            {
                targetMask = GlobalSettings.TargetMasks[owner.gameObject.layer];
            }
        }
    }
    private void Awake()
    {
        ProjectileBody = GetComponent<Rigidbody>();
        //stunEvent = new StunEvent(stunDuration);
        //soundEvent = new SoundEvent(radius * stunDuration * 5, transform.position, owner.gameObject.layer);
    }
    private void OnEnable()
    {
        ProjectileBody.AddRelativeForce(Vector3.forward * velocity);
        ProjectileBody.useGravity = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        EventBus<TakeDamage>.Raise(other.transform.root.GetInstanceID(), new TakeDamage(damage, transform.root, other));
        Destroy(gameObject);
    }
}
