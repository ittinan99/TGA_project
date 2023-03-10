using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGA.Utilities;
using TGA.GameData;
using TGA.SceneManagement;

namespace TGA.Gameplay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private SceneController sceneController;

        [SerializeField]
        private GameDataManager gameDataManager;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
        }

        private void Start()
        {
            startInitialize();
        }

        void Update()
        {

        }

        public void startInitialize()
        {
            Debug.Log("============================ START INITIALIZE GAME =================================");
            StartCoroutine(initializeGame());
        }

        IEnumerator initializeGame()
        {
            gameDataManager.StartInitialize();
            yield return new WaitUntil(() => gameDataManager.IsIniialize);

            sceneController = SharedContext.Instance.Get<SceneController>();
            sceneController.LoadSceneAsync("TestOnline");
        }
    }
}

