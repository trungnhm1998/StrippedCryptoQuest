using System;
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
        [field: SerializeField] public RanchController RanchController { get; private set; }
        
        [field: SerializeField] public UIBeastOrganization UIBeastOrganization { get; private set; }
      
        [field: SerializeField] public UIBeastList UIGameList { get; private set; }
        [field: SerializeField] public UIBeastList UIWalletList { get; private set; }
        private void OnDisable()
        {
            BaseStateBehaviour[] behaviours = StateMachine.GetBehaviours<BaseStateBehaviour>();
            foreach (BaseStateBehaviour behaviour in behaviours) behaviour.Exit();
        }
    }
}