using CryptoQuest.Item;

namespace CryptoQuest.Battle.Components.SpecialSkillBehaviours
{
    public interface IStealable
    {
        bool TrySteal(out ItemInfo item);
    }
}