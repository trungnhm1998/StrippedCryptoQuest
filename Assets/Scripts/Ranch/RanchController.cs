using CryptoQuest.Input;
using CryptoQuest.Ranch.UI;
using UnityEngine;

namespace CryptoQuest.Ranch
{
    public class RanchController : MonoBehaviour
    {
        [field: SerializeField] public RanchSystem RanchSystem { get; private set; }
        [field: SerializeField] public RanchDialogsManager DialogManager { get; private set; }
        [field: SerializeField] public MerchantsInputManager Input { get; private set; }
        [field: SerializeField] public UIRanch UIRanch { get; private set; }

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

        public void Initialize()
        {
            RanchOpened();
            UIRanch.Initialize();
            DialogManager.ShowWelcomeDialog();
        }
        
        public void HideDialogs()
        {
            DialogManager.NormalDialogue.Hide();
        }

        private void RanchOpened()
        {
            UIRanch.FarmOpened();
            DialogManager.RanchOpened();
        }

        private void RanchClosed()
        {
            UIRanch.FarmClosed();
        }
    }
}