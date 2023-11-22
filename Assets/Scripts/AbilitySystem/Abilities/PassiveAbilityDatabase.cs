using IndiGames.Core.Database;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class PassiveAbilityDatabase : AssetReferenceDatabaseT<int, PassiveAbility>
    {
#if UNITY_EDITOR
        protected override int Editor_GetInstanceId(PassiveAbility asset) => asset.Id;

        protected override bool Editor_Validate((PassiveAbility asset, string path) data) =>
            !data.path.Contains("Template") && base.Editor_Validate(data);
#endif
    }
}