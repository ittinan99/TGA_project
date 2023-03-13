using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TGA.Gameplay;
using UnityEngine;

namespace TGA.GameData 
{
    [CreateAssetMenu(fileName = "New SkillConfig", menuName = "Skill")]
    public class SkinInfoConfig : ScriptableObject
    {
        [SerializeField]
        private SkillInfo skillInfo;
    }

    [Serializable]
    public class SkillInfo
    {
        [JsonProperty("id")]
        public string id;

        [JsonProperty("class_type")]
        public DarkMagicEnum ClassType;

        [JsonProperty("skill_type")]
        public SkillTypeEnum SkillType;

        [JsonProperty("avaliable_level")]
        public int AvaliableLevel;

        [JsonProperty("previous_skill_id")]
        public string PreviousSkillId;

        [JsonProperty("cooldown")]
        public float Cooldown;

        [JsonProperty("skill_sprite_atlas")]
        public string SkillSpriteAtlas;

        [JsonProperty("vfx")]
        public string VFXPrefabId;

        [JsonProperty("animation_name")]
        public string AnimationName;
    }

    public enum SkillTypeEnum
    {
        None,
        Active,
        Passive
    }
}

