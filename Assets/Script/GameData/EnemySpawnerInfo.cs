using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TGA.Gameplay;
using UnityEngine;

namespace TGA.GameData
{
    [CreateAssetMenu(fileName = "New EnemySpawnerConfig", menuName = "EnemySpawner")]
    public class EnemySpawnerConfig : ScriptableObject
    {
        public EnemySpawnerInfo enemySpawnerInfo;
    }

    [Serializable]
    public class EnemySpawnerInfo 
    {
        [JsonProperty("id")]
        public string id;

        [JsonProperty("wave_infos")]
        public List<WaveInfo> waveInfos;
    }

    [Serializable]
    public class WaveInfo
    {
        [JsonProperty("id")]
        public string id;

        [JsonProperty("enemy_wave_amount")]
        public int EnemyWaveAmount;

        [JsonProperty("spawn_infos")]
        public List<SpawnInfo> SpawnInfos;
    }

    [Serializable]
    public class SpawnInfo
    {
        [JsonProperty("id")]
        public string EnemyId;

        [JsonProperty("enemy_amount")]
        public int EnemyAmount;

        [JsonProperty("spawn_pos")]
        public string SpawnPos;

        [JsonProperty("spawn_time")]
        public float NextSpawnTime;
    }
}
