using System;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Presenters;
using CryptoQuest.Ranch.UI;
using CryptoQuest.Ranch.Upgrade.Presenters;
using CryptoQuest.Ranch.Upgrade.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.State
{
    public class RanchStateController : MonoBehaviour
    {
        public Action OpenSwapEvent;
        public Action OpenUpgradeEvent;
        public Action OpenEvolveEvent;
        public Action ExitStateEvent;

        [field: Header("Controllers")]
        [field: SerializeField] private Animator StateMachine { get; set; }

        [field: SerializeField] public RanchController Controller { get; private set; }
        [field: SerializeField] public RanchDialogsController DialogController { get; private set; }

        [field: Header("Data")]
        [field: SerializeField] public BeastInventorySO BeastInventory { get; private set; }

        [field: Header("UI")]
        [field: SerializeField] public UIBeastSwap UIBeastSwap { get; private set; }

        [field: SerializeField] public UIBeastUpgrade UIBeastUpgrade { get; private set; }
        [field: SerializeField] public UIBeastEvolve UIBeastEvolve { get; private set; }
        [field: SerializeField] public UIConfigBeastUpgradePresenter UIConfig { get; private set; }

        [field: Header("Presenters")]
        [field: SerializeField] public EvolvePresenter EvolvePresenter { get; private set; }

        [field: SerializeField] public UpgradePresenter UpgradePresenter { get; private set; }


        [field: Header("Evolve")]
        public int BeastEvolveId { get; set; }
        public bool EvolveStatus { get; set; }

        private void OnDisable()
        {
            BaseStateBehaviour[] behaviours = StateMachine.GetBehaviours<BaseStateBehaviour>();
            foreach (BaseStateBehaviour behaviour in behaviours) behaviour.Exit();
        }
    }
}