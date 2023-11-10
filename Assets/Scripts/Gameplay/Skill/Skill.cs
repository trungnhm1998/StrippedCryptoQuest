using System;
using CryptoQuest.Gameplay.Battle.Core;
using UnityEngine;

namespace CryptoQuest.Gameplay.Skill
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/[Obsolete] Ability Data", fileName = "Skill")]
    [Obsolete]
    public class Skill : ScriptableObject
    {
        public SkillInfo SkillInfo;
    }
}