using System.Collections.Generic;

namespace TGA.Utilities
{
    public abstract class Singleton
    {
        protected static readonly List<Singleton> singletonList = new List<Singleton>();

        /// <summary>
        /// clear all of the singleton
        /// </summary>
        public static void ClearAllSingleton()
        {
            for (int i = 0; i < singletonList.Count; i++)
            {
                Singleton singleton = singletonList[i];
                singleton.Clear();
            }

            singletonList.Clear();
        }

        /// <summary>
        /// clear this singleton
        /// </summary>
        protected abstract void Clear();
    }

    public abstract class Singleton<T> : Singleton where T : Singleton<T>, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    singletonList.Add(instance);
                }
                return instance;
            }
        }

        public static bool HasInstance { get { return instance != null; } }

        protected override void Clear()
        {
            instance = null;
        }
    }
}

