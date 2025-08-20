using EventBus;
using UnityEngine;
using UnityEngine.Events;

namespace HP
{
    public class HPComponent : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; set; }
        [field: SerializeField] public int CurrentHealth { get; protected set; }
        /// <summary>
        /// Fires when this entity dies.
        /// </summary>
        public UnityEvent OnDeath;
        public UnityEvent<int> OnDamageTaken;
        protected void Awake()
        {
            //add damage binding for this entity
            EventBus<TakeDamage>.AddActions(transform.GetInstanceID(), TakeDamage);
        }
        protected void OnEnable()
        {
            CurrentHealth = MaxHealth;
        }
        /// <summary>
        /// Take damage. Invoke OnDeath when HP reaches 0. 
        /// </summary>
        /// <param name="dmg">Damage event.</param>
        public void TakeDamage(TakeDamage dmg)
        {
            CurrentHealth -= dmg.Damage;
            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
                return;
            }
            OnDamageTaken?.Invoke(CurrentHealth);
        }
        protected void OnDestroy()
        {
            //clear this binding from the event bus.
            EventBus<TakeDamage>.RemoveActions(transform.GetInstanceID(), TakeDamage);
        }
    }
}