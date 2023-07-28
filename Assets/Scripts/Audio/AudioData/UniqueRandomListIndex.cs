namespace CryptoQuest.Audio.AudioData
{
    public class UniqueRandomListIndex : RandomListIndex
    {
        protected override int GenerateRandomIndex(int elementCount)
        {
            int nextIndex;
            do
            {
                nextIndex = base.GenerateRandomIndex(elementCount);
            } while (elementCount > 1 && nextIndex == Value);

            return nextIndex;
        }
    }
}