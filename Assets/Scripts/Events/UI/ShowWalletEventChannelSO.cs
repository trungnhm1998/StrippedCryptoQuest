using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI
{
    public class ShowWalletEventChannelSO : ScriptableObject
    {
        public struct Context
        {
            public bool ShowGolds;
            public bool ShowDiamonds;
            public bool ShowSouls;
        }

        public event UnityAction<Context> ShowEvent;
        public event UnityAction HideEvent;

        private bool _showGolds = true;

        public ShowWalletEventChannelSO EnableAll(bool enabled = true)
        {
            _showDiamonds = _showGolds = _showSouls = enabled;
            return this;
        }

        public ShowWalletEventChannelSO EnableGold(bool enabled = true)
        {
            _showGolds = enabled;
            return this;
        }

        private bool _showDiamonds = true;

        public ShowWalletEventChannelSO EnableDiamond(bool enabled = true)
        {
            _showDiamonds = enabled;
            return this;
        }

        private bool _showSouls = true;

        public ShowWalletEventChannelSO EnableSouls(bool enabled = true)
        {
            _showSouls = enabled;
            return this;
        }

        public void Show() => OnShow();

        private void OnShow()
        {
            ShowEvent.SafeInvoke(new Context()
            {
                ShowGolds = _showGolds,
                ShowDiamonds = _showDiamonds,
                ShowSouls = _showSouls
            }, $"Event was raised on {name} but no one was listening.");

            _showGolds = true;
            _showDiamonds = true;
            _showSouls = true;
        }

        public void Hide() => HideEvent.SafeInvoke($"Event was raised on {name} but no one was listening.");
    }
}