using System;

namespace IndiGames.Firebase.Struct
{
    [Serializable]
    public struct FirebaseUser
    {
        public string displayName;

        public string email;

        public bool isAnonymous;

        public bool isEmailVerified;

        public FirebaseUserMetadata metadata;

        public string phoneNumber;

        public FirebaseUserProvider[] providerData;

        public string providerId;

        public string uid;
    }

    [Serializable]
    public class FirebaseUserMetadata
    {
        public ulong lastSignInTimestamp;

        public ulong creationTimestamp;
    }

    [Serializable]
    public class FirebaseUserProvider
    {
        public string displayName;

        public string email;

        public string photoUrl;

        public string providerId;

        public string userId;
    }
}