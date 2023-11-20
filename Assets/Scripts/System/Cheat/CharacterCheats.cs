using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandTerminal;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class CharacterCheats : MonoBehaviour, ICheatInitializer
    {
        // create singleton
        public static CharacterCheats Instance { get; private set; }
        private readonly Dictionary<int, Battle.Components.Character> _cache = new();

        private void Awake()
        {
            _cache.Clear();
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("get.characters", GetCharacters, 0, 0,
                "get.characters, get all characters in current context");
        }

        private void GetCharacters(CommandArg[] obj)
        {
            var characters =
                FindObjectsByType<Battle.Components.Character>(FindObjectsInactive.Exclude,
                    FindObjectsSortMode.InstanceID);
            foreach (var character in characters.Reverse())
            {
                if (!_cache.TryAdd(character.GetInstanceID(), character)) return;
                Debug.Log($"Character [{character.DisplayName}] id: [{character.GetInstanceID()}]");

#if UNITY_EDITOR
                // Very handy in editor but since I can't remove from autocomplete when character not valid 
                // it's not pratical in build since tester will test many battle
                Terminal.Autocomplete.Register(character.GetInstanceID().ToString());
#endif
            }
        }

        public Battle.Components.Character GetCharacter(int instanceId) =>
            _cache.TryGetValue(instanceId, out var character) ? character : null;
    }
}