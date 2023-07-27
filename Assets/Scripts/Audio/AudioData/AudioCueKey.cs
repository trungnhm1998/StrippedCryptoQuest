using System;

namespace CryptoQuest.Audio.AudioData
{
    public struct AudioCueKey
    {
        public static AudioCueKey Invalid = new AudioCueKey(-1, null);

        public int Value;
        public AudioCueSO AudioCue;

        public AudioCueKey(int value, AudioCueSO audioCue = null)
        {
            Value = value;
            AudioCue = audioCue;
        }

        /// <summary>
        /// Override the Equals method to provide custom comparison logic for AudioCueKey objects.
        /// </summary>
        /// <param name="obj">The object to compare with the current AudioCueKey.</param>
        /// <returns>True if the specified object is equal to the current AudioCueKey; otherwise, false.</returns>
        public override bool Equals(Object obj) =>
            obj is AudioCueKey x && Value == x.Value && AudioCue == x.AudioCue;

        /// <summary>
        /// Override the GetHashCode method to provide a custom hash code for AudioCueKey objects.
        /// </summary>
        /// <returns>The hash code for the current AudioCueKey.</returns>
        public override int GetHashCode() => Value.GetHashCode() ^ AudioCue.GetHashCode();

        /// <summary>
        /// Override the equality operator (==) to compare two AudioCueKey objects for equality.
        /// </summary>
        /// <param name="x">The first AudioCueKey to compare.</param>
        /// <param name="y">The second AudioCueKey to compare.</param>
        /// <returns>True if the two AudioCueKey objects are equal; otherwise, false.</returns>
        public static bool operator ==(AudioCueKey x, AudioCueKey y) =>
            x.Value == y.Value && x.AudioCue == y.AudioCue;

        /// <summary>
        /// Override the inequality operator (!=) to compare two AudioCueKey objects for inequality.
        /// </summary>
        /// <param name="x">The first AudioCueKey to compare.</param>
        /// <param name="y">The second AudioCueKey to compare.</param>
        /// <returns>True if the two AudioCueKey objects are not equal; otherwise, false.</returns>
        public static bool operator !=(AudioCueKey x, AudioCueKey y) => !(x == y);
    }
}