using CryptoQuest.Ranch.UI;
using UnityEngine;

namespace CryptoQuest.Ranch
{
    public class RanchController : MonoBehaviour
    {
        [field: SerializeField] public RanchSystem RanchSystem { get; private set; }
        [field: SerializeField] public UIRanch UIRanch { get; private set; }
        [field: SerializeField] public RanchDialogsManager RanchDialogsManager { get; private set; }
        [field: SerializeField] public RanchDialogsManager DialogManager { get; private set; }
        [field: SerializeField] public RanchInputManager Input { get; private set; }

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
            UIRanch.Init();
            DialogManager.ShowWelcomeDialog();
        }
        
        public void HideDialogs()
        {
            DialogManager.NormalDialogue.Hide();
        }

        private void RanchOpened()
        {
            UIRanch.FarmOpened();
            RanchDialogsManager.RanchOpened();
        }

        private void RanchClosed()
        {
            UIRanch.FarmClosed();
            RanchDialogsManager.RanchClosed();
        }
    }
}