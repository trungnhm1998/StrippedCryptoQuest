using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public class EnemySpec : CharacterInformation<EnemyDef, EnemySpec>
    {
        public event Action<string> NameChanged;
        private string _displayName = string.Empty;

        public string DisplayName
        {
            get => _displayName;
            private set
            {
                _displayName = value;
                NameChanged?.Invoke(_displayName);
            }
        }

        public override void Init(EnemyDef data)
        {
            base.Init(data);
            if (IsValid() == false) return;

            Data.Name.GetLocalizedStringAsync(); // this should load the localized string when setting postfix later
            Data.Name.StringChanged += UpdateDisplayName;
        }

        private string _postfix = string.Empty;

        private void UpdateDisplayName(string value)
        {
            DisplayName = $"{value}{_postfix}";
        }

        public IEnumerator SetDisplayName(string postfix)
        {
            _postfix = postfix;
            if (Data.Name.IsEmpty)
            {
                Debug.LogWarning($"Localized string not set using default name {Data.Name}");
                DisplayName = $"{Data.Name}{postfix}";
                yield break;
            }

            var handle = Data.Name.GetLocalizedStringAsync();
            yield return handle;
            var loadedSuccess = handle.IsValid() && handle.IsDone && handle.Result != null &&
                                handle.Status == AsyncOperationStatus.Succeeded;
            if (!loadedSuccess)
            {
                Debug.LogWarning($"Failed to load localized string for enemy using default name {Data.Name}");
                DisplayName = $"{Data.Name}{postfix}";
                yield break;
            }

            DisplayName = $"{handle.Result}{postfix}";
        }

        public override void Release()
        {
            Data.Name.StringChanged -= UpdateDisplayName;
            base.Release(); // this need to be after because Data will be null
        }

        /// <summary>
        /// Get all lootable items from enemy based on their <see cref="Drop"/> configs
        /// </summary>
        /// <returns>Cloned loot</returns>
        public List<LootInfo> GetLoots()
        {
            var drops = Data.Drops;
            var loots = new List<LootInfo>();
            foreach (var drop in drops) loots.Add(drop.CreateLoot());
            return loots;
        }
    }
}