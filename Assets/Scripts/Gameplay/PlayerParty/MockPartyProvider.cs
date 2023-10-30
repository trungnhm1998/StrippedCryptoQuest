using CryptoQuest.Character.Hero;
using CryptoQuest.Item.Equipment;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class MockPartyProvider : MonoBehaviour, IPartyProvider
    {
        [SerializeField] private MockParty _mockParty;

        public HeroSpec[] GetParty() => _mockParty.GetParty();
        public void SetParty(HeroSpec[] newSpecs) => _mockParty.SetParty(newSpecs);

        #region SaveSystem
        public IEnumerator CoFromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var heroSpecsData = JsonConvert.DeserializeObject<List<string>>(json);
                var heroSpecs = new List<HeroSpec>();
                foreach (var spec in heroSpecsData)
                {
                    var heroData = JsonConvert.DeserializeObject<HeroSpecData>(spec);
                    var heroSpec = new HeroSpec()
                    {
                        Id = heroData.Id,
                        Experience = heroData.Experience,
                        Equipments = new()
                        {
                            Slots = new()
                        }
                    };

                    if (!string.IsNullOrEmpty(heroData.UnitSOGuid))
                    {
                        var soHandle = Addressables.LoadAssetAsync<UnitSO>(heroData.UnitSOGuid);
                        yield return soHandle;
                        if (soHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            heroSpec.Unit = soHandle.Result;
                        }
                    }
                    var equipmentSlotsData = JsonConvert.DeserializeObject<List<EquipmentSlotData>>(heroData.EquipmentSlots);
                    foreach (var slotData in equipmentSlotsData)
                    {
                        EquipmentInfo equipment = null;
                        if (slotData.Equipment != null)
                        {
                            equipment = new EquipmentInfo(slotData.Equipment.DefinitionId, slotData.Equipment.Level)
                            {
                                Id = slotData.Equipment.Id
                            };
                            if (!string.IsNullOrEmpty(slotData.Equipment.DefGuid))
                            {
                                var defSoHandle = Addressables.LoadAssetAsync<EquipmentDef>(slotData.Equipment.DefGuid);
                                yield return defSoHandle;
                                if (defSoHandle.Status == AsyncOperationStatus.Succeeded)
                                {
                                    equipment.Def = defSoHandle.Result;
                                }
                            }
                            if (!string.IsNullOrEmpty(slotData.Equipment.PrefabGuid))
                            {
                                var prefabSoHandle = Addressables.LoadAssetAsync<EquipmentPrefab>(slotData.Equipment.PrefabGuid);
                                yield return prefabSoHandle;
                                if (prefabSoHandle.Status == AsyncOperationStatus.Succeeded)
                                {
                                    equipment.Prefab = prefabSoHandle.Result;
                                }
                            }
                        }
                        var slot = new EquipmentSlot()
                        {
                            Type = slotData.Type,
                            Equipment = equipment
                        };
                        heroSpec.Equipments.Slots.Add(slot);
                    }
                    heroSpecs.Add(heroSpec);
                }
                SetParty(heroSpecs.ToArray());
            }
            yield return null;
        }

        public string ToJson()
        {
            List<string> heroSpecs = new();
            foreach (var spec in GetParty())
            {
                List<EquipmentSlotData> equipmentSlots = new();
                if (spec.Equipments.Slots != null && spec.Equipments.Slots.Count > 0)
                {
                    foreach (var equipmentSlot in spec.Equipments.Slots)
                    {
                        var equipmentData = new EquipmentData()
                        {
                            DefGuid = equipmentSlot.Equipment.Def != null ? equipmentSlot.Equipment.Def.Guid : null,
                            PrefabGuid = equipmentSlot.Equipment.Prefab != null ? equipmentSlot.Equipment.Prefab.Guid : null,
                            Id = equipmentSlot.Equipment.Id,
                            DefinitionId = equipmentSlot.Equipment.DefinitionId,
                            Level = equipmentSlot.Equipment.Level
                        };
                        var equipmentSlotData = new EquipmentSlotData()
                        {
                            Type = equipmentSlot.Type,
                            Equipment = equipmentData
                        };
                        equipmentSlots.Add(equipmentSlotData);
                    }
                }
                var data = new HeroSpecData()
                {
                    Id = spec.Id,
                    Experience = spec.Experience,
                    UnitSOGuid = spec.Unit != null ? spec.Unit.Guid : null,
                    EquipmentSlots = JsonConvert.SerializeObject(equipmentSlots)
                };
                heroSpecs.Add(JsonConvert.SerializeObject(data));
            }
            return JsonConvert.SerializeObject(heroSpecs);
        }
        #endregion
    }
}