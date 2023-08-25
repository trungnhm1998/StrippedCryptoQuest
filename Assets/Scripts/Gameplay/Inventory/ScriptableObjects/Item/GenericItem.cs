using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    public class GenericItem : ScriptableObject
    {
        [field: Header("Item Generic Data")]
        [field: SerializeField] public string ID { get; private set; }

        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor.ImportBatchData"/>
        /// </summary>
        /// <param name="id"></param>
        public void Editor_SetID(string id)
        {
            ID = id;
        }
#endif
    }
}