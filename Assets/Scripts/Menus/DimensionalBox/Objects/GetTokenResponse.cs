using System;

namespace CryptoQuest.Menus.DimensionalBox.Objects
{
    [Serializable]
    public class GetTokenResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public Data data;

        [Serializable]
        public class Data
        {
            public string address;
            public int metad;
            public int diamond;
        }
    }
}