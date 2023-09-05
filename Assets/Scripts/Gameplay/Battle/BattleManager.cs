using System.Collections;
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

        private IBattleInitializer _battleInitializer;

        private void Awake()
        {
            _battleInitializer = GetComponent<IBattleInitializer>();
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
            yield return _battleInitializer.LoadEnemies();
            _spiral.HideSpiral();
        }

        private void FinishInitBattle()
        {
            _input.EnableBattleInput();
            Debug.Log("BattleManager::Battle Initialized");
        }
    }
}