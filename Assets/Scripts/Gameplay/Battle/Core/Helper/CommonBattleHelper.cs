using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CryptoQuest.Gameplay.Battle.Helper
{
    public static class CommonBattleHelper
    {
        /// <summary>
        /// <para>This method will random from 0 to sum of the weights to get the distance</para>
        /// <para>And reduce that until the distance is small than zero</para>
        /// <para>So the higher weigth will have more chance to be in the distance</para>
        /// Read more: <ref>https://blog.bruce-hill.com/a-faster-weighted-random-choice</ref> 
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
    }
}