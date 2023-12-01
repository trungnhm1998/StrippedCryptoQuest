using System;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UICharacterEquipmentSlot : MonoBehaviour
    {
        [field: SerializeField] public ESlot SlotType { get; private set; }
        [field: SerializeField] public EEquipmentCategory Category { get; private set; }

        [SerializeField] private UIEquipment _equipment;

        public void Init(IEquipment equipment)
        {
            if (!equipment.IsValid()) return;
            _equipment.Init(equipment);
            _equipment.gameObject.SetActive(true);
        }

        public void Reset()
        {
            _equipment.Reset();
            _equipment.gameObject.SetActive(false);
        }

        public void OnPressed()
        {
            Pressed?.Invoke(this);
        }

        public static event Action<UICharacterEquipmentSlot> Pressed;
    }
}