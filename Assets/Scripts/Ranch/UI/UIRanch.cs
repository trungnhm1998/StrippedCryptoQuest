using CryptoQuest.Ranch.State;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.UI
{
    public class UIRanch : MonoBehaviour
    {
        private const float DEFAULT_SELECTION_DELAY = 0.1f;
        
        [SerializeField] private RanchStateController _stateController;

        [SerializeField] private Button _defaultSelection;
        [SerializeField] private GameObject _swapPanel;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _evolvePanel;
        [SerializeField] private GameObject _farmOverview;
        [SerializeField] private GameObject _selectActionPanel;

        private void SelectDefault() => _defaultSelection.Select();

        public void FarmOpened()
        {
            Invoke(nameof(SelectDefault), DEFAULT_SELECTION_DELAY);
            Init();
        }

        public void FarmClosed()
        {
            _farmOverview.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnUpgradeButtonPressed()
        {
            _upgradePanel.SetActive(true);
            _farmOverview.SetActive(false);
            _stateController.OpenUpgradeEvent?.Invoke();
        }

        public void OnEvolveButtonPressed()
        {
            _evolvePanel.SetActive(true);
            _farmOverview.SetActive(false);
            _stateController.OpenEvolveEvent?.Invoke();
        }

        public void OnSwapButtonPressed()
        {
            _swapPanel.SetActive(true);
            _farmOverview.SetActive(false);
            _stateController.OpenSwapEvent?.Invoke();
        }

        public void Init()
        {
            _swapPanel.SetActive(false);
            _evolvePanel.SetActive(false);
            _upgradePanel.SetActive(false);

            _selectActionPanel.SetActive(true);
            _farmOverview.SetActive(true);
        }
    }
}