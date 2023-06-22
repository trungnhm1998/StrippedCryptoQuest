using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CryptoQuest;
using CryptoQuest.Characters;

namespace Tests.EditMode
{
    public class CharacterFaceDirectionTests
    {
        [Test]
        public void CharacterFaceDirection_Default_Return_North()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            Assert.AreEqual(Character.EFacingDirection.South, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToNorth_Return_North()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SetFacingDirection(Character.EFacingDirection.North);
            Assert.AreEqual(Character.EFacingDirection.North, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToSouth_Return_South()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SetFacingDirection(Character.EFacingDirection.South);
            Assert.AreEqual(Character.EFacingDirection.South, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToWest_Return_West()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SetFacingDirection(Character.EFacingDirection.West);
            Assert.AreEqual(Character.EFacingDirection.West, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToEast_Return_East()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SetFacingDirection(Character.EFacingDirection.East);
            Assert.AreEqual(Character.EFacingDirection.East, character.GetFacingDirection());
        }
        [Test]
        public void CharacterFaceDirection_SetToDefault_Return_South()
        {
            var characterStateSO = ScriptableObject.CreateInstance<CharacterStateSO>();
            var character = new PlayerController();
            character.CharacterStateSO = characterStateSO;
            character.SetFacingDirection(new Character.EFacingDirection());
            Assert.AreEqual(Character.EFacingDirection.South, character.GetFacingDirection());
        }
    }
}

