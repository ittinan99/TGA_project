using System.Collections;
using UnityEngine;

namespace TGA.Utilities
{
    public class CoroutineHelper : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SharedContext.Instance.Add(this);
        }

        /// <summary>
        /// Get CoroutineHelper
        /// </summary>
        /// <returns></returns>
        public static CoroutineHelper Get()
        {
            var coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();

#if UNITY_EDITOR
            if(coroutineHelper == null)
            {
                var go = new GameObject();
                go.name = "CoroutineHelper";
                coroutineHelper = go.AddComponent<CoroutineHelper>();
            }
#endif

            Debug.Assert(coroutineHelper, "CoroutineHelper not found!!");

            return coroutineHelper;
        }

        /// <summary>
        /// Play coroutine
        /// </summary>
        /// <param name="coroutine">IEnumerator function</param>
        /// <returns>Played Coroutine</returns>
        public Coroutine Play(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        /// <summary>
        /// Stop coroutine
        /// </summary>
        /// <param name="coroutine">IEnumerator function</param>
        public void Stop(IEnumerator coroutine)
        {
            if (this != null && coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Stop coroutine
        /// </summary>
        /// <param name="coroutine">Played coroutine</param>
        public void Stop(Coroutine coroutine)
        {
            if (this != null && coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Stop played coroutine and play new coroutine
        /// Note: Use to stop previous coroutine before play new one
        /// </summary>
        /// <param name="stopCoroutine">Coroutine that want to stop(previous coroutine)</param>
        /// <param name="playCoroutine">Coroutine that want to play(new coroutine)</param>
        /// <returns></returns>
        public Coroutine Restart(Coroutine stopCoroutine, IEnumerator playCoroutine)
        {
            Stop(stopCoroutine);
            return Play(playCoroutine);
        }
    }
}