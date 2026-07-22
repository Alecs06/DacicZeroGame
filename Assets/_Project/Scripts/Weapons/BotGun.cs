using EventBus;
using HP;
using UnityEngine;
namespace Weapons
{
    public class BotGun : WeaponBase
    {
        #region Parameters
        /// <summary>
        /// minimum error when shooting
        /// </summary>
        [SerializeField] protected float minError;
        /// <summary>
        /// maximum error when shooting
        /// </summary>
        [SerializeField] protected float maxError;
        /// <summary>
        /// how fast accuracy increases when shooting
        /// </summary>
        [SerializeField] protected float accBuildUp;
        /// <summary>
        /// how fast accuracy decreases when not shooting
        /// </summary>
        [SerializeField] protected float accDecay;
        [SerializeField] protected int damage = 1;
        [Tooltip("Cosmetic stuff")]
        [SerializeField] protected AudioClip audioClip;
        [SerializeField] protected float lineDuration;
        #endregion
        #region Other fields
        /// <summary>
        /// Current error of the gun. Don't fuck with this.
        /// </summary>
        protected float error;
        /// <summary>
        /// Current error of the gun. Don't fuck with this.
        /// </summary>
        protected float currentError
        {
            get
            {
                return error;
            }
            set
            {
                error = Mathf.Clamp(value, minError, maxError);
            }
        }
        /// <summary>
        /// Raycast result used for shooting. Don't fuck with this.
        /// </summary>
        RaycastHit hit;
        /// <summary>
        /// Cached value from global settings. -||-
        /// </summary>
        float maxRaycastDist;
        protected AudioSource audioSource;
        protected LineRenderer lineRenderer;
        #endregion
        #region Setup
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            audioSource = GetComponent<AudioSource>();
            maxRaycastDist = GlobalSettings.MaxRaycastDist;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            lineRenderer.enabled = false;
            currentError = maxError;
        }
        #endregion
        #region Main methods
        protected override void Update()
        {
            base.Update();
            if (lineRenderer.enabled)
            {
                if (Time.time - timeLastShot >= lineDuration)
                {
                    lineRenderer.enabled = false;
                }
            }
        }
        protected override void Fire()
        {
            //reset the fireCooldown of the gun
            cooldownTo = Time.time + fireCooldown;
            timeLastShot = Time.time;
            Vector3 dir = transform.forward;
            lineRenderer.enabled = true;
            audioSource.PlayOneShot(audioClip);
            if (currentError > 0)
            {
                float a = Random.Range(0, 360);
                dir += (transform.right * Mathf.Cos(a) + transform.up *
                    Mathf.Sin(a)) * currentError;
                //Debug.DrawRay(transform.position, dir, Color.blue, 10);
            }
            if (Physics.Raycast(transform.position, dir, out hit, maxRaycastDist,
                GlobalSettings.TargetMasks[gameObject.layer]))
            {
                //we hit something, damage it
                lineRenderer.SetPositions(new Vector3[2] { transform.position,
                    hit.point });
                //raise a damage event for whatever we hit
                EventBus<TakeDamage>.Raise(hit.transform.root.GetInstanceID(),
                    new TakeDamage(damage, transform.root, hit.collider));
            }
            else
            {
                lineRenderer.SetPositions(new Vector3[2] { transform.position,
                    transform.position + dir * maxRaycastDist });
            }
            //accuracy increases while firing
            currentError -= accBuildUp * fireCooldown;
        }
        protected override void HandleNotFiring()
        {
            currentError += accDecay * Time.deltaTime;
        }
        #endregion
    }
}