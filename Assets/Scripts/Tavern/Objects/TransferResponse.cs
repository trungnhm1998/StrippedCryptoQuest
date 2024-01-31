using System;
using CryptoQuest.Sagas.Objects;

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
        public float diamond;
        public int soul;
        public long time;
        public CharactersResponse.Data data;
    }
}