using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(menuName = "QuestSystem/Chainable Action Node/PlayBattleNode")]
    public class PlayBattleChainableNode : ActionChainableNodeSO
    {
        [SerializeField] private Battlefield _battlefieldToLoad;
        [SerializeField] private ActionChainableNodeSO _lostActionChainableNode;

        [Header("Events setup")]
        [SerializeField] private BattleResultEventSO _battleWonEvent;

        [SerializeField] private BattleResultEventSO _battleLostEvent;

        public override void Execute()
        {
            _battleWonEvent.EventRaised += WonBattle;
            _battleLostEvent.EventRaised += LostBattle;
            BattleLoader.RequestLoadBattle(_battlefieldToLoad);
        }

        private void LostBattle(Battlefield battlefield)
        {
            _battleWonEvent.EventRaised -= WonBattle;
            _battleLostEvent.EventRaised -= LostBattle;
            if (_lostActionChainableNode == null)
                return;
            _lostActionChainableNode.Execute();
        }

        private void WonBattle(Battlefield battlefield)
        {
            _battleWonEvent.EventRaised -= WonBattle;
            _battleLostEvent.EventRaised -= LostBattle;
            ExecuteNextNode();
        }
    }
}