using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGA.Gameplay
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class EnemyInfo : MonoBehaviour
    {
        [SerializeField]
        private string id;
        public string Id => id;

        [SerializeField]
        private string enemyName;
        public string EnemyName => enemyName;

        [SerializeField]
        private EnemyTypeEnum enemyType;
        public EnemyTypeEnum EnemyType => enemyType;

        [SerializeField]
        private string enemyPrefabId;
        public string EnemyPrefabId => enemyPrefabId;

        [SerializeField]
        private float maxHealth;
        public float MaxHealth => maxHealth;

        [SerializeField]
        private float maxSpeed;
        public float MaxSpeed => maxSpeed;
    }

    public enum EnemyTypeEnum
    {
        None,
        Melee,
        Ranged,
        Tank
    }
}
