using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Map
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PartyLeavingValidator : MonoBehaviour
    {
        [SerializeField] private PartySO _party;
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private LocalizedString _message;
        [SerializeField] private UnityEvent _onShowDialogAndBlock;
        [SerializeField] private UnityEvent _onShowGateAndDisableBlock;

        private UIGenericDialog _dialogue;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            UpdateGateAndVisibility();
        }

        private void UpdateGateAndVisibility()
        {
            bool isPartyFull = _party.GetParty().Length == 4;

            if (!isPartyFull) _onShowDialogAndBlock.Invoke();
            else _onShowGateAndDisableBlock.Invoke();
        }

        public void ShowMerchantDialogue() => GenericDialogController.Instance.InstantiateAsync(SetDialogueReference);

        private void SetDialogueReference(UIGenericDialog dialog)
        {
            _gameState.UpdateGameState(EGameState.Dialogue);
            _dialogue = dialog;
            _dialogue
                .WithMessage(_message)
                .RequireInput()
                .WithHideCallback(HideCallBack)
                .Show();
        }

        private void HideCallBack()
        {
            _gameState.UpdateGameState(EGameState.Field);
        }
    }
}