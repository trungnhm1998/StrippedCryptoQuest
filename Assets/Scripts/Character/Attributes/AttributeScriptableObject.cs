using UnityEngine;
using CoreAttributeSO = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    [CreateAssetMenu(menuName = "CryptoQuest/Character/Attribute")]
    public class AttributeScriptableObject : CoreAttributeSO
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [SerializeField] private string _shortName;
        [SerializeField] private Sprite _icon;
    }
}