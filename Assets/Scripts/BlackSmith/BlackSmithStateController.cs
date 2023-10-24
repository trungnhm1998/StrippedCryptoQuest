using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.EvolveStates;
using CryptoQuest.BlackSmith.Upgrade.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.StateMachine
{
    public class BlackSmithStateController : MonoBehaviour
    {
        [field: SerializeField] public BlackSmithInputManager Input { get; private set; }
        public UnityAction OpenUpgradeEvent;
        public UnityAction OpenEvolveEvent;
        public UnityAction CloseStateEvent;
    }
}
