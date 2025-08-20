using EventBus;
using UnityEngine;
namespace Interaction
{
    public struct InteractionEvent : IEvent
    {
        public Transform Interactor;
        public InteractionEvent(Transform interactor)
        {
            Interactor = interactor;
        }
    }
}