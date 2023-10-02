using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// I need a mono for coroutine, or using a tween library but it still using mono under the hood
    /// </summary>
    public class PresentBehaviour : MonoBehaviour
    {
        public void ExecuteCharacterCommands(IEnumerable<Components.Character> characters, Action onComplete)
        {
            StartCoroutine(CoPresentation(characters, onComplete));
        }

        private IEnumerator CoPresentation(IEnumerable<Components.Character> characters, Action onComplete)
        {
            foreach (var character in characters)
            {
                character.ExecuteCommand();
                // TODO: different command will have different log, e.g. buff entire party would show line by line and cost 4s
                yield return new WaitForSeconds(1f);
            }

            onComplete?.Invoke();
        }
    }
}