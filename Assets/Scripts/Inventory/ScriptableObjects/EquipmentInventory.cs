using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Inventory.ScriptableObjects
{
    /// <summary>
    /// Use single SO for cross scene inventory
    /// </summary>
    public class EquipmentInventory : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<IEquipment> Equipments { get; private set; } = new();

        public void Add(IEquipment item) => Equipments.Add(item);

        public void Remove(IEquipment ctxItem)
        {
            for (var index = 0; index < Equipments.Count; index++)
            {
                var item = Equipments[index];
                if (item.Id != ctxItem.Id) continue;
                Equipments.RemoveAt(index);
                return;
            }
        }

        public IEquipment FindEquipment(int id)
        {
            for (var index = 0; index < Equipments.Count; index++)
            {
                var equipment = Equipments[index];
                if (equipment.Id == id)
                    return equipment;
            }

            return null;
        }

        public bool TryFindEquipment(int id, out IEquipment equipment)
        {
            equipment = FindEquipment(id);
            return equipment != null;
        }
    }
}