using System.Collections.Generic;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory
{
    public class CharacterEquipmentsBehaviour : MonoBehaviour
    {
        [field: SerializeField] public HeroDataSO HeroData { get; private set; }

        [field: Header("Equipment slots"), SerializeField]
        public InventorySO Inventory { get; private set; }

        [field: SerializeField]
        public List<EquipmentSlot> Equipments { get; private set; }

        [field: Header("Effect"), SerializeField]
        private EffectSystemBehaviour _effectSystemBehaviour;

        [field: SerializeField] private InfiniteEffectScriptableObject _baseEquipmentEffect;

        [field: Header("Listening events"), SerializeField]
        private EquipmentEventChannelSO _onEquipItem;

        [SerializeField] private EquipmentEventChannelSO _onUnequipItem;

        private InfiniteEffectScriptableObject _equipmentEffect;

        private void OnEnable()
        {
            _onEquipItem.EventRaised += EquipItem;
            _onUnequipItem.EventRaised += UnequipItem;
        }

        private void OnDisable()
        {
            _onEquipItem.EventRaised -= EquipItem;
            _onUnequipItem.EventRaised -= UnequipItem;
        }

        private void Awake()
        {
            InitEquipments();
        }

        public void InitEquipments()
        {
            InitEquipments(HeroData.Equipments);
        }

        private void InitEquipments(CharacterEquipments equipments)
        {
            Equipments.Clear();
            Equipments = equipments.GetEquippingSlots();
        }

        private void EquipItem(ESlotType type, EquipmentInfo equipment)
        {
            HeroData.Equipments.Equip(type, equipment);
            ApplyEffect(equipment);
        }

        private void UnequipItem(ESlotType type, EquipmentInfo equipment)
        {
            HeroData.Equipments.Unequip(type, equipment);
            ClearEffect();
        }

        private void ApplyEffect(EquipmentInfo equipment)
        {
            // var attributes = equipment.Item.StatsDef.Attributes;
            // _equipmentEffect = Instantiate(_baseEquipmentEffect, transform);
            // var modifiers = new EffectAttributeModifier[attributes.Length];
            // for (int i = 0; i < attributes.Length; i++)
            // {
            //     var attribute = attributes[i];
            //     modifiers[i] = new EffectAttributeModifier
            //     {
            //         Attribute = attribute.Attribute,
            //         ModifierType = EAttributeModifierType.Add,
            //         Value = attribute.MinValue
            //     };
            // }
            //
            // _equipmentEffect.EffectDetails.Modifiers = modifiers;
            //
            // var effect = _effectSystemBehaviour.GetEffect(_equipmentEffect);
            // _effectSystemBehaviour.ApplyEffectToSelf(effect);
        }

        private void ClearEffect()
        {
            var currentEffect = _effectSystemBehaviour.GetEffect(_equipmentEffect);
            _effectSystemBehaviour.RemoveEffect(currentEffect);
        }
    }
}