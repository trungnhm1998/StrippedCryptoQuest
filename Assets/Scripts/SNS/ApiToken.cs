using System;

namespace CryptoQuest.SNS
{
    [Serializable]
    public class ApiToken
    {
        public string token;

        public DateTime expires;
    }
}
