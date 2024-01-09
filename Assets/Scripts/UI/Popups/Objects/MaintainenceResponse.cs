using System;

namespace CryptoQuest.UI.Popups.Objects
{
    [Serializable]
    public class MaintainenceResponse
    {
        public int code;
        public bool success;
        public string message;
        public long time;
        public Data data;

        [Serializable]
        public class Data
        {
            public string description;
            public string descriptionJp;
            public string startAt;
            public string endAt;
        }
    }
}