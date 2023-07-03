using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.FadeController
{
    public class CryptoQuestFadeController : AbstractFadeController
    {
        [SerializeField] private Image _fadeImg;

        protected override void FadeInLogic()
        {
            _fadeImg.CrossFadeAlpha(1, FadeConfig.Duration, false);
        }

        protected override void FadeOutLogic()
        {
            _fadeImg.CrossFadeAlpha(0, FadeConfig.Duration, false);
        }
    }
}