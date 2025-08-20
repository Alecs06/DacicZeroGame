using Spawning;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    /// <summary>
    /// Represents something that is poolable.
    /// </summary>
    /// <typeparam name="Id">The type of id by which the object will be categorized 
    /// in a multipool.</typeparam>
    public interface IPoolable<Id> : ISpawnable<Id>
    {
        /// <summary>
        /// The id by which the object will be categorized in a multipool.
        /// </summary>
        Id ID { get; }
        /// <summary>
        /// Reset this poolable object.
        /// </summary>
        void ResetObject();
    }
    /// <summary>
    /// Handles pooling for multiple categories of objects.
    /// </summary>
    /// <typeparam name="Id">The type of id by which the objects will be categorized.</typeparam>
    /// <typeparam name="T">The type of objects that will be stored.</typeparam>
    public class MultiPool<Id, T> where T : IPoolable<Id>
    {
        protected Dictionary<Id, Stack<T>> pools = new();
        /// <summary>
        /// Try to get an object from the pool.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <param name="result">The object, if a valid one was found.</param>
        /// <returns>True if an object was found.</returns>
        public bool TryGet(Id id, out T result)
        {
            result = default;
            Stack<T> pool;
            if (pools.TryGetValue(id, out pool))
            {
                if (pool.Count > 0)
                {
                    result = pool.Pop();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Release an object into its coresponding pool. Will create a new pool if none is found.
        /// </summary>
        /// <param name="entity">The object to be released.</param>
        public void Release(T entity)
        {
            Stack<T> pool;
            if (!pools.TryGetValue(entity.ID, out pool))
            {
                pool = new();
                pools.Add(entity.ID, pool);
            }
            pool.Push(entity);
        }
        /// <summary>
        /// Clears and deletes underlying pools.
        /// </summary>
        public void Clear()
        {
            Debug.Log($"Clearing multipool of {typeof(Id)}.");
            foreach (var pool in pools.Values)
            {
                pool.Clear();
            }
            pools.Clear();
        }
    }
}