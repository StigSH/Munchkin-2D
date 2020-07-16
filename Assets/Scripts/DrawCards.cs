using Mirror;
using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : NetworkBehaviour
{

    public PlayerManager playerManager;

    public void OnClick()
    {


        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();

        
        playerManager.CmdDealCards();

     
    }
}
