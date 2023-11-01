using CryptoQuest.Quest.Components;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public abstract class SaveObjectBase<TRef> : ISaveObject
    {
        public TRef RefObject { get; private set; }

        public SaveObjectBase(TRef obj)
        {
            RefObject = obj;
        }

        public abstract string Key { get; }

        public abstract string ToJson();

        public abstract IEnumerator CoFromJson(string json, Action<bool> callback = null);
    }
}