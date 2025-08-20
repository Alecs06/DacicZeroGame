using System;
using System.Collections.Generic;

namespace EventBus
{
    public static class EventBus<T> where T : IEvent
    {
        static Dictionary<int, EventBinding<T>> bindings = new();
        static void Clear()
        {
            bindings.Clear();
        }
        /// <summary>
        /// Raise a binding.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="event">The value of the IEvent parameter.</param>
        /// <returns>True if the binding was found and raised.</returns>
        public static bool Raise(int bindingId, T @event)
        {
            EventBinding<T> binding;
            if (bindings.TryGetValue(bindingId, out binding))
            {
                binding.Invoke(@event);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add a new binding.
        /// </summary>
        /// <param name="id">The id of the new binding. Defaults to 0.</param>
        /// <returns>True if the binding was successfully added.</returns>
        public static bool AddBinding(int id = 0)
        {
            return bindings.TryAdd(id, new());
        }
        /// <summary>
        /// Clear and then remove an existing binding.
        /// </summary>
        /// <param name="id">The id of the binding to remove.</param>
        /// <returns>True if the binding was successfully removed.</returns>
        public static bool RemoveBinding(int id = 0)
        {
            EventBinding<T> binding;
            if (bindings.TryGetValue(id, out binding))
            {
                binding.Clear();
                binding = null;
                bindings.Remove(id);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Clears an existing binding.
        /// </summary>
        /// <param name="id">The id of the binding to clear.</param>
        /// <returns>True if the binding was found.</returns>
        public static bool ClearBinding(int id = 0)
        {
            EventBinding<T> binding;
            if (bindings.TryGetValue(id, out binding))
            {
                binding.Clear();
                binding = null;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add actions to a binding. By default will attempt to add a new binding if none is found, and add the actions to it.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <param name="addBinding">If true, will attempt to add a new binding if none is found, and add the actions to it. Defaults to true.</param>
        /// <returns>True if the binding was found, or if it was successfully added.</returns>
        public static bool AddActions(int bindingId, Action<T> action = null, Action actionNoArgs = null, bool addBinding = true)
        {
            EventBinding<T> e;
            if (bindings.TryGetValue(bindingId, out e))
            {
                e.Add(action);
                e.Add(actionNoArgs);
                return true;
            }
            if (bindings.TryAdd(bindingId, new()))
            {
                e = bindings[bindingId];
                e.Add(action);
                e.Add(actionNoArgs);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes actions from a binding.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <returns></returns>
        public static bool RemoveActions(int bindingId, Action<T> action = null,
            Action actionNoArgs = null)
        {
            EventBinding<T> e;
            if (bindings.TryGetValue(bindingId, out e))
            {
                e.Remove(action);
                e.Remove(actionNoArgs);
                return true;
            }
            return false;
        }
    }
}