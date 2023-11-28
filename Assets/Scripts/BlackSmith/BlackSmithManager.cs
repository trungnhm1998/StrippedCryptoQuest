using System;
using CryptoQuest.BlackSmith.ScriptableObjects;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithManager : MonoBehaviour
    {
        [field: SerializeField] public MerchantsInputManager BlackSmithInput { get; private set; }

        [field: SerializeField] public UIBlackSmithOverview OverviewUI { get; private set; }
        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }

        [Header("Listening to")]
        [SerializeField] private ShowBlackSmithEventChannelSO _openBlackSmithEvent;

        private BlackSmithStateMachine _stateMachine;

        private void OnEnable()
        {
            _openBlackSmithEvent.EventRaised += InitStateMachine;
        }

        private void OnDisable()
        {
            _openBlackSmithEvent.EventRaised -= InitStateMachine;
        }

        private void OnDestroy()
        {
            _stateMachine?.CurrentState?.OnDestroy();
        }

        private void InitStateMachine()
        {
            _stateMachine = new(this);
        }
    }
}
