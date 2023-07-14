using UnityEngine;
using System.Collections;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    public class HeroUnit : CharacterUnit
    {
        [Header("Events")]
        [SerializeField] private BattleUnitEventChannelSO _heroTurnEventChannel;

        public override IEnumerator Prepare()
        {
            _heroTurnEventChannel.RaiseEvent(this);
            yield return base.Prepare();
        }
    }
}