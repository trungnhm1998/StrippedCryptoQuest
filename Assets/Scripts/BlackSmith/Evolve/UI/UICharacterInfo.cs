using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UICharacterInfo : MonoBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private List<UIAttribute> _attribute;
        [SerializeField] private UIEquipmentPreviewer _previewer;
        private AttributeSystemBehaviour _attributeSystemBehaviour;
        private Equipment _equipment = null;
        private HeroBehaviour _hero;
        private int _previewSlotIndex = 0;

        public void LoadCharacterDetail(HeroBehaviour hero)
        {
            _equipment = null;
            _hero = hero;
            _displayName.StringReference = hero.DetailsInfo.LocalizedName;
            _attributeSystemBehaviour = hero.GetComponent<AttributeSystemBehaviour>();
            foreach (var attribute in _attribute)
            {
                SetStat(attribute, attribute.Attribute);
            }
        }

        private void SetStat(UIAttribute attribute, AttributeScriptableObject attributeSO)
        {
            if (_attributeSystemBehaviour.TryGetAttributeValue(attributeSO, out AttributeValue value))
            {
                attribute.SetValue(value.CurrentValue);
            }
        }

        public void Preview(Equipment equipment)
        {
            // _previewer.PreviewEquipment(equipmentInfo, equipmentInfo.AllowedSlots[_previewSlotIndex], _hero);
            _equipment = equipment;
        }

        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }
    }
}
