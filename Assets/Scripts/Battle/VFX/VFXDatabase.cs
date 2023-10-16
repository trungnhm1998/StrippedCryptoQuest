using CryptoQuest.Item;
using IndiGames.Core.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class VFXDatabase : AssetReferenceDatabaseT<string, VFXDataSO>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(VFXDataSO vfx) => vfx.Id;
#endif
    }
}
