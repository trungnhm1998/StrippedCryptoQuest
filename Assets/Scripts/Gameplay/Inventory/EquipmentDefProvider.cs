using System.Collections;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IEquipmentDefProvider
    {
        IEnumerator Load(EquipmentInfo equipment);
    }

    public class EquipmentDefProvider : MonoBehaviour, IEquipmentDefProvider
    {
        [SerializeField] private EquipmentDatabaseSO _equipmentDatabase;
        [SerializeField] private EquipmentDefineDatabase _definitionDatabase;
        
        public IEnumerator Load(EquipmentInfo equipment)
        {
            yield return _definitionDatabase.LoadDataById(equipment.DefinitionId);
            var def = _definitionDatabase.GetDataById(equipment.DefinitionId);
            yield return _equipmentDatabase.LoadDataById(def.PrefabId);
            var prefab = _equipmentDatabase.GetDataById(def.PrefabId);

            equipment.Def = def;
            equipment.Prefab = prefab;
        }
    }
}