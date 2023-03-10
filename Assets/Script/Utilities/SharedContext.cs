using System;
using System.Collections.Generic;
using UnityEngine;

namespace TGA.Utilities
{
    /// <summary>
    /// holding references to shared objects
    /// </summary>
    public class SharedContext : Singleton<SharedContext>
    {
        private readonly Dictionary<Type, object> objects = new Dictionary<Type, object>();

        public SharedContext()
        {
        }

        /// <summary>
        /// add object to shared context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Add<T>(T obj) where T : class
        {
            Type key = typeof(T);
            if (!objects.ContainsKey(key))
            {
                objects.Add(key, obj);
            }
            else
            {
                Debug.Log($"Object of same key detected: {key.AssemblyQualifiedName}");
            }
        }

        /// <summary>
        /// remove object from shared context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>(T obj) where T : class
        {
            this.Remove<T>();
        }

        /// <summary>
        /// remove object of specific type from shared context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>() where T : class
        {
            objects.Remove(typeof(T));
        }

        /// <summary>
        /// get object of specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Get<T>() where T : class
        {
            Type key = typeof(T);
            objects.TryGetValue(typeof(T), out object result);
            return result as T;
        }

        /// <summary>
        /// try getting object of specific type
        /// </summary>
        public bool TryGet<T>(out T resultT) where T : class
        {
            bool result = objects.TryGetValue(typeof(T), out object resultObj);
            resultT = resultObj as T;
            return result;
        }
    }
}
