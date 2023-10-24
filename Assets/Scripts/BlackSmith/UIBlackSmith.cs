using System.Collections;
using CryptoQuest.BlackSmith.StateMachine;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    public class UIBlackSmith : MonoBehaviour
    {
        [SerializeField] private BlackSmithStateController _stateController;
        [SerializeField] private MultiInputButton _defaultSelection;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _evolvePanel;
        [SerializeField] private GameObject _blackSmithOverview;
        [SerializeField] private GameObject _selectActionPanel;

        public void BlackSmithOpened()
        {
            Init();
            _blackSmithOverview.SetActive(true);
            StartCoroutine(CoSelectDefault());
        }

        public void BlackSmithClosed()
        {
            _blackSmithOverview.SetActive(false);
            StopCoroutine(CoSelectDefault());
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

        private void Init()
        {
            _evolvePanel.SetActive(false);
            _upgradePanel.SetActive(false);
            _selectActionPanel.SetActive(true);
        }
    }
}