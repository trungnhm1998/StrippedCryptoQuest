using System.Collections;
using System.ComponentModel;
using CryptoQuest.Character.Reaction;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests
{
    [TestFixture]
    public class ReactionControllerTests
    {
        private const string REACTION_CONTROLLER_PREFAB_ASSET_PATH =
            "Assets/Prefabs/Characters/Reaction/ReactionController.prefab";
        private const string REACTION_ANIMATOR_ASSET_PATH =
            "Assets/Animation/Reaction/Reaction.controller";

        [TestFixture]
        public class SmokeTests
        {
            private ReactionController _reactionControllerPrefab;

            [SetUp]
            public void SetUp()
            {
                _reactionControllerPrefab =
                    AssetDatabase.LoadAssetAtPath<ReactionController>(REACTION_CONTROLLER_PREFAB_ASSET_PATH);
            }

            [Test]
            public void ReactionControllerPrefab_Exists()
            {
                Assert.NotNull(_reactionControllerPrefab);
            }

            [Test]
            public void ReactionControllerPrefab_HasAnimator()
            {
                var animator = _reactionControllerPrefab.GetComponent<Animator>();
                Assert.NotNull(animator);
            }

            [Test]
            public void ReactionControllerPrefab_AnimatorController_HasCorrectController()
            {
                var animator = _reactionControllerPrefab.GetComponent<Animator>();
                var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(REACTION_ANIMATOR_ASSET_PATH);
                Assert.AreEqual(controller, animator.runtimeAnimatorController);
            }

            [Test]
            public void ReactionControllerPrefab_HasSpriteRenderer()
            {
                var spriteRenderer = _reactionControllerPrefab.GetComponent<SpriteRenderer>();
                Assert.NotNull(spriteRenderer);
            }

            [Test]
            public void ReactionControllerPrefab_CanInstantiate()
            {
                var reactionController = Object.Instantiate(_reactionControllerPrefab);
                Assert.NotNull(reactionController);
                Object.DestroyImmediate(reactionController.gameObject);
            }

            [Test]
            public void ReactionAssets_ShouldHaveCorrectCount()
            {
                var reactions = AssetDatabase.FindAssets("t:Reaction");
                Assert.AreEqual(20, reactions.Length);
            }
        }

        [TestFixture]
        public class UnitTests
        {
            private ReactionController _reactionControllerPrefab;
            private ReactionController _reactionController;

            [SetUp]
            public void SetUp()
            {
                _reactionControllerPrefab =
                    AssetDatabase.LoadAssetAtPath<ReactionController>(REACTION_CONTROLLER_PREFAB_ASSET_PATH);
                _reactionController = Object.Instantiate(_reactionControllerPrefab);
            }

            [TearDown]
            public void TearDown()
            {
                Object.DestroyImmediate(_reactionController.gameObject);
            }

            [Test]
            public void ShowEmote_SetsSpriteRendererSprite()
            {
                var emote = ScriptableObject.CreateInstance<Reaction>();
                var emoteReactionIcon = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
                emote.ReactionIcon = emoteReactionIcon;
                _reactionController.ShowReaction(emote);
                Assert.AreEqual(emoteReactionIcon, _reactionController.GetComponent<SpriteRenderer>().sprite);
            }

            [Test]
            public void ShowEmote_WithAllEmotes_ShouldHaveCorrectSprite()
            {
                var reactions = AssetDatabase.FindAssets("t:Reaction");

                foreach (var reaction in reactions)
                {
                    var emote = AssetDatabase.LoadAssetAtPath<Reaction>(AssetDatabase.GUIDToAssetPath(reaction));
                    _reactionController.ShowReaction(emote);
                    Assert.AreEqual(emote.ReactionIcon,
                        _reactionController.GetComponent<SpriteRenderer>().sprite);
                }
            }
        }
    }
}