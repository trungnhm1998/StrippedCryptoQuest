using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
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
        private AttributeSystemBehaviour _attributeSystemBehaviour;

        public void LoadCharacterDetail(HeroBehaviour hero)
        {
            _avatar.sprite = hero.Avatar;
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
    }
}
