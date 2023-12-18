using CryptoQuest.Merchant;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class SellPanel : MonoBehaviour
    {
        [SerializeField] private Button[] _tabTypes;
        [SerializeField] private MerchantInput _input;
        [SerializeField] private GameObject _selectActionPanel;
        [SerializeField] private LocalizedString _sellingString;

        private int _selectedTab;
        private UIGenericDialog _genericDialog;

        private void Awake()
        {
            GenericDialogController.Instance.InstantiateAsync(dialog => _genericDialog = dialog);
        }

        private void OnEnable()
        {
            EnableInput();
            SelectTab(0);
            _genericDialog
                .WithMessage(_sellingString)
                .Show();
        }

        public void EnableInput()
        {
            _input.CancelEvent += BackToSelectAction;
            _input.ChangeTabEvent += ChangeTab;
        }

        private void SelectTab(int index, bool invoke = true)
        {
            _tabTypes[_selectedTab].image.sprite = _tabTypes[_selectedTab].spriteState.disabledSprite;
            _selectedTab = index;
            var selectedTab = _tabTypes[_selectedTab];
            if (invoke) selectedTab.onClick.Invoke();
            selectedTab.image.sprite = selectedTab.spriteState.pressedSprite;
        }

        private void ChangeTab(float dir)
        {
            var index = _selectedTab + (int)dir;
            index %= _tabTypes.Length;
            if (index < 0) index = _tabTypes.Length - 1;
            SelectTab(index);
        }

        private void OnDisable() => DisableInput();

        public void DisableInput()
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