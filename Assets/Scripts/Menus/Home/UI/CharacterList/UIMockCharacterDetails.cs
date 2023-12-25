using CryptoQuest.Character.Hero;
using CryptoQuest.UI.Character;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI.CharacterList
{
    /// <summary>
    /// This class will be removed after the new updates of the Characters System are ready.
    /// Please ignore the dirty codes here.
    /// </summary>
    public class UIMockCharacterDetails : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _characterElement;
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private UIAttribute _strength;
        [SerializeField] private UIAttribute _vitality;
        [SerializeField] private UIAttribute _agility;
        [SerializeField] private UIAttribute _intelligence;
        [SerializeField] private UIAttribute _luck;
        [SerializeField] private UIAttribute _attack;
        [SerializeField] private UIAttribute _defence;
        [SerializeField] private UIAttribute _magicAttack;

        public void FillData(HeroSpec spec)
        {
            _localizedName.StringReference = spec.Origin.DetailInformation.LocalizedName;
            _level.text = $"Lv 1";
            _characterElement.sprite = spec.Elemental.Icon;
            SetHp(spec.Stats.Attributes[0].MinValue, spec.Stats.Attributes[0].MinValue);
            SetMp(spec.Stats.Attributes[1].MinValue, spec.Stats.Attributes[1].MinValue);
            SetStrength(spec.Stats.Attributes[2].MinValue);
            SetVitality(spec.Stats.Attributes[3].MinValue);
            SetAgility(spec.Stats.Attributes[4].MinValue);
            SetIntelligence(spec.Stats.Attributes[5].MinValue);
            SetLuck(spec.Stats.Attributes[6].MinValue);
            SetAttack(100);
            SetDefence(100);
            SetMagicAttack(100);
        }

        private void SetHp(float currentHp, float maxHp)
        {
            _hpBar.SetValue(currentHp);
            _hpBar.SetMaxValue(maxHp);
        }

        private void SetMp(float currentMp, float maxMp)
        {
            _mpBar.SetValue(currentMp);
            _mpBar.SetMaxValue(maxMp);
        }

        private void SetStrength(float value)
        {
            _strength.SetValue(value);
        }

        private void SetVitality(float value)
        {
            _vitality.SetValue(value);
        }

        private void SetAgility(float value)
        {
            _agility.SetValue(value);
        }

        private void SetIntelligence(float value)
        {
            _intelligence.SetValue(value);
        }

        private void SetLuck(float value)
        {
            _luck.SetValue(value);
        }

        private void SetAttack(float value)
        {
            _attack.SetValue(value);
        }

        private void SetDefence(float value)
        {
            _defence.SetValue(value);
        }

        private void SetMagicAttack(float value)
        {
            _magicAttack.SetValue(value);
        }
    }
}