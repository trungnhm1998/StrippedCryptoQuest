using CryptoQuest.Networking.API;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Networking
{
    [CreateAssetMenu(menuName = "Create Credentials", fileName = "Credentials", order = 0)]
    public class Credentials : ScriptableObject
    {
        [field: SerializeField] public UserProfile Profile { get; private set; }
        [field: SerializeField] public ApiToken AccessToken { get; private set; }
        [field: SerializeField] public ApiToken RefreshToken { get; private set; }

        private void OnEnable()
        {
            ServiceProvider.Provide(this);
        }

        public void Save(Data responseData)
        {
            Profile = responseData.user;
            AccessToken = responseData.token.access;
            RefreshToken = responseData.token.refresh;
        }
    }
}