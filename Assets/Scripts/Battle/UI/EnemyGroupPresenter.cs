using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Enemy;
using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.Battle.UI
{
    public class EnemyGroupPresenter : MonoBehaviour
    {
        [SerializeField] private EnemyPartyManager _enemyPartyManager;
        [SerializeField] private Transform _groupContainer;
        [SerializeField] private UIEnemyGroup _groupPrefab;
        private readonly List<UIEnemyGroup> _groupsButton = new();

        public void Show(bool selectFirstGroup = false)
        {
            DestroyAllChildren();
            var groups = CreateGroups();

            foreach (var group in groups)
            {
                var uiGroup = Instantiate(_groupPrefab, _groupContainer);
                uiGroup.Init(group);
                _groupsButton.Add(uiGroup);
            }

            _groupContainer.gameObject.SetActive(true);

            if (selectFirstGroup)
            {
                DOVirtual.DelayedCall(0.5f, () => _groupsButton[0].Select());
            }
        }

        public void Hide()
        {
            _groupContainer.gameObject.SetActive(false);
        }

        private void DestroyAllChildren()
        {
            foreach (var group in _groupsButton)
            {
                Destroy(group);
            }

            _groupsButton.Clear();
        }

        private List<EnemyGroup> CreateGroups()
        {
            var enemies = _enemyPartyManager.Enemies;
            var groups = new Dictionary<EnemyDef, EnemyGroup>();

            foreach (var enemy in enemies)
            {
                if (enemy.IsValid() == false) continue;
                if (!groups.TryGetValue(enemy.Def, out var group))
                {
                    group = new EnemyGroup()
                    {
                        Def = enemy.Def,
                        Enemies = new List<EnemyBehaviour>()
                    };

                    groups.Add(enemy.Def, group);
                }

                group.Enemies.Add(enemy);
            }

            return groups.Values.ToList();
        }
    }
}