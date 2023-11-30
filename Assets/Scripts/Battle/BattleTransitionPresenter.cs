using CryptoQuest.System.TransitionSystem;
using CryptoQuest.UI.SpiralFX;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class BattleTransitionPresenter : MonoBehaviour
    {
        [SerializeField] private SpiralConfigSO _spiral;
        [SerializeField] private TransitionEventChannelSO _transitionEventChannelSo;
        [SerializeField] private AbstractTransition _transitionOut;
    
        public float TransitDuration => _spiral.Duration;

        public void TransitOut()
        {
            _transitionEventChannelSo.RaiseEvent(_transitionOut);
        }
    }
}