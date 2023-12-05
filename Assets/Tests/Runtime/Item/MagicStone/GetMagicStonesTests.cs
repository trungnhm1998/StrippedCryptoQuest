using System.Collections;
using CryptoQuest.Actions;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Item.MagicStone
{
    [TestFixture]
    public class GetMagicStonesTests
    {
        [UnityTest, Category("Integration")]
        public IEnumerator FetchProfileCharactersAction_FetchCorrectStoneIntoClient()
        {
            LogAssert.ignoreFailingMessages = true;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode
            ("Assets/Scenes/Maps/ElrodsCastle/ElrodsCastleTown_TheTavernF1.unity",
                new LoadSceneParameters(LoadSceneMode.Single));
            yield return new WaitForSeconds(5f);
            ActionDispatcher.Dispatch(new FetchProfileCharactersAction());
            yield return new WaitForSeconds(5f);
            var stoneInventory = AssetDatabase.LoadAssetAtPath<MagicStoneInventory>(
                "Assets/ScriptableObjects/Inventories/StoneInventory.asset");
            Assert.AreEqual(6, stoneInventory.MagicStones.Count);
        }
    }
}