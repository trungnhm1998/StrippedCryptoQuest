using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public interface IEvadable
    {
        bool TryEvade();
    }

    [DisallowMultipleComponent]
    public class EvadeBehaviour : CharacterComponentBase, IEvadable
    {
        [SerializeField] private AttributeScriptableObject _evasionAttribute;

        public bool TryEvade()
        {
            Character.AttributeSystem.TryGetAttributeValue(_evasionAttribute, out var evasionValue);
            var random = Random.value;
            var isEvaded = random <= evasionValue.CurrentValue / 100f;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"EvadeBehaviour::TryEvade: Evasion: {evasionValue.CurrentValue / 100f}; random {random}"
                      + $"evaded {isEvaded}");
#endif
            return isEvaded;
        }
    }
}