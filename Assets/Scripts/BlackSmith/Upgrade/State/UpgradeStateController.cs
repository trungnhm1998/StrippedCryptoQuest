using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class UpgradeStateController : MonoBehaviour
    {
        public UnityAction UpgradeEvent;
        public UnityAction ExitUpgradeEvent;
        [field: SerializeField] public BlackSmithDialogsPresenter DialogsPresenter { get; private set; }
        [field: SerializeField] public MerchantsInputManager InputManager { get; private set; }
        [field: SerializeField] public UpgradePresenter UpgradePresenter { get; private set; }
        [field: SerializeField] public GameObject SelectedEquipmentPanel { get; private set; }
        [field: SerializeField] public GameObject UpgradeEquipmentPanel { get; private set; }
        [field: SerializeField] public GameObject UpgradeResultPanel { get; private set; }
        [field: SerializeField] public GameObject SelectActionPanel { get; private set; }
    }
}