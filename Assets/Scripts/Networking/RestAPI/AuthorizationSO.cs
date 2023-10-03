using CryptoQuest.SNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Networking.RestAPI
{
    [CreateAssetMenu(menuName ="AUthro/x")]
    public class AuthorizationSO : ScriptableObject
    {
        public UserProfile Profile;

        public ApiToken AccessToken;

        public ApiToken RefreshToken;

        public void Clear()
        {
            Profile = null;
            AccessToken = null;
            RefreshToken = null;
        }    
    }
}
