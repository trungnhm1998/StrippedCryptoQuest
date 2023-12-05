using CryptoQuest.Battle.Components;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStonePassiveController : CharacterComponentBase
    {
        public void ApplyPassives(IMagicStone stone)
        {
            foreach (var passive in stone.Passives) Character.AbilitySystem.GiveAbility(passive);
        }

        public void RemovePassives(IMagicStone stone)
        {
            foreach (var passive in stone.Passives) Character.AbilitySystem.RemoveAbility(passive);
        }
    }
}