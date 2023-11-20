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
        [SerializeField] private GameObject _farmOverview;
        [SerializeField] private GameObject _selectActionPanel;

        private void SelectDefault() => _defaultSelection.Select();

        public void FarmOpened() => Initialize();

        public void FarmClosed()
        {
            _farmOverview.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnUpgradeButtonPressed()
        {
            _farmOverview.SetActive(false);
            _stateController.OpenUpgradeEvent?.Invoke();
        }

        public void OnEvolveButtonPressed()
        {
            _farmOverview.SetActive(false);
            _stateController.OpenEvolveEvent?.Invoke();
        }

        public void OnSwapButtonPressed()
        {
            _farmOverview.SetActive(false);
            _stateController.OpenSwapEvent?.Invoke();
        }

        public void Initialize()
        {
            _selectActionPanel.SetActive(true);
            _farmOverview.SetActive(true);
            Invoke(nameof(SelectDefault), DEFAULT_SELECTION_DELAY);
        }
    }
}