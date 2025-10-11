using UnityEngine;

namespace Shared.Utils.Listing
{
    public static class Pooler
    {
        #region constructors

        public static void Pool<T, T1>(ActorPool<T> actorPool, T1[] array) where T : MonoBehaviour where T1 : MonoBehaviour
        {
            foreach (var mono in array)
            {
                if (!typeof(T).IsAssignableFrom(mono.GetType()))
                    continue;
                
                actorPool.Enqueue(mono as T);
            }
        }
        
        public static void Pool<T, T1> (ActorList<T> actorList, T1[] array) where T : class where T1 : MonoBehaviour
        {
            foreach (var mono in array)
            {
                if (!typeof(T).IsAssignableFrom(mono.GetType()))
                    return;
                
                actorList.Add(mono as T);
            }
            
            actorList.UpdateList();
        }

        public static void PoolWithAttribute<T, T1> (ActorPool<T> actorPool, T1[] array) where T : MonoBehaviour where T1 : MonoBehaviour
        {
            foreach (var mono in array)
            {
                var type = mono.GetType();
                
                if (type.GetCustomAttributes(typeof(Pooled), false).Length <= 0)
                    continue;
                
                if (!typeof(T).IsAssignableFrom(type))
                    return;
                
                actorPool.Enqueue(mono as T);
            }
        }
        
        public static void PoolWithAttribute<T, T1> (ActorList<T> actorList, T1[] array) where T : class where T1 : MonoBehaviour
        {
            foreach (var mono in array)
            {
                var type = mono.GetType();
                
                if (type.GetCustomAttributes(typeof(Pooled), false).Length <= 0)
                    continue;
                
                if (!typeof(T).IsAssignableFrom(type))
                    return;
                
                actorList.Add(mono as T);
            }
            
            actorList.UpdateList();
        }

        #endregion
    }
}