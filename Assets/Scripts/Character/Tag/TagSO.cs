using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Character.Tag
{
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Tag", fileName = "Create Tag", order = 0)]
    public class TagSO : TagScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public LocalizedString AddedMessage { get; private set; }
        [field: SerializeField] public LocalizedString AffectMessage { get; private set; }
        [field: SerializeField] public LocalizedString RemoveMessage { get; private set; }
    }
}