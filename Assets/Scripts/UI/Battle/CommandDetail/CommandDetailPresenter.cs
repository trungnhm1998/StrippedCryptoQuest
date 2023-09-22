using UnityEngine;
using System;

namespace CryptoQuest.UI.Battle.CommandDetail
{
    public class CommandDetailPresenter : MonoBehaviour
    {
        public static event Action<int> InspectButton;
        public static Action<ICommandDetailModel> RequestShowCommandDetail;

        [SerializeField] private UICommandDetailPanel _commandDetailPanel;

        private void OnEnable()
        {
            RequestShowCommandDetail += ShowCommandDetail; 
            UICommandDetailButton.InspectingButton += InspectingButton;
        }

        private void OnDisable()
        {
            RequestShowCommandDetail -= ShowCommandDetail; 
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