using System;
using System.Collections.Generic;
using CryptoQuest.Character.Behaviours;
using UnityEngine;
using EFacingDirection = CryptoQuest.Character.MonoBehaviours.CharacterBehaviour.EFacingDirection;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class OffsetOnFacingDirectionBehaviour : MonoBehaviour
    {
        [Serializable]
        public struct FacingOffset
        {
            public EFacingDirection Direction;
            public Vector2 Offset;
        }

        [Header("Configs")]
        [SerializeField] private FacingBehaviour _facingBehaviour;
        [SerializeField] private FacingOffset[] _facingOffsets = new FacingOffset[4];

        private readonly Dictionary<EFacingDirection, FacingOffset> _facingOffsetDictionary =
            new Dictionary<EFacingDirection, FacingOffset>();

        private void Awake()
        {
            foreach (FacingOffset facingOffset in _facingOffsets)
            {
                _facingOffsetDictionary.Add(facingOffset.Direction, facingOffset);
            }
        }

        private void OnEnable()
        {
            _facingBehaviour.OnFacingDirectionChanged += HandleFacingDirectionChanged;
        }

        private void OnDisable()
        {
            _facingBehaviour.OnFacingDirectionChanged -= HandleFacingDirectionChanged;
        }

        private void HandleFacingDirectionChanged(EFacingDirection direction)
        {
            transform.localPosition = _facingOffsetDictionary[direction].Offset;
        }
    }
}
