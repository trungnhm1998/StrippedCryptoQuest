using System.Collections.Generic;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastSwap : UIAbstractFarm
    {
        [field: SerializeField] public UIBeastList InBoxBeastList { get; private set; }
        [field: SerializeField] public UIBeastList InGameBeastList { get; private set; }

        private List<UIBeastItem> _toGame = new();
        public List<UIBeastItem> ToGame => _toGame;


        private List<UIBeastItem> _toWallet = new();
        public List<UIBeastItem> ToWallet => _toWallet;

        public bool IsValid() => _toGame.Count > 0 || _toWallet.Count > 0;

        public void TransferBeast()
        {
            var toWallet = InBoxBeastList.SelectedItems;
            var toGame = InGameBeastList.SelectedItems;

            if (toWallet.Count == 0 && toGame.Count == 0)
            {
                Debug.Log($"<color=red>No item selected</color>");
                return;
            }

            _toWallet = toWallet;
            _toGame = toGame;

            Debug.Log($"game={_toGame.Count} -- wallet={_toWallet.Count}");
        }

        public void OnTransferring()
        {
            InGameBeastList.Interactable = InBoxBeastList.Interactable = false;
        }

        public void SwitchList(Vector2 axis)
        {
            switch (axis.x)
            {
                case 0:
                    return;
                case > 0:
                    InBoxBeastList.TryFocus();
                    break;
                case < 0:
                    InGameBeastList.TryFocus();
                    break;
            }
        }

        public void ResetSelected()
        {
            InBoxBeastList.Reset();
            InGameBeastList.Reset();
            ActionDispatcher.Dispatch(new FetchProfileBeastsAction());
        }
    }
}