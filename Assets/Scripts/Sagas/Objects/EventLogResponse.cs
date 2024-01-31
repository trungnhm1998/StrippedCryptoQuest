using System;

namespace CryptoQuest.Sagas.Objects
{
    [Serializable]
    public class EventLogResponse
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string uuid { get; set; }
        public int gold { get; set; }
        public float diamond { get; set; }
        public int soul { get; set; }
        public long time { get; set; }
    }
}