using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Evolve.UI
{
    public class UIBeastDetail : MonoBehaviour
    {
        private const int MAX_STAR = 5;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _illustration;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private List<Image> _listStar;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private List<UIAttribute> _attributeBar;
        [SerializeField] private Sprite _evolveStar;
        [SerializeField] private Sprite _currentStar;
        [SerializeField] private Sprite _defaultStar;
        [SerializeField] private BeastAttributeSystemSO beastAttributeSystemSo;

        private IBeastAvatarProvider _beastAvatarProvider;

        private void OnEnable()
        {
            beastAttributeSystemSo.EventRaised += UpdateBeastStats;
        }

        private void OnDisable()
        {
            beastAttributeSystemSo.EventRaised -= UpdateBeastStats;
        }

        public void SetupUI(IBeast beast)
        {
            _beastAvatarProvider = GetComponent<IBeastAvatarProvider>();
            _icon.sprite = beast.Elemental.Icon;
            _displayName.StringReference = beast.LocalizedName;
            _level.text = $"Lv{beast.Level.ToString()}";
            StartCoroutine(_beastAvatarProvider.LoadAvatarAsync(_illustration, beast));
            SetBeastStar(beast);
        }

        private void SetBeastStar(IBeast beast)
        {
            for (int i = 0; i < MAX_STAR; i++)
            {
                _listStar[i].sprite = i < beast.Stars ? _currentStar : _defaultStar;
            }
            _listStar[beast.Stars].sprite = _evolveStar;
        }
        
        
        private void UpdateBeastStats(AttributeSystemBehaviour attributeValues)
        {
            foreach (var attribute in attributeValues.AttributeValues)
            {
                foreach (var attributeValue in _attributeBar)
                {
                    if (attribute.Attribute == attributeValue.Attribute)
                    {
                        attributeValue.SetValue(attribute.BaseValue);
                    }
                }
            }
        }
    }
}