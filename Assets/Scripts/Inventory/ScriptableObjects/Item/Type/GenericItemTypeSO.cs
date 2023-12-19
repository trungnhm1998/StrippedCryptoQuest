using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Inventory.ScriptableObjects.Item.Type
{
    public class GenericItemTypeSO : ScriptableObject
    {
        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
    }
}