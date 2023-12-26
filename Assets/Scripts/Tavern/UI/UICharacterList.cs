using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterList : MonoBehaviour
    {
        [SerializeField] private PartySO _party;

        [SerializeField] protected Transform _scrollRectContent;
        [SerializeField] protected UITavernItem _itemPrefab;

        private IObjectPool<UITavernItem> _pool;
        private List<UITavernItem> _items = new();

        private void OnEnable()
        {
            _pool ??= new ObjectPool<UITavernItem>(OnCreate, OnGet, OnRelease, OnDestroyPool);
        }

        public void SetData(List<HeroSpec> data)
        {
            ReleaseAllItemInPool();
            foreach (var heroData in data)
            {
                UITavernItem item = _pool.Get();
                item.SetItemInfo(heroData);
                IdentifyItemParent(item);
            }
        }

        public void HandleInPartyHeroes()
        {
            foreach (var partySlot in _party.GetParty())
            {
                if (partySlot.IsValid() == false) continue;
                var isMain = partySlot.Hero.Id == 0;
                if (isMain) continue;
                foreach (var heroItem in _items)
                {
                    if (partySlot.Hero.Id != heroItem.Id) continue;
                    heroItem.LockInPartyHeroes(true);
                }
            }
        }

        public void SelectDefault()
        {
            var firstButton = _scrollRectContent.GetComponentInChildren<Button>();
            firstButton.Select();
        }

        private void IdentifyItemParent(UITavernItem item) => item.Parent = _scrollRectContent;

        public void SetInteractableAllButtons(bool isEnabled)
        {
            foreach (Transform item in _scrollRectContent)
                item.GetComponent<Button>().enabled = isEnabled;
        }

        public void UpdateList()
        {
            foreach (var item in _items)
                item.EnablePendingTag(false);
        }

        #region Pool

        private UITavernItem OnCreate()
        {
            var item = Instantiate(_itemPrefab, _scrollRectContent);
            return item;
        }

        private void OnGet(UITavernItem item)
        {
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            _items.Add(item);
        }

        private void OnRelease(UITavernItem item) => item.gameObject.SetActive(false);

        private void OnDestroyPool(UITavernItem item) => Destroy(item.gameObject);

        private void ReleaseAllItemInPool()
        {
            foreach (var item in _items)
                _pool.Release(item);

            _items = new();
        }

        #endregion
    }
}