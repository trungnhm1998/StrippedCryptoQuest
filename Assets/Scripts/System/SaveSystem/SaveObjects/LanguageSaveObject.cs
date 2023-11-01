using CryptoQuest.Language;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public class LanguageSaveObject : SaveObjectBase<LanguageManager>
    {
        public LanguageSaveObject(LanguageManager obj): base(obj)
        {
        }

        public override string Key => "Language";

        public override string ToJson()
        {
            return JsonConvert.SerializeObject(RefObject.SaveData);
        }

        public override IEnumerator CoFromJson(string json, Action<bool> callback = null)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var saveData = (LanguageSave)JsonConvert.DeserializeObject(json);
                    if(saveData != null)
                    {
                        RefObject.SaveData = saveData;
                        if (callback != null) { callback(true); }
                        yield break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            if (callback != null) { callback(false); }
            yield break;
        }
    }
}