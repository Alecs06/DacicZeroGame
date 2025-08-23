using EventBus;
using UnityEngine;

namespace AI
{
    public struct RequestPositionEvent : IEvent
    {
        public Vector3 Position;
        public float ExpirationTime;
        public int Priority;
        public RequestPositionEvent(Vector3 position, float expirationTime = 20, int priority = 1)
        {
            Position = position;
            ExpirationTime = expirationTime;
            Priority = priority;
        }
    }
}