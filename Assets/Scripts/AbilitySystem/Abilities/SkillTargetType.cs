using System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Create SkillTargetType", fileName = "SkillTargetType", order = 0)]
    public class SkillTargetType : GenericEventChannelSO<CastSkillAbility>
    {
        [Flags]
        public enum Type
        {
            Self = 0,
            Ally = 1,
            AllAllies = 1 << 1,
            SameTeam = Self | Ally | AllAllies,
            Enemy = 1 << 2,
            AllEnemies = 1 << 3,
            EnemyGroup = 1 << 4,
        }
        
        [FormerlySerializedAs("_targetType")] [SerializeField] private Type _type;
        public Type Target => _type;
    }
}