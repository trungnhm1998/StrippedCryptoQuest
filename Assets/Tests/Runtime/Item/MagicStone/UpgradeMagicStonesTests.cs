using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Actions;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using NSubstitute;
using NUnit.Framework;
using TinyMessenger;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using CryptoQuest.BlackSmith.UpgradeStone.Sagas;

namespace CryptoQuest.Tests.Runtime.Item.MagicStone
{
    [TestFixture]
    public class UpgradeMagicStonesTests
    {
        private IMagicStoneResponseConverter _converter;
        private VoidEventChannelSO _sceneLoadedEvent;
        private TinyMessageSubscriptionToken _getStoneToken;
        private TinyMessageSubscriptionToken _upgradeSuccessToken;
        private TinyMessageSubscriptionToken _upgradeFailedToken;
        private TinyMessageSubscriptionToken _getStoneFailedToken;
        private TinyMessageSubscriptionToken _fillInventoryToken;
        private MagicStonesResponse _response;

        private bool _sceneLoaded;
        private bool _finishTest;

        [SetUp]
        public void Setup()
        {
            _finishTest = false;
            _sceneLoadedEvent = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(
                "Assets/ScriptableObjects/Events/SceneManagement/SceneLoadedEventChannel.asset");
        }

        [UnityTest, Category("Integration")]
        public IEnumerator FetchAndTryEvolveStones_IfThere3StonesWithSameDefinitionAndLevel_ShouldSuccess()
        {
            _sceneLoaded = false;

            LogAssert.ignoreFailingMessages = true;
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode
            ("Assets/Scenes/WIP/TestBlackSmith.unity", new LoadSceneParameters(LoadSceneMode.Single));
        
            _sceneLoadedEvent.EventRaised += SetSceneLoaded;
            yield return new WaitUntil(() => _sceneLoaded);

            _upgradeSuccessToken = ActionDispatcher.Bind<ResponseUpgradeStoneSuccess>((ctx) =>
                {
                    ActionDispatcher.Unbind(_upgradeSuccessToken);
                    Assert.NotNull(ctx.Stone);
                    _finishTest = true;
                } 
            );

            _upgradeFailedToken = ActionDispatcher.Bind<ResponseUpgradeStoneFailed>((ctx) =>
                {
                    ActionDispatcher.Unbind(_upgradeFailedToken);
                    _finishTest = true;
                } 
            );

            _getStoneToken = ActionDispatcher.Bind<GetStonesResponsed>((ctx) => 
            {
                ActionDispatcher.Unbind(_getStoneToken);
                _response = ctx.Response;
                Assert.NotNull(_response);

                var stoneDatas = _response.data.stones;

                if (stoneDatas.Length <= 2)
                {
                    Debug.Log($"Not enough stones to test");
                    _finishTest = true;
                }


                var stones = new List<IMagicStone>();
                foreach (var stoneData in stoneDatas)
                {
                    stones.Add(_converter.Convert(stoneData));
                }

                // group and get list of stones that has same Definition id and level
                var groupedStones = stones.GroupBy(stone => stone.Definition.ID).Where(group => group.Count() >= 3);
                // get first 3 stones
                var selectedStones = groupedStones.First().Take(3);
                // assert same stones
                Assert.IsTrue(selectedStones.Count() == 3);
                Assert.IsTrue(selectedStones.All(stone => stone.Definition.ID == selectedStones.First().Definition.ID));
                
                ActionDispatcher.Dispatch(new RequestUpgradeStone(selectedStones.ToArray()));
            });

            _getStoneFailedToken = ActionDispatcher.Bind<GetStonesFailed>((ctx) => 
            {
                ActionDispatcher.Unbind(_getStoneFailedToken);
                _finishTest = true;
            });

            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
            yield return new WaitUntil(() => _finishTest);
            Assert.IsTrue(_finishTest);
        }

        private void SetSceneLoaded()
        {
            Debug.Log($"Scene loaded");
            _sceneLoaded = true;
        }

        [TearDown]
        public void TearDown()
        {
            _sceneLoadedEvent.EventRaised -= SetSceneLoaded;
            _sceneLoaded = false;
            if (_getStoneToken != null)
                ActionDispatcher.Unbind(_getStoneToken);
            if (_fillInventoryToken != null)
                ActionDispatcher.Unbind(_fillInventoryToken);
            if (_getStoneFailedToken != null)
                ActionDispatcher.Unbind(_getStoneFailedToken);
            if (_upgradeSuccessToken != null)
                ActionDispatcher.Unbind(_upgradeSuccessToken);
            if (_upgradeFailedToken != null)
                ActionDispatcher.Unbind(_upgradeFailedToken);

            _finishTest = false;
        }
    }
}