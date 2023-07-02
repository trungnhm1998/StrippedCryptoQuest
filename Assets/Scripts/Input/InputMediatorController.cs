using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Input
{
    public class InputMediatorController : MonoBehaviour
    {
        [SerializeField] private  InputMediatorSO _inputMediator;
        [SerializeField] private VoidEventChannelSO _sceneUnloadingEvent;

        private void OnEnable()
        {
            _sceneUnloadingEvent.EventRaised += SceneUnloadingEvent_Raised;
        }

        private void OnDisable()
        {
            _sceneUnloadingEvent.EventRaised -= SceneUnloadingEvent_Raised;
        }

        private void SceneUnloadingEvent_Raised()
        {
            _inputMediator.DisableAllInput();
        }
    }
}