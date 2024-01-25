using System;
using CryptoQuest.Actions;
using CryptoQuest.Events;
using IndiGames.Firebase.Bridge;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Sagas
{
    [Serializable]
    public class FirebaseAuthScript
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class AuthenticateUsingTwitter : AuthenticationSagaBase<LoginUsingTwitter>
    {
        [SerializeField] private LocalizedString _errorMessage;
        [SerializeField] private LocalizedStringEventChannelSO _localizedErrorPopupEventSO;

        protected override void HandleAuthenticate(LoginUsingTwitter ctx)
        {
            FirebaseAuth.SignInWithTwitter(gameObject.name, nameof(OnUserSignedIn), nameof(OnUserSignedOut));
        }

        protected override void OnUserSignedOut(string json)
        {
            base.OnUserSignedOut(json);
            FirebaseAuthScript response = JsonConvert.DeserializeObject<FirebaseAuthScript>(json);
            if (response.Code != ErrorCode.ACCOUNT_EXISTS) return;

            _localizedErrorPopupEventSO.RaiseEvent(_errorMessage);
        }
    }

    public static class ErrorCode
    {
        public const string ACCOUNT_EXISTS = "auth/account-exists-with-different-credential";
    }
}