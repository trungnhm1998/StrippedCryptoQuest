using CryptoQuest.AbilitySystem.Attributes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class BattleFixtureBase : FixtureBase
    {
        private const string ATTRIBUTE_SETS = "Assets/ScriptableObjects/Character/Attributes/AttributeSets.asset";
        private const string CHARACTER_PREFAB_ASSET = "Assets/Prefabs/Battle/Character.prefab";
        private AttributeSets _attributeSets;

        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
            _attributeSets = AssetDatabase.LoadAssetAtPath<AttributeSets>(ATTRIBUTE_SETS);
        }

        internal GameObject CreateCharacterFromPrefab(string name = null) =>
            CreateGameObjectFromPrefab(CHARACTER_PREFAB_ASSET, name);
    }
}