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
        [SerializeField] private BattleContext _battleContext;

        public void ExecuteCharacterCommands(IEnumerable<Components.Character> characters, Action onComplete)
        {
            StartCoroutine(CoPresentation(characters, onComplete));
        }

        private IEnumerator CoPresentation(IEnumerable<Components.Character> characters, Action onComplete)
        {
            ChangeAllEnemiesOpacity(0.5f);

            foreach (var character in characters)
            {
                character.UpdateTarget(_battleContext);
                yield return character.ExecuteCommand();
            }

            ChangeAllEnemiesOpacity(1f);
            onComplete?.Invoke();
        }

        private void ChangeAllEnemiesOpacity(float f)
        {
            foreach (var enemy in _battleContext.Enemies)
            {
                if (enemy.IsValid()) enemy.SetAlpha(f);
            }
        }
    }
}