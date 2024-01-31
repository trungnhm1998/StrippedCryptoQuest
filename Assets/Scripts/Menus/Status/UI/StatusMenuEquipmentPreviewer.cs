using CryptoQuest.Battle.Components;
using EquipmentInfo = CryptoQuest.Item.Equipment.Equipment;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu.Character;
using CryptoQuest.UI.Tooltips.Equipment;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Menus.Status.UI
{
    public class StatusMenuEquipmentPreviewer : MonoBehaviour
    {
        [SerializeField] private UIStatusMenu _statusMenu;
        [SerializeField] private HeroEquipmentPreviewer _previewer;
        [SerializeField] private UIAttribute[] _attributeUis;
        [SerializeField] private VoidEventChannelSO _previewEquipEventChannel;
        [SerializeField] private VoidEventChannelSO _previewUnequipEventChannel;

        private EquipmentsController _equipmentController;
        private HeroBehaviour _hero;

        private void OnEnable()
        {
            InitPreviewer(_statusMenu.InspectingHero);
            _previewEquipEventChannel.EventRaised += PreviewEquip;
            _previewUnequipEventChannel.EventRaised += PreviewUnequip;
            _statusMenu.InspectingHeroChanged += InitPreviewer;
        }

        private void OnDisable()
        {
            _previewer.ResetPreview();
            _previewEquipEventChannel.EventRaised -= PreviewEquip;
            _previewUnequipEventChannel.EventRaised -= PreviewUnequip;
            _statusMenu.InspectingHeroChanged -= InitPreviewer;
            UnregistEvents();
        }

        /// <summary>
        // Need to reset previewer when enter this state so it can re-init character
        /// </summary>
        public void ResetPreviewer()
        {
            _hero = null;
            _equipmentController = null;
        }

        public void UnequipPressed() => _previewer.ResetPreview();

        private void InitPreviewer(HeroBehaviour hero)
        {
            if (_hero != null && hero == _hero)
            {
                RegistEvents();
                CacheCurrentAttributesValue();
                return;
            }
            _hero = hero;
            _equipmentController = _hero.GetComponent<EquipmentsController>();

            UnregistEvents();

            _previewer.SetPreviewHero(hero);
            
            CacheCurrentAttributesValue();
            
            // Because when equip/unequip stats change so we need to update previewer
            RegistEvents();
        }

        private void CacheCurrentAttributesValue()
        {
            foreach (var attributeUI in _attributeUis)
            {
                if (attributeUI.Attribute == null) continue;
                if (_hero.AttributeSystem.TryGetAttributeValue(attributeUI.Attribute, out AttributeValue value))
                {
                    _previewer.CacheCurrentAttributes(attributeUI.Attribute, attributeUI, value.CurrentValue);
                }
            }
        }

        private void RegistEvents()
        {
            if (_equipmentController == null) return;
            _equipmentController.Equipped += EquipPreviewEquipment;
            _equipmentController.Removed += RemovePreviewEquipment;
        }

        private void UnregistEvents()
        {
            if (_equipmentController == null) return;
            _equipmentController.Equipped -= EquipPreviewEquipment;
            _equipmentController.Removed -= RemovePreviewEquipment;
        }

        private void EquipPreviewEquipment(IEquipment equipment) 
        {
            _previewer.ClonedHero.TryGetComponent<EquipmentsController>(out var equipmentsController);
            equipmentsController.Equip(equipment, _statusMenu.ModifyingSlot);
            CacheCurrentAttributesValue();
        }

        private void RemovePreviewEquipment(IEquipment equipment) 
        {
            _previewer.ClonedHero.TryGetComponent<EquipmentsController>(out var equipmentsController);
            equipmentsController.Unequip(equipment);
            CacheCurrentAttributesValue();
        }


        private void PreviewEquip()
        {
            var equipment = GetEquipmentFromSelectedObject();
            _previewer.PreviewEquip(equipment, _statusMenu.ModifyingSlot);
        }

        private void PreviewUnequip()
        {
            var equipment = GetEquipmentFromSelectedObject();
            _previewer.PreviewUnequip(equipment, _statusMenu.ModifyingSlot);
        }

        private IEquipment GetEquipmentFromSelectedObject()
        {
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == null) return new EquipmentInfo();
            var provider = selectedObject.GetComponent<ITooltipEquipmentProvider>();
            return provider.Equipment;
        }
    }
}