using CryptoQuest.Character.Ability;
using IndiGames.Core.Database;

namespace CryptoQuest.Character
{
    public class SkillDatabase : AssetReferenceDatabaseT<int, CastSkillAbility>
    {
#if UNITY_EDITOR

        protected override int Editor_GetInstanceId(CastSkillAbility asset) => asset.Parameters.Id;

        protected override bool Editor_Validate((CastSkillAbility asset, string path) data)
        {
            return data.asset.Parameters.Id != 0;
        }
#endif
    }
}