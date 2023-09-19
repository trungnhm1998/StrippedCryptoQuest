using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace CryptoQuest.SNS
{
    /// <summary>
    /// Attachable script to handle SNS login
    /// </summary>
    public class FirebaseAuthScript : MonoBehaviour
    {
        [Serializable]
        class LoginRequestPayload {
            public LoginRequestPayload(string token)
            {
                this.token = token;
            }
            public string token;
        }

        [Serializable]
        class LoginResponsePayload {
            public LoginData data;
        }

        [Serializable]
        class LoginData {
            public UserProfile user;
            public ApiTokenData token;
        }

        [Serializable]
        class ApiTokenData {
            public ApiToken access;
            public ApiToken refresh;
        }

        const string BACKEND_URL = "http://dev.api-game.crypto-quest.org:3000/v1";

        private UserProfile _profile;
        public UserProfile Profile { get { return _profile; } }

        private ApiToken _accessToken;
        public ApiToken AccessToken { get { return _accessToken; } }

        private ApiToken _refreshToken;
        public ApiToken RefreshToken { get { return _refreshToken; } }

        public InputField emailInputField;

        public InputField passwordInputField;

        private FirebaseUser _fbUser;

        void Start()
        {
            FirebaseAuth.OnAuthStateChanged(gameObject.name, "OnUserSignedIn", "OnUserSignedOut");
        }

        IEnumerator LoginWithBackend()
        {
            LoginRequestPayload payload = new LoginRequestPayload(_fbUser.stsTokenManager.accessToken);
            using (UnityWebRequest request = UnityWebRequest.Post(BACKEND_URL + "/auth/crypto/login", JsonUtility.ToJson(payload), "application/json"))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    Debug.Log("Payload: " + request.downloadHandler.text);
                    LoginResponsePayload responsePayload = JsonUtility.FromJson<LoginResponsePayload>(request.downloadHandler.text);
                    if(responsePayload != null && responsePayload.data != null)
                    {
                        _profile = responsePayload.data.user;
                        Debug.Log("API Logged: " + Profile.id +  " - " + Profile.email);

                        _accessToken = responsePayload.data.token.access;
                        Debug.Log("AccessToken: " + AccessToken.token);

                        _refreshToken = responsePayload.data.token.refresh;
                        Debug.Log("RefreshToken: " + RefreshToken.token);
                    }
                }
            }
        }

        public void OnUserSignedIn(string userJson)
        {
            if(!IsLoggedIn())
            {
                _fbUser = JsonUtility.FromJson<FirebaseUser>(userJson);
                if(_fbUser != null) {
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
            Debug.LogError(error);
        }

        public bool IsLoggedIn() {
            return _fbUser != null && _profile != null && _accessToken != null;
        }

        public void SignOut()
        {
            if(IsLoggedIn())
            {
                FirebaseAuth.SignOut(gameObject.name, "OnUserSignedOut", "OnError");
            }
        }

        public void CreateUserWithEmailAndPassword()
        {
            if(!IsLoggedIn())
            {
                FirebaseAuth.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }

        public void SignInWithEmailAndPassword()
        {
            if(!IsLoggedIn())
            {
                FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "OnUserSignedIn", "OnError");
            }
            else
            {
                Debug.Log("[DBG] User already logged in" + _fbUser.email);
            }
        }

        public void SignInWithGoogle()
        {
            if(!IsLoggedIn())
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
            if(!IsLoggedIn())
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
