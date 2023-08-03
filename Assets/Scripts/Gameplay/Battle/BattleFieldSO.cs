using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Battle Field")]
    public class BattleFieldSO : ScriptableObject
    {
        public List<BattleEncounterSetup> BattleEncounterSetups;
        public float EncounterRate = 5f;
        public bool IsBattleEscapable = true;
        public AssetReferenceT<Sprite> BattleBackground;

        public BattleDataSO GetBattleToInit()
        {
            float randomValue = Random.value;
            BattleEncounterSetup selectedBattle = BattleEncounterSetups[BattleEncounterSetups.Count - 1];
            for (int i = 0; i < BattleEncounterSetups.Count; i++)
            {
                randomValue -= BattleEncounterSetups[i].Probability;
                if (randomValue <= 0)
                {
                    selectedBattle = BattleEncounterSetups[i];
                    break;
                }
            }

            return selectedBattle.BattleData;
        }
    }

    [Serializable]
    public class BattleEncounterSetup
    {
        public float Probability;
        public BattleDataSO BattleData;
    }
}