using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastList : MonoBehaviour
    {
        private const bool COLLECTION_CHECK = true;
        private const int DEFAULT_POOL_SIZE = 10;
        [SerializeField] private int _poolSize = 10;
        public Transform Child => _scrollRect.content;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIBeastItem _beastItemPrefab;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private List<BeastData> _beastList = new();
        public List<BeastData> Data => _beastList;
        private List<UIBeastItem> _beastUI = new();

        private IObjectPool<UIBeastItem> _beastUIPool;

        private IObjectPool<UIBeastItem> BeastUIPool =>
            _beastUIPool ??= new ObjectPool<UIBeastItem>(OnCreate, OnGet, OnRelease, OnDestroyBeast,
                COLLECTION_CHECK, DEFAULT_POOL_SIZE, _poolSize);

        private void OnDisable()
        {
            CleanUpScrollView();
        }

        public void SetData(List<BeastData> data)
        {
            _beastList = data;
            CleanUpScrollView();
            RenderData();
        }

        public void SetEnableButtons(bool isEnable = true)
        {
            foreach (var item in _beastUI) item.EnableButton(isEnable);
        }

        public void UpdateList()
        {
            foreach (var item in _beastUI) item.EnablePendingTag(false);
        }

        private void CleanUpScrollView()
        {
            foreach (var ui in _beastUI)
            {
                BeastUIPool.Release(ui);
            }

            _beastUI.Clear();
        }

        private void RenderData()
        {
            foreach (var itemData in _beastList)
            {
                var item = BeastUIPool.Get();
                item.SetItemInfo(itemData);
            }
        }

        #region Pool

        private void OnDestroyBeast(UIBeastItem beast) => Destroy(beast.gameObject);

        private void OnRelease(UIBeastItem beast) => beast.gameObject.SetActive(false);

        private void OnGet(UIBeastItem beast)
        {
            _beastUI.Add(beast);
            beast.transform.SetAsLastSibling();
            beast.gameObject.SetActive(true);
        }

        private UIBeastItem OnCreate() => Instantiate(_beastItemPrefab, _scrollRect.content);

        #endregion
    }
}