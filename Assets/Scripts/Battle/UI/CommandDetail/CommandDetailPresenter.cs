using System;
using UnityEngine;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    [Obsolete]
    public class CommandDetailPresenter : MonoBehaviour
    {
        public static event Action<int> InspectButton;

        [SerializeField] private UICommandDetailPanel _commandDetailPanel;

        public void ShowCommandDetail(ICommandDetailModel infos)
        {
            _commandDetailPanel.ShowCommandDetail(infos);
        }

        private void InspectingButton(int index)
        {
            InspectButton?.Invoke(index);
        }
    }
}