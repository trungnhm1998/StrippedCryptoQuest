using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    [Serializable]
    public class KeyValue
    {
        public string Key;

        [TextArea]
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
        public DateTime SavedTime;
        public string PlayerName = "";
        public string UUID = "";
        public List<KeyValue> Objects = new();

        [NonSerialized] private Dictionary<string, int> _objectsDictionary = new();

        private Dictionary<string, int> ObjectsDictionary
        {
            get
            {
                if (_objectsDictionary.Count == Objects.Count) return _objectsDictionary;
                _objectsDictionary.Clear();
                for (var index = 0; index < Objects.Count; index++)
                {
                    var data = Objects[index];
                    _objectsDictionary.Add(data.Key, index);
                }

                return _objectsDictionary;
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            value = "";
            if (!ObjectsDictionary.TryGetValue(key, out var index)) return false;
            value = Objects[index].Value;
            return true;
        }

        public string this[string key]
        {
            get => ObjectsDictionary.TryGetValue(key, out var index) ? Objects[index].Value : "";
            set
            {
                if (ObjectsDictionary.TryGetValue(key, out var index))
                {
                    Objects[index].Value = value;
                    return;
                }

                Objects.Add(new KeyValue(key, value));
            }
        }
    }
}