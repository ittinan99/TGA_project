using System.Collections;
using System.Collections.Generic;
using TGA.GameData;
using TGA.Gameplay;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillConfig", menuName = "SkillConfig")]
public class SkillInfoConfig : ScriptableObject
{
    public string id;

    public DarkMagicEnum ClassType;

    public SkillTypeEnum SkillType;

    public int AvaliableLevel;

    public string PreviousSkillId;

    public float Cooldown;

    public Sprite SkillSprite;

    public string VFXPrefabId;

    public string AnimationName;
}
