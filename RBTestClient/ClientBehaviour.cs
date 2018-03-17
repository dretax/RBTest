using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = System.Random;

namespace RBTestClient
{
    public class ClientBehaviour : UnityEngine.MonoBehaviour
    {
        [RPC]
        public void ReceiveHardwareIDData(Dictionary<ulong, string> data)
        {
            RBTestClient.Instance.StoredPlayerSteamIDsWithHWID = data;
        }

        [RPC]
        public void YourOwnRandomString(string myownrandomstring)
        {
            // do something with my own random string.
        }
    }
}