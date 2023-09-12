using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Input;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
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
        [SerializeField] private SpiralConfigSO _spiral;
        [SerializeField] private BattleInputSO _input;
        [SerializeField, Header("Listen")] private VoidEventChannelSO _sceneLoadedEvent;

        private IBattleInitializer _initializer;
        private Coroutine _initCoroutine;

        private void Awake()
        {
            _initializer = GetComponent<IBattleInitializer>();
        }

        private void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += InitBattle;
            _spiral.DoneSpiralOut += FinishInitBattle;
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= InitBattle;
            _spiral.DoneSpiralOut -= FinishInitBattle;
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
            yield return AutoSkip();
        }


        // TODO: REMOVE THIS
        private IEnumerator AutoSkip()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            yield return new WaitForSeconds(2f);
            WinBattle();
#else
            yield break;
#endif
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
        }
    }
}