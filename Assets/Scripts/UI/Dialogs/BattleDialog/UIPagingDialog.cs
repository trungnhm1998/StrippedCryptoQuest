using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class UIPagingDialog : ModalWindow<UIPagingDialog>
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private Text _dialogueText;
        [SerializeField] private GameObject _continueMark;
        [SerializeField] private float _delayBetweenLine = 0.5f;

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _onPagingDialogClose;

        private PagingDialog _dialogue;
        private int _currentPageIndex = 0;
        private bool _isShowingLines = false;


        private Sequence _showLineSeq;

        private void OnEnable()
        {
            _inputMediator.NextDialoguePressed += NextDialog;
        }

        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= NextDialog;
        }

        private void NextDialog()
        {
            if (_isShowingLines) return;
            _currentPageIndex++;
            PlayDialoguePageWithIndex(_currentPageIndex);
        }

        private void PlayDialoguePageWithIndex(int dialogueIndex)
        {
            _isShowingLines = true;
            _continueMark.SetActive(false);
            if (_dialogue.Pages.Count <= dialogueIndex)
            {
                Close();
                return;
            }
            var page = _dialogue.Pages[dialogueIndex];
            if (page.Lines.Count <= 0)
            {
                _isShowingLines = false;
                NextDialog();
                return;
            }
            
            _dialogueText.text = "";
            _showLineSeq = DOTween.Sequence();
            foreach (var line in page.Lines)
            {
                _showLineSeq.AppendCallback(() => _dialogueText.text += $"{line}\n")
                .AppendInterval(_delayBetweenLine);
            }
            _showLineSeq.OnComplete(() => {
                _isShowingLines = false;
                _continueMark.SetActive(true);
            });
        }

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            _inputMediator.EnableDialogueInput();
            PlayDialoguePageWithIndex(0);
        }

        public UIPagingDialog SetDialogue(PagingDialog dialogueArgs)
        {
            _dialogue = dialogueArgs;
            return this;
        }

        public override UIPagingDialog Close()
        {
            gameObject.SetActive(false);
            _onPagingDialogClose.RaiseEvent();
            return base.Close();
        }

        protected override void CheckIgnorableForClose() {}
    }
}
