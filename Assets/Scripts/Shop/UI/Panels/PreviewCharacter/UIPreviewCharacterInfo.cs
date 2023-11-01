using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menus.Status.UI.Equipment;
using CryptoQuest.Menus.Status.UI.Stats;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Menu;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Shop.UI.Panels.PreviewCharacter
{
    public class UIPreviewCharacterInfo : MonoBehaviour
    {
        [Header("Character Info UI References")]
        [SerializeField] private Image _avatar;

        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private UIEquipmentPreviewer _previewer;
        [SerializeField] private List<UIAttribute> _attributes;

        private HeroBehaviour _hero;
        private AttributeSystemBehaviour _attributeSystemBehaviour;
        private EquipmentInfo _equipment = null;
        private int _previewSlotIndex = 0;
        public void Init(HeroBehaviour hero)
        {
            _equipment = null;
            _hero = hero;
            SetAvatar(_hero.Avatar);
            SetName(_hero.DetailsInfo.LocalizedName);

            _attributeSystemBehaviour = _hero.GetComponent<AttributeSystemBehaviour>();

            foreach (var attribute in _attributes)
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
        
        private void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }    

        private void SetName(LocalizedString displayName)
        {
            _localizedName.StringReference = displayName;
        }    

        public void Preview(EquipmentInfo equipmentInfo)
        {
            _previewer.PreviewEquipment(equipmentInfo, equipmentInfo.AllowedSlots[_previewSlotIndex], _hero);
            _equipment = equipmentInfo;
        }    
    }
}
