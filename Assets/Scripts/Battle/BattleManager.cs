using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Input;
using CryptoQuest.System;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    [Serializable]
    public struct BattleContext
    {
        public LootInfo[] Loots;
    }

    public class BattleManager : MonoBehaviour
    {
        public delegate void BattleEvent(BattleContext context);

        public static event BattleEvent BattleCompleted;
        public event Action Initialized;
        [SerializeField] private SpiralConfigSO _spiral;
        [SerializeField] private BattleInputSO _input;
        [SerializeField] private EnemyPartyBehaviour _enemyParty;
        [SerializeField, Header("Listen")] private VoidEventChannelSO _sceneLoadedEvent;

        private IBattleInitializer _initializer;
        private Coroutine _initCoroutine;
        public List<EnemyBehaviour> Enemies => _enemyParty.Enemies;

        private void Awake()
        {
            _initializer = GetComponent<IBattleInitializer>();
        }

        private void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += InitBattle;
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= InitBattle;
        }

        private void InitBattle()
        {
            Debug.Log("BattleManager::InitBattle()");
            _initCoroutine = StartCoroutine(CoInitBattle());
        }

        private IEnumerator CoInitBattle()
        {
            yield return _initializer.LoadEnemies();
            _spiral.HideSpiral();
            yield return new WaitForSeconds(_spiral.Duration);
            FinishInitBattle();
        }

        /// <summary>
        /// DEBUG FUNCTION
        /// TODO: REMOVE THIS
        /// </summary>
        public void WinBattle()
        {
            _input.DisableAllInput();
            if (_initCoroutine != null) StopCoroutine(_initCoroutine);
            var context = new BattleContext();
            List<LootInfo> loots = new();
            var enemies = _initializer.Enemies;
            // TODO: This also return cloned loot, but we already clone it in RewardManager?
            foreach (var enemy in enemies)
                loots.AddRange(enemy.GetLoots());
            if (loots.Count > 0) context.Loots = RewardManager.CloneAndMergeLoots(loots.ToArray());

            BattleCompleted?.Invoke(context);
        }

        private void FinishInitBattle()
        {
            _input.EnableBattleInput();
            Debug.Log("BattleManager::Battle Initialized");

            Initialized?.Invoke();
        }
    }
}