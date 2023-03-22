using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGA.GameData;
using Photon.Pun;
using System.IO;

namespace TGA.Gameplay
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public EnemySpawnerInfo enemySpawnerConfig;

        [SerializeField]
        private List<GameObject> spawnPosList;

        [SerializeField]
        private RandomBuffController randomBuffController;

        public float preparingTime;

        float currentWavetimer = 0f;
        int currentWaveEnemyAmount = 0;
        int enemyDieAmount = 0;

        bool isInit;

        public bool IsPreparing;

        int waveAmount;
        int currentWave;
        WaveInfo currentWaveInfo;

        private Coroutine spawnEnemyCoroutine;

        PhotonView PV;

        private void Awake()
        {
            PV = GetComponent<PhotonView>();
        }

        void Start()
        {
            isInit = true;

            randomBuffController.Initialize(this);

            if (!PhotonNetwork.LocalPlayer.IsMasterClient) { return; }
            SetupAndStartSpawner();
        }

        void Update()
        {
            if (!isInit) { return; }

            if (!IsPreparing) { return; }

            currentWavetimer += Time.deltaTime;
        }

        void SetupAndStartSpawner()
        {
            waveAmount = enemySpawnerConfig.waveInfos.Count;
            currentWave = 0;
            enemyDieAmount = 0;

            spawnEnemyCoroutine = StartCoroutine(StartSpawnEnemyByWave());
        }

        IEnumerator StartSpawnEnemyByWave()
        {
            while (currentWave < waveAmount)
            {
                currentWaveInfo = enemySpawnerConfig.waveInfos[currentWave];
                currentWaveEnemyAmount = currentWaveInfo.EnemyWaveAmount;

                foreach (var spawnInfo in currentWaveInfo.SpawnInfos)
                {
                    SpawnEnemy(spawnInfo);

                    yield return new WaitForSeconds(spawnInfo.NextSpawnTime);
                }

                yield return new WaitUntil(() => currentWaveEnemyAmount == enemyDieAmount);

                Wavefinish();

                yield return new WaitForSeconds(preparingTime);
                yield return new WaitUntil(() => !IsPreparing);
            }
        }

        void SpawnEnemy(SpawnInfo spawnInfo)
        {
            for (int i = 0; i < spawnInfo.EnemyAmount; i++)
            {
                GameObject SpawnPoint = GameObject.Find(spawnInfo.SpawnPos);
                var enemy = PhotonNetwork.Instantiate(Path.Combine("Photonprefabs", "enemy", spawnInfo.EnemyId), SpawnPoint.transform.position, SpawnPoint.transform.rotation, 0, new object[] { PV.ViewID });

                enemy.GetComponent<EnemyStat>().OnEnemyDieCallback += OnEnemyDie;
            }
        }

        void OnEnemyDie()
        {
            enemyDieAmount++;
        }

        void Wavefinish()
        {
            Debug.Log($"============= Wave {currentWave + 1} / {waveAmount} Complete ==============");

            currentWave++;
            currentWavetimer = 0f;
            enemyDieAmount = 0;

            randomBuffController.PopulateRandomBuffCard();
        }
    }

}
