using CryptoQuest.Events.UI;
using CryptoQuest.Input;
using CryptoQuest.Ranch.UI;
using UnityEngine;

namespace CryptoQuest.Ranch
{
    public class RanchController : MonoBehaviour
    {
        [field: SerializeField] public RanchSystem RanchSystem { get; private set; }
        [field: SerializeField] public RanchDialogsController DialogController { get; private set; }
        [field: SerializeField] public MerchantsInputManager Input { get; private set; }
        [field: SerializeField] public UIRanch UIRanch { get; private set; }

        [field: Header("Events")]
        [field: SerializeField]
        public ShowWalletEventChannelSO ShowWalletEventChannel { get; private set; }

        private void OnEnable()
        {
            RanchSystem.RanchOpenedEvent += RanchOpened;
            RanchSystem.RanchClosedEvent += RanchClosed;
        }

        private void OnDisable()
        {
            RanchSystem.RanchOpenedEvent -= RanchOpened;
            RanchSystem.RanchClosedEvent -= RanchClosed;
        }

        public void Initialize() => UIRanch.Initialize();
        private void RanchOpened() => UIRanch.FarmOpened();
        private void RanchClosed() => UIRanch.FarmClosed();
    }
}