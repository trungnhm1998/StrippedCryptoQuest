using System;

namespace IndiGames.Firebase.Struct
{
    [Serializable]
    public struct FirebaseError
    {
        public string code;
        public string message;
        public string details;
    }
}