using System;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.SaveSystem.SaveObjects
{
    public class ChestSaveObject : SaveObjectBase<OpenedChestsSO>
    {
        public ChestSaveObject(OpenedChestsSO obj) : base(obj) { }

        public override string Key => "Chests";

        public override string ToJson() => JsonUtility.ToJson(RefObject.Chests);

        public override bool FromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    JsonUtility.FromJsonOverwrite(json, RefObject.Chests);
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