using CryptoQuest.Battle.Components;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Battle
{
    [TestFixture]
    public class TurnDurationalEffectTests
    {
        private const string PREFAB_PATH = "Assets/Prefabs/Battle/HeroGAS.prefab";

        [Test]
        public void OnTurn_WithOneTurn_ShouldExpiredOnNextTurn()
        {
            var heroGameObject = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
            var hero = heroGameObject.GetComponent<CryptoQuest.Battle.Components.Character>();
            hero.TryGetComponent(out CommandExecutor commandExecutor);

        }
    }
}