using CryptoQuest.Inventory;
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
        [SerializeField] private HeroSpecInitializer _specInitializer;
        [SerializeField] private UITransferCharacter _transferPanel;
        [SerializeField] private UIRecruitPanel _recruitPanel;

        private void OnEnable()
        {
            _input.ShowDetailEvent += ShowTooltip;
            _transferPanel.Closed += HideTooltip;
            _recruitPanel.Closed += HideTooltip;
        }

        private void OnDisable()
        {
            _input.ShowDetailEvent -= ShowTooltip;
            _transferPanel.Closed -= HideTooltip;
            _recruitPanel.Closed -= HideTooltip;
        }

        private void HideTooltip() => _showTooltipEvent.RaiseEvent(false);

        private void ShowTooltip()
        {
            var currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            var heroProvider = currentSelectedGameObject.GetComponent<ITooltipHeroProvider>();
            if (heroProvider == null) return;
            _showTooltipEvent.RaiseEvent(true);
        }
    }
}