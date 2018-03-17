namespace RBTestClient
{
    public class CharacterWaiter : UnityEngine.MonoBehaviour
    {
        /// <summary>
        /// This method keeps checking if the local player character is not null. (If It's null we are chilling in the menu.)
        /// </summary>
        void Update()
        {
            if (PlayerClient.GetLocalPlayer() != null && PlayerClient.GetLocalPlayer().controllable != null)
            {
                Character player = PlayerClient.GetLocalPlayer().controllable.GetComponent<Character>();
                if (player != null)
                {
                    // If we are connected to a server, disable the current behaviour and activate the other one containing the RPCs.
                    this.enabled = false;
                    RBTestClient.Instance.StartMyRPCs();
                }
            }
        }
    }
}