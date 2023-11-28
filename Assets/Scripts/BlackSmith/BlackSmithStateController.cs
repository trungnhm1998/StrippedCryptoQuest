using System;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    [Obsolete]
    public class BlackSmithStateController : MonoBehaviour
    {
        public UnityAction OpenUpgradeEvent;
        public UnityAction OpenEvolveEvent;
        public UnityAction<bool> ExitStateEvent;
    }
}
