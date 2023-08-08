using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using UnityEngine.Search;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Weapon Item", menuName = "Crypto Quest/Inventory/Weapon Item")]
    public class WeaponSO : EquipmentSO
    {
#if UNITY_EDITOR
        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor"/> 
        /// </summary>
        /// <param name="weaponType"></param>
        public void Editor_SetEquipmentType(WeaponTypeSO weaponType)
        {
            base.Editor_SetEquipmentType(weaponType);
        }
#endif
    }
}