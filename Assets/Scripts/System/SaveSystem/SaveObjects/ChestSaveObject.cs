using CryptoQuest.Gameplay.Loot;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public class ChestSaveObject : SaveObjectBase<ChestManager>
    {
        public ChestSaveObject(ChestManager obj) : base(obj)
        {
        }

        public override string Key => "Chest";

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
                    var saveData = (ChestSave)JsonConvert.DeserializeObject(json);
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