using System.Collections;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public interface IPresentCommand
    {
        public IEnumerator Present();
    }
}