using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using CryptoQuest.Environment;
using CryptoQuest.Networking.RestAPI;

namespace CryptoQuest.SNS
{
    /// <summary>
    /// Attachable script to handle SNS login
    /// </summary>
    public class FirebaseAuthScript : MonoBehaviour
    {
        public Action<UserProfile> OnSignedInSuccess;
        public Action<string> OnSignedInFailed;

        [Serializable]
        public class LoginRequestPayload
        {
            public LoginRequestPayload(string token)
            {
                this.token = token;
            }

            public string token;
        }

        [Serializable]
        public class LoginResponsePayload
        {
            public LoginData data;
        }

        [Serializable]
        public class LoginData
        {
            public UserProfile user;
            public ApiTokenData token;
        }

        [Serializable]
        public class ApiTokenData
        {
            public ApiToken access;
            public ApiToken refresh;
        }

        [SerializeField] private EnvironmentSO _environmentSO;
        [SerializeField] private AuthorizationSO _authorizationSO;

        private string _backEndUrl => _environmentSO.BackEndUrl;

        public UserProfile Profile
        {
            get { return _authorizationSO.Profile; }
        }

        public ApiToken AccessToken
        {
            get { return _authorizationSO.AccessToken; }
        }

        public ApiToken RefreshToken
        {
            get { return _authorizationSO.RefreshToken; }
        }

        public InputField emailInputField;

        public InputField passwordInputField;

        private FirebaseUser _fbUser;

        public void Start()
        {
#if !UNITY_EDITOR
            FirebaseAuth.OnAuthStateChanged(gameObject.name, "OnUserSignedIn", "OnUserSignedOut");
#endif
        }

        private IEnumerator LoginWithBackend()
        {
            LoginRequestPayload payload = new LoginRequestPayload(_fbUser.stsTokenManager.accessToken);
            using (UnityWebRequest request = UnityWebRequest.Post(_backEndUrl + "/auth/crypto/login",
                       JsonUtility.ToJson(payload), "application/json"))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                    OnSignedInFailed?.Invoke(request.error);
                }
                else
                {
                    Debug.Log("Payload: " + request.downloadHandler.text);
                    LoginResponsePayload responsePayload =
                        JsonUtility.FromJson<LoginResponsePayload>(request.downloadHandler.text);
                    if (responsePayload != null && responsePayload.data != null)
                    {
                        _authorizationSO.Profile = responsePayload.data.user;
                        Debug.Log("API Logged: " + Profile.id + " - " + Profile.email);

                        _authorizationSO.AccessToken = responsePayload.data.token.access;
                        Debug.Log("AccessToken: " + AccessToken.token);

                        _authorizationSO.RefreshToken = responsePayload.data.token.refresh;
                        Debug.Log("RefreshToken: " + RefreshToken.token);

                        OnSignedInSuccess?.Invoke(_authorizationSO.Profile);
                    }
                }
            }
        }

        public void OnUserSignedIn(string userJson)
        {
            if (!IsLoggedIn())
            {
                _fbUser = JsonUtility.FromJson<FirebaseUser>(userJson);
                if (_fbUser != null)
                {
                    StartCoroutine(LoginWithBackend());
                }
            }
        }

        public void OnUserSignedOut(string info)
        {
            Debug.Log(info);
            _fbUser = null;
            _authorizationSO.Clear();
        }

        public void OnError(string error)
        {
            OnSignedInFailed?.Invoke(error);
            Debug.LogError(error);
        }

        public bool IsLoggedIn()
        {
            return _fbUser != null && Profile != null && AccessToken != null;
        }

        public void SignOut()
        {
            if (IsLoggedIn())
            {
                FirebaseAuth.SignOut(gameObject.name, "OnUserSignedOut", "OnError");
            }
        }

        public void CreateUserWithEmailAndPassword()
        {
            if (!IsLoggedIn())
            {
                FirebaseAuth.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text,
                    gameObject.name, "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }

        public void SignInWithEmailAndPassword()
        {
            if (!IsLoggedIn())
            {
                FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name,
                    "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }

        public void SignInWithEmailAndPassword(string email, string password)
        {
            if (!IsLoggedIn())
            {
                FirebaseAuth.SignInWithEmailAndPassword(email, password, gameObject.name, "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }

        public void SignInWithGoogle()
        {
            if (!IsLoggedIn())
            {
                FirebaseAuth.SignInWithGoogle(gameObject.name, "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }

        public void SignInWithFacebook()
        {
            if (!IsLoggedIn())
            {
                FirebaseAuth.SignInWithFacebook(gameObject.name, "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }
    }
}