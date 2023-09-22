using System.Collections;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IEquipmentDefProvider
    {
        IEnumerator CoLoadEquipmentById(string id);
        EquipmentDef GetEquipmentDefById(string id);
    }

    public class EquipmentDefProvider : MonoBehaviour, IEquipmentDefProvider
    {
        [SerializeField] private EquipmentDefineDatabase _definitionDatabase;
        public IEnumerator CoLoadEquipmentById(string id) => _definitionDatabase.LoadDataById(id);
        public EquipmentDef GetEquipmentDefById(string id) => _definitionDatabase.GetDataById(id);
    }
}