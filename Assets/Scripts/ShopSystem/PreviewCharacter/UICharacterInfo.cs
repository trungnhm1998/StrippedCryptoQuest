using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem.PreviewCharacter
{
    public class UICharacterInfo : MonoBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private List<UIAttribute> _attribute;
        [SerializeField] private CharacterEquipmentPreviewer _characterPreviewer;
        private AttributeSystemBehaviour _attributeSystemBehaviour;
        private HeroBehaviour _hero;
        private int _previewSlotIndex = 0;

        public void LoadCharacterDetail(PartySlot heroSlot)
        {
            _hero = heroSlot.HeroBehaviour;
            _displayName.StringReference = _hero.DetailsInfo.LocalizedName;
            _attributeSystemBehaviour = _hero.AttributeSystem;
            foreach (var attribute in _attribute)
            {
                SetStat(attribute, attribute.Attribute);
            }
            _characterPreviewer.SetCharacter(heroSlot);
        }

        private void SetStat(UIAttribute attribute, AttributeScriptableObject attributeSO)
        {
            if (_attributeSystemBehaviour.TryGetAttributeValue(attributeSO, out AttributeValue value))
            {
                attribute.SetValue(value.CurrentValue);
                _characterPreviewer.CacheCurrentAttributes(attributeSO, attribute, value.CurrentValue);
            }
        }

        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }
    }
}
