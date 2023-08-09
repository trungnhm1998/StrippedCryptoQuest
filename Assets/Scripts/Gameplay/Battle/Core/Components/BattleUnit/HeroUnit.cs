using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class HeroUnit : CharacterUnit
    {
        [Header("Events")]
        [SerializeField] private BattleUnitEventChannelSO _heroTurnEventChannel;

        protected override void InitBattleLogic()
        {
            UnitLogic = new HeroUnitLogic(this, TagConfig);
            UnitLogic.Init();
        }

        public override IEnumerator Prepare()
        {
            _heroTurnEventChannel.RaiseEvent(this);
            yield return base.Prepare();
        }
    }
}