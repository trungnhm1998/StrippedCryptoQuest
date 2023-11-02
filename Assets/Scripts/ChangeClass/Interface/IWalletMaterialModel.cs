
using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.ChangeClass.Interfaces
{
    public interface IWalletMaterialModel
    {
        public bool IsLoaded { get; }
        public List<IMaterialModel> MaterialData { get; }
        public IEnumerator CoGetData();
    }
}