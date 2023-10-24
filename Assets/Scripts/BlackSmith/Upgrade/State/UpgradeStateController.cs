using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class UpgradeStateController : MonoBehaviour
    {
        public UnityAction UpgradeEvent;
        public UnityAction ExitUpgradeEvent;
        [field: SerializeField] public BlackSmithInputManager InputManager { get; private set; }
        [field: SerializeField] public UIBlackSmith UIBlackSmith { get; private set; }
        [field: SerializeField] public GameObject SelectedEquipmentPanel { get; private set; }
        [field: SerializeField] public GameObject UpgradeEquipmentPanel { get; private set; }
        [field: SerializeField] public GameObject UpgradeResultPanel { get; private set; }
        [field: SerializeField] public GameObject SelectActionPanel { get; private set; }
        [SerializeField] private UnityEvent _initListEquipment;

        public void InstantiateEquipment()
        {
            _initListEquipment.Invoke();
        }
    }
}