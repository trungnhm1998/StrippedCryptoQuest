using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using CryptoQuest.Environment;

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

        private string _backEndUrl => _environmentSO.BackEndUrl;

        private UserProfile _profile;

        public UserProfile Profile
        {
            get { return _profile; }
        }

        private ApiToken _accessToken;

        public ApiToken AccessToken
        {
            get { return _accessToken; }
        }

        private ApiToken _refreshToken;

        public ApiToken RefreshToken
        {
            get { return _refreshToken; }
        }

        public InputField emailInputField;

        public InputField passwordInputField;

        private FirebaseUser _fbUser;

        public void Start()
        {
            FirebaseAuth.OnAuthStateChanged(gameObject.name, "OnUserSignedIn", "OnUserSignedOut");
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
                        _profile = responsePayload.data.user;
                        Debug.Log("API Logged: " + Profile.id + " - " + Profile.email);

                        _accessToken = responsePayload.data.token.access;
                        Debug.Log("AccessToken: " + AccessToken.token);

                        _refreshToken = responsePayload.data.token.refresh;
                        Debug.Log("RefreshToken: " + RefreshToken.token);

                        OnSignedInSuccess?.Invoke(_profile);
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
            _accessToken = null;
            _refreshToken = null;
            _profile = null;
        }

        public void OnError(string error)
        {
            OnSignedInFailed?.Invoke(error);
            Debug.LogError(error);
        }

        public bool IsLoggedIn()
        {
            return _fbUser != null && _profile != null && _accessToken != null;
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