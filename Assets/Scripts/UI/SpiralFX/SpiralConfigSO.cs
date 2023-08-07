using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.SpiralFX
{
    [CreateAssetMenu(menuName = "IndiGames Core/UI/Spiral Config")]
    public class SpiralConfigSO : ScriptableObject
    {
        public Color Color;
        public float Duration = 1f;
        public event UnityAction SpiralIn;
        public event UnityAction SpiralOut;
        public event UnityAction FadeOut; 
        public event UnityAction DoneFadeOut;
        public event UnityAction DoneSpiralIn;
        public event UnityAction DoneSpiralOut;

        public void OnFinishSpiralIn()
        {
            DoneSpiralIn?.Invoke();
        }

        public void OnFinishSpiralOut()
        {
            DoneSpiralOut?.Invoke();
        }
        public void OnFinishFadeOut()
        {
            DoneFadeOut?.Invoke();
        }

        public void OnSpiralIn()
        {
            SpiralIn?.Invoke();
        }

        public void OnSpiralOut()
        {
            SpiralOut?.Invoke();
        }

        public void OnFadeOut()
        {
            FadeOut?.Invoke(); 
        }
    }
}