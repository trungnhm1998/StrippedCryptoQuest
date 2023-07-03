using DG.Tweening;
using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.FadeController
{
    public class CryptoQuestFadeController : AbstractFadeController
    {
        [SerializeField] private Image _fadeImg;

        [SerializeField] private Color _fadeInColor;
        [SerializeField] private Color _fadeOutColor;

        protected override void FadeInLogic()
        {
            _fadeImg.DOBlendableColor(_fadeInColor, FadeConfig.Duration);
        }

        protected override void FadeOutLogic()
        {
            _fadeImg.DOBlendableColor(_fadeOutColor, FadeConfig.Duration);
        }
    }
}