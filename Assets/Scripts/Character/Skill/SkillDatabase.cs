using CryptoQuest.Character.Ability;
using IndiGames.Core.Database;

namespace CryptoQuest.Character
{
    public class SkillDatabase : AssetReferenceDatabaseT<int, CastableAbility>
    {
#if UNITY_EDITOR

        protected override int Editor_GetInstanceId(CastableAbility asset) => asset.Parameters.Id;

        protected override bool Editor_Validate((CastableAbility asset, string path) data)
        {
            return data.asset.Parameters.Id != 0;
        }
#endif
    }
}