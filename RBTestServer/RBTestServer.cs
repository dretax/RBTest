using System;
using Fougerite;
using Fougerite.Events;
using RustBuster2016Server;
using UnityEngine;
using Random = System.Random;

namespace RBTestServer
{
    public class RBTestServer : Fougerite.Module
    {
        private GameObject myGameObject;
        private TestBehaviour myTestBehaviour;
        
        public override string Name
        {
            get { return "RBTestServer"; }
        }

        public override string Author
        {
            get { return "DreTaX"; }
        }

        public override string Description
        {
            get { return "RBTestServer"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override void Initialize()
        {
            myGameObject = new GameObject();
            myTestBehaviour = myGameObject.AddComponent<TestBehaviour>();
            
            Hooks.OnPlayerSpawned += myTestBehaviour.OnPlayerSpawned;
            Hooks.OnPlayerDisconnected += myTestBehaviour.OnPlayerDisconnected;
            API.OnRustBusterLogin += myTestBehaviour.OnRustBusterLogin;

            API.OnRustBusterUserMessage += OnRustBusterUserMessage;
        }

        public override void DeInitialize()
        {
            Hooks.OnPlayerSpawned -= myTestBehaviour.OnPlayerSpawned;
            Hooks.OnPlayerDisconnected -= myTestBehaviour.OnPlayerDisconnected;
            API.OnRustBusterLogin -= myTestBehaviour.OnRustBusterLogin;
            API.OnRustBusterUserMessage -= OnRustBusterUserMessage;
            
            if (myGameObject != null) UnityEngine.Object.Destroy(myGameObject);
        }

        public void OnRustBusterUserMessage(API.RustBusterUserAPI user, Message msgc)
        {
            if (msgc.PluginSender == "RBTestClient")
            {
                string fullmessage = msgc.MessageByClient;
                string[] split = fullmessage.Split('-');
                if (split[0] == "randomnumber")
                {
                    string randomnumber = split[1];
                    System.Random rnd = new Random();
                    msgc.ReturnMessage = rnd.Next(1000, 3000).ToString();
                }
            }
        }
    }
}