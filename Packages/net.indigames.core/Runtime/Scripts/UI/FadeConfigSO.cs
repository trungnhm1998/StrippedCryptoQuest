using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace IndiGames.Core.UI
{
    public class FadeConfigSO : ScriptableObject
    {
        [SerializeField] private float _duration = .3f;
        [SerializeField] private float _waitDuration = .7f;
        [field: SerializeField] public Color FadeInColor;
        [field: SerializeField] public Color FadeOutColor;
        public float Duration => _duration;
        public float WaitDuration => _waitDuration;

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