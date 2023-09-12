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
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private SpiralConfigSO _spiral;
        [SerializeField] private BattleInputSO _input;
        [SerializeField, Header("Listen")] private VoidEventChannelSO _sceneLoadedEvent;

        private IBattleInitializer _initializer;

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
            StartCoroutine(CoInitBattle());
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
            List<LootInfo> loots = new();
            var enemies = _initializer.Enemies;
            foreach (var enemy in enemies)
                loots.AddRange(enemy.GetLoots());
            if (loots.Count > 0) RewardManager.RewardPlayer(loots.ToArray());
            // unload battle
#else
            yield break;
#endif
        }

        private void FinishInitBattle()
        {
            _input.EnableBattleInput();
            Debug.Log("BattleManager::Battle Initialized");
        }
    }
}