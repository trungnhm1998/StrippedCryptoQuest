using IndiGames.Core.Database;
using CryptoQuest.Gameplay.Battle;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterDatabase : AssetReferenceDatabaseT<string, EncounterData>
    {
#if UNITY_EDITOR
        protected override string Editor_GetInstanceId(EncounterData data) => data.ID;
#endif
    }
}