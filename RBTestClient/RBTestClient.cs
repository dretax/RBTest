using System;
using System.Collections.Generic;
using UnityEngine;

namespace RBTestClient
{
    public class RBTestClient : RustBuster2016.API.RustBusterPlugin
    {
        Dictionary<ulong, string> StoredPlayerSteamIDsWithHWID = new Dictionary<ulong, string>();
        private GameObject go;
        private ClientBehaviour cb;
        internal static RBTestClient Instance;
        
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
            
        }

        public override void Initialize()
        {
            go = new GameObject();
            cb = go.AddComponent<ClientBehaviour>();
            Instance = this;
        }

        [RPC]
        public void ReceiveHardwareIDData(ulong uid, string hwid)
        {
            StoredPlayerSteamIDsWithHWID[uid] = hwid;
            
        }
    }
}