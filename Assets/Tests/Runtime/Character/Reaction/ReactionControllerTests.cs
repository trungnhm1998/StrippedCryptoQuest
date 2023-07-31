using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.Reaction;
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
        private List<CryptoQuest.Character.Reaction.Reaction> _reactions;
        private ReactionController _controller;
        private SpriteRenderer _controllerSpriteRenderer;
        private const string REACTIONS_TEST_SCENE = "Assets/Tests/Runtime/Reactions.unity";

        // private bool _initialized; // use this for one time setup but remember to tear down

        [UnitySetUp]
        public IEnumerator OneTimeSetup()
        {
            // if (_initialized) yield break;

            // _initialized = true; // work around for OneTimeSetup

            var reactionGUIDs = AssetDatabase.FindAssets("t:Reaction");

            _reactions = new List<CryptoQuest.Character.Reaction.Reaction>();
            foreach (var guid in reactionGUIDs)
            {
                var reaction = AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction.Reaction>(
                    AssetDatabase.GUIDToAssetPath(guid));
                _reactions.Add(reaction);
            }

            Assert.Greater(_reactions.Count, 0, "Reactions asset count must be greater than 0");

            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(REACTIONS_TEST_SCENE,
                new LoadSceneParameters(LoadSceneMode.Single));

            _controller = Object.FindObjectOfType<ReactionController>();

            Assert.NotNull(_controller, "Cannot find ReactionController in scene");

            _controllerSpriteRenderer = _controller.GetComponent<SpriteRenderer>();
        }

        [UnityTest]
        public IEnumerator ShowReaction_ShouldHideAfter2Seconds()
        {
            _controller.ShowReaction(_reactions[0]);
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
            _controller.ShowReaction(_reactions[0], 0f);

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
            _controller.ShowReaction(_reactions[0], -1f);

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

            Assert.IsTrue(time >= 2.5f && time <= 2.6f, $"Reaction should hide after 2 seconds(Default) but was {time}");
        }

        [UnityTest]
        public IEnumerator ShowReaction_AfterHide_ReactionHiddenEventShouldBeRaise()
        {
            var raised = false;

            void Handler()
            {
                raised = true;
            }

            _controller.ReactionHidden += Handler;
            _controller.ShowReaction(_reactions[0]);
            yield return new WaitForSeconds(3f);
            Assert.IsTrue(raised, "ReactionHidden event should be raised");
            _controller.ReactionHidden -= Handler;
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithAngryReaction_ShouldRaiseHiddenEventAfterTwoPointFiveSeconds()
        {
            var angryReaction = _reactions[1];
            _controller.ShowReaction(angryReaction);
            Assert.AreEqual(angryReaction.ReactionIcon, _controllerSpriteRenderer.sprite, "Should be angry sprite");
            yield return new WaitForSeconds(2.1f); // coroutine cannot be exactly 2 seconds
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithAgreeReactionAndZeroWait_ShouldHideAfterPointFiveSeconds()
        {
            _controller.ShowReaction(_reactions[0], 0f);
            Assert.AreEqual(_reactions[0].ReactionIcon, _controllerSpriteRenderer.sprite);
            yield return new WaitForSeconds(0.51f);
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithAnotherReaction_ShouldStopPreviousCoroutineAndShowNewReactionCorrectly()
        {
            _controller.ShowReaction(_reactions[0]);
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

            _controller.ReactionHidden += ReactionHiddenRaised;
            _controller.ShowReaction(_reactions[1]);
            Assert.AreEqual(_reactions[1].ReactionIcon, _controllerSpriteRenderer.sprite);
            yield return new WaitForSeconds(3f);
            _controller.ReactionHidden -= ReactionHiddenRaised;
        }

        [UnityTest]
        public IEnumerator ShowReaction_AllReaction_ShouldDisplayCorrectly()
        {
            foreach (var reaction in _reactions)
            {
                _controller.ShowReaction(reaction);
                Assert.AreEqual(reaction.ReactionIcon, _controllerSpriteRenderer.sprite);
                yield return new WaitForSeconds(.5f); // show at least .5 seconds
            }
        }
#endif
    }
}