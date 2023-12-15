using CryptoQuest.Merchant;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class SellPanel : MonoBehaviour
    {
        [SerializeField] private Button[] _tabTypes;
        [SerializeField] private MerchantInput _input;
        [SerializeField] private GameObject _selectActionPanel;

        private int _selectedTab;

        private void OnEnable()
        {
            _input.CancelEvent += BackToSelectAction;
            _input.ChangeTabEvent += ChangeTab;
            SelectTab(0);
        }

        private void SelectTab(int index, bool invoke = true)
        {
            _tabTypes[_selectedTab].image.sprite = _tabTypes[_selectedTab].spriteState.disabledSprite;
            _selectedTab = index;
            var selectedTab = _tabTypes[_selectedTab];
            selectedTab.image.sprite = selectedTab.spriteState.pressedSprite;
            if (invoke) selectedTab.onClick.Invoke();
        }

        private void ChangeTab(float dir)
        {
            var index = _selectedTab + (int)dir;
            index %= _tabTypes.Length;
            if (index < 0) index = _tabTypes.Length - 1;
            SelectTab(index);
        }

        private void OnDisable()
        {
            _input.CancelEvent -= BackToSelectAction;
            _input.ChangeTabEvent -= ChangeTab;
        }

        private void BackToSelectAction()
        {
            _selectActionPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}