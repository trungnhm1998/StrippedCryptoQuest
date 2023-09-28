using System.Collections;
using CryptoQuest.Menu;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UISocialPanel : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _requestWalletLoginEventChannel;
        [SerializeField] private VoidEventChannelSO _requestTwitterLoginEventChannel;
        [SerializeField] private VoidEventChannelSO _requestFacebookLoginEventChannel;
        [SerializeField] private VoidEventChannelSO _requestGmailLoginEventChannel;
        [field: SerializeField] public Button WalletLoginBtn { get; private set; }
        [field: SerializeField] public Button TwitterLoginBtn { get; private set; }
        [field: SerializeField] public Button FacebookLoginBtn { get; private set; }
        [field: SerializeField] public Button GmailLoginBtn { get; private set; }
        [field: SerializeField] public Button MailLoginBtn { get; private set; }
        [field: SerializeField] public GameObject LoginFailedPanel { get; private set; }

        public void RequestFacebookLogin()
        {
            _requestFacebookLoginEventChannel.RaiseEvent();
        }

        public void RequestWalletLogin()
        {
            _requestWalletLoginEventChannel.RaiseEvent();
        }

        public void RequestTwitterLogin()
        {
            _requestTwitterLoginEventChannel.RaiseEvent();
        }

        public void RequestGmailLogin()
        {
            _requestGmailLoginEventChannel.RaiseEvent();
        }
    }
}