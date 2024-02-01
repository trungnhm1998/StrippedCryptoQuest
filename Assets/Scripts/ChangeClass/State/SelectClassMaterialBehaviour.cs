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
        private UIClassMaterial _firstClassMaterial;
        private UIClassMaterial _secondClassMaterial;
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

            _stateController.DialogController.Dialogue
                .SetMessage(_message).Show();
            EnableButtonInteractable();
        }

        private void EnableButtonInteractable()
        {
            _firstClassMaterial = _stateController.ClassBerserkerControllerMaterial.IsValid
                ? _stateController.ClassBerserkerControllerMaterial.BerserkerClassMaterials
                : _stateController.Presenter.FirstClassMaterials;
            _secondClassMaterial = _stateController.Presenter.SecondClassMaterials;
            _stateController.Presenter.EnableClassInteractable(false);
            _stateController.ConfirmMaterial.EnableButtonInteractable(true, _firstClassMaterial);
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
            if (_stateController.ClassBerserkerControllerMaterial.IsValid)
            {
                SelectClassBerserkerMaterial();
                return;
            }

            if (_firstClassMaterial.ListClassCharacter.Count == 0 ||
                _secondClassMaterial.ListClassCharacter.Count == 0) return;

            var currentCharacter = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacter>();

            switch (_index)
            {
                case 0:
                    _stateController.ConfirmMaterial.SetFirstClassMaterial(_firstClassMaterial, currentCharacter);
                    _stateController.ConfirmMaterial.FilterClassMaterial(currentCharacter, _secondClassMaterial);
                    break;
                case 1:
                    _stateController.ConfirmMaterial.SetLastClassMaterial(_secondClassMaterial, currentCharacter);
                    ChangeState();
                    break;
                default:
                    Debug.LogError("[ChangeClass]:: unknown class material index");
                    break;
            }

            _index++;
        }

        private void SelectClassBerserkerMaterial()
        {
            var currentCharacter = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacter>();
            _stateController.ConfirmMaterial.SetBerserkerMaterial(currentCharacter);
            ChangeState();
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