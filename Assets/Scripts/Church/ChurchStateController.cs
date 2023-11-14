using System;
using CryptoQuest.Church.State;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Church
{
    public class ChurchStateController : MonoBehaviour
    {
        [SerializeField] private Animator _stateMachine;
        [field: SerializeField] public MerchantsInputManager Input { get; private set; }
        [field: SerializeField] public ChurchPresenter Presenter { get; private set; }
        [field: SerializeField] public ChurchDialogConroller DialogController { get; private set; }
        public Action ExitStateEvent;
        public bool IsExitState;

        private void OnDisable()
        {
            var behaviours = _stateMachine.GetBehaviours<BaseStateBehaviour>();
            foreach (var behaviour in behaviours) behaviour.Exit();
        }
    }
}
