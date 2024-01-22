using CryptoQuest.Sagas;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Popups
{
    public class LoadTitleSceneOnCloseErrorPopup : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _closedAllPopupsEventChannel;

        private void OnEnable()
        {
            _closedAllPopupsEventChannel.EventRaised += LoadTitleScene;
        }

        private void OnDisable()
        {
            _closedAllPopupsEventChannel.EventRaised -= LoadTitleScene;
        }

        private void LoadTitleScene()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning("Will load TitleScene in staging production build here");
#else
            ActionDispatcher.Dispatch(new GoToTitleAction());
#endif
        }
    }
}