using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    [Serializable]
    public class KeyValue
    {
        public string Key;

        public string Value;

        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    [Serializable]
    public class SaveData
    {
        public string PlayerName;
        public string LastExploreScene;
        public List<KeyValue> objects = new();
    }
}