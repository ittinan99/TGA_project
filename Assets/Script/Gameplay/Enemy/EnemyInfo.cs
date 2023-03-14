using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGA.Gameplay
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class EnemyInfo : ScriptableObject
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
        private float walkSpeed;
        public float WalkSpeed => walkSpeed;

        [SerializeField]
        private float runSpeed;
        public float RunSpeed => runSpeed;


        [SerializeField]
        private float attackDistance;
        public float AttackDistance => attackDistance;

        [SerializeField]
        private float detectRange;
        public float DetectRange => detectRange;

        [SerializeField]
        private float aggroRange;
        public float AggroRange => aggroRange;

        [SerializeField]
        private List<AnimationClip> attackAnimation;
        public List<AnimationClip> AttackAnimation => attackAnimation;

    }

    public enum EnemyTypeEnum
    {
        None,
        Melee,
        Ranged,
        Tank
    }
}
