using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;

namespace CryptoQuest.ShopSystem.PreviewCharacter
{
    public struct AttributeUIValue
    {
        public UIAttribute AttributeUI;
        public float CurrentValue;
    }

    public class CharacterEquipmentPreviewer : MonoBehaviour
    {
        private Dictionary<AttributeScriptableObject, AttributeUIValue> _cachedAttributes = new();

        private PartySlot _clonedSlot;
        private CharacterBehaviour _character => _clonedSlot.HeroBehaviour;
        public bool IsValid => _clonedSlot != null;

        private void OnEnable()
        {
            if (_clonedSlot == null) return;
            _clonedSlot.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (_clonedSlot == null) return;
            _clonedSlot.gameObject.SetActive(false);
        }

        public void SetCharacter(PartySlot characterSlot)
        {
            Clone(characterSlot);
        }

        public void CacheCurrentAttributes(AttributeScriptableObject attribute, UIAttribute uiAttribute, float value)
        {
            var attributeUIValue = new AttributeUIValue()
            {
                AttributeUI = uiAttribute,
                CurrentValue = value
            };
            if (_cachedAttributes.ContainsKey(attribute))
            {
                _cachedAttributes[attribute] = attributeUIValue;
                return;
            }
            _cachedAttributes.Add(attribute, attributeUIValue);
        }

        public void PreviewEquip(IEquipment equipment)
        {
            ResetPreview();

            if (!_character.IsValid() || !equipment.IsValid()) return;
            if (!_character.TryGetComponent<EquipmentsController>(out var equipmentController))
                return;

            var attributeSystem = _character.AttributeSystem;

            equipmentController.Equip(equipment, equipment.AllowedSlots[0]);

            PreviewValue();
        }

        public void PreviewUnequip(IEquipment equipment)
        {
            ResetPreview();

            if (!_character.IsValid() || !equipment.IsValid()) return;
            if (!_character.TryGetComponent<EquipmentsController>(out var equipmentController))
                return;

            var attributeSystem = _character.AttributeSystem;

            equipmentController.Unequip(equipment.AllowedSlots[0]);

            PreviewValue();
        }

        public void ResetPreview()
        {
            foreach (var attribute in _cachedAttributes.Values)
            {
                attribute.AttributeUI.ResetAttributeUI();
            }
        }

        private void Clone(PartySlot slot)
        {
            if (_clonedSlot != null)
            {
                Destroy(_clonedSlot);
            }

            _clonedSlot = Instantiate(slot);
            var cloneSpec = new PartySlotSpec()
            {
                Hero = slot.Spec.Hero,
            };
            cloneSpec.EquippingItems.Slots = slot.Spec.EquippingItems.Slots.ToList();
            _clonedSlot.Init(cloneSpec);

            // Remove this so equip/unequip wont affect server or inventory
            RemoveComponent(typeof(EquipmentsNetworkController));
            RemoveComponent(typeof(InventoryEquipmentsController));
        }

        private void RemoveComponent(Type componentType)
        {
            var component = _character.GetComponent(componentType);
            Destroy(component);
        }

        private void PreviewValue()
        {
            var attributeSystem = _character.AttributeSystem;

            foreach (var attributeValue in attributeSystem.AttributeValues)
            {
                if (!_cachedAttributes.TryGetValue(attributeValue.Attribute, out var cachedAttribute))
                    continue;

                cachedAttribute.AttributeUI.CompareValue(cachedAttribute.CurrentValue,
                    attributeValue.CurrentValue);
            }
        }
    }
}
