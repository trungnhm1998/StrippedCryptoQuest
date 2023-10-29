using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using CryptoQuest.UI.Common;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UISocialPanel : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponentInChildren<SelectButtonOnEnable>().Select();
        }

        public void RequestFacebookLogin()
        {
            ActionDispatcher.Dispatch(new LoginUsingFacebook());
        }

        public void RequestWalletLogin()
        {
            ActionDispatcher.Dispatch(new LoginUsingWallet());
        }

        public void RequestTwitterLogin()
        {
            ActionDispatcher.Dispatch(new LoginUsingTwitter());
        }

        public void RequestGmailLogin()
        {
            ActionDispatcher.Dispatch(new LoginUsingGoogle());
        }

        public void RequestEmailAndPasswordLogin()
        {
            ActionDispatcher.Dispatch(new LoginUsingEmail());
        }
    }
}