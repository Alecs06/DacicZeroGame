using Detection;
using EventBus;
using HP;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using Weapons;

public class Projectile : MonoBehaviour
{
    public float velocity = 0;
    public int damage = 0;
    public bool isEnhanced = false;
    public event Action OnExpire = delegate { };
    [SerializeField] protected Rigidbody ProjectileBody;
    [SerializeField] protected LayerMask obstructionMask = 1 << 0;
    protected List<Collider> HitEnemies = new List<Collider>();
    public bool hasGravity = false;
    public float yawOffset = 0;

    protected Transform owner;
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

    protected virtual void FixedUpdate() { }
    protected virtual void Awake()
    {
        ProjectileBody = GetComponent<Rigidbody>();
        //stunEvent = new StunEvent(stunDuration);
        //soundEvent = new SoundEvent(radius * stunDuration * 5, transform.position, owner.gameObject.layer);
    }
    protected virtual void OnEnable()
    {
        Vector3 direction = Quaternion.Euler(0f, yawOffset, 0f) * Vector3.forward;
        ProjectileBody.AddRelativeForce(direction * velocity);
        ProjectileBody.useGravity = hasGravity;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        EventBus<TakeDamage>.Raise(other.transform.root.GetInstanceID(), new TakeDamage(damage, transform.root, other));
        Destroy(gameObject);
    }
    protected void OnDestroy()
    {
        OnExpire.Invoke();
    }
}
