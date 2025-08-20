namespace Spawning
{
    /// <summary>
    /// Represents something that can be spawned.
    /// </summary>
    /// <typeparam name="Id">The type of id by which the object will be categorized.</typeparam>
    public interface ISpawnable<Id>
    {
        /// <summary>
        /// Execute this when the object is first created.
        /// </summary>
        /// <param name="data">An ObjectData instance that the object needs in 
        /// order to function.</param>
        public void Init(ObjectData<Id> data);
    }
    public interface ISpawner<Id, T> where T : ISpawnable<Id>
    {
        /// <summary>
        /// Create a new object from an ObjectData instance.
        /// </summary>
        /// <param name="data">The data which will be used to create a new object.</param>
        /// <returns>The new object that was created.</returns>
        public T Create(ObjectData<Id> data);
    }
}