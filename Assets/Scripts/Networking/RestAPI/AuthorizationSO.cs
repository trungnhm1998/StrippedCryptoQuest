using CryptoQuest.SNS;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CryptoQuest.SNS.FirebaseAuthScript;

namespace CryptoQuest.Networking.RestAPI
{
    [CreateAssetMenu(menuName ="AUthro/x")]
    public class AuthorizationSO : ScriptableObject
    {
        public Action<ApiToken> OnAccessTokenChanged;

        public UserProfile Profile;

        public ApiToken AccessToken;

        public ApiToken RefreshToken;

        public void Init(string dataJson)
        {
            LoginResponsePayload responsePayload = JsonConvert.DeserializeObject<LoginResponsePayload>(dataJson);

            if(responsePayload != null && responsePayload.Data != null)
            {
                Profile = responsePayload.Data.User;
                Debug.Log("API Logged: " + Profile.id + " - " + Profile.email);

                AccessToken = responsePayload.Data.Token.access;
                Debug.Log("AccessToken: " + AccessToken.Token);

                RefreshToken = responsePayload.Data.Token.refresh;
                Debug.Log("RefreshToken: " + RefreshToken.Token);

                OnAccessTokenChanged?.Invoke(AccessToken);
            }
        }    

        public void Clear()
        {
            Profile = null;
            AccessToken = null;
            RefreshToken = null;
        }    
    }
}
