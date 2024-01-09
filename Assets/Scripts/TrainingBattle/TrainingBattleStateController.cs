using System;
using CryptoQuest.Input;
using CryptoQuest.TrainingBattle.State;
using UnityEngine;

namespace CryptoQuest.TrainingBattle
{
    public class TrainingBattleStateController : MonoBehaviour
    {
        [SerializeField] private Animator _stateMachine;
        [field: SerializeField] public MerchantsInputManager Input { get; private set; }
        [field: SerializeField] public TrainingBattleDialogController DialogController { get; private set; }
        public Action ExitStateEvent;
        public bool IsExitState { get; set;}
        public int PartyId { get; private set;}
        
        private void OnDisable()
        {
            var behaviours = _stateMachine.GetBehaviours<BaseStateBehaviour>();
            foreach (var behaviour in behaviours) behaviour.Exit();
        }

        public void SetPartyId(int partyId)
        {
            PartyId = partyId;
        }
    }
}