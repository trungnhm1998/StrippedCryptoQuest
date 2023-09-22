using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;

namespace CryptoQuest.System
{
    /// <summary>
    /// Instead of using Event that could raise on multiple instances, I only want 1 instance 
    /// Act as service locator for the inventory, party and equipment controllers
    ///
    /// The safest way to work with this is create a scene where we know these services are available
    /// inject into this provider
    ///
    /// Then load the next scene where we need these services
    ///
    /// This is an anti-pattern, every time a new service is added, we need to update this provider
    /// We need to inject before actually using, all setup has to be done before hand
    /// TODO: refactor to use dependency injection
    /// </summary>
    public static class ServiceProvider
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        public static void Provide<T>(T service)
        {
            Debug.Log($"Bind service [{service.GetType().Name}]");
            Services[typeof(T)] = service;
        }

        public static T GetService<T>()
        {
            return (T)Services[typeof(T)];
        }
    }
}