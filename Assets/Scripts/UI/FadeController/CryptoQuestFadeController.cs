using System.Collections;
using DG.Tweening;
using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.FadeController
{
    public class CryptoQuestFadeController : AbstractFadeController
    {
        [SerializeField] private Image _fadeImg;

        [SerializeField] private Color _fadeInColor;
        [SerializeField] private Color _fadeOutColor;

        protected override void FadeInLogic()
        {
            _fadeImg.enabled = true;
            _fadeImg.DOBlendableColor(_fadeInColor, FadeConfig.Duration);
        }

        protected override void FadeOutLogic()
        {
            _fadeImg.DOBlendableColor(_fadeOutColor, FadeConfig.Duration);
            StartCoroutine(CoFadeOut());
        }

        private IEnumerator CoFadeOut()
        {
            yield return new WaitForSeconds(FadeConfig.Duration);
            _fadeImg.enabled = false;
        }
    }
}