using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Ability
{
    [CreateAssetMenu(menuName = "Create SkillTargetType", fileName = "SkillTargetType", order = 0)]
    public class SkillTargetType : GenericEventChannelSO<CastableAbility> { }
}