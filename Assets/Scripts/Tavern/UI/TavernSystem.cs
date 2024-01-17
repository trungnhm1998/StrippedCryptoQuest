using CryptoQuest.Gameplay;
using CryptoQuest.Tavern.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class TavernSystem : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private ShowTavernEventChannelSO _showTavernEventChannel;
        [SerializeField] private UIOverview _overviewPanel;
        [SerializeField] private UITransferCharacter _transferCharacterPanel;
        [SerializeField] private UIRecruitPanel _recruitCharacterPanel;

        private void OnEnable()
        {
            _showTavernEventChannel.EventRaised += OpenTavernSystem;
        }

        private void OnDisable()
        {
            _showTavernEventChannel.EventRaised -= OpenTavernSystem;
        }

        private void OpenTavernSystem()
        {
            _gameState.UpdateGameState(EGameState.Merchant);
            OpenOverviewPanel();
        }

        private void OpenOverviewPanel()
        {
            _overviewPanel.gameObject.SetActive(true);
            _overviewPanel.Closed += CloseTavernSystem;
        }

        private void CloseTavernSystem()
        {
            CloseOverviewPanel();
            _gameState.UpdateGameState(EGameState.Field);
        }

        private void CloseOverviewPanel()
        {
            _overviewPanel.Closed -= CloseOverviewPanel;
            _overviewPanel.gameObject.SetActive(false);
        }

        public void OpenTransferCharacterPanel()
        {
            CloseOverviewPanel();
            _transferCharacterPanel.gameObject.SetActive(true);
            _transferCharacterPanel.Closed += CloseTransferCharacterPanel;
        }

        private void CloseTransferCharacterPanel()
        {
            _transferCharacterPanel.Closed -= CloseTransferCharacterPanel;
            _transferCharacterPanel.gameObject.SetActive(false);
            OpenOverviewPanel();
        }

        public void OpenRecruitCharacterPanel()
        {
            CloseOverviewPanel();
            _recruitCharacterPanel.gameObject.SetActive(true);
            _recruitCharacterPanel.Closed += CloseRecruitCharacterPanel;
        }

        private void CloseRecruitCharacterPanel()
        {
            _recruitCharacterPanel.Closed -= CloseRecruitCharacterPanel;
            _recruitCharacterPanel.gameObject.SetActive(false);
            OpenOverviewPanel();
        }
    }
}