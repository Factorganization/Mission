using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Utils.Listing
{
    public class ActorPool<T> : IEnumerable<T> where T : MonoBehaviour
    {
        #region properties

        public Queue<T> ActorQueue { get; private set; }

        public int Count => ActorQueue.Count;
        
        public Type Type => typeof(T);
        
        #endregion

        #region constructors

        public ActorPool(Transform parentTransform = null)
        {
            ActorQueue = new Queue<T>();
            
            _poolParent = parentTransform;
        }
        
        public ActorPool(PoolData<T> poolData, Transform parentTransform = null)
        {
            _poolParent = parentTransform;

            Init(poolData.actorPrefab, poolData.poolCount, poolData.pooledPosition);
        }
        
        #endregion
        
        #region methodes

        private void Init(T prefab, int count, Vector3 position)
        {
            ActorQueue = new Queue<T>();

            for (var i = 0; i < count; i++)
            {
                var o = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, _poolParent);
                ActorQueue.Enqueue(o);
            }
        }
        
        public T Peek() => ActorQueue.Peek();
        
        public void Enqueue(T actor) => ActorQueue.Enqueue(actor);
        
        public T Dequeue() => ActorQueue.Dequeue();

        public T Pool()
        {
            var a = Dequeue();
            Enqueue(a);
            
            return a;
        }
        
        public IEnumerator<T> GetEnumerator() => ActorQueue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        #endregion

        #region fields
        
        private readonly Transform _poolParent;

        #endregion
    }

    [Serializable]
    public struct PoolData<T> where T : MonoBehaviour
    {
        #region constructors
        
        public PoolData(T actorPrefab, int poolCount, Vector3 pooledPosition)
        {
            this.actorPrefab = actorPrefab;
            this.poolCount = poolCount;
            this.pooledPosition = pooledPosition;
        }
        
        #endregion
        
        #region fields
        
        public T actorPrefab;
        
        public Vector3 pooledPosition;
        
        public int poolCount;
        
        #endregion
    }
}