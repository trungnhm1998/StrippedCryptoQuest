using UnityEngine;

namespace CryptoQuest.Quest
{
    /// <summary>
    /// This is where we define the actor that will be connected to a specific quest.
    /// </summary>
    [CreateAssetMenu(fileName = "Actor", menuName = "Quest System/Actor")]
    public class Actor : ScriptableObject
    {
        [SerializeField] private string _id;
        public string Id => _id;
    }
}