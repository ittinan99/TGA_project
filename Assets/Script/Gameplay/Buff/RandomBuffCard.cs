using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGA.Gameplay
{
    [CreateAssetMenu(fileName = "New Buff Card", menuName = "BuffCard")]
    public class RandomBuffCard : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Description;
        public StatusTypeEnum StatusType;
        public StatusSubTypeEnum StatusSubType;
        public TargetEnum TargetType;
        public Sprite Sprite;
        public float StatusRatio;
        public int Rarity;
    }

    public enum StatusTypeEnum
    {
        Buff,
        Debuff
    }

    public enum StatusSubTypeEnum
    {
        Speed,
        Health,
        Attack,
        Money,
        SpawnRate
    }

    public enum TargetEnum
    {
        Player,
        MeleeEnemy,
        RangeEnemy,
        Boss,
        Shop
    }
}

