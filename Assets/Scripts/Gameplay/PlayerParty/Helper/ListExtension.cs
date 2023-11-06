using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty.Helper
{
    public static class ListExtension
    {
        public static void SortElement<T>(this List<T> elements, int sourceIndex, int destinationIndex)
        {
            T elementToMove = elements[sourceIndex];

            if (!elements.IsIndexValid(sourceIndex) || !elements.IsIndexValid(destinationIndex))
            {
                Debug.LogError("Invalid source or destination index");
                return;
            }

            if (sourceIndex == destinationIndex)
            {
                Debug.LogWarning("Source is the same as destination index");
                return;
            }

            if (sourceIndex < destinationIndex)
            {
                // Shift others forward
                for (int i = sourceIndex; i < destinationIndex; i++)
                {
                    elements[i] = elements[i + 1];
                }
            }
            else
            {
                // Shift others backward
                for (int i = sourceIndex; i > destinationIndex; i--)
                {
                    elements[i] = elements[i - 1];
                }
            }

            elements[destinationIndex] = elementToMove;
        }

        public static bool IsIndexValid<T>(this List<T> list, int index)
        {
            return (0 <= index) && (index < list.Count);
        }
    }
}