using System;
using CryptoQuest.Language.Settings;
using UnityEngine;

namespace CryptoQuest.SaveSystem.SaveObjects
{
    public class LanguageSaveObject : SaveObjectBase<LanguageSettingSO>
    {
        public LanguageSaveObject(LanguageSettingSO obj) : base(obj) { }

        public override string Key => RefObject.Guid;

        public override string ToJson()
        {
            return JsonUtility.ToJson(RefObject);
        }

        public override bool FromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    JsonUtility.FromJsonOverwrite(json, RefObject);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            return false;
        }
    }
}