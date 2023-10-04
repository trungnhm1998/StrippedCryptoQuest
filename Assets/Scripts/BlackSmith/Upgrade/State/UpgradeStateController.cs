using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class UpgradeStateController : MonoBehaviour
    {
        [field: SerializeField] public InputMediatorSO Input { get; private set; }
        [field: SerializeField] public GameObject UpgradePanel {get; private set;}
    }
}