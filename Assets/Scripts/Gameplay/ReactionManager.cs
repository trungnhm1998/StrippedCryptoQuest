using System.Collections.Generic;
using CryptoQuest.Character;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class ReactionManager : MonoBehaviour
    {
        [SerializeField] private List<Reaction> _reactions = new();

        private Dictionary<string, Reaction> _reactionDictionary = new();

        private NPCBehaviour _interactingNpc;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var reactionGuids = AssetDatabase.FindAssets($"t:{nameof(Reaction)}");
            _reactions = new List<Reaction>();
            foreach (var reactionGuid in reactionGuids)
            {
                var reactionPath = AssetDatabase.GUIDToAssetPath(reactionGuid);
                var reaction = AssetDatabase.LoadAssetAtPath<Reaction>(reactionPath);
                _reactions.Add(reaction);
            }
        }
#endif

        private void Awake()
        {
            foreach (var reaction in _reactions)
            {
                _reactionDictionary.Add(reaction.name, reaction);
            }
        }

        private void OnEnable()
        {
            NPCBehaviour.Interacted += NpcInteracted;
        }

        private void OnDisable()
        {
            NPCBehaviour.Interacted -= NpcInteracted;
        }

        private void NpcInteracted(NPCBehaviour npc)
        {
            _interactingNpc = npc;
        }

        public void ShowReaction(string reactionName)
        {
            if (_interactingNpc == null)
            {
                Debug.LogWarning("No NPC is interacting with the player.");
                return;
            }
            
            if (!_reactionDictionary.ContainsKey(reactionName))
            {
                Debug.LogWarning($"Reaction with name {reactionName} does not exist.");
                return;
            }
            
            _interactingNpc.ShowReaction(_reactionDictionary[reactionName]);
        }
    }
}