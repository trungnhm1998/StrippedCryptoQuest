using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UICommandPanel : AbstractBattlePanelContent
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _itemPrefab;

        private List<GameObject> _childButton = new List<GameObject>();


        public override void Init(List<ButtonInfo> infomations)
        {
            Clear();
            _content.SetActive(true);
            foreach (var info in infomations)
            {
                var item = Instantiate(_itemPrefab, _content.transform);
                _childButton.Add(item);
                item.GetComponent<UICommandContent>().Init(info);
            }

            if (_childButton == null || _childButton.Count == 0) return;

            var firstButton = _childButton[0];
            firstButton.GetComponent<Button>().Select();
        }

        public override void Clear()
        {
            foreach (var button in _childButton)
            {
                Destroy(button);
            }

            _childButton.Clear();
            return;
        }
    }
}