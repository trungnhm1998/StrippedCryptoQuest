using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Character
{
    public class UICharacterInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private UIAttributeBar _hpBar;
        [SerializeField] private UIAttributeBar _mpBar;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        private HeroBehaviour _hero;

        public void Init(HeroBehaviour hero)
        {
            _hero = hero;
            // TODO: REFACTOR CHARACTER

            _attributeChangeEvent.AttributeSystemReference = _hero.GetComponent<AttributeSystemBehaviour>();
        }

        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
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
