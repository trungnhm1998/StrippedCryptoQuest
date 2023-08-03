using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest
{
    public enum ECharacterClass
    {
        MainCharacter = 0,
        Hero = 1,
        Archer = 2,
        Priest = 3
    }
    
    public enum ECharacterSkill
    {
        SelfCast = 0, 
        TargetCast = 1
    }

    [CreateAssetMenu(menuName = "Crypto Quest/Ability/Skill")]
    public class SkillSO : ScriptableObject
    {
        public ECharacterClass CharacterClass;
        public ECharacterSkill TypeOfSkill;
        public Sprite Icon;
        public LocalizedString Name;
        public LocalizedString Description;
        public float Mana;
    }
}
