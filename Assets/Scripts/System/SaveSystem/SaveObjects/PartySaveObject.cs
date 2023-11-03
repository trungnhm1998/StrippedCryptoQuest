using CryptoQuest.Gameplay.PlayerParty;
using System;
using System.Collections;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public class PartySaveObject : SaveObjectBase<PartyManager>
    {
        public PartySaveObject(PartyManager obj): base(obj)
        {
        }

        public override string Key => "Party";

        public override string ToJson()
        {
            // return RefObject.PartyProvider.ToJson();
            return "";
        }

        public override IEnumerator CoFromJson(string json, Action<bool> callback = null)
        {
            yield break;
            // if (!string.IsNullOrEmpty(json))
            // {
            //     yield return RefObject.PartyProvider.CoFromJson(json);
            //     if (callback != null) { callback(true); }
            //     yield break;
            // }
            // if (callback != null) { callback(false); }
            // yield break;
        }
    }
}