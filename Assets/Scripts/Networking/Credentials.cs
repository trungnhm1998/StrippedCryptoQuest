using CryptoQuest.Sagas;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Networking
{
    /// <summary>
    /// This act as in memory data only "reducer" for authentication
    /// </summary>
    [CreateAssetMenu(menuName = "Create Credentials", fileName = "Credentials", order = 0)]
    public class Credentials : ScriptableObject
    {
        /// <summary>
        /// Only when login using email and password
        /// any social using Google, Facebook, Twitter, Wallet will not have this
        /// </summary>
        [field: SerializeField] public string Email { get; set; }

        [field: SerializeField] public string Password { get; set; }
        [field: SerializeField] public CredentialResponse Profile { get; set; }

        private void OnEnable()
        {
            ServiceProvider.Provide(this);
        }
    }
}