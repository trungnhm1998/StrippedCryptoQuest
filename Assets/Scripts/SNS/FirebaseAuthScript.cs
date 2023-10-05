using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using CryptoQuest.Environment;
using CryptoQuest.Networking.RestAPI;
using Newtonsoft.Json;
using System.Net;
using CryptoQuest.System;
using CryptoQuest.Networking;

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
            [JsonProperty("data")]
            public LoginData Data;
        }

        [Serializable]
        public class LoginData
        {
            [JsonProperty("user")]
            public UserProfile User;
            [JsonProperty("token")]
            public ApiTokenData Token;
        }

        [Serializable]
        public class ApiTokenData
        {
            [JsonProperty("access")]
            public ApiToken access;
            [JsonProperty("refresh")]
            public ApiToken refresh;
        }

        [SerializeField] private AuthorizationSO _authorizationSO;

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

        private const string URL_LOGIN = "/auth/crypto/login";

        public void Start()
        {
#if !UNITY_EDITOR
            FirebaseAuth.OnAuthStateChanged(gameObject.name, "OnUserSignedIn", "OnUserSignedOut");
#endif
        }

        private void LoginWithBackend()
        {
            var restAPINetworkController = ServiceProvider.GetService<IRestAPINetworkController>();

            LoginRequestPayload payload = new LoginRequestPayload(_fbUser.stsTokenManager.accessToken);

            restAPINetworkController.Post(URL_LOGIN, JsonConvert.SerializeObject(payload), OnLoginBESuccess, OnLoginBEFail);
        }

        private void OnLoginBEFail(Exception exception)
        {
            Debug.Log(exception.Message);
            OnSignedInFailed?.Invoke(exception.Message);
        }   
        
        private void OnLoginBESuccess(UnityWebRequest request)
        {
            Debug.Log("Payload: " + request.downloadHandler.text);

            _authorizationSO.Init(request.downloadHandler.text);
            
            if(_authorizationSO.Profile != null)
            {
                OnSignedInSuccess?.Invoke(_authorizationSO.Profile);
            }    
        }    

        public void OnUserSignedIn(string userJson)
        {
            if (!IsLoggedIn())
            {
                _fbUser = JsonUtility.FromJson<FirebaseUser>(userJson);
                if (_fbUser != null)
                {
                    LoginWithBackend();
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