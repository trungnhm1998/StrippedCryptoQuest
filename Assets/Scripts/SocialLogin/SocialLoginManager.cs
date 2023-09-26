using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.SocialLogin
{
    public class SocialLoginManager : MonoBehaviour
    {
        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _requestLoginEvent;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _loginSuccessEvent;

        private void OnEnable()
        {
            _requestLoginEvent.EventRaised += OnRequestLogin;
        }

        private void OnDisable()
        {
            _requestLoginEvent.EventRaised -= OnRequestLogin;
        }

        private void OnRequestLogin()
        {
            Debug.Log("SocialLoginManager.OnRequestLogin");
            //TODO: indigames/CryptoQuestClient#1330
            _loginSuccessEvent.RaiseEvent();
        }
    }
}