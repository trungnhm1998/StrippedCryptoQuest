using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public class UICharacterInfo : MonoBehaviour
    {
        [Header("Character Infomation")]
        [SerializeField] private AttributeScriptableObject _hpAttribute;
        [SerializeField] private AttributeScriptableObject _maxHpAttribute;
        [SerializeField] private AttributeScriptableObject _mpAttribute;
        [SerializeField] private AttributeScriptableObject _maxMpAttribute;
        [SerializeField] private AttributeScriptableObject _expAttribute;
        [SerializeField] private AttributeScriptableObject _maxExpAttribute;

        [Header("UI Components")]
        [SerializeField] private GameObject _content;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _currentHp;
        [SerializeField] private TMP_Text _maxHp;
        [SerializeField] private Image _HpBar;
        [SerializeField] private TMP_Text _currentMp;
        [SerializeField] private TMP_Text _maxMp;
        [SerializeField] private Image _MpBar;
        [SerializeField] private Image _ExpBar;
        [SerializeField] private TMP_Text _class;
        [SerializeField] private Image _element;
        [SerializeField] private Image _avatar;

        private HeroDataSO _charInfo;
        public HeroDataSO CharInfo { get => _charInfo; set => _charInfo = value; }

        private AttributeSystemBehaviour charAttributes;
        public AttributeSystemBehaviour CharAttributes { get => charAttributes; set => charAttributes = value; }

        private void Start()
        {
            CheckExistence();
        }

        private void CheckExistence()
        {
            if (CharInfo == null)
                _content.SetActive(false);
            else
            {
                _content.SetActive(true);
                InitCharacterInfo();
            }
        }

        private void InitCharacterInfo()
        {
            SetName();
            SetLevel();
            SetHP();
            SetMP();
            SetEXP();
            SetClass();
            SetElemental();
            SetAvatar();
        }

        private void SetName()
        {
            _name.text = CharInfo.Name;
        }

        private void SetLevel()
        {
            _level.text = string.Format(_level.text, CharInfo.Level);
        }

        private void SetHP()
        {
            CharAttributes.TryGetAttributeValue(_hpAttribute, out var curHp);
            CharAttributes.TryGetAttributeValue(_maxHpAttribute, out var maxHp);

            _currentHp.text = ((int)curHp.CurrentValue).ToString();
            _maxHp.text = ((int)maxHp.CurrentValue).ToString();

            _HpBar.fillAmount = curHp.CurrentValue / maxHp.CurrentValue;
        }

        private void SetMP()
        {
            CharAttributes.TryGetAttributeValue(_mpAttribute, out var curMp);
            CharAttributes.TryGetAttributeValue(_maxMpAttribute, out var maxMp);
        
            _currentMp.text = ((int)curMp.CurrentValue).ToString();
            _maxMp.text = ((int)maxMp.CurrentValue).ToString();
        
            _MpBar.fillAmount = curMp.CurrentValue / maxMp.CurrentValue;
        }
        
        private void SetEXP()
        {
            CharAttributes.TryGetAttributeValue(_expAttribute, out var curExp);
            CharAttributes.TryGetAttributeValue(_maxExpAttribute, out var maxExp);

            if (maxExp.CurrentValue <= 0)
            {
                maxExp = new AttributeValue()
                {
                    BaseValue = 100,
                    CurrentValue = 100
                };
            }
            _ExpBar.fillAmount = curExp.CurrentValue / maxExp.CurrentValue;
        }

        private void SetClass()
        {
            _class.text = CharInfo.name;
        }

        private void SetElemental()
        {
            _element.sprite = CharInfo.ElementIcon;
        }

        private void SetAvatar()
        {
            _avatar.sprite = CharInfo.Avatar;
        }

        public void SetData(HeroDataSO infoReceived, AttributeSystemBehaviour attributesReceived)
        {
            CharInfo = infoReceived;
            CharAttributes = attributesReceived;
        }
    }
}