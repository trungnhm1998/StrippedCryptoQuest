using System.Collections.Generic;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UICommandPanel : AbstractBattlePanelContent
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private UICommandContent _itemPrefab;
        [SerializeField] private NavigationAutoScroll _navigationAutoScroll;

        private IObjectPool<UICommandContent> _uiCommandContentPool;

        private void Awake()
        {
            _uiCommandContentPool = new ObjectPool<UICommandContent>(OnCreate, OnGet, OnRelease, OnDestroyPool);
        }

        public override void Init(List<AbstractButtonInfo> informations)
        {
            _content.SetActive(true);
            foreach (var info in informations)
            {
                var item = _uiCommandContentPool.Get();
                item.Init(info);
                _navigationAutoScroll.LastButton = item.GetComponent<RectTransform>();
            }


            var firstButton = _content.GetComponentInChildren<Button>();
            if (!firstButton) return;
            _navigationAutoScroll.FirstButton = firstButton.GetComponent<RectTransform>();
            firstButton.Select();
        }

        public override void Clear()
        {
            _content.SetActive(false);
        }

        private UICommandContent OnCreate()
        {
            var button = Instantiate(_itemPrefab, _content.transform);
            button.ObjectPool = _uiCommandContentPool;
            return button;
        }

        private void OnGet(UICommandContent obj)
        {
            obj.transform.SetAsLastSibling();
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(UICommandContent obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPool(UICommandContent obj)
        {
            Destroy(obj.gameObject);
        }
    }
}