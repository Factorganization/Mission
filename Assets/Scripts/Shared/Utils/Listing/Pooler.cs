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
                if (mono is not T t)
                    continue;
                
                actorPool.Enqueue(t);
            }
        }
        
        public static void Pool<T, T1> (ActorList<T> actorList, T1[] array) where T : class where T1 : MonoBehaviour
        {
            foreach (var mono in array)
            {
                if (mono is not T t)
                    continue;
                
                actorList.Add(t);
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
                
                if (mono is not T t)
                    continue;
                
                actorPool.Enqueue(t);
            }
        }
        
        public static void PoolWithAttribute<T, T1> (ActorList<T> actorList, T1[] array) where T : class where T1 : MonoBehaviour
        {
            foreach (var mono in array)
            {
                var type = mono.GetType();
                
                if (type.GetCustomAttributes(typeof(Pooled), false).Length <= 0)
                    continue;
                
                if (mono is not T t)
                    continue;
                
                actorList.Add(t);
            }
            
            actorList.UpdateList();
        }

        #endregion
    }
}