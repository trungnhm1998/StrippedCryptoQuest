using System;
using System.Linq;
using CryptoQuest.Character.Enemy;

namespace CryptoQuest.Battle.Components
{
    public class StealableBehaviour : CharacterComponentBase
    {
        private Drop[] _stealableDrops = Array.Empty<Drop>();

        public override void Init()
        {
            var provider = Character.GetComponent<IDropsProvider>();
            _stealableDrops = provider.GetDrops().Where(d => d.Stealable).ToArray();
        }
    }
}