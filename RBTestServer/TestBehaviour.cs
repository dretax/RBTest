using System;
using System.Collections.Generic;
using System.Linq;
using Fougerite.Events;
using RustBuster2016Server;
using UnityEngine;
using NetworkPlayer = UnityEngine.NetworkPlayer;
using Random = System.Random;

namespace RBTestServer
{
    public class TestBehaviour : UnityEngine.MonoBehaviour
    {
        public Dictionary<ulong, API.RustBusterUserAPI> RBConnectionList = new Dictionary<ulong, API.RustBusterUserAPI>();
        public Dictionary<ulong, API.RustBusterUserAPI> CurrentOnlinePlayers = new Dictionary<ulong, API.RustBusterUserAPI>();
        
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        /// <summary>
        /// If our player disconnected while connecting, make sure to remove him from the memory.
        /// </summary>
        /// <param name="player"></param>
        public void OnPlayerDisconnected(Fougerite.Player player)
        {
            if (RBConnectionList.ContainsKey(player.UID))
            {
                RBConnectionList.Remove(player.UID);
            }

            if (CurrentOnlinePlayers.ContainsKey(player.UID))
            {
                CurrentOnlinePlayers.Remove(player.UID);
            }
        }

        /// <summary>
        /// If we have an incoming RB connection store It, and wait until the player actually spawns on the server. (Loaded.)
        /// </summary>
        /// <param name="user"></param>
        public void OnRustBusterLogin(API.RustBusterUserAPI user)
        {
            RBConnectionList[Convert.ToUInt64(user.SteamID)] = user;
        }

        /// <summary>
        /// If our player spawned and loaded everything.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="se"></param>
        public void OnPlayerSpawned(Fougerite.Player player, SpawnEvent se)
        {
            if (RBConnectionList.ContainsKey(player.UID))
            {
                var data = RBConnectionList[player.UID];
                CurrentOnlinePlayers[player.UID] = data; // Lets store the player for later use.
                
                Dictionary<ulong, string> IDandHWID = new Dictionary<ulong, string>(); // Create a dictionary to send to the client side.
                
                
                Dictionary<ulong, API.RustBusterUserAPI> CopiedDictionary = new Dictionary<ulong, API.RustBusterUserAPI>(CurrentOnlinePlayers);
                // Start looping through the dictionary by copying It, so we wont interfere with other for cycles. (Copying is the new Dictionary() above^)
                foreach (var x in CopiedDictionary.Keys)
                {
                    IDandHWID[x] = CopiedDictionary[x].HardwareID;
                }
                
                // UDP, using uLink's RPC API. Server -> Client. (Only possible by uLink's API, because RB doesn't support Server to Client connections.)
                networkView.RPC("ReceiveHardwareIDData", UnityEngine.RPCMode.Others, CopiedDictionary);
                
                //You can send RPC to a specific player too using this code:
                uLink.NetworkView.Get(player.PlayerClient.networkView).RPC("YourOwnRandomString", player.NetworkPlayer, RandomString(8));

                RBConnectionList.Remove(player.UID); // Remove player from the temp list.
            }
        }
    }
}
