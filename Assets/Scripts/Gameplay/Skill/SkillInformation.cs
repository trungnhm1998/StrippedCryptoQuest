using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Gameplay.Skill
{
    [Serializable]
    public class SkillInformation
    {
        public AbilitySO abilitySo;

        public SkillInformation(AbilitySO abilitySO)
        {
            abilitySo = abilitySO;
        }
    }
}