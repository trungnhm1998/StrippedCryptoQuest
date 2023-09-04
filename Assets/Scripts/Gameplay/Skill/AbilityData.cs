using CryptoQuest.Gameplay.Battle.Core;
using UnityEngine;

namespace CryptoQuest.Gameplay.Skill
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability/Ability Data", fileName = "Ability Data")]
    public class AbilityData : ScriptableObject
    {
        public SkillInfo SkillInfo;
    }
}