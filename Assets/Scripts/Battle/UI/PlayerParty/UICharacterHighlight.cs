using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    [RequireComponent(typeof(UICharacterBattleInfo))]
    public class UICharacterHighlight : MonoBehaviour
    {
        [SerializeField] private UICharacterBattleInfo _characterUI;
        [SerializeField] private Image _background;
        [SerializeField] private Color _highlightColor;

        private Color _normalColor;

        private TinyMessageSubscriptionToken _heroEventToken;

        private void Awake()
        {
            _normalColor = _background.color;
        }

        private void OnValidate()
        {
            _characterUI = GetComponent<UICharacterBattleInfo>();
        }

        private void OnEnable()
        {
            _heroEventToken = BattleEventBus.SubscribeEvent<HighlightHeroEvent>(SetHighlightHero);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_heroEventToken);
        }

        private void SetHighlightHero(HighlightHeroEvent heroEvent)
        {
            bool isHeroValid = _characterUI.Hero.IsValidAndAlive() && 
                (heroEvent.Hero != null && heroEvent.Hero == _characterUI.Hero);
            SetHighlight(isHeroValid);
        }

        private void SetHighlight(bool value)
        {
            _background.color = value ? _highlightColor : _normalColor;
        }
    }
}
