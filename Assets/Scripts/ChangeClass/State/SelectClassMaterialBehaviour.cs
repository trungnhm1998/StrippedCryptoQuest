using CryptoQuest.ChangeClass.View;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace CryptoQuest.ChangeClass.State
{
    public class SelectClassMaterialBehaviour : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChangeClassStateController _stateController;
        private MerchantsInputManager _input;
        private Animator _animator;
        private static readonly int _submit = Animator.StringToHash("isConfirm");
        private static readonly int _exit = Animator.StringToHash("isChangeClass");
        [SerializeField] private int _index;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _index = 0;
            _animator = animator;
            _stateController = _animator.GetComponent<ChangeClassStateController>();
            _input = _stateController.Input;
            _input.SubmitEvent += SelectedClassMaterial;
            _input.CancelEvent += ExitState;
            _input.ShowDetailEvent += ShowDetail;
            _input.NavigateEvent += HideDetail;
            _stateController.Presenter.EnableClassInteractable(false);
            _stateController.ConfirmMaterial.EnableButtonInteractable(true, _index);
            _stateController.DialogController.Dialogue
                .SetMessage(_message).Show();
        }

        private void ShowDetail()
        {
            var currentCharacter = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacter>();
            _stateController.ConfirmMaterial.ShowDetail(currentCharacter);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _input.SubmitEvent -= SelectedClassMaterial;
            _input.CancelEvent -= ExitState;
            _input.ShowDetailEvent -= ShowDetail;
            _input.NavigateEvent -= HideDetail;
        }

        private void HideDetail(Vector2 arg0)
        {
            _stateController.ConfirmMaterial.HideDetail();
        }

        private void SelectedClassMaterial()
        {
            if (_stateController.Presenter.ListClassMaterial[_index].ListClassCharacter.Count == 0) return;
            var currentCharacter = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacter>();
            _stateController.ConfirmMaterial.EnableButtonInteractable(false, _index);
            _index++;

            if (_index < _stateController.Presenter.ListClassMaterial.Count)
            {
                _stateController.ConfirmMaterial.FirstClassMaterialEvent?.Invoke(currentCharacter);
                _stateController.ConfirmMaterial.FilterClassMaterial(currentCharacter, _index);
                _stateController.ConfirmMaterial.HideDetail();
                currentCharacter.EnableButtonBackground(true);
            }
            else
            {
                _stateController.ConfirmMaterial.LastClassMaterialEvent?.Invoke(currentCharacter);
                currentCharacter.EnableButtonBackground(true);
                ChangeState();
            }
        }

        private void ChangeState()
        {
            _animator.SetTrigger(_submit);
            _stateController.ConfirmMaterial.HideDetail();
        }

        private void ExitState()
        {
            _stateController.ConfirmMaterial.HideDetail();
            _animator.SetTrigger(_exit);
            _stateController.Presenter.Init();
        }
    }
}