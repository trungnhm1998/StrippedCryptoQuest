using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.AddressableAssets;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "ActionData", menuName = "Gameplay/Battle/Action Data/Battle Action Data")]
    public class BattleActionDataSO : ScriptableObject
    {
        [field: SerializeField]
        public LocalizedString Log { get; private set; }
        [field: SerializeField]
        public AssetReference VFXPrefab { get; private set; }

        public AbilitySystemBehaviour Target { get; private set; }

        public void Init(AbilitySystemBehaviour target = null)
        {
            Log.Clear();
            Target = target;
        }

        public void AddStringVar(string variableName, string value)
        {
            var variableValue = new StringVariable();
            variableValue.Value = value;
            Log.Add(variableName, variableValue);
        }

        public void AddFloatVar(string variableName, float value)
        {
            var variableValue = new FloatVariable();
            variableValue.Value = value;
            Log.Add(variableName, variableValue);
        }
    }
}
