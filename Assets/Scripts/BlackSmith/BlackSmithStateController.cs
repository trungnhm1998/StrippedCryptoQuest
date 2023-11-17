using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithStateController : MonoBehaviour
    {
        [field: SerializeField] public UIBlackSmith UIBlackSmith { get; private set; }
        public UnityAction OpenUpgradeEvent;
        public UnityAction OpenEvolveEvent;
        public UnityAction<bool> ExitStateEvent;
    }
}
