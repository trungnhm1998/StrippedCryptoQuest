using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.UI
{
    public class FadeConfigSO : ScriptableObject
    {
        [SerializeField] private float _duration = .3f;
        public float Duration => _duration;

        public event UnityAction FadeIn;
        public event UnityAction FadeOut;
        public UnityAction FadeInComplete;
        public UnityAction FadeOutComplete;

        public void OnFadeIn()
        {
            FadeIn?.Invoke();
        }

        public void OnFadeOut()
        {
            FadeOut?.Invoke();
        }
    }
}