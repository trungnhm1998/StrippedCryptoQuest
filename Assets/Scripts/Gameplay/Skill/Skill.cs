using CryptoQuest.Gameplay.Battle.Core;
using UnityEngine;

namespace CryptoQuest.Gameplay.Skill
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability/Ability Data", fileName = "Skill")]
    public class Skill : ScriptableObject
    {
        public SkillInfo SkillInfo;
    }
}