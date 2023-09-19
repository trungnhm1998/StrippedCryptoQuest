using System;

namespace CryptoQuest.SNS
{
    [Serializable]
    public class UserProfile
    {
        public string id;

        public string name;

        public string socialUserName;

        public string email;

        public string avatar_image_url;

        public string walletAddress;
    }
}
