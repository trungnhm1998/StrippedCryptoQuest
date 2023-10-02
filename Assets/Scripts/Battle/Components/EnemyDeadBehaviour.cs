using System.Linq;
using CryptoQuest.Character.Tag;
using DG.Tweening;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EnemyDeadBehaviour : CharacterComponentBase
    {
        [SerializeField] private float _fadeoutDuration = 1.5f;
        private EnemyBehaviour _enemyBehaviour;

        public override void Init()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
            _enemyBehaviour.AbilitySystem.TagSystem.TagAdded += CheckIfDeadTagAdded;
        }

        private void OnDestroy()
        {
            if (IsValid()) _enemyBehaviour.AbilitySystem.TagSystem.TagAdded -= CheckIfDeadTagAdded;
        }

        private void CheckIfDeadTagAdded(TagScriptableObject[] tagsAdded)
        {
            if (tagsAdded.Contains(TagsDef.Dead))
            {
                // tween fade out
                _enemyBehaviour.SetAlpha(1f);
                DOTween.To(() => _enemyBehaviour.Color.a,
                    x => _enemyBehaviour.SetAlpha(x),
                    0f, _fadeoutDuration).SetEase(Ease.InCubic);
            }
        }
    }
}