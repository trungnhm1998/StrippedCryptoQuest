using CryptoQuest.Inn.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.Inn
{
    public class InnPresenter : MonoBehaviour
    {
        [SerializeField] private InnDialogsManager _innDialogsManager;
        [SerializeField] private InnTransitionManager _innTransitionManager;
        [SerializeField] private InnController _innController;

        [field: Header("Config"), SerializeField]
        public float InnPrice { get; private set; }

        [Header("Listening on Channels")]
        [SerializeField] private ShowInnEventChannelSO _showInnEventChannel;

        private void OnEnable()
        {
            _showInnEventChannel.EventRaised += ShowInnRequested;
            _innDialogsManager.RequestFadeEvent += OnFadeRequested;
            _innDialogsManager.RequestHealEvent += OnRestoreRequested;
            _innDialogsManager.RequestHide += OnHideRequested;
        }

        private void OnDisable()
        {
            _showInnEventChannel.EventRaised -= ShowInnRequested;
            _innDialogsManager.RequestFadeEvent -= OnFadeRequested;
            _innDialogsManager.RequestHealEvent -= OnRestoreRequested;
            _innDialogsManager.RequestHide -= OnHideRequested;
        }

        private void ShowInnRequested()
        {
            _innDialogsManager.gameObject.SetActive(true);
            _innDialogsManager.ShowDialog();
            
            _innController.ShowWallet();
        }

        private void OnFadeRequested(bool isFadeIn)
        {
            if (isFadeIn)
            {
                _innTransitionManager.FadeIn();
                return;
            }

            _innTransitionManager.FadeOut();
        }

        private void OnHideRequested()
        {
            _innDialogsManager.gameObject.SetActive(false);
            
            _innController.HideWallet();
        }

        private void OnRestoreRequested()
        {
            _innController.RestoreParty();
        }
    }
}