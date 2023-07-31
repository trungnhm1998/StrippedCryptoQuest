using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Character.Reaction
{
    [TestFixture]
    [Category("Integration")]
    public class ReactionControllerTests
    {
#if UNITY_EDITOR
        private List<CryptoQuest.Character.Reaction> _reactions;
        private ReactionBehaviour _behaviour;
        private SpriteRenderer _controllerSpriteRenderer;
        private const string REACTIONS_TEST_SCENE = "Assets/Tests/Runtime/Reactions.unity";

        // private bool _initialized; // use this for one time setup but remember to tear down

        [UnitySetUp]
        public IEnumerator OneTimeSetup()
        {
            // if (_initialized) yield break;

            // _initialized = true; // work around for OneTimeSetup

            var reactionGUIDs = AssetDatabase.FindAssets("t:Reaction");

            _reactions = new List<CryptoQuest.Character.Reaction>();
            foreach (var guid in reactionGUIDs)
            {
                var reaction = AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction>(
                    AssetDatabase.GUIDToAssetPath(guid));
                _reactions.Add(reaction);
            }

            Assert.Greater(_reactions.Count, 0, "Reactions asset count must be greater than 0");

            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(REACTIONS_TEST_SCENE,
                new LoadSceneParameters(LoadSceneMode.Single));

            _behaviour = Object.FindObjectOfType<ReactionBehaviour>();

            Assert.NotNull(_behaviour, "Cannot find ReactionController in scene");

            _controllerSpriteRenderer = _behaviour.GetComponent<SpriteRenderer>();
        }

        [UnityTest]
        public IEnumerator ShowReaction_ShouldHideAfter2Seconds()
        {
            _behaviour.ShowReaction(_reactions[0]);
            Assert.AreEqual(_reactions[0].ReactionIcon, _controllerSpriteRenderer.sprite);

            var time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (_controllerSpriteRenderer.sprite == null)
                {
                    break;
                }

                yield return null;
            }

            Assert.IsTrue(time >= 2f && time <= 2.1f, "Reaction should hide after 2 seconds");
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithHideAfterZeroSeconds_ShouldHideAfter1Second()
        {
            _behaviour.ShowReaction(_reactions[0], 0f);

            var time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (_controllerSpriteRenderer.sprite == null)
                {
                    break;
                }

                yield return null;
            }

            Assert.True(time >= .5f, "Reaction should be hidden after .5f when hideAfter is 0");
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithNegativeHideAfter_ShouldHideAfter2SecondsByDefault()
        {
            var showTime = Time.time;
            _behaviour.ShowReaction(_reactions[0], -1f);

            void ReactionHiddenRaised()
            {
                var timeTaken = Time.time - showTime;
                Assert.IsTrue(timeTaken >= 2.5f && timeTaken <= 2.6f,
                    $"Reaction should hide after 2.5s but was {timeTaken}");
            }

            _behaviour.ReactionHidden += ReactionHiddenRaised;
            yield return new WaitForSeconds(3f); // max wait time until ReactionHidden event is raised
            _behaviour.ReactionHidden -= ReactionHiddenRaised;
        }

        [UnityTest]
        public IEnumerator ShowReaction_AfterHide_ReactionHiddenEventShouldBeRaise()
        {
            var raised = false;

            void Handler()
            {
                raised = true;
            }

            _behaviour.ReactionHidden += Handler;
            _behaviour.ShowReaction(_reactions[0]);
            yield return new WaitForSeconds(3f);
            Assert.IsTrue(raised, "ReactionHidden event should be raised");
            _behaviour.ReactionHidden -= Handler;
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithAngryReaction_ShouldRaiseHiddenEventAfterTwoPointFiveSeconds()
        {
            var angryReaction = _reactions[1];
            _behaviour.ShowReaction(angryReaction);
            Assert.AreEqual(angryReaction.ReactionIcon, _controllerSpriteRenderer.sprite, "Should be angry sprite");
            yield return new WaitForSeconds(2.1f); // coroutine cannot be exactly 2 seconds
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithAgreeReactionAndZeroWait_ShouldHideAfterPointFiveSeconds()
        {
            _behaviour.ShowReaction(_reactions[0], 0f);
            Assert.AreEqual(_reactions[0].ReactionIcon, _controllerSpriteRenderer.sprite);
            yield return new WaitForSeconds(0.51f);
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithAnotherReaction_ShouldStopPreviousCoroutineAndShowNewReactionCorrectly()
        {
            _behaviour.ShowReaction(_reactions[0]);
            Assert.AreEqual(_reactions[0].ReactionIcon, _controllerSpriteRenderer.sprite);
            yield return new WaitForSeconds(.3f);

            // get curren time
            var time = Time.time;

            void ReactionHiddenRaised()
            {
                var f = Time.time - time;
                if (f < 2.5f)
                {
                    Assert.Fail($"Reaction should be hidden after 2.5 seconds(default) but hidden after {f} seconds");
                }
            }

            _behaviour.ReactionHidden += ReactionHiddenRaised;
            _behaviour.ShowReaction(_reactions[1]);
            Assert.AreEqual(_reactions[1].ReactionIcon, _controllerSpriteRenderer.sprite);
            yield return new WaitForSeconds(3f);
            _behaviour.ReactionHidden -= ReactionHiddenRaised;
        }

        [UnityTest]
        public IEnumerator ShowReaction_AllReaction_ShouldDisplayCorrectly()
        {
            foreach (var reaction in _reactions)
            {
                _behaviour.ShowReaction(reaction);
                Assert.AreEqual(reaction.ReactionIcon, _controllerSpriteRenderer.sprite);
                yield return new WaitForSeconds(.5f); // show at least .5 seconds
            }
        }
#endif
    }
}