using UnityEngine;

namespace CryptoQuest.Inn
{
    public class InnTransitionManager : MonoBehaviour
    {
        [Header("Fade Config")]
        [SerializeField] private TransitionEventChannelSO _requestTransition;

        [SerializeField] private AbstractTransition _fadeInTransition;
        [SerializeField] private AbstractTransition _fadeOutTransition;

        public void FadeIn() => _requestTransition.RaiseEvent(_fadeInTransition);

        public void FadeOut() => _requestTransition.RaiseEvent(_fadeOutTransition);
    }
}