using System.Collections;
using CryptoQuest.Events;
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
        }

        private void OnDisable()
        {
            _requestTwitterLoginEvent.EventRaised -= OnRequestTwitterLogin;
            _requestFacebookLoginEvent.EventRaised -= OnRequestFacebookLogin;
            _requestGoogleLoginEvent.EventRaised -= OnRequestGoogleLogin;
            _requestWalletLoginEvent.EventRaised -= OnRequestWalletLogin;
            _requestAuthenticateFormWithFormEvent.EventRaised -= OnLoginWithEmailAndPassword;
        }

        private void OnRequestFacebookLogin()
        {
            StartCoroutine(CoLoginSuccess());
        }

        private void OnRequestGoogleLogin()
        {
            _firebaseAuthScript.SignInWithGoogle();
            Subscribe();
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
            Subscribe();
        }

        private void Subscribe()
        {
            _firebaseAuthScript.OnSignedInSuccess += OnSignInSuccess;
            _firebaseAuthScript.OnSignedInFailed += OnSignInFailed;
        }

        private void Unsubscribe()
        {
            _firebaseAuthScript.OnSignedInSuccess -= OnSignInSuccess;
            _firebaseAuthScript.OnSignedInFailed -= OnSignInFailed;
        }

        private void OnSignInSuccess(UserProfile userProfile)
        {
            Unsubscribe();
            _loginSuccessEvent.RaiseEvent();
        }

        private void OnSignInFailed(string error)
        {
            Unsubscribe();
            _loginFailEvent.RaiseEvent(error);
        }

        /// <summary>
        /// These are a temporary methods to simulate login results 
        /// TODO: Remove these methods when all the login is implemented
        /// </summary>
        private IEnumerator CoLoginSuccess()
        {
            yield return new WaitForSeconds(5f);
            _loginSuccessEvent.RaiseEvent();
        }

        private IEnumerator CoLoginFail()
        {
            yield return new WaitForSeconds(3f);
            _loginFailEvent.RaiseEvent("Login Failed");
        }
    }
}