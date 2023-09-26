using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Item
{
    public class ConsumableDatabase : GenericAssetReferenceDatabase<string, ConsumableSO>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(ConsumableSO consumable) => consumable.ID;
#endif
    }
}
