using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Battle
{
    internal class UIAttackPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UIAttackContent _itemPrefab;
        [SerializeField] private MockBattleAttackBus attackBus;

        public override void Init()
        {
            foreach (var mob in attackBus.Mobs)
            {
                var item = Instantiate(_itemPrefab, content.transform);
                item.Init(new UIAttackContent.Attack()
                {
                    mob = mob
                });
            }
        }
    }
}