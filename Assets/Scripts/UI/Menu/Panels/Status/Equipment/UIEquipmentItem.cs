using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    // wrapper for UIEquipment
    public class UIEquipmentItem : MonoBehaviour
    {
        [SerializeField] private UIEquipment _equipment;

        public void Init(EquipmentInfo equipment)
        {
            _equipment.Init(equipment);
        }
    }
}