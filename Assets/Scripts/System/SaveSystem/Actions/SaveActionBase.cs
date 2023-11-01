using CryptoQuest.Core;
using System;

namespace CryptoQuest.System.SaveSystem.Actions
{
	public abstract class SaveActionBase<TRef> : ActionBase
	{
		public TRef RefObject { get; private set; }

		public SaveActionBase(TRef obj)
		{
			RefObject = obj;
		}
	}

    public abstract class SaveCompletedActionBase : ActionBase
    {
        public bool IsSuccess { get; set; }
        public SaveCompletedActionBase(bool result) { IsSuccess = result; }
    }
}