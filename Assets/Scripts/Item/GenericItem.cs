using CryptoQuest.Item.Consumable;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Item
{
    public class GenericItem : SerializableScriptableObject
    {
        [field: Header("Item Generic Data")]
        [field: SerializeField] public string ID { get; private set; }

        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public AssetReferenceT<Sprite> Image { get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor.ImportBatchData"/>
        /// </summary>
        /// <param name="id"></param>
        public void Editor_SetID(string id)
        {
            ID = id;
        }

        // set display name an description editor

        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor.ImportBatchData"/>
        /// </summary>
        /// <param name="displayName"></param>
        public void Editor_SetDisplayName(LocalizedString displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor.ImportBatchData"/>
        /// </summary>
        /// <param name="description"></param>
        public void Editor_SetDescription(LocalizedString description)
        {
            Description = description;
        }
#endif
    }
}