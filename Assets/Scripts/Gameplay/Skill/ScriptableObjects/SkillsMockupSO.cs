using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Skill.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability/List Skill")]
    public class SkillsMockupSO : ScriptableObject
    {
        public List<SkillInformation> Skills;
    }
}
