using Entity;
using EventBus;
using UnityEngine;

public class BotGun : MonoBehaviour
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
    /// <summary>
    /// how much time the gun needs between shots
    /// </summary>
    [SerializeField] protected float shotCooldown;
    [SerializeField] protected int damage = 1;
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
    /// When true, the gun will fire when it is off cooldown.
    /// </summary>
    [field: SerializeField] public bool Firing { get; set; }
    /// <summary>
    /// Raycast result used for shooting. Don't fuck with this.
    /// </summary>
    RaycastHit hit;
    /// <summary>
    /// When did the gun last fire. Used for cooldown. Don't fuck with this.
    /// </summary>
    float timeLastShot = -1;
    /// <summary>
    /// Cached value from global settings. -||-
    /// </summary>
    float maxRaycastDist;
    #endregion
    #region Setup
    private void Awake()
    {
        maxRaycastDist = GlobalSettings.MaxRaycastDist;
    }
    private void OnEnable()
    {
        currentError = maxError;
        Firing = false;
    }
    #endregion
    #region Main methods
    private void Update()
    {
        if (Firing)
        {
            if (Time.time - timeLastShot >= shotCooldown)
            {
                //the gun is off cooldown
                Fire();
            }
        }
        else
        {
            //accuracy decays when not firing
            currentError += accDecay * Time.deltaTime;
        }
    }
    protected void Fire()
    {
        //reset the cooldown of the gun
        timeLastShot = Time.time;
        Vector3 dir = transform.forward;
        if (currentError > 0)
        {
            float a = Random.Range(0, 360);
            dir += (transform.right * Mathf.Cos(a) + transform.up * Mathf.Sin(a)) * currentError;
            //Debug.DrawRay(transform.position, dir, Color.blue, 10);
        }
        if (Physics.Raycast(transform.position, dir, out hit, maxRaycastDist,
            GlobalSettings.TargetMasks[gameObject.layer]))
        {
            //we hit something, damage it
            Debug.DrawLine(transform.position, hit.point);
            //raise a damage event for whatever we hit
            EventBus<TakeDamage>.Raise(hit.transform.root.GetInstanceID(),
                new TakeDamage(damage, transform.root, hit.collider));
        }
        else
        {
            Debug.DrawRay(transform.position, dir * maxRaycastDist);
        }
        //accuracy increases while firing
        currentError -= accBuildUp * shotCooldown;
    }
    #endregion
}