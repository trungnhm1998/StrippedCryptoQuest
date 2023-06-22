using Core.Runtime.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Input
{
    public class InputMediatorController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediatorSO;
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
            _inputMediatorSO.DisableAllInput();
        }
    }
}