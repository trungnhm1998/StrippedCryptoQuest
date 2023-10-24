using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    public class UICharacterBattleInfo : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _characterName;
        [SerializeField] private Image _avatar;
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        public HeroBehaviour Hero { get; private set; }

        public void Init(HeroBehaviour hero)
        {
            Hero = hero;
            // TODO: REFACTOR CHARACTER

            if (Hero == null) return;
            SetLocalizedName(hero.DetailsInfo.LocalizedName);
            _attributeChangeEvent.AttributeSystemReference = Hero.GetComponent<AttributeSystemBehaviour>();
        }

        public void SetBattleAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }

        public void SetLocalizedName(LocalizedString localizedName)
        {
            _characterName.StringReference = localizedName;
        }

        public virtual void SetCurrentHp(float currentHp)
        {
            _hpBar.SetValue(currentHp);
        }

        public void SetMaxHp(float maxHp)
        {
            _hpBar.SetMaxValue(maxHp);
        }

        public virtual void SetCurrentMp(float currentMp)
        {
            _mpBar.SetValue(currentMp);
        }

        public void SetMaxMp(float maxMp)
        {
            _mpBar.SetMaxValue(maxMp);
        }
    }
}
