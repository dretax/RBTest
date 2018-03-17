using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace RBTestClient
{
    public class RBTestClient : RustBuster2016.API.RustBusterPlugin
    {
        internal Dictionary<ulong, string> StoredPlayerSteamIDsWithHWID = new Dictionary<ulong, string>();
        internal static RBTestClient Instance;
        private GameObject _gameObject;
        private CharacterWaiter waiter;
        
        private ClientBehaviour cb;
        
        public override string Name
        {
            get { return "RBTestClient"; }
        }

        public override string Author
        {
            get { return "DreTaX"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override void DeInitialize()
        {
            if (cb != null) UnityEngine.Object.Destroy(cb);
        }

        public override void Initialize()
        {
            Instance = this;
            _gameObject = new GameObject();
            waiter = _gameObject.AddComponent<CharacterWaiter>();
            
            // TPC connection using RB's TCP API from Client -> Server
            // We can do this because RB's TCP communication system doesn't need actual unityengine network receivers.
            string receivedmessage = RBTestClient.Instance.SendMessageToServer("randomnumber-" + new Random().Next(0, 6));
            RustBuster2016.API.Hooks.LogData("RBTestClient", "We have received the server's random number which is: " + receivedmessage);
        }
        

        public void StartMyRPCs()
        {
            if (_gameObject != null) {UnityEngine.Object.Destroy(_gameObject);}
            // Add our Behaviour that is containing all the RPC methods to the player.
            cb = PlayerClient.GetLocalPlayer().gameObject.AddComponent<ClientBehaviour>();
        }

        /// <summary>
        /// We can use our stored data safely, so our for cycles won't break when ulink RPC is updating the variable.
        /// </summary>
        public Dictionary<ulong, string> GetHWIDsSafe
        {
            get
            {
                return new Dictionary<ulong, string>(StoredPlayerSteamIDsWithHWID);
            }
        }
    }
}