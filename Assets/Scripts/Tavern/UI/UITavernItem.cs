using System;
using CryptoQuest.Character.Hero;
using CryptoQuest.Character.LevelSystem;
using CryptoQuest.UI.Tooltips.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UITavernItem : MonoBehaviour
    {
        public static event UnityAction<UITavernItem> Pressed;

        [SerializeField] private Image _classIcon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _inPartyTag;

        [Header("Events")]
        [SerializeField] private ShowTooltipEvent _tooltipEnabledEventChannel;

        public Transform Parent { get; set; }
        public int Id { get; private set; }

        private bool _isSelected = false;
        private bool _isInParty = false;

        public HeroSpec Hero { get; private set; }

        public void SetItemInfo(HeroSpec heroInfo)
        {
            ResetItemStatus();

            Id = heroInfo.Id;
            var levelCalculator = new LevelCalculator(heroInfo.Stats.MaxLevel);
            _level.text = $"Lv{levelCalculator.CalculateCurrentLevel(heroInfo.Experience)}";
            _localizedName.StringReference = heroInfo.Origin.DetailInformation.LocalizedName;
            _localizedName.RefreshString();
            Hero = heroInfo;
        }

        public void LockInPartyHeroes(bool isInParty)
        {
            _isInParty = isInParty;
            _inPartyTag.SetActive(_isInParty);
        }

        public void OnSelectToTransfer()
        {
            if (_isInParty) return;
            Pressed?.Invoke(this);

            _isSelected = !_isSelected;
            EnablePendingTag(_isSelected);
        }

        public void EnablePendingTag(bool enable) => _pendingTag.SetActive(enable);

        private void ResetItemStatus()
        {
            _isSelected = false;
            _isInParty = false;
            LockInPartyHeroes(false);
        }

        public void OnInspectDetails() => _tooltipEnabledEventChannel.RaiseEvent(true);
        public void OnDeselect() => _tooltipEnabledEventChannel.RaiseEvent(false);
        private void OnDisable() => _tooltipEnabledEventChannel.RaiseEvent(false);
    }
}