using System;

namespace CryptoQuest.Sagas.Objects
{
    [Serializable]
    public class CommonResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
    }
}