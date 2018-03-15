using System;
using System.Runtime.InteropServices;

namespace RBTestClient
{
    public class ClientBehaviour : UnityEngine.MonoBehaviour
    {
        public void SendRandomNumberToServer()
        {
            // UDP Connection using uLink's RPC method API from Client -> Server
            networkView.RPC("IReceivedTheHWIDThankyou", UnityEngine.RPCMode.Server, new Random().Next(0, 6));
            
            // TPC connection using RB's TCP API from Client -> Server
            string receivedmessage = RBTestClient.Instance.SendMessageToServer("randomnumber-" + new Random().Next(0, 6));
            RustBuster2016.API.Hooks.LogData("RBTestClient", "We have received the server's random number which is: " + receivedmessage);
            
        }
    }
}