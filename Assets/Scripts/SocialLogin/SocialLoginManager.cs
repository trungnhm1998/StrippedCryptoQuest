using System.Collections;
using CryptoQuest.Events;
using CryptoQuest.Networking.API;
using CryptoQuest.SNS;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.SocialLogin
{
    public class AuthenticateFormInfo
    {
        public string Email;
        public string Password;
    }

    public class SocialLoginManager : MonoBehaviour
    {
        [SerializeField] private FirebaseAuthScript _firebaseAuthScript;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _requestTwitterLoginEvent;
        [SerializeField] private VoidEventChannelSO _requestFacebookLoginEvent;
        [SerializeField] private VoidEventChannelSO _requestGoogleLoginEvent;
        [SerializeField] private VoidEventChannelSO _requestWalletLoginEvent;
        [SerializeField] private AuthenticateFormInfoEventChannelSO _requestAuthenticateFormWithFormEvent;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _loginSuccessEvent;
        [SerializeField] private StringEventChannelSO _loginFailEvent;

        private void OnEnable()
        {
            _requestTwitterLoginEvent.EventRaised += OnRequestTwitterLogin;
            _requestFacebookLoginEvent.EventRaised += OnRequestFacebookLogin;
            _requestGoogleLoginEvent.EventRaised += OnRequestGoogleLogin;
            _requestWalletLoginEvent.EventRaised += OnRequestWalletLogin;
            _requestAuthenticateFormWithFormEvent.EventRaised += OnLoginWithEmailAndPassword;
            _firebaseAuthScript.OnSignedInSuccess += OnSignInSuccess;
            _firebaseAuthScript.OnSignedInFailed += OnSignInFailed;
        }

        private void OnDisable()
        {
            _requestTwitterLoginEvent.EventRaised -= OnRequestTwitterLogin;
            _requestFacebookLoginEvent.EventRaised -= OnRequestFacebookLogin;
            _requestGoogleLoginEvent.EventRaised -= OnRequestGoogleLogin;
            _requestWalletLoginEvent.EventRaised -= OnRequestWalletLogin;
            _requestAuthenticateFormWithFormEvent.EventRaised -= OnLoginWithEmailAndPassword;
            _firebaseAuthScript.OnSignedInSuccess -= OnSignInSuccess;
            _firebaseAuthScript.OnSignedInFailed -= OnSignInFailed;
        }

        private void OnRequestFacebookLogin()
        {
            StartCoroutine(CoLoginSuccess());
        }

        private void OnRequestGoogleLogin()
        {
            _firebaseAuthScript.SignInWithGoogle();
            
        }

        private void OnRequestTwitterLogin()
        {
            StartCoroutine(CoLoginSuccess());
        }

        private void OnRequestWalletLogin()
        {
            StartCoroutine(CoLoginFail());
        }

        private void OnLoginWithEmailAndPassword(AuthenticateFormInfo authenticateFormInfo)
        {
            _firebaseAuthScript.SignInWithEmailAndPassword(authenticateFormInfo.Email, authenticateFormInfo.Password);
        }

        private void OnSignInSuccess(UserProfile userProfile)
        {
            _loginSuccessEvent.RaiseEvent();
        }

        private void OnSignInFailed(string error)
        {
            _loginFailEvent.RaiseEvent(error);
        }

        /// <summary>
        /// These are a temporary methods to simulate login results 
        /// TODO: Remove these methods when all the login is implemented
        /// </summary>
        private IEnumerator CoLoginSuccess()
        {
#if UNITY_EDITOR
            yield return new WaitForSeconds(.5f);
#else
            yield return new WaitForSeconds(1f);
#endif
            OnSignInSuccess(null);
        }

        private IEnumerator CoLoginFail()
        {
#if UNITY_EDITOR
            yield return new WaitForSeconds(.5f);
#else
            yield return new WaitForSeconds(1f);
#endif
           OnSignInFailed("Login Failed");
        }
    }
}