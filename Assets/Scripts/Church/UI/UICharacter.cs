using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Church.UI
{
    public class UICharacter : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _avatar;
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        public void ConfigureCharacter(HeroBehaviour hero, IHeroAvatarProvider avatar)
        {
            _displayName.StringReference = hero.LocalizedName;
            _avatar.sprite = hero.Avatar;
            _attributeChangeEvent.AttributeSystemReference = hero.GetComponent<AttributeSystemBehaviour>();
            StartCoroutine(CoLoadAvatar(hero, avatar));
        }

        private IEnumerator CoLoadAvatar(HeroBehaviour hero, IHeroAvatarProvider avatar)
        {
            yield return avatar.LoadAvatarAsync(hero);
            _avatar.sprite = hero.Avatar;
        }

        public void SetCurrentHp(float currentHp)
        {
            _hpBar.SetValue(currentHp);
        }

        public void SetMaxHp(float maxHp)
        {
            _hpBar.SetMaxValue(maxHp);
        }

        public void SetCurrentMp(float currentMp)
        {
            _mpBar.SetValue(currentMp);
        }

        public void SetMaxMp(float maxMp)
        {
            _mpBar.SetMaxValue(maxMp);
        }
    }
}
