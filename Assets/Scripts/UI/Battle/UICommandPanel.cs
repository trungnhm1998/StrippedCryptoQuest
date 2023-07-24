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
        [SerializeField] private GameObject _itemPrefab;

        private List<GameObject> _buttonPool = new List<GameObject>();
        private IObjectPool<UICommandContent> _uiCommandContentPool;

        private void Awake()
        {
            _uiCommandContentPool = new ObjectPool<UICommandContent>(OnCreate, OnGet, OnRelease, OnDestroyPool);
        }

        private void OnDestroyPool(UICommandContent obj)
        {
            Destroy(obj.gameObject);
        }

        private void OnRelease(UICommandContent obj)
        {
            obj.gameObject.SetActive(false);
            _buttonPool.Remove(obj.gameObject);
        }

        private void OnGet(UICommandContent obj)
        {
            obj.transform.SetAsLastSibling();
            obj.gameObject.SetActive(true);
        }

        private UICommandContent OnCreate()
        {
            var go = Instantiate(_itemPrefab, _content.transform);
            _buttonPool.Add(go);
            var button = go.GetComponent<UICommandContent>();
            button.ObjectPool = _uiCommandContentPool;
            return button;
        }

        public override void Init(List<ButtonInfo> informations)
        {
            _content.SetActive(true);
            foreach (var info in informations)
            {
                var item = _uiCommandContentPool.Get();
                item.Init(info);
            }

            if (_buttonPool == null || _buttonPool.Count == 0) return;

            var firstButton = _buttonPool[0];
            firstButton.GetComponent<Button>().Select();
        }

        public override void Clear()
        {
            foreach (Transform transform in _content.transform)
            {
                var button = transform.GetComponent<UICommandContent>();
                if (button && button.gameObject.activeInHierarchy)
                    button.ReleaseToPool();
            }

            _content.SetActive(false);
        }
    }
}