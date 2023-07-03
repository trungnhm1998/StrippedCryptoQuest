using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.UI.FadeController
{
    [CreateAssetMenu(menuName = "IndiGames Core/UI/Fade Config")]
    public class FadeConfigSO : ScriptableObject
    {
        [SerializeField] private float _duration = .3f;
        public float Duration => _duration;

        public event UnityAction FadeIn;
        public event UnityAction FadeOut;

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