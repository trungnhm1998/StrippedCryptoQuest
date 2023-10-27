using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Networking
{
    public class LoginController : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;

        private void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += Login;
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= Login;
        }

        private void Login()
        {
            ActionDispatcher.Dispatch(new LoginAction("test@mail.com", "pass"));
            // _loginAction.Dispatch("Test");
            // _loginAction.Dispatch<SomeClass>("Test").Subscribe(OnResponse);
        }
    }
}