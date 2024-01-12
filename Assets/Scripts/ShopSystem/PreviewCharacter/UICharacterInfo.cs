using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu.Character;
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
        [SerializeField] private HeroEquipmentPreviewer _characterPreviewer;
        private AttributeSystemBehaviour _attributeSystemBehaviour;
        private HeroBehaviour _hero;
        private int _previewSlotIndex = 0;

        public void LoadCharacterDetail(HeroBehaviour hero)
        {
            _hero = hero;
            _displayName.StringReference = _hero.DetailsInfo.LocalizedName;
            _attributeSystemBehaviour = _hero.AttributeSystem;
            foreach (var attribute in _attribute)
            {
                SetStat(attribute, attribute.Attribute);
            }
            _characterPreviewer.SetPreviewHero(_hero);
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
