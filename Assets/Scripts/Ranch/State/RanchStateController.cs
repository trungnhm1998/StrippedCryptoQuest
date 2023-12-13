using System;
using CryptoQuest.Ranch.Evolve.Presenters;
using CryptoQuest.Ranch.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.State
{
    public class RanchStateController : MonoBehaviour
    {
        public Action OpenSwapEvent;
        public Action OpenUpgradeEvent;
        public Action OpenEvolveEvent;
        public Action ExitStateEvent;

        [field: SerializeField] private Animator StateMachine { get; set; }
        [field: SerializeField] public RanchController Controller { get; private set; }

        [field: Header("UI")]
        [field: SerializeField] public UIBeastSwap UIBeastSwap { get; private set; }

        [field: SerializeField] public UIBeastUpgrade UIBeastUpgrade { get; private set; }
        [field: SerializeField] public UIBeastEvolve UIBeastEvolve { get; private set; }
        [field: SerializeField] public EvolvePresenter EvolvePresenter { get; private set; }
        [field: SerializeField] public RanchDialogsManager DialogManager { get; private set; }

        private void OnDisable()
        {
            BaseStateBehaviour[] behaviours = StateMachine.GetBehaviours<BaseStateBehaviour>();
            foreach (BaseStateBehaviour behaviour in behaviours) behaviour.Exit();
        }
    }
}