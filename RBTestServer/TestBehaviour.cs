using System;
using System.Collections.Generic;
using Fougerite.Events;
using RustBuster2016Server;
using UnityEngine;
using NetworkPlayer = UnityEngine.NetworkPlayer;

namespace RBTestServer
{
    public class TestBehaviour : UnityEngine.MonoBehaviour
    {
        public Dictionary<ulong, API.RustBusterUserAPI> RBConnectionList = new Dictionary<ulong, API.RustBusterUserAPI>();
        
        public void OnPlayerDisconnected(Fougerite.Player player)
        {
            if (RBConnectionList.ContainsKey(player.UID))
            {
                RBConnectionList.Remove(player.UID);
            }
        }

        public void OnRustBusterLogin(API.RustBusterUserAPI user)
        {
            RBConnectionList[Convert.ToUInt64(user.SteamID)] = user;
        }

        public void OnPlayerSpawned(Fougerite.Player player, SpawnEvent se)
        {
            if (RBConnectionList.ContainsKey(player.UID))
            {
                var data = RBConnectionList[player.UID];

                string hwid = data.HardwareID;
                
                // UDP, using uLink's RPC API. Server -> Client. (Only possible by uLink's API, because RB doesn't support Server to Client connections.)
                
                networkView.RPC("ReceiveHardwareIDData", UnityEngine.RPCMode.Others, player.UID, hwid);
                
                //You can send RPC to a specific player too using this code:
                uLink.NetworkView.Get(Player.PlayerClient.networkView).RPC("ReceiveHardwareIDData", player.NetworkPlayer, player.UID, hwid);

                RBConnectionList.Remove(player.UID);
            }
        }

        [RPC]
        public void IReceivedTheHWIDThankyou(int randomnumber)
        {
            //todo
        }
    }
}
