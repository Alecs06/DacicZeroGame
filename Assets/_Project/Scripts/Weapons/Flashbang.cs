using Detection;
using EventBus;
using System.Collections.Generic;
using UnityEngine;
namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Flashbang : MonoBehaviour
    {
        [SerializeField] float velocity, radius, stunDuration;
        [SerializeField] Rigidbody rb;
        Transform owner;
        LayerMask targetMask;
        [SerializeField] protected LayerMask obstructionMask = 1 << 0;
        Collider[] colliders;
        StunEvent stunEvent;
        [SerializeField] AudioClip clip;
        SoundEvent soundEvent;
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
            rb = GetComponent<Rigidbody>();
            stunEvent = new StunEvent(stunDuration);
            soundEvent = new SoundEvent(radius * stunDuration * 5, transform.position, owner.gameObject.layer);
        }
        private void OnEnable()
        {
            rb.AddRelativeForce(Vector3.forward * velocity);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root == owner) return;
            soundEvent.Position = transform.position;
            EventBus<SoundEvent>.Raise(0, soundEvent);
            AudioSource.PlayClipAtPoint(clip, transform.position);
            HashSet<int> set = new() { owner.GetInstanceID() };
            colliders = new Collider[GlobalSettings.MaxTargets];
            int nrOfHits = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, targetMask);
            for (int i = 0; i < nrOfHits; i++)
            {
                var tr = colliders[i].transform.root;
                if (!set.Contains(tr.GetInstanceID()))
                {
                    if (!Physics.Linecast(transform.position, tr.position, obstructionMask))
                    {
                        set.Add(tr.GetInstanceID());
                        EventBus<StunEvent>.Raise(tr.GetInstanceID(), stunEvent);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}