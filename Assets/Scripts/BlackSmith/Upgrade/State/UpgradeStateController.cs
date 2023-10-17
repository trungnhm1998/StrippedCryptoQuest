using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class UpgradeStateController : MonoBehaviour
    {
        public UnityAction OnUpgradeSuccess;
        [field: SerializeField] public BlackSmithInputManager InputManager { get; private set; }
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