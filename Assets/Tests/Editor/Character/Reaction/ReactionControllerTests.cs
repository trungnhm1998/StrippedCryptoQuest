using CryptoQuest.Character;
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

        private static ReactionBehaviour _reactionBehaviourPrefab;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _reactionBehaviourPrefab =
                AssetDatabase.LoadAssetAtPath<ReactionBehaviour>(REACTION_CONTROLLER_PREFAB_ASSET_PATH);
        }

        [TestFixture]
        public class PrefabSmokeTests
        {
            [Test]
            public void ReactionControllerPrefab_Exists()
            {
                Assert.NotNull(_reactionBehaviourPrefab);
            }

            [Test]
            public void ReactionControllerPrefab_HasAnimator()
            {
                var animator = _reactionBehaviourPrefab.GetComponent<Animator>();
                Assert.NotNull(animator);
            }

            [Test]
            public void ReactionControllerPrefab_AnimatorController_HasCorrectController()
            {
                var animator = _reactionBehaviourPrefab.GetComponent<Animator>();
                var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(REACTION_ANIMATOR_ASSET_PATH);
                Assert.AreEqual(controller, animator.runtimeAnimatorController);
            }

            [Test]
            public void ReactionControllerPrefab_HasSpriteRenderer()
            {
                var spriteRenderer = _reactionBehaviourPrefab.GetComponent<SpriteRenderer>();
                Assert.NotNull(spriteRenderer);
            }

            [Test]
            public void ReactionControllerPrefab_SpriteRenderer_ShouldNotHaveSprite()
            {
                var sprite = _reactionBehaviourPrefab.GetComponent<SpriteRenderer>().sprite;
                Assert.IsNull(sprite);
            }

            [Test]
            public void ReactionControllerPrefab_CanInstantiate()
            {
                var reactionController = Object.Instantiate(_reactionBehaviourPrefab);
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
                        AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction>(
                            AssetDatabase.GUIDToAssetPath(reactionGuid));
                    Assert.NotNull(reactionSO.ReactionIcon);
                }
            }
        }

        private ReactionBehaviour _reactionBehaviourInstance;

        [SetUp]
        public void Setup()
        {
            _reactionBehaviourInstance = Object.Instantiate(_reactionBehaviourPrefab);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_reactionBehaviourInstance);
        }

        [Test]
        public void ShowReaction_SetsSpriteRendererSprite()
        {
            var reaction = ScriptableObject.CreateInstance<CryptoQuest.Character.Reaction>();
            var reactionIcon = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
            reaction.ReactionIcon = reactionIcon;
            _reactionBehaviourInstance.ShowReaction(reaction);
            Assert.AreEqual(reactionIcon, _reactionBehaviourInstance.GetComponent<SpriteRenderer>().sprite);
        }

        [Test]
        public void ShowReaction_WithAllEmotes_ShouldHaveCorrectSprite()
        {
            var reactions = AssetDatabase.FindAssets("t:Reaction");

            foreach (var guid in reactions)
            {
                var reaction = AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction>(
                    AssetDatabase.GUIDToAssetPath(guid));
                _reactionBehaviourInstance.ShowReaction(reaction);
                Assert.AreEqual(reaction.ReactionIcon,
                    _reactionBehaviourInstance.GetComponent<SpriteRenderer>().sprite);
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

            _reactionBehaviourInstance.ShowingReaction += ShowingReactionCalled;

            _reactionBehaviourInstance.ShowReaction(ScriptableObject
                .CreateInstance<CryptoQuest.Character.Reaction>());

            Assert.True(called);
            _reactionBehaviourInstance.ShowingReaction -= ShowingReactionCalled;
        }
    }
}