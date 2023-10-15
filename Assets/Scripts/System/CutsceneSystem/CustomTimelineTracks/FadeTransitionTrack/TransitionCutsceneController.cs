using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.FadeTransitionTrack
{
    public class TransitionCutsceneController : MonoBehaviour
    {
        public static Action<float> OnBlackOut;
        [SerializeField] private Image _blackScreen;

        private void OnEnable()
        {
            _blackScreen.gameObject.SetActive(false);
            OnBlackOut += TriggerDurationalBlackout;
        }

        private void OnDisable()
        {
            OnBlackOut -= TriggerDurationalBlackout;
        }

        private void TriggerDurationalBlackout(float duration)
        {
            StartCoroutine(CoBlackOut(duration));
        }

        private IEnumerator CoBlackOut(float duration)
        {
            _blackScreen.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _blackScreen.gameObject.SetActive(false);
        }
    }
}