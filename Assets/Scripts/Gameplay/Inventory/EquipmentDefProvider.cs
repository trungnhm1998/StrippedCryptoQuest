using System;
using System.Collections;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IEquipmentDefProvider
    {
        IEnumerator Provide(EquipmentInfo equipment, Action<EquipmentDef, EquipmentPrefab> initEquipmentCallback);
    }

    public class EquipmentDefProvider : MonoBehaviour, IEquipmentDefProvider
    {
        [SerializeField] private EquipmentDatabaseSO _equipmentDatabase;
        [SerializeField] private EquipmentDefineDatabase _definitionDatabase;

        public IEnumerator Provide(EquipmentInfo equipment, Action<EquipmentDef, EquipmentPrefab> initEquipmentCallback)
        {
            yield return _definitionDatabase.LoadDataById(equipment.DefinitionId);
            var def = _definitionDatabase.GetDataById(equipment.DefinitionId);
            yield return _equipmentDatabase.LoadDataById(def.PrefabId);
            var prefab = _equipmentDatabase.GetDataById(def.PrefabId);

            initEquipmentCallback(def, prefab);
        }
    }
}