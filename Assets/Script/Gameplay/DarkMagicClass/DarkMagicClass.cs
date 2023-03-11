using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGA.Gameplay
{
    [CreateAssetMenu(fileName = "New Dark Magic Class", menuName = "DarkMagicClass")]
    public class DarkMagicClass : ScriptableObject
    {
        [SerializeField]
        private string className;
        public string ClassName => className;

        [SerializeField]
        private DarkMagicEnum classType;
        public DarkMagicEnum ClassType => classType;

        [SerializeField]
        private GameObject mainWeaponPrefab;
        public GameObject MainWeaponPrefab => mainWeaponPrefab;

        [SerializeField]
        private GameObject subWeaponPrefab;
        public GameObject SubWeaponPrefab => subWeaponPrefab;

        [SerializeField]
        private float maxHealth;
        public float MaxHealth => maxHealth;

        [SerializeField]
        private float maxStamina;
        public float MaxStamina => maxStamina;

        [SerializeField]
        private float speed;
        public float Speed => speed;
    }

    public enum DarkMagicEnum
    {
        None,
        Attacker,
        Defender,
        Supporter
    }
}

