using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Character.Behaviours
{
    public class InteractBehaviour : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Transform _interactionBoxTransform;

        private IInteractionManager _interactionManager;

        private void Awake()
        {
            _interactionManager = GetComponent<IInteractionManager>();
        }

        private void OnEnable()
        {
            _inputMediator.InteractEvent += InteractEvent_Raised;
        }

        private void OnDisable()
        {
            _inputMediator.InteractEvent -= InteractEvent_Raised;
        }

        private void InteractEvent_Raised()
        {
            _interactionManager.Interact();
        }
    }
}
