using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// I need a mono for coroutine, or using a tween library but it still using mono under the hood
    /// </summary>
    public class Presentation : MonoBehaviour
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
                yield return new WaitForSeconds(1f);
            }
            
            onComplete?.Invoke();
        }
    }
}