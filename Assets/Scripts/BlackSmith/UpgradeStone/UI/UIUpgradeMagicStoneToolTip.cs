using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Extensions;
using CryptoQuest.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIUpgradeMagicStoneToolTip : UITooltipBase
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] protected TMP_Text _lvlText;
        [SerializeField] protected GameObject _passiveInfoHolder;
        [SerializeField] protected UIPassiveInfoDetail _passiveInfoDetail;
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _upgradeColor;

        protected IObjectPool<UIPassiveInfoDetail> _passivePool;
        private bool _isBaseStone;
        protected IMagicStone _stone;
        protected List<UIPassiveInfoDetail> _cachedItems = new();

        protected override bool CanShow()
        {
            if (_stone == null) return false;
            if (!_stone.IsValid()) return false;
            return true;
        }

        protected override void Init()
        {
            _passivePool ??= new ObjectPool<UIPassiveInfoDetail>(OnCreate, OnGet,
                OnRelease, OnDestroyPool);
            SetupInfo();
        }

        public void SetData(IMagicStone stone, bool isBaseStone)
        {
            _stone = stone;
            _isBaseStone = isBaseStone;
        }

        public virtual void SetupInfo()
        {
            CleanUpPool();
            var color = GetColor();
            _icon.LoadSpriteAndSet(_stone.Definition.Image);
            _name.StringReference = _stone.Definition.DisplayName;
            _lvlText.text = $"{_stone.Level}";
            _lvlText.color = color;
            foreach (var passive in _stone.Passives)
            {
                var equipmentUI = _passivePool.Get();
                equipmentUI.Initialize(passive.Description,
                    passive.Context.SkillInfo.SkillParameters.BasePower + "%", color);
            }
        }

        private Color GetColor()
        {
            return _isBaseStone ? Color.white : Color.green;
        }

        protected void CleanUpPool()
        {
            while (_cachedItems.Count > 0)
            {
                _passivePool.Release(_cachedItems[0]);
            }

            _cachedItems.Clear();
        }

        #region Pool-handler

        private UIPassiveInfoDetail OnCreate()
        {
            var passiveInfoDetail = Instantiate(_passiveInfoDetail, _passiveInfoHolder.transform);
            return passiveInfoDetail;
        }

        private void OnGet(UIPassiveInfoDetail item)
        {
            _cachedItems.Add(item);
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
        }

        private void OnRelease(UIPassiveInfoDetail item)
        {
            _cachedItems.Remove(item);
            item.gameObject.SetActive(false);
        }

        private void OnDestroyPool(UIPassiveInfoDetail item)
        {
            Destroy(item.gameObject);
        }

        #endregion
    }
}