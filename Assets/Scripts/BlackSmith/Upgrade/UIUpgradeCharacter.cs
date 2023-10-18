using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;


namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeCharacter : MonoBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private List<UIAttribute> _attribute;
        [SerializeField] private UIEquipmentPreviewer _previewer;
        private AttributeSystemBehaviour _attributeSystemBehaviour;
        private IHeroAvatarProvider _heroAvatarProvider;
        private EquipmentInfo _equipment = null;
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
            StartCoroutine(CoLoadAvatar(heroAvatar));
        }

        private void SetStat(UIAttribute attribute, AttributeScriptableObject attributeSO)
        {
            if (_attributeSystemBehaviour.TryGetAttributeValue(attributeSO, out AttributeValue value))
            {
                attribute.SetValue(value.CurrentValue);
            }
        }

        public void Preview(EquipmentInfo equipmentInfo)
        {
            if(_equipment != null)
            {
                _previewer.PreviewUnequipEquipment(_equipment, _hero);
            }
            _previewer.PreviewEquipment(equipmentInfo, equipmentInfo.AllowedSlots[_previewSlotIndex], _hero);
            _equipment = equipmentInfo;
        }

        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }
    }
}
