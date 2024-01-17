using CryptoQuest.Battle.Presenter.Commands;
using CryptoQuest.Input;
using CryptoQuest.Merchant;
using CryptoQuest.UI.Dialogs.BattleDialog;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.ShopSystem
{
    public class TransactionResultPanel : MonoBehaviour
    {
        [SerializeField] private MerchantInput _merchantInput;
        [SerializeField] private LocalizedString _strSuccess;
        [SerializeField] private LocalizedString _strFailed;
        [SerializeField] private UnityEvent _showing;
        [SerializeField] private UnityEvent _hiding;

        public TransactionResultPanel AddHideCallback(UnityAction callback)
        {
            _hiding.AddListener(callback);
            return this;
        }

        public void ShowSuccess()
        {
            ShowDialog(_strSuccess);
        }

        public void ShowFailed()
        {
            ShowDialog(_strFailed);
        }

        private void ShowDialog(LocalizedString message)
        {
            GenericDialogController.Instance.InstantiateAsync(dialog =>
            {
                dialog
                    .WithHideCallback(() =>
                    {
                        DOVirtual.DelayedCall(0, () =>
                        {
                            _hiding?.Invoke();
                            _hiding?.RemoveAllListeners();
                        });
                        // Require input in dialog cause merchant input disable
                        _merchantInput.EnableInput();
                    })
                    .RequireInput()
                    .WithMessage(message)
                    .Show();
                _showing?.Invoke();
            });
        }
    }
}