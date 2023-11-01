using CryptoQuest.Gameplay.Inventory;
using System;
using System.Collections;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public class InventorySaveObject : SaveObjectBase<InventoryController>
    {
        public InventorySaveObject(InventoryController obj): base(obj)
        {
        }

        public override string Key => "Inventory";

        public override string ToJson()
        {
            return RefObject.Inventory.ToJson();
        }

        public override IEnumerator CoFromJson(string json, Action<bool> callback = null)
        {
            if (!string.IsNullOrEmpty(json))
            {
                yield return RefObject.Inventory.CoFromJson(json);
                if (callback != null) { callback(true); }
                yield break;
            }
            if (callback != null) { callback(false); }
            yield break;
        }
    }
}