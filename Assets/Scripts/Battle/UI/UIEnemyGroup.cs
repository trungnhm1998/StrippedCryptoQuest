using CryptoQuest.Menu;
using UnityEngine;

namespace CryptoQuest.Battle.UI
{
    public class UIEnemyGroup : MonoBehaviour
    {
        [SerializeField] private MultiInputButton _button;
        public int Count => _group.Count;
        public string Name => _group.Def.Name.GetLocalizedString();
        private EnemyGroup _group;

        public void Select() => _button.Select();

        public void Init(EnemyGroup group)
        {
            _group = group;
        }
    }
}