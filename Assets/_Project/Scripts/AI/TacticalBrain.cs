using EventBus;
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
        protected RequestPositionEvent? @event;
        protected float remainingRequestTime;
        protected void Awake()
        {
            EventBus<RequestPositionEvent>.AddActions(transform.root.GetInstanceID(), ReceivePositionRequest);
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
        private void OnDrawGizmos()
        {
            if (PatrolPoints.Length > 1)
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
    }
}