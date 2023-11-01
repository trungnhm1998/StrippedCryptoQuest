using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public class SceneSaveObject : SaveObjectBase<SceneScriptableObject>
    {
        public SceneSaveObject(SceneScriptableObject obj) : base(obj)
        {
        }

        public override string Key => "Scene";

        public override string ToJson()
        {
            return JsonUtility.ToJson(RefObject);
        }

        public override IEnumerator CoFromJson(string json, Action<bool> callback = null)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JsonUtility.FromJsonOverwrite(json, RefObject);
                if (callback != null) { callback(true); }
                yield break;
            }
            if (callback != null) { callback(false); }
            yield break;
        }
    }
}