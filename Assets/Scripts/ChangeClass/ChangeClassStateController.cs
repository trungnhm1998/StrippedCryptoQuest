using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Input;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassStateController : MonoBehaviour
    {
        [field: SerializeField] public ChangeClassDialogController DialogController { get; private set; }
        [field: SerializeField] public ChangeClassPreviewPresenter ConfirmMaterial { get; private set; }
        [field: SerializeField] public MerchantsInputManager Input { get; private set; }
        [field: SerializeField] public ChangeClassPresenter Presenter { get; private set; }
        [field: SerializeField] public ChangeClassManager Manager { get; private set; }
        [field: SerializeField] public Button DefaultButton { get; private set; }
        public UnityAction ExitStateEvent;
        [SerializeField] private Animator _animator;
        private static readonly int _openChangeClassState = Animator.StringToHash("isChangeClassState");

        private void OnEnable()
        {
            Manager.IsOpenChangeClassState += EnableChangeClassState;
        }

        private void OnDisable()
        {
            Manager.IsOpenChangeClassState -= EnableChangeClassState;
        }

        private void EnableChangeClassState(bool isActive)
        {
            _animator.SetBool(_openChangeClassState, isActive);
        }
    }
}
