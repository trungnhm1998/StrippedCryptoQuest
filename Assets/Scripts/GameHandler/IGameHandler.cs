using UnityEngine;
using System.Collections.Generic;
using System;

namespace CryptoQuest.GameHandler
{
    public interface IGameHandler
    {
        void Handle(object request);
        IGameHandler SetNext(IGameHandler nextHandler);
    }
}