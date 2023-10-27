using System;
using CryptoQuest.Networking;
using CryptoQuest.Networking.API;
using CryptoQuest.System;
using Newtonsoft.Json;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;

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

        // TODO: REFACTOR NETWORK
        // public UserProfile Profile
        // {
        //     get { return _authorizationSO.Profile; }
        // }
        //
        // public ApiToken AccessToken
        // {
        //     get { return _authorizationSO.AccessToken; }
        // }
        //
        // public ApiToken RefreshToken
        // {
        //     get { return _authorizationSO.RefreshToken; }
        // }

        public InputField emailInputField;

        public InputField passwordInputField;

        private FirebaseUser _fbUser;
        private bool _isSigning = false;

        private const string URL_LOGIN = "/auth/crypto/login";

        public void Start()
        {
            _isSigning = false;
#if !UNITY_EDITOR
            FirebaseAuth.OnAuthStateChanged(gameObject.name, "OnUserSignedIn", "OnUserSignedOut");
#endif
        }

        private void LoginWithBackend()
        {
            var restAPINetworkController = ServiceProvider.GetService<IRestClient>();

            LoginRequestPayload payload = new LoginRequestPayload(_fbUser.stsTokenManager.accessToken);

            // restAPINetworkController.Post(URL_LOGIN, JsonConvert.SerializeObject(payload), OnLoginBESuccess,
            //     OnLoginBEFail);
        }

        private void OnLoginBEFail(Exception exception)
        {
            Debug.Log(exception.Message);
            OnSignedInFailed?.Invoke(exception.Message);
            _isSigning = false;
        }

        private void OnLoginBESuccess(ResponseHelper res)
        {
            Debug.Log("Payload: " + res.Text);

            // _authorizationSO.Init(res.Text);
            //
            // if (_authorizationSO.Profile != null)
            // {
            //     Debug.Log("FirebaseAuthScript: Login BE success");
            //     OnSignedInSuccess?.Invoke(_authorizationSO.Profile);
            // }

            _isSigning = false;
        }

        public void OnUserSignedIn(string userJson)
        {
            if (!IsLoggedIn() && !_isSigning)
            {
                _isSigning = true;
                _fbUser = JsonUtility.FromJson<FirebaseUser>(userJson);
                if (_fbUser != null)
                {
                    Debug.Log("FirebaseAuthScript: Try to login BE");
                    LoginWithBackend();
                }
                else
                {
                    _isSigning = false;
                }
            }
        }

        public void OnUserSignedOut(string info)
        {
            Debug.Log(info);
            _fbUser = null;
            // _authorizationSO.Clear();
        }

        public void OnError(string error)
        {
            OnSignedInFailed?.Invoke(error);
            Debug.LogError(error);
        }

        public bool IsLoggedIn()
        {
            // return _fbUser != null && Profile != null && AccessToken != null;
            return false;
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
                Debug.Log("FirebaseAuthScript: Create user with email and password");
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
                Debug.Log("FirebaseAuthScript: Sign in with email and password no param");
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
                Debug.Log("FirebaseAuthScript: Sign in with email and passs with param");
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
                Debug.Log("FirebaseAuthScript: Sign with google");
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
                Debug.Log("FirebaseAuthScript: Sign with Facebook");
                FirebaseAuth.SignInWithFacebook(gameObject.name, "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }
    }
}