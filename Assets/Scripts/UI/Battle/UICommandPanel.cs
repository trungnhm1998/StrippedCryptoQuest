using System.Collections.Generic;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UICommandPanel : AbstractBattlePanelContent
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _itemPrefab;

        private List<GameObject> _buttonPool = new List<GameObject>();
        private Queue<UICommandContent> _commandQueue = new Queue<UICommandContent>();

        private void Awake()
        {
            for (int i = 0; i < 7; i++)
            {
                var button = InstantiateButton();
                ReturnButtonToPool(button);
            }
        }

        private GameObject InstantiateButton()
        {
            var button = Instantiate(_itemPrefab, _content.transform);
            button.SetActive(false);
            return button;
        }

        private void ReturnButtonToPool(GameObject button)
        {
            button.SetActive(false);
            _buttonPool.Add(button);
            var content = button.GetComponent<UICommandContent>();

            if (content == null) return;
            _commandQueue.Enqueue(content);
        }

        private UICommandContent GetButtonFromPool()
        {
            if (_commandQueue.Count == 0)
            {
                var newItem = InstantiateButton();
                ReturnButtonToPool(newItem);
            }

            var uiCommandContent = _commandQueue.Dequeue();
            uiCommandContent.gameObject.SetActive(true);
            return uiCommandContent;
        }

        public override void Init(List<ButtonInfo> informations)
        {
            _content.SetActive(true);
            foreach (var info in informations)
            {
                var item = GetButtonFromPool();
                item.Init(info);
            }

            if (_buttonPool == null || _buttonPool.Count == 0) return;

            var firstButton = _buttonPool[0];
            firstButton.GetComponent<Button>().Select();
        }

        public override void Clear()
        {
            foreach (var button in _buttonPool)
            {
                Destroy(button);
            }

            _buttonPool.Clear();
            _commandQueue.Clear();
            _content.SetActive(false);
        }
    }
}