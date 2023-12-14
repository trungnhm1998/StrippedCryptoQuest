using System;

namespace CryptoQuest.Sagas.Objects
{
    [Serializable]
    public class ProfileResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public ProfileDataResponse data;
    }

    [Serializable]
    public class ProfileDataResponse
    {
        public string socialUserId;
        public string name;
        public string socialUserName;
        public int socialPlatform;
        public string walletAddress;
    }
}