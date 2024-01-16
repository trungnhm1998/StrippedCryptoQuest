using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Utilities
{
    public class LoadingTransitionController : MonoBehaviour
    {
        [SerializeField] private Image _character;
        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private float _duration = 1f;

        [Header("Scene loading events")] [SerializeField]
        private LoadSceneEventChannelSO _loadMap;

        [SerializeField] private LoadSceneEventChannelSO _loadTitle;
#if UNITY_EDITOR
        [SerializeField] private LoadSceneEventChannelSO _editorColdBoot;
#endif
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        private Sequence _inLogicSequence;
        private Sequence _outLogicSequence;

        private void OnEnable()
        {
            _loadMap.LoadingRequested += FadeLoadingIn;
            _loadTitle.LoadingRequested += FadeLoadingIn;

#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested += FadeLoadingIn;
#endif

            _sceneLoaded.EventRaised += FadeLoadingOut;
        }

        private void OnDisable()
        {
            _loadMap.LoadingRequested -= FadeLoadingIn;
            _loadTitle.LoadingRequested -= FadeLoadingIn;

#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested -= FadeLoadingIn;
#endif

            _sceneLoaded.EventRaised -= FadeLoadingOut;
        }

        private void FadeLoadingIn(Object _)
        {
            _inLogicSequence?.Kill();
            _inLogicSequence = DOTween.Sequence();

            _inLogicSequence
                .Append(_character.DOFade(1, _duration))
                .Join(_loadingText.DOFade(1, _duration));
        }

        private void FadeLoadingOut()
        {
            _outLogicSequence?.Kill();
            _outLogicSequence = DOTween.Sequence();

            _outLogicSequence
                .AppendInterval(_duration)
                .Join(_character.DOFade(0, _duration))
                .Join(_loadingText.DOFade(0, _duration));
        }
    }
}