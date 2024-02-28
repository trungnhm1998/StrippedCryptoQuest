﻿using System;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.CommandDetail;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.UI
{
    public class EnemyGroupPresenter : MonoBehaviour
    {
        private const float SELECTED_ALPHA = 1f;
        private const float DESELECTED_ALPHA = 0.5f;

        [SerializeField] private EnemyPartyManager _enemyPartyManager;
        [SerializeField] private UICommandDetailPanel _enemyGroupUI;

        private TinyMessageSubscriptionToken _selectedEventToken;
        private TinyMessageSubscriptionToken _deSelectedEventToken;
        private Action<EnemyGroup> _confirmedEnemyGroupCallback;

        private bool _interactable;

        public void Show(bool interactable = false)
        {
            _interactable = interactable;
            _selectedEventToken = BattleEventBus.SubscribeEvent<SelectedDetailButtonEvent>(OnSelectedGroup);
            _deSelectedEventToken = BattleEventBus.SubscribeEvent<DeSelectedDetailButtonEvent>(OnDeSelectedGroup);


            ShowEnemyGroupUI(interactable);
            
            SetAllGroupAlpha(interactable ? DESELECTED_ALPHA : SELECTED_ALPHA);
        }

        private void ShowEnemyGroupUI(bool interactable = false)
        {
            _enemyGroupUI.Interactable = interactable;
            var groups = _enemyPartyManager.EnemyParty.EnemyGroups;
            var model = new CommandDetailModel();
            foreach (var group in groups)
            {
                if (group.Count <= 0) continue;
                model.AddInfo(new EnemyGroupButtonInfo(group, OnConfirmGroup, interactable));
            }
            _enemyGroupUI.ShowCommandDetail(model);
            _enemyGroupUI.SetActiveContent(true);
        }

        public void Hide()
        {
            BattleEventBus.UnsubscribeEvent(_selectedEventToken);
            BattleEventBus.UnsubscribeEvent(_deSelectedEventToken);

            SetAllGroupAlpha(SELECTED_ALPHA);

            HideEnemyGroupUI();
        }

        private void HideEnemyGroupUI()
        {
        #if UNITY_EDITOR
            // Prevent error when turn off play mode in editor only
            if (_enemyGroupUI == null) return;
        #endif
            _enemyGroupUI.Interactable = false;
            _enemyGroupUI.SetActiveContent(false);
        }

        public void RegisterSelectEnemyGroupCallback(Action<EnemyGroup> callback)
        {
            _confirmedEnemyGroupCallback = callback;
        }

        private void OnSelectedGroup(SelectedDetailButtonEvent eventObject)
        {
            var groups = _enemyPartyManager.EnemyParty.EnemyGroups;
            var selectedGroup = groups[eventObject.Index];
            SetEnemyGroupAlpha(selectedGroup, SELECTED_ALPHA);
        }

        private void OnDeSelectedGroup(DeSelectedDetailButtonEvent eventObject)
        {
            if (!_interactable) return;
            var groups = _enemyPartyManager.EnemyParty.EnemyGroups;
            var selectedGroup = groups[eventObject.Index];
            SetEnemyGroupAlpha(selectedGroup, DESELECTED_ALPHA);
        }

        private void SetEnemyGroupAlpha(EnemyGroup group, float alpha)
        {
            foreach (var enemy in group.Enemies)
            {
                if (!enemy.IsValidAndAlive()) continue;
                enemy.SetAlpha(alpha);
            }
        }

        private void SetAllGroupAlpha(float alpha)
        {
            var groups = _enemyPartyManager.EnemyParty.EnemyGroups;
            foreach (var group in groups)
            {
                SetEnemyGroupAlpha(group, alpha);
            }
        }

        private void OnConfirmGroup(EnemyGroup group)
        {
            _confirmedEnemyGroupCallback?.Invoke(group);
        }
    }
    
    [Serializable]
    public class EnemyGroupButtonInfo : ButtonInfoBase
    {
        private EnemyGroup _enemyGroup;
        private Action<EnemyGroup> _enemyGroupCallback;

        public EnemyGroupButtonInfo(EnemyGroup enemyGroup, Action<EnemyGroup> enemyGroupCallback,
            bool interactable) : base("", isInteractable: interactable)
        {
            _enemyGroup = enemyGroup;
            LocalizedLabel = _enemyGroup.Def.Name;
            if (_enemyGroup.Count > 1)
            {
                Value = $"x{_enemyGroup.Count.ToString()}";
            }
            _enemyGroupCallback = enemyGroupCallback;
        }

        public override void OnHandleClick()
        {
            _enemyGroupCallback?.Invoke(_enemyGroup);
        }
    }
}