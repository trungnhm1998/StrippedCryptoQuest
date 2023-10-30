using System;

namespace CryptoQuest.Sagas.Objects
{
    [Serializable]
    public class FirebaseUser
    {
        public string uid;
        public string email;
        public bool emailVerified;
        public bool isAnonymous;
        public ProviderData[] providerData;
        public StsTokenManager stsTokenManager;
        public string createdAt;
        public string lastLoginAt;
        public string apiKey;
        public string appName;
    }

    [Serializable]
    public class ProviderData
    {
        public string providerId;
        public string uid;
        public object displayName;
        public string email;
        public object phoneNumber;
        public object photoURL;
    }

    [Serializable]
    public class StsTokenManager
    {
        public string refreshToken;
        public string accessToken;
        public long expirationTime;
    }
}