using EventBus;
using System.Collections.Generic;
using UnityEngine;
namespace AI
{
    public class TacticalBrain : MonoBehaviour
    {
        public Vector3? RequestedPosition
        {
            get
            {
                if (@event != null)
                {
                    return @event.Value.Position;
                }
                return null;
            }
        }
        public Transform[] PatrolPoints;
        [SerializeField] protected float alertRadius = 10, expirationTime = 20;
        [SerializeField] protected int priority = 1;
        #region Other fields
        protected RequestPositionEvent? @event;
        protected float remainingRequestTime;
        protected Collider[] colliders = new Collider[GlobalSettings.MaxTargets];
        #endregion
        protected void Awake()
        {
            EventBus<RequestPositionEvent>.AddActions(transform.root.GetInstanceID(), ReceivePositionRequest);
        }
        protected void OnDestroy()
        {
            EventBus<RequestPositionEvent>.RemoveActions(transform.root.GetInstanceID(), ReceivePositionRequest);
        }
        protected void Update()
        {
            if (@event != null)
            {
                remainingRequestTime -= Time.deltaTime;
                if (remainingRequestTime < 0)
                {
                    @event = null;
                }
            }
        }
        public void ReceivePositionRequest(RequestPositionEvent request)
        {
            if (@event == null || @event.Value.Priority < request.Priority)
            {
                @event = request;
                remainingRequestTime = request.ExpirationTime;
            }
        }
        public void SendAlert(Vector3 vector3)
        {
            int nr = Physics.OverlapSphereNonAlloc(transform.position, alertRadius,
                colliders, 1 << gameObject.layer);
            var newEvent = new RequestPositionEvent(vector3, expirationTime, priority);
            HashSet<int> transformSet = new() { transform.root.GetInstanceID() };
            for (int i = 0; i < nr; i++)
            {
                var tr = colliders[i].transform.root.GetInstanceID();
                if (transformSet.Add(tr))
                {
                    EventBus<RequestPositionEvent>.Raise(tr, newEvent);
                }
            }
        }
        #region Debugging
        private void OnDrawGizmos()
        {
            if (PatrolPoints != null && PatrolPoints.Length > 1)
            {
                Gizmos.color = Color.magenta;
                for (int i = 0; i < PatrolPoints.Length - 1; i++)
                {
                    if (PatrolPoints[i] != null && PatrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(PatrolPoints[i].position, PatrolPoints[i + 1].position);
                    }
                }
                Gizmos.DrawLine(PatrolPoints[PatrolPoints.Length - 1].position, PatrolPoints[0].position);
            }
        }
        #endregion
    }
}