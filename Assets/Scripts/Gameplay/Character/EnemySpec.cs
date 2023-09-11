using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public class EnemySpec : CharacterInformation<EnemyDef, EnemySpec>
    {
        private string _displayName = string.Empty;
        public string DisplayName => _displayName;

        public IEnumerator SetDisplayName(string postFix)
        {
            if (Data.Name.IsEmpty)
            {
                Debug.LogWarning($"Localized string not set using default name {Data.Name}");
                _displayName = $"{Data.Name}{postFix}";
                yield break;
            }

            var handle = Data.Name.GetLocalizedStringAsync();
            yield return handle;
            var loadedSuccess = handle.IsValid() && handle.IsDone && handle.Result != null &&
                                handle.Status == AsyncOperationStatus.Succeeded;
            if (!loadedSuccess)
            {
                Debug.LogWarning($"Failed to load localized string for enemy using default name {Data.Name}");
                _displayName = $"{Data.Name}{postFix}";
                yield break;
            }

            _displayName = $"{handle.Result}{postFix}";
            Debug.Log($"Enemy name is {_displayName}");
        }
    }
}