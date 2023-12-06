using CryptoQuest.Battle.Components;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStonePassiveController : CharacterComponentBase
    {
        private PassivesController _passiveController;

        private void Awake()
        {
            _passiveController = GetComponent<PassivesController>();
        }

        public void ApplyPassives(IMagicStone stone)
        {
            foreach (var passive in stone.Passives)
            {
                _passiveController.ApplyPassive(passive);
            }
        }

        public void RemovePassives(IMagicStone stone)
        {
            foreach (var passive in stone.Passives)
            {
                // _passiveController.RemoveAbility(passive);
            }
        }
    }
}