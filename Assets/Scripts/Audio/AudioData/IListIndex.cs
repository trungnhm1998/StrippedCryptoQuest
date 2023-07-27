namespace CryptoQuest.Audio.AudioData
{
    public interface IListIndex
    {
        int Value { get; }
        IListIndex GoForward(int elementCount);
        IListIndex GoBackward(int elementCount);
    }
}