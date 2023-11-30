using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using CharacterComponent = CryptoQuest.Battle.Components.Character;

namespace CryptoQuest.Battle.Extensions
{
    public static class CommonBattleHelper
    {
        /// <summary>
        /// <para>This method will random from 0 to sum of the weights to get the distance</para>
        /// <para>And reduce that until the distance is small than zero</para>
        /// <para>So the higher weigth will have more chance to be in the distance</para>
        /// Read more: https://blog.bruce-hill.com/a-faster-weighted-random-choice
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static int WeightedRandomIndex(IEnumerable<int> weights, int seed = -1)
        {
            if (weights.Count() <= 0) return -1;
            var sumWeight = weights.Sum(x => x);
            var remainingDist = (seed >= 0) ? seed : Random.Range(0f, sumWeight);
            var index = 0;
            foreach (var weight in weights)
            {
                remainingDist -= weight;
                if (remainingDist < 0) return index;
                index++;
            }

            return -1;
        }

        public static float GetHighestAttributeValue<T>(this List<T> characters, AttributeScriptableObject attribute)
            where T : CharacterComponent
        {
            var highestValue = 0f;
            foreach (var character in characters)
            {
                if (!character.IsValidAndAlive()) continue;
                var attributeSystem = character.AttributeSystem;
                if (!attributeSystem.TryGetAttributeValue(attribute, out var attributeValue)) continue;
                if (attributeValue.CurrentValue >= highestValue)
                {
                    highestValue = attributeValue.CurrentValue;
                }
            }

            return highestValue;
        }
    }
}