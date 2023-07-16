using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character
{
    public enum TypeOfEmote
    {
        Smile = 0,
        Laugh = 1,
        Angry = 2,
        Crying = 3,
        Smirking = 4,
        Screaming = 5,
        OpenMouth = 6,
        Happy = 7,
        ShookHead = 8,
        Weary = 9,
        SlightlySmile = 10,
        Love = 11,
        Sulking = 12,
        Doubt = 13,
        Thinking = 14,
        NervousSmile = 15,
        QuestionMark = 16,
        Exclamation = 17,
        Agree = 18,
        Disagree = 19
    }

    [CreateAssetMenu(fileName = "Emote", menuName = "Crypto Quest/Emote")]
    public class EmoteSO : ScriptableObject
    {
        public TypeOfEmote TypeOfEmote;
        public Sprite ReactionIcon;
    }
}
