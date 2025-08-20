using System;

namespace EventBus
{
    public interface IEvent { }

    public class EventBinding<T> where T : IEvent
    {
        Action<T> onEvent = _ => { };
        Action onEventNoArgs = () => { };

        public EventBinding() { }
        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        public void Add(Action onEvent) => onEventNoArgs += onEvent;
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

        public void Add(Action<T> onEvent) => this.onEvent += onEvent;
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;

        public void Invoke(T @event)
        {
            onEvent.Invoke(@event);
            onEventNoArgs.Invoke();
        }

        public void Clear()
        {
            onEvent = null;
            onEventNoArgs = null;
        }
    }
}