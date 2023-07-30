using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CryptoQuest.Character.Reaction;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Character.Reaction
{
#if UNITY_EDITOR

    [TestFixture]
    public class ReactionTests
    {
        private ReactionController _reactionController;

        [UnitySetUp]
        public IEnumerator ReactionControllerPrefab_CanInstantiate()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/Prefabs/Characters/Reaction/ReactionController.prefab");
            _reactionController = Object.Instantiate(prefab).GetComponent<ReactionController>();
            yield return null;
            Assert.NotNull(_reactionController);
            // var reactions = AssetDatabase.FindAssets("t:Reaction");
            // foreach (var reaction in reactions)
            // {
            //     var reactionAssetPath = AssetDatabase.GUIDToAssetPath(reaction);
            //     var reactionAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(reactionAssetPath);
            //     var reactionController = new GameObject("ReactionController", typeof(Character.Reaction.ReactionController));
            //     var reactionControllerComponent = reactionController.GetComponent<Character.Reaction.ReactionController>();
            //     reactionControllerComponent.ShowEmote((Character.Reaction.Reaction) reactionAsset);
            //     yield return null;
            // }
        }

        [UnityTest]
        public IEnumerator ShowReaction_WithAgree_ShouldShowUp()
        {
            var agreeReaction =
                AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction.Reaction>(
                    "Assets/ScriptableObjects/Emote/Agree.asset");
            _reactionController.ShowReaction(agreeReaction);
            
            yield return new WaitForSeconds(2f);
            
            Assert.AreEqual(agreeReaction.ReactionIcon, _reactionController.GetComponent<SpriteRenderer>().sprite);
        }

        [UnityTest]
        public IEnumerator ShowReaction_AllReactions_ShouldNotBreak()
        {
            var reactions = AssetDatabase.FindAssets("t:Reaction");
            foreach (var reaction in reactions)
            {
                var reactionAssetPath = AssetDatabase.GUIDToAssetPath(reaction);
                var reactionAsset = AssetDatabase.LoadAssetAtPath<CryptoQuest.Character.Reaction.Reaction>(reactionAssetPath);
                _reactionController.ShowReaction(reactionAsset);
                yield return new WaitForSeconds(2f);
            }
        }
    }
#endif
}