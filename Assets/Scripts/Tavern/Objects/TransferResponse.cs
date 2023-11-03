using System;

namespace CryptoQuest.Tavern.Objects
{
    [Serializable]
    public class TransferResponse
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
    }
}