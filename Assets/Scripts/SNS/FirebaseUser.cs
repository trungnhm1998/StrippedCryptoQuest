using System;

namespace CryptoQuest.SNS
{
    [Serializable]
    public class FirebaseProvider
    {
        public string providerId;

        public string uid;

        public string displayName;

        public string email;

        public string photoURL;

        public string phoneNumber;
    }

    [Serializable]
    public class FirebaseToken
    {
        public string accessToken;
        
        public string refreshToken;

        public DateTime expirationTime;
    }

    [Serializable]
    public class FirebaseUser
    {
        public string uid;
        
        public string email;

        public bool emailVerified;
        
        public string displayName;

        public bool isAnonymous;
        
        public string photoURL;

        public DateTime createdAt;

        public DateTime lastLoginAt;

        public FirebaseProvider[] providerData;
        
        public FirebaseToken stsTokenManager;
    }
}