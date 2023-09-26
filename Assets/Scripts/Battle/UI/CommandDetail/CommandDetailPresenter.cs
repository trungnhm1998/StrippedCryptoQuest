using System;
using UnityEngine;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public class CommandDetailPresenter : MonoBehaviour
    {
        public static event Action<int> InspectButton;

        [SerializeField] private UICommandDetailPanel _commandDetailPanel;

        private void OnEnable()
        {
            UICommandDetailButton.InspectingButton += InspectingButton;
        }

        private void OnDisable()
        {
            UICommandDetailButton.InspectingButton -= InspectingButton;
        }

        public void ShowCommandDetail(ICommandDetailModel infos)
        {
            _commandDetailPanel.ShowCommandDetail(infos.Infos);
        }

        private void InspectingButton(int index)
        {
            InspectButton?.Invoke(index);
        }
    }
}