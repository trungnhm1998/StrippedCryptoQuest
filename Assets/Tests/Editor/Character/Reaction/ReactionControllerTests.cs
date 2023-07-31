using CryptoQuest.Character.Reaction;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Character.Reaction
{
    [TestFixture]
    public class ReactionControllerTests
    {
        private const string REACTION_CONTROLLER_PREFAB_ASSET_PATH =
            "Assets/Prefabs/Characters/Reaction/ReactionController.prefab";

        private const string REACTION_ANIMATOR_ASSET_PATH =
            "Assets/Animation/Reaction/Reaction.controller";

        private static ReactionController _reactionControllerPrefab;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _reactionControllerPrefab =
                AssetDatabase.LoadAssetAtPath<ReactionController>(REACTION_CONTROLLER_PREFAB_ASSET_PATH);
        }

        [TestFixture]
        public class PrefabSmokeTests
        {
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
            public void ReactionControllerPrefab_SpriteRenderer_ShouldNotHaveSprite()
            {
                var sprite = _reactionControllerPrefab.GetComponent<SpriteRenderer>().sprite;
                Assert.IsNull(sprite);
            }

            [Test]
            public void ReactionControllerPrefab_CanInstantiate()
            {
                var reactionController = Object.Instantiate(_reactionControllerPrefab);
                Assert.NotNull(reactionController);
                Object.DestroyImmediate(reactionController);
            }

            [Test]
            public void ReactionAssets_ShouldHaveCorrectCountAndSpriteMustNotNull()
            {
                var reactions = AssetDatabase.FindAssets("t:Reaction");
                Assert.AreEqual(20, reactions.Length);
                foreach (var reactionGuid in reactions)
                {
                    // load
                    var reactionSO =
                        AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction.Reaction>(
                            AssetDatabase.GUIDToAssetPath(reactionGuid));
                    Assert.NotNull(reactionSO.ReactionIcon);
                }
            }
        }

        private ReactionController _reactionControllerInstance;

        [SetUp]
        public void Setup()
        {
            _reactionControllerInstance = Object.Instantiate(_reactionControllerPrefab);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_reactionControllerInstance);
        }

        [Test]
        public void ShowReaction_SetsSpriteRendererSprite()
        {
            var reaction = ScriptableObject.CreateInstance<CryptoQuest.Character.Reaction.Reaction>();
            var reactionIcon = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
            reaction.ReactionIcon = reactionIcon;
            _reactionControllerInstance.ShowReaction(reaction);
            Assert.AreEqual(reactionIcon, _reactionControllerInstance.GetComponent<SpriteRenderer>().sprite);
        }

        [Test]
        public void ShowReaction_WithAllEmotes_ShouldHaveCorrectSprite()
        {
            var reactions = AssetDatabase.FindAssets("t:Reaction");

            foreach (var guid in reactions)
            {
                var reaction = AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction.Reaction>(
                    AssetDatabase.GUIDToAssetPath(guid));
                _reactionControllerInstance.ShowReaction(reaction);
                Assert.AreEqual(reaction.ReactionIcon,
                    _reactionControllerInstance.GetComponent<SpriteRenderer>().sprite);
            }
        }

        [Test]
        public void ShowReaction_ShouldRaiseShowingEvent()
        {
            var called = false;

            void ShowingReactionCalled()
            {
                called = true;
            }

            _reactionControllerInstance.ShowingReaction += ShowingReactionCalled;

            _reactionControllerInstance.ShowReaction(ScriptableObject
                .CreateInstance<CryptoQuest.Character.Reaction.Reaction>());

            Assert.True(called);
            _reactionControllerInstance.ShowingReaction -= ShowingReactionCalled;
        }
    }
}