using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle.ResultStates
{
    public class WonState : MonoBehaviour, IResultState
    {
        [SerializeField] private BattleUnloader _unloader;
        [SerializeField] private BattleContext _context;
        [SerializeField] private BattleLootManager _battleLootManager;

        public EBattleResult Result => EBattleResult.Won;

        public void OnEnter(BattleStateMachine stateMachine)
        {
            stateMachine.StartCoroutine(CoOnPresentWon());
        }

        public void OnExit(BattleStateMachine stateMachine) { }

        private IEnumerator CoOnPresentWon()
        {
            var loots = _battleLootManager.GetDroppedLoots();
            yield return _unloader.FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleWonEvent()
            {
                Battlefield = _context.CurrentBattlefield,
                Loots = loots
            });
        }

        public void RaiseSetEvent()
        {
            BattleEventBus.RaiseEvent(new TurnWonEvent());
        }
    }
}