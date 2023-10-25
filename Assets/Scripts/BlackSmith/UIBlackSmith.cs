using System.Collections;
using CryptoQuest.BlackSmith.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith
{
    public class UIBlackSmith : MonoBehaviour
    {
        [SerializeField] private BlackSmithStateController _stateController;
        [SerializeField] private Button _defaultSelection;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _evolvePanel;
        [SerializeField] private GameObject _blackSmithOverview;
        [SerializeField] private GameObject _selectActionPanel;

        public void BlackSmithOpened()
        {
            StartCoroutine(CoSelectDefault());
            _blackSmithOverview.SetActive(true);
        }

        public void BlackSmithClosed()
        {
            StopCoroutine(CoSelectDefault());
            _blackSmithOverview.SetActive(false);
        }

        private IEnumerator CoSelectDefault()
        {
            yield return null;
            _defaultSelection.Select();
        }

        public void OnUpgradeButtonPressed()
        {
            _upgradePanel.SetActive(true);
            _stateController.OpenUpgradeEvent?.Invoke();
        }

        public void OnEvolveButtonPressed()
        {
            _selectActionPanel.SetActive(false);
            _stateController.OpenEvolveEvent?.Invoke();
        }

        public void Init()
        {
            _evolvePanel.SetActive(false);
            _upgradePanel.SetActive(false);
            _selectActionPanel.SetActive(true);
        }
    }
}