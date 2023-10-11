using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Create SkillTargetType", fileName = "SkillTargetType", order = 0)]
    public class SkillTargetType : GenericEventChannelSO<CastSkillAbility> { }
}