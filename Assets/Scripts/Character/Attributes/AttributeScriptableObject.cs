using UnityEngine;
using UnityEngine.Localization;
using CoreAttributeSO = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    /// <summary>
    /// Base Attribute
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Attribute")]
    public class AttributeScriptableObject : CoreAttributeSO
    {
        [SerializeField] private Sprite _icon;
        [field: SerializeField] public LocalizedString DisplayName { get; private set; }

    }
}