using CryptoQuest.BlackSmith.ScriptableObjects;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithManager : MonoBehaviour
    {
        [SerializeField] private MerchantsInputManager _blackSmithInput;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private ShowBlackSmithEventChannelSO _openBlackSmithEvent;
        [SerializeField] private BlackSmithStateController _blacksmithController;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent _blackSmithOpenedEvent;
        [SerializeField] private UnityEvent _blackSmithClosedEvent;
        private bool isExitState;

        private void OnEnable()
        {
            _openBlackSmithEvent.EventRaised += OpenBlackSmith;
            _blackSmithInput.CancelEvent += CloseBlackSmith;
            _blacksmithController.ExitStateEvent += CanExitBlackSmith;
        }

        private void OnDisable()
        {
            _openBlackSmithEvent.EventRaised -= OpenBlackSmith;
            _blackSmithInput.CancelEvent -= CloseBlackSmith;
            _blacksmithController.ExitStateEvent -= CanExitBlackSmith;
        }

        private void OpenBlackSmith()
        {
            EnableBlackSmithInput();
            _blacksmithController.gameObject.SetActive(true);
            _blackSmithOpenedEvent.Invoke();
        }

        private void CloseBlackSmith()
        {
            if (!isExitState) return;
            DisableBlackSmithInput();
            _blacksmithController.gameObject.SetActive(false);
            _blackSmithClosedEvent.Invoke();
        }

        private void EnableBlackSmithInput()
        {
            _blackSmithInput.EnableInput();
        }

        private void DisableBlackSmithInput()
        {
            _blackSmithInput.DisableInput();
        }

        private void CanExitBlackSmith(bool isExit)
        {
            isExitState = isExit;
        }
    }
}
