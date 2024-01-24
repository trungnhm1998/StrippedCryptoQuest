using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.UI;
using UnityEngine;

namespace CryptoQuest.System.TransitionSystem
{
    public class TransitionInputController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private SpiralConfigSO _spiral;
        [SerializeField] private FadeConfigSO _fade;

        private void OnEnable()
        {
            _spiral.SpiralIn += DisableInput;
            _fade.FadeIn += DisableInput;
            _spiral.FadeOut += EnableInput;
            _fade.FadeOut += EnableInput;
        }

        private void OnDisable()
        {
            _spiral.SpiralIn -= DisableInput;
            _fade.FadeIn -= DisableInput;
            _spiral.FadeOut -= EnableInput;
            _fade.FadeOut -= EnableInput;
        }

        private void DisableInput()
        {
            _input.InputActions.Disable();
        }

        private void EnableInput() => StartCoroutine(CoWaitForFadeOut());

        private IEnumerator CoWaitForFadeOut()
        {
            float time = _fade.Duration + _fade.WaitDuration;
            while (time >= 0)
            {
                time -= Time.deltaTime;
                _input.InputActions.Disable();
                yield return null;
            }

            _input.InputActions.Enable();
        }
    }
}