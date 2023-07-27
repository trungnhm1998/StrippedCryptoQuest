using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.SpiralFX
{
    [CreateAssetMenu(menuName = "IndiGames Core/UI/Spiral Config")]
    public class SpiralConfigSO : ScriptableObject
    {
        [SerializeField] private float _duration = 1f;

        public float Duration => _duration;
        public Color Color;
        public event UnityAction SpiralIn;
        public event UnityAction SpiralOut;

        public void OnSpiralIn()
        {
            SpiralIn?.Invoke();
        }

        public void OnSpiralOut()
        {
            SpiralOut?.Invoke();
        }
    }
}