using UnityEngine;

namespace IndiGames.Core.UI.FadeController
{
    public abstract class AbstractFadeController : MonoBehaviour
    {
        [SerializeField] protected FadeConfigSO FadeConfig;

        private void OnEnable()
        {
            FadeConfig.FadeIn += FadeIn_Raised;
            FadeConfig.FadeOut += FadeOut_Raised;
        }

        private void OnDisable()
        {
            FadeConfig.FadeIn -= FadeIn_Raised;
            FadeConfig.FadeOut -= FadeOut_Raised;
        }

        private void FadeIn_Raised()
        {
            FadeInLogic();
        }

        private void FadeOut_Raised()
        {
            FadeOutLogic();
        }

        protected abstract void FadeInLogic();
        protected abstract void FadeOutLogic();
    }
}