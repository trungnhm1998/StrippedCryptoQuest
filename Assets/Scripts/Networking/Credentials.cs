using System;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Networking
{
    /// <summary>
    /// This act as in memory data only "reducer" for authentication
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Networking/Credentials", fileName = "Credentials", order = 0)]
    public class Credentials : ScriptableObject
    {
        /// <summary>
        /// Only when login using email and password
        /// any social using Google, Facebook, Twitter, Wallet will not have this
        /// </summary>
        [field: SerializeField] public string Email { get; set; }

        [field: SerializeField] public string Password { get; set; }
        [field: SerializeField] public CredentialResponse Profile { get; set; }

        /// <summary>
        /// Check access token to validate if user has logged in
        /// </summary>
        public bool IsLoggedIn()
        {
            if (Profile != null && Profile.token != null && Profile.token.access != null)
            {
                var accessToken = Profile.token.access;
                if (!string.IsNullOrEmpty(accessToken.token))
                {
                    return DateTime.Parse(accessToken.expires).CompareTo(DateTime.Now) > 0;
                }
            }
            return false;
        }
    }
}