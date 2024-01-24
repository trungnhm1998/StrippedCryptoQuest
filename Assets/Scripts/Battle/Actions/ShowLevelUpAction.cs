using CryptoQuest.Battle.Components;
using IndiGames.Core.Events;

namespace CryptoQuest.Battle.Actions
{
    public class ShowLevelUpAction : ActionBase { }

    public class LevelUpAfterAddExpAction : ActionBase
    {
        public HeroBehaviour Hero { get; }
        public bool IsLevelUp;

        public LevelUpAfterAddExpAction(HeroBehaviour hero, bool isLevelUp)
        {
            Hero = hero;
            IsLevelUp = isLevelUp;
        }
    }
}