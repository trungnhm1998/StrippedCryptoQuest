using CryptoQuest.Menus.Home.UI.CharacterList;
using CryptoQuest.Merchant;
using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Tavern.UI.Tooltip
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField] private ShowTooltipEvent _showTooltipEvent;
        [SerializeField] private MerchantInput _input;
        [SerializeField] private UICharacterDetails _tooltip;

        private void OnEnable()
        {
            _input.ShowDetailEvent += ShowTooltip;
            _showTooltipEvent.EventRaised += HandleShowTooltip;
        }

        private void OnDisable()
        {
            _input.ShowDetailEvent -= ShowTooltip;
            _showTooltipEvent.EventRaised -= HandleShowTooltip;
        }
        
        private void HandleShowTooltip(bool isShow)
        {
        }

        private void ShowTooltip()
        {
            var currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            var heroProvider = currentSelectedGameObject.GetComponent<ITooltipHeroProvider>();
            if (heroProvider == null) return;
        }
    }
}